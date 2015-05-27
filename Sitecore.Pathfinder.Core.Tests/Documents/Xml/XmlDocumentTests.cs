﻿namespace Sitecore.Pathfinder.Documents.Xml
{
  using NUnit.Framework;
  using Sitecore.Pathfinder.Documents;

  [TestFixture]
  public class XmlDocumentTests : Tests
  {
    [TestFixtureSetUp]
    public void Setup()
    {
      this.Start();
    }

    [Test]
    public void InvalidXmlTests()
    {
      var sourceFile = new SourceFile(this.Services.FileSystem, "test.txt");

      var doc = new XmlTextSnapshot(sourceFile, "<Item>");
      Assert.AreEqual(TextNode.Empty, doc.Root);

      doc = new XmlTextSnapshot(sourceFile, string.Empty);
      Assert.AreEqual(TextNode.Empty, doc.Root);
    }

    [Test]
    public void ItemTests()
    {
      var sourceFile = new SourceFile(this.Services.FileSystem, "test.txt");

      var doc = new XmlTextSnapshot(sourceFile, "<Item><Field Name=\"Text\" Value=\"123\" /></Item>");
      var root = doc.Root;
      Assert.IsNotNull(root);
      Assert.AreEqual("Item", root.Name);
      Assert.AreEqual(1, root.ChildNodes.Count);

      var field = root.ChildNodes[0];
      Assert.AreEqual("Field", field.Name);
      Assert.AreEqual("Text", field.GetAttributeValue("Name"));
      Assert.AreEqual("123", field.GetAttributeValue("Value"));
      Assert.AreEqual(0, field.ChildNodes.Count);
    }

    [Test]
    public void ValueTests()
    {
      var sourceFile = new SourceFile(this.Services.FileSystem, "test.txt");

      var doc = new XmlTextSnapshot(sourceFile, "<Item><Field Name=\"Text\">123</Field></Item>");
      var root = doc.Root;
      var field = root.ChildNodes[0];
      Assert.AreEqual("Field", field.Name);
      Assert.AreEqual("Text", field.GetAttributeValue("Name"));
      Assert.AreEqual("123", field.GetAttributeValue("[Value]"));
      Assert.AreEqual(string.Empty, field.Value);
      Assert.AreEqual(0, field.ChildNodes.Count);
    }
  }
}