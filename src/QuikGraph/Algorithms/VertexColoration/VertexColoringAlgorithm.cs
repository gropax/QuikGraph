﻿using System.Collections.Generic;
using System.Linq;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.VertexColoring
{
    /// <summary>
    /// Algorithm that walk through a graph and color vertices with the minimum number of colors possible.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class VertexColoringAlgorithm<TVertex, TEdge> : AlgorithmBase<UndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexColoringAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public VertexColoringAlgorithm([NotNull] UndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        /// <summary>
        /// Vertices colors.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TVertex, int?> Colors { get; } = new Dictionary<TVertex, int?>();

        /// <summary>
        /// Fired when a vertex is colored.
        /// </summary>
        public event VertexAction<TVertex> VertexColored;

        private void OnVertexColored([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            VertexColored?.Invoke(vertex);
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            Colors.Clear();
            
            // Initialize remaining vertices as unassigned
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                Colors[vertex] = null; // No color is assigned to vertex
            }

            int vertexCount = VisitedGraph.VertexCount;
            TVertex firstVertex = VisitedGraph.Vertices.First();

            // Assign the first color to first vertex
            Colors[firstVertex] = 0;
            OnVertexColored(firstVertex);

            // A temporary array to store the available colors. True
            // value of available[usedColor] would mean that the color usedColor is
            // assigned to one of its adjacent vertices
            bool[] available = new bool[vertexCount];
            for (int usingColor = 0; usingColor < vertexCount; ++usingColor)
            {
                available[usingColor] = false;
            }

            // Assign colors to remaining vertexCount-1 vertices
            foreach (TVertex vertex in VisitedGraph.Vertices.Skip(1))
            {
                // Process all adjacent vertices and flag their colors as unavailable
                foreach (TEdge adjacentEdges in VisitedGraph.AdjacentEdges(vertex))
                {
                    TVertex adjacentVertex = adjacentEdges.GetOtherVertex(vertex);
                    if (Colors[adjacentVertex].HasValue)
                    {
                        available[Colors[adjacentVertex].Value] = true;
                    }
                }

                // Find the first available color
                int usingColor;
                for (usingColor = 0; usingColor < vertexCount; usingColor++)
                {
                    if (!available[usingColor])
                        break;
                }

                // Assign the found color
                Colors[vertex] = usingColor;
                OnVertexColored(vertex);

                // Reset the values back to false for the next iteration
                foreach (TEdge adjacentEdges in VisitedGraph.AdjacentEdges(vertex))
                {
                    if (Colors[adjacentEdges.GetOtherVertex(vertex)].HasValue)
                    {
                        // ReSharper disable once PossibleInvalidOperationException, Justification: Was assigned a color just before
                        var usedColor = Colors[adjacentEdges.GetOtherVertex(vertex)].Value;
                        available[usedColor] = false;
                    }
                }
            }
        }

        #endregion
    }
}