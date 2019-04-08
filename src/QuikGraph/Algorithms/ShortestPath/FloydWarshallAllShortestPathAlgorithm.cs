﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.IO;
using QuickGraph.Algorithms.Services;
using QuickGraph.Collections;

namespace QuickGraph.Algorithms.ShortestPath
{
    /// <summary>
    /// Floyd-Warshall all shortest path algorith,
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
    public class FloydWarshallAllShortestPathAlgorithm<TVertex, TEdge> 
        : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly Func<TEdge, double> weights;
        private readonly IDistanceRelaxer distanceRelaxer;
        private readonly Dictionary<SEquatableEdge<TVertex>, VertexData> data;

        struct VertexData
        {
            public readonly double Distance;
            readonly TVertex _predecessor;
            readonly TEdge _edge;
            readonly bool edgeStored;

            public bool TryGetPredecessor(out TVertex predecessor)
            {
                predecessor = this._predecessor;
                return !this.edgeStored;
            }

            public bool TryGetEdge(out TEdge _edge)
            {
                _edge = this._edge;
                return this.edgeStored;
            }

            public VertexData(double distance, TEdge _edge)
            {
                this.Distance = distance;
                this._predecessor = default(TVertex);
                this._edge = _edge;
                this.edgeStored = true;
            }

            public VertexData(double distance, TVertex predecessor)
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(predecessor != null);
#endif

                this.Distance = distance;
                this._predecessor = predecessor;
                this._edge = default(TEdge);
                this.edgeStored = false;
            }

#if SUPPORTS_CONTRACTS
            [ContractInvariantMethod]
            void ObjectInvariant()
            {
                Contract.Invariant(this.edgeStored ? this._edge != null : this._predecessor != null);
            }
#endif

            public override string ToString()
            {
                if (this.edgeStored)
                    return String.Format("e:{0}-{1}", this.Distance, this._edge);
                else
                    return String.Format("p:{0}-{1}", this.Distance, this._predecessor);
            }
        }

        public FloydWarshallAllShortestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
            )
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(weights != null);
            Contract.Requires(distanceRelaxer != null);
#endif

            this.weights = weights;
            this.distanceRelaxer = distanceRelaxer;
            this.data = new Dictionary<SEquatableEdge<TVertex>, VertexData>();
        }

        public FloydWarshallAllShortestPathAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer)
            : base(visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(weights != null);
            Contract.Requires(distanceRelaxer != null);
#endif

            this.weights =weights;
            this.distanceRelaxer = distanceRelaxer;
            this.data = new Dictionary<SEquatableEdge<TVertex>, VertexData>();
        }

        public FloydWarshallAllShortestPathAlgorithm(
            IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> weights)
            : this(visitedGraph, weights, DistanceRelaxers.ShortestDistance)
        {
        }

        public bool TryGetDistance(TVertex source, TVertex target, out double cost)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            VertexData value;
            if (this.data.TryGetValue(new SEquatableEdge<TVertex>(source, target), out value))
            {
                cost = value.Distance;
                return true;
            }
            else
            {
                cost = -1;
                return false;
            }
        }

        public bool TryGetPath(
            TVertex source,
            TVertex target,
            out IEnumerable<TEdge> path)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            if (source.Equals(target))
            {
                path = null;
                return false;
            }

#if DEBUG && !SILVERLIGHT
            var set = new HashSet<TVertex>();
            set.Add(source); 
            set.Add(target);
#endif

            var edges = new EdgeList<TVertex, TEdge>();
            var todo = new Stack<SEquatableEdge<TVertex>>();
            todo.Push(new SEquatableEdge<TVertex>(source, target));
            while (todo.Count > 0)
            {
                var current = todo.Pop();
#if SUPPORTS_CONTRACTS
                Contract.Assert(!current.Source.Equals(current.Target));
#endif
                VertexData data;
                if (this.data.TryGetValue(current, out data))
                {
                    TEdge edge;
                    if (data.TryGetEdge(out edge))
                        edges.Add(edge);
                    else
                    {
                        TVertex intermediate;
                        if (data.TryGetPredecessor(out intermediate))
                        {
#if SUPPORTS_CONTRACTS
#if DEBUG && !SILVERLIGHT
                            Contract.Assert(set.Add(intermediate));
#endif
#endif
                            todo.Push(new SEquatableEdge<TVertex>(intermediate, current.Target));
                            todo.Push(new SEquatableEdge<TVertex>(current.Source, intermediate));
                        }
                        else
                        {
#if SUPPORTS_CONTRACTS
                            Contract.Assert(false);
#endif
                            path = null;
                            return false;
                        }
                    }
                }
                else
                {
                    // no path found
                    path = null;
                    return false;
                }
            }

#if SUPPORTS_CONTRACTS
            Contract.Assert(todo.Count == 0);
            Contract.Assert(edges.Count > 0);
#endif

            path = edges.ToArray();
            return true;
        }

        protected override void InternalCompute()
        {
            var cancelManager = this.Services.CancelManager;
            // matrix i,j -> path
            this.data.Clear();

            var vertices = this.VisitedGraph.Vertices;
            var edges = this.VisitedGraph.Edges;

            // prepare the matrix with initial costs
            // walk each edge and add entry in cost dictionary
            foreach (var edge in edges)
            {
                var ij = EdgeExtensions.ToVertexPair<TVertex, TEdge>(edge);
                var cost = this.weights(edge);
                VertexData value;
                if (!data.TryGetValue(ij, out value))
                    data[ij] = new VertexData(cost, edge);
                else if (cost < value.Distance)
                    data[ij] = new VertexData(cost, edge);
            }
            if (cancelManager.IsCancelling) return;

            // walk each vertices and make sure cost self-cost 0
            foreach (var v in vertices)
                data[new SEquatableEdge<TVertex>(v, v)] = new VertexData(0, default(TEdge));

            if (cancelManager.IsCancelling) return;

            // iterate k, i, j
            foreach (var vk in vertices)
            {
                if (cancelManager.IsCancelling) return;
                foreach (var vi in vertices)
                {
                    var ik = new SEquatableEdge<TVertex>(vi, vk);
                    VertexData pathik;
                    if(data.TryGetValue(ik, out pathik))
                        foreach (var vj in vertices)
                        {
                            var kj = new SEquatableEdge<TVertex>(vk, vj);

                            VertexData pathkj;
                            if (data.TryGetValue(kj, out pathkj))
                            {
                                double combined = this.distanceRelaxer.Combine(pathik.Distance, pathkj.Distance);
                                var ij = new SEquatableEdge<TVertex>(vi, vj);
                                VertexData pathij;
                                if (data.TryGetValue(ij, out pathij))
                                {
                                    if (this.distanceRelaxer.Compare(combined, pathij.Distance) < 0)
                                        data[ij] = new VertexData(combined, vk);
                                }
                                else
                                    data[ij] = new VertexData(combined, vk);
                            }
                        }
                }
            }

            // check negative cycles
            foreach (var vi in vertices)
            {
                var ii = new SEquatableEdge<TVertex>(vi, vi);
                VertexData value;
                if (data.TryGetValue(ii, out value) &&
                    value.Distance < 0)
                    throw new NegativeCycleGraphException();
            }
        }

        [Conditional("DEBUG")]
        public void Dump(TextWriter writer)
        {
            writer.WriteLine("data:");
            foreach (var kv in this.data)
                writer.WriteLine("{0}->{1}: {2}", 
                    kv.Key.Source, 
                    kv.Key.Target, 
                    kv.Value.ToString());
        }
    }
}