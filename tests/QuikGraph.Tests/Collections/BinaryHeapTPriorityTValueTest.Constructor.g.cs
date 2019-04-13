// <copyright file="BinaryHeapTPriorityTValueTest.Constructor.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using Microsoft.Pex.Engine.Exceptions;
using Microsoft.Pex.Framework.Generated;
using NUnit.Framework;

namespace QuickGraph.Collections
{
    internal partial class BinaryHeapTPriorityTValueTest
    {
//[Test]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException338()
{
    try
    {
      this.Constructor<int, Edge<int>>(int.MinValue);
      Assert.Fail("Must throw contract exception.");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[Test]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
public void Constructor703()
{
    this.Constructor<int, Edge<int>>(0);
}
[Test]
[Ignore("Was already ignored")]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException308()
{
    try
    {
      this.Constructor<int, SEdge<int>>(int.MinValue);
      Assert.Fail("Must throw contract exception.");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[Test]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
public void Constructor70302()
{
    this.Constructor<int, SEdge<int>>(0);
}
[Test]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
public void Constructor70301()
{
    this.Constructor<int, int>(0);
}
[Test]
[Ignore("Was already ignored")]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException735()
{
    try
    {
      this.Constructor<int, int>(int.MinValue);
      Assert.Fail("Must throw contract exception.");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
    }
}
