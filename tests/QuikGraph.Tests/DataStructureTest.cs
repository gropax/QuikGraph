﻿using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuickGraph.Tests
{
    [TestFixture]
    internal class DataStructureTest : QuikGraphUnitTests
    {
        [Test]
        public void DisplayLinkedList()
        {
            var target = new LinkedList<int>();
            target.AddFirst(0);
            target.AddFirst(1);
        }
    }
}
