using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using QuickGraph.Tests;
using QuikGraph.Tests;

namespace QuickGraph
{

    public class TestVertexType
    {
        public TestVertexType(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }

    /// <summary>This class contains parameterized unit tests for GraphExtensions</summary>
    [TestFixture]
    [PexClass(typeof(GraphExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    internal class GraphExtensionsTest : QuikGraphUnitTests
    {
        [Test]
        public void DictionaryToVertexAndEdgeListGraph()
        {
            var dic = new Dictionary<int, int[]>();
            dic.Add(0, new int[] { 1, 2 });
            dic.Add(1, new int[] { 2 });
            dic.Add(2, new int[] { });

            var graph = dic.ToVertexAndEdgeListGraph(
                kv => Array.ConvertAll(kv.Value, v => new SEquatableEdge<int>(kv.Key, v))
             );

            foreach (var kv in dic)
            {
                Assert.IsTrue(graph.ContainsVertex(kv.Key));
                foreach (var i in kv.Value)
                {
                    Assert.IsTrue(graph.ContainsVertex(i));
                    Assert.IsTrue(graph.ContainsEdge(kv.Key, i));
                }
            }
        }

        [Test]
        public void CollectionOfEdgesToAdjacencyGraph()
        {
            int numVertices = 4;
            var vertices = new Collection<TestVertexType>();
            for (int i = 0; i < numVertices; i++)
            {
                vertices.Add(new TestVertexType(i.ToString()));
            }

            var edges = TestHelper.CreateAllPairwiseEdges(vertices, vertices, 
                (source, target) => new Edge<TestVertexType>(source, target));

            var adjGraph = edges.ToAdjacencyGraph<TestVertexType, Edge<TestVertexType>>();

            //Verify that the right number of vertices were created
            Assert.AreEqual(numVertices, adjGraph.Vertices.ToList().Count);

        }

        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!1&gt;, Boolean)</summary>
        [PexMethod]
        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph<TVertex, TEdge>(IEnumerable<TEdge> edges, bool allowParallelEdges)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph(IEnumerable`1<!!1>, Boolean)
            AdjacencyGraph<TVertex, TEdge> result
               = GraphExtensions.ToAdjacencyGraph<TVertex, TEdge>(edges, allowParallelEdges);
            return result;
        }

        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!1&gt;)</summary>
        [PexMethod]
        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph01<TVertex, TEdge>(IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph01(IEnumerable`1<!!1>)
            AdjacencyGraph<TVertex, TEdge> result
               = GraphExtensions.ToAdjacencyGraph<TVertex, TEdge>(edges);
            return result;
        }

        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;, Boolean)</summary>
        [PexMethod]
        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph02<TVertex, TEdge>(
            IEnumerable<TVertex> vertices,
            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
            bool allowParallelEdges
        )
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph02(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>, Boolean)
            AdjacencyGraph<TVertex, TEdge> result
               = GraphExtensions.ToAdjacencyGraph<TVertex, TEdge>
                  (vertices, outEdgesFactory, allowParallelEdges);
            return result;
        }

        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;)</summary>
        [PexMethod]
        public AdjacencyGraph<TVertex, TEdge> ToAdjacencyGraph03<TVertex, TEdge>(
            IEnumerable<TVertex> vertices,
            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory
        )
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph03(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>)
            AdjacencyGraph<TVertex, TEdge> result
               = GraphExtensions.ToAdjacencyGraph<TVertex, TEdge>(vertices, outEdgesFactory);
            return result;
        }

        /// <summary>Test stub for ToAdjacencyGraph(IEnumerable`1&lt;SEquatableEdge`1&lt;!!0&gt;&gt;)</summary>
        [PexMethod]
        [PexGenericArguments(typeof(int))]
        public AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> ToAdjacencyGraph04<TVertex>(IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            // TODO: add assertions to method GraphExtensionsTest.ToAdjacencyGraph04(IEnumerable`1<SEquatableEdge`1<!!0>>)
            AdjacencyGraph<TVertex, SEquatableEdge<TVertex>> result
               = GraphExtensions.ToAdjacencyGraph<TVertex>(vertexPairs);
            return result;
        }

        /// <summary>Test stub for ToBidirectionalGraph(IVertexAndEdgeListGraph`2&lt;!!0,!!1&gt;)</summary>
        [PexMethod]
        public IBidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph(IVertexAndEdgeListGraph`2<!!0,!!1>)
            IBidirectionalGraph<TVertex, TEdge> result
               = GraphExtensions.ToBidirectionalGraph<TVertex, TEdge>(graph);
            return result;
        }

        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!1&gt;, Boolean)</summary>
        [PexMethod]
        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph01<TVertex, TEdge>(IEnumerable<TEdge> edges, bool allowParallelEdges)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph01(IEnumerable`1<!!1>, Boolean)
            BidirectionalGraph<TVertex, TEdge> result
               = GraphExtensions.ToBidirectionalGraph<TVertex, TEdge>
                  (edges, allowParallelEdges);
            return result;
        }

        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!1&gt;)</summary>
        [PexMethod]
        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph02<TVertex, TEdge>(IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph02(IEnumerable`1<!!1>)
            BidirectionalGraph<TVertex, TEdge> result
               = GraphExtensions.ToBidirectionalGraph<TVertex, TEdge>(edges);
            return result;
        }

        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;, Boolean)</summary>
        [PexMethod]
        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph03<TVertex, TEdge>(
            IEnumerable<TVertex> vertices,
            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory,
            bool allowParallelEdges
        )
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph03(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>, Boolean)
            BidirectionalGraph<TVertex, TEdge> result
               = GraphExtensions.ToBidirectionalGraph<TVertex, TEdge>
                  (vertices, outEdgesFactory, allowParallelEdges);
            return result;
        }

        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;!!0&gt;, Func`2&lt;!!0,IEnumerable`1&lt;!!1&gt;&gt;)</summary>
        [PexMethod]
        public BidirectionalGraph<TVertex, TEdge> ToBidirectionalGraph04<TVertex, TEdge>(
            IEnumerable<TVertex> vertices,
            Func<TVertex, IEnumerable<TEdge>> outEdgesFactory
        )
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph04(IEnumerable`1<!!0>, Func`2<!!0,IEnumerable`1<!!1>>)
            BidirectionalGraph<TVertex, TEdge> result
               = GraphExtensions.ToBidirectionalGraph<TVertex, TEdge>
                  (vertices, outEdgesFactory);
            return result;
        }

        /// <summary>Test stub for ToBidirectionalGraph(IEnumerable`1&lt;SEquatableEdge`1&lt;!!0&gt;&gt;)</summary>
        [PexMethod]
        [PexGenericArguments(typeof(int))]
        public BidirectionalGraph<TVertex, SEquatableEdge<TVertex>> ToBidirectionalGraph05<TVertex>(IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            // TODO: add assertions to method GraphExtensionsTest.ToBidirectionalGraph05(IEnumerable`1<SEquatableEdge`1<!!0>>)
            BidirectionalGraph<TVertex, SEquatableEdge<TVertex>> result
               = GraphExtensions.ToBidirectionalGraph<TVertex>(vertexPairs);
            return result;
        }

        /// <summary>Test stub for ToUndirectedGraph(IEnumerable`1&lt;!!1&gt;)</summary>
        [PexMethod]
        public UndirectedGraph<TVertex, TEdge> ToUndirectedGraph<TVertex, TEdge>(IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToUndirectedGraph(IEnumerable`1<!!1>)
            UndirectedGraph<TVertex, TEdge> result
               = GraphExtensions.ToUndirectedGraph<TVertex, TEdge>(edges);
            return result;
        }

        /// <summary>Test stub for ToUndirectedGraph(IEnumerable`1&lt;!!1&gt;, Boolean)</summary>
        [PexMethod]
        public UndirectedGraph<TVertex, TEdge> ToUndirectedGraph01<TVertex, TEdge>(IEnumerable<TEdge> edges, bool allowParralelEdges)
            where TEdge : IEdge<TVertex>
        {
            // TODO: add assertions to method GraphExtensionsTest.ToUndirectedGraph01(IEnumerable`1<!!1>, Boolean)
            UndirectedGraph<TVertex, TEdge> result =
              GraphExtensions.ToUndirectedGraph<TVertex, TEdge>(edges, allowParralelEdges);
            return result;
        }

        /// <summary>Test stub for ToUndirectedGraph(IEnumerable`1&lt;SEquatableEdge`1&lt;!!0&gt;&gt;)</summary>
        [PexMethod]
        [PexGenericArguments(typeof(int))]
        public UndirectedGraph<TVertex, SEquatableEdge<TVertex>> ToUndirectedGraph02<TVertex>(IEnumerable<SEquatableEdge<TVertex>> vertexPairs)
        {
            // TODO: add assertions to method GraphExtensionsTest.ToUndirectedGraph02(IEnumerable`1<SEquatableEdge`1<!!0>>)
            UndirectedGraph<TVertex, SEquatableEdge<TVertex>> result
               = GraphExtensions.ToUndirectedGraph<TVertex>(vertexPairs);
            return result;
        }
    }
}
