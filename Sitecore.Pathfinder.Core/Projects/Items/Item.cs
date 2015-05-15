namespace Sitecore.Pathfinder.Projects.Items
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.CompilerServices;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.TextDocuments;

  public class Item : ItemBase
  {
    public Item([NotNull] IProject project, [NotNull] string projectUniqueId, [NotNull] ITextNode textNode) : base(project, projectUniqueId, textNode)
    {
    }

    [NotNull]
    public IList<Field> Fields { get; } = new List<Field>();

    [IndexerName("Field")]
    public string this[string fieldName]
    {
      get
      {
        var field = this.Fields.FirstOrDefault(f => string.Compare(f.Name, fieldName, StringComparison.OrdinalIgnoreCase) == 0);
        return field?.Value ?? string.Empty;
      }

      set
      {
        var field = this.Fields.FirstOrDefault(f => string.Compare(f.Name, fieldName, StringComparison.OrdinalIgnoreCase) == 0);
        if (field == null)
        {
          field = new Field(this.TextNode)
          {
            Name = fieldName
          };

          this.Fields.Add(field);
        }

        field.Value = value;
      }
    }

    public override void Bind()
    {
      base.Bind();

      foreach (var field in this.Fields)
      {
        // todo: use regular expression to detect paths, guids, piped guids - possibly an field handler for Link, Images, Rich Text fields
        if (field.Value.StartsWith("/sitecore", StringComparison.OrdinalIgnoreCase))
        {
          this.References.AddFieldReference(field.Value);
        }
      }
    }

    public void Merge([NotNull] Item newItem)
    {
      // todo: throw exception if item and newItem value differ
      if (!string.IsNullOrEmpty(newItem.ItemName))
      {
        this.ItemName = newItem.ItemName;
      }

      if (!string.IsNullOrEmpty(newItem.DatabaseName))
      {
        this.DatabaseName = newItem.DatabaseName;
      }

      if (!string.IsNullOrEmpty(newItem.TemplateIdOrPath))
      {
        this.TemplateIdOrPath = newItem.TemplateIdOrPath;
      }

      if (!string.IsNullOrEmpty(newItem.Icon))
      {
        this.Icon = newItem.Icon;
      }

      if (!newItem.IsEmittable)
      {
        this.IsEmittable = false;
      }

      // todo: add TextNode
      // todo: add SourceFile
      foreach (var newField in newItem.Fields)
      {
        var field = this.Fields.FirstOrDefault(f => string.Compare(f.Name, newField.Name, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(f.Language, newField.Language, StringComparison.OrdinalIgnoreCase) == 0 && f.Version == newField.Version);
        if (field == null)
        {
          this.Fields.Add(newField);
          continue;
        }

        // todo: throw exception if item and newItem value differ
        field.Value = newField.Value;
      }
    }
  }
}