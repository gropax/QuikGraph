﻿using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace QuickGraph.Collections
{
    /// <summary>This class contains parameterized unit tests for SoftHeap`2</summary>
    [TestFixture]
    [PexClass(typeof(SoftHeap<,>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SoftHeapTKeyTValueTest
    {
        [PexMethod(MaxBranches = 160000)]
        public void Add([PexAssumeNotNull]int[] keys)
        {
            PexAssume.TrueForAll(keys, k => k < int.MaxValue);
            PexAssume.IsTrue(keys.Length > 0);

            var target = new SoftHeap<int, int>(1/4.0, int.MaxValue);
            Console.WriteLine("expected error rate: {0}", target.ErrorRate);
            foreach (var key in keys)
            {
                var count = target.Count;
                target.Add(key, key + 1);
                Assert.AreEqual(count + 1, target.Count);
            }

            int lastMin = int.MaxValue;
            int error = 0;
            while (target.Count > 0)
            {
                var kv = target.DeleteMin();
                if (lastMin < kv.Key)
                    error++;
                lastMin = kv.Key;
                Assert.AreEqual(kv.Key + 1, kv.Value);
            }

            Console.WriteLine("error rate: {0}", error / (double)keys.Length);
            Assert.IsTrue(error / (double)keys.Length <= target.ErrorRate);
        }
    }
}
