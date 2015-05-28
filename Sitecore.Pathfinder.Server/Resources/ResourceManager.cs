﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Sitecore.Pathfinder.Resources
{
  using System.Text;
  using Sitecore.IO;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Zip;

  public class ResourceManager
  {
    [NotNull]
    public string BuildResourceFile()
    {
      TempFolder.EnsureFolder();

      var fileName = FileUtil.MapPath(TempFolder.GetFilename("Pathfinder.Resources.zip"));
      using (var zip = new ZipWriter(fileName))
      {
        this.GenerateRenderingXsdSchema(zip, "http://www.sitecore.net/pathfinder/layouts/master", "master");
        this.GenerateRenderingXsdSchema(zip, "http://www.sitecore.net/pathfinder/layouts/core", "core");
      }

      return fileName;
    }

    protected void GenerateRenderingXsdSchema([NotNull] ZipWriter zip, [NotNull] string schemaNamespace, [NotNull] string databaseName)
    {
      var generator = new XsdSchemaGenerator();
      var schema = generator.Generate(schemaNamespace, databaseName);
      zip.AddEntry(".schemas\\layout." + databaseName + ".xsd", Encoding.UTF8.GetBytes(schema));
    }
  }
}
