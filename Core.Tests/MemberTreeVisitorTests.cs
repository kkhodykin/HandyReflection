using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HandyReflection.Core.Descriptors;
using HandyReflection.Core.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandyReflection.Core.Tests
{
  /// <summary>
  /// Summary description for MemberTreeVisitorTests
  /// </summary>
  [TestClass]
  public class MemberTreeVisitorTests
  {
    [TestMethod]
    public void QueryInstanceBindingFlag()
    {
      
      var initialQuery = Enumerable.Empty<MemberDescriptor>().AsQueryable();
      var query = initialQuery.Where(x => x.Belonging == MemberBelonging.Instance);

      var visitor = new ReflectionVisitor<MemberBaseVisitor>();
      var expression = visitor.Visit(query.Expression);
      Assert.AreEqual(visitor.Filter.BindingFlags, BindingFlags.Instance);

      query =
        initialQuery.Where(
          x =>
            x.MemberTypes == MemberTypes.Method && x.Belonging == MemberBelonging.Instance &&
            x.Visibility == MemberVisibility.NonPublic && x.MemberInfo.Name == "Test");

      visitor = new ReflectionVisitor<MemberBaseVisitor>();
      expression = visitor.Visit(query.Expression);
      Assert.AreEqual(visitor.Filter.BindingFlags, BindingFlags.Instance | BindingFlags.NonPublic);
    }
  }
}
