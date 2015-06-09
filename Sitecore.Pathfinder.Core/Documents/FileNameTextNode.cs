﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System.Collections.Generic;
using Sitecore.Pathfinder.Diagnostics;

namespace Sitecore.Pathfinder.Documents
{
    public class FileNameTextNode : ITextNode
    {
        public FileNameTextNode([NotNull] string itemName, [NotNull] ISnapshot snapshot)
        {
            Value = itemName;
            Snapshot = snapshot;
        }

        public IEnumerable<ITextNode> Attributes { get; } = new TextNode[0];

        public IEnumerable<ITextNode> ChildNodes { get; } = new TextNode[0];

        public string Name { get; } = string.Empty;

        public ITextNode Parent { get; } = null;

        public TextPosition Position { get; } = TextPosition.Empty;

        public ISnapshot Snapshot { get; }

        public string Value { get; }

        public string GetAttributeValue(string attributeName, string defaultValue = "")
        {
            return string.Empty;
        }

        public ITextNode GetTextNodeAttribute(string attributeName)
        {
            return null;
        }

        public bool SetValue(string value)
        {
            // todo: rename file
            return false;
        }
    }
}
