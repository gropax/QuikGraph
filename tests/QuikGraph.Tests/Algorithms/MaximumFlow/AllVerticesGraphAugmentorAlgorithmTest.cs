﻿using Microsoft.Pex.Framework;
using NUnit.Framework;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms.MaximumFlow
{
    [TestFixture]
    internal class AllVerticesGraphAugmentorAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void AugmentAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.Augment(g);
        }

        [PexMethod]
        public void Augment(
            IMutableVertexAndEdgeListGraph<string, Edge<string>> g)
        {
            int vertexCount = g.VertexCount;
            int edgeCount = g.EdgeCount;
            int vertexId = g.VertexCount+1;
            int edgeID = g.EdgeCount+1;
            using (var augmentor = new AllVerticesGraphAugmentorAlgorithm<string, Edge<string>>(
                g,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)
                ))
            {
                augmentor.Compute();
                VerifyCount(g, augmentor, vertexCount);
                VerifySourceConnector(g, augmentor);
                VerifySinkConnector(g, augmentor);
            }
            Assert.AreEqual(g.VertexCount, vertexCount);
            Assert.AreEqual(g.EdgeCount, edgeCount);
        }

        private static void VerifyCount<TVertex,TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex,TEdge> g, 
            AllVerticesGraphAugmentorAlgorithm<TVertex,TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(vertexCount + 2, g.VertexCount);
            Assert.IsTrue(g.ContainsVertex(augmentor.SuperSource));
            Assert.IsTrue(g.ContainsVertex(augmentor.SuperSink));
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g, 
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSource))
                    continue;
                if (v.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(g.ContainsEdge(augmentor.SuperSource, v));
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> g, 
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (var v in g.Vertices)
            {
                if (v.Equals(augmentor.SuperSink))
                    continue;
                if (v.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(g.ContainsEdge(v, augmentor.SuperSink));
            }
        }

    }

    public sealed class StringVertexFactory
    {
        private int id = 0;

        public StringVertexFactory()
            : this("Super")
        { }

        public StringVertexFactory(string prefix)
        {
            this.Prefix = prefix;
        }

        public string Prefix { get; set; }

        public string CreateVertex()
        {
            return this.Prefix + (++id).ToString();
        }
    }
}
