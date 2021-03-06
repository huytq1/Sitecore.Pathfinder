﻿// © 2015-2017 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Linq;
using System.Web;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace Sitecore.Pathfinder.shell.pathfinder
{
    public class ExportDatabase : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var exports = (context.Request.QueryString["r"] ?? "master").ToLowerInvariant();
            string databaseName;

            switch (exports)
            {
                case "master":
                    databaseName = "master";
                    break;
                case "core":
                case "speak1":
                case "speak2":
                    databaseName = "core";
                    break;
                default:
                    throw new InvalidOperationException("Unknown export - use ?r=master|core|speak1|speak2");
            } 

            using (new SecurityDisabler())
            {
                var database = Factory.GetDatabase(databaseName);

                using (var output = new XmlTextWriter(context.Response.Output))
                {
                    output.Formatting = Formatting.Indented;

                    output.WriteStartElement("Exports");
                    output.WriteAttributeString("Database", databaseName);

                    switch (exports)
                    {
                        case "master":
                            WriteItem(output, database, "/sitecore");
                            WriteItem(output, database, "/sitecore/content");
                            WriteItem(output, database, "/sitecore/content/Home");
                            WriteItems(output, database, "/sitecore/layout", "/sitecore/layout/Simulators");
                            WriteItem(output, database, "/sitecore/media library");
                            WriteItem(output, database, "/sitecore/system");
                            WriteItems(output, database, "/sitecore/system/languages");
                            WriteItems(output, database, "/sitecore/system/workflows");
                            WriteItems(output, database, "/sitecore/templates");
                            break;

                        case "core":
                            WriteItem(output, database, "/sitecore");
                            WriteItem(output, database, "/sitecore/client");
                            WriteItem(output, database, "/sitecore/client/Applications");
                            WriteItem(output, database, "/sitecore/layout");
                            WriteItem(output, database, "/sitecore/layout/Controllers");
                            WriteItems(output, database, "/sitecore/layout/devices");
                            WriteItem(output, database, "/sitecore/layout/Layouts");
                            WriteItem(output, database, "/sitecore/layout/Models");
                            WriteItem(output, database, "/sitecore/layout/Renderings");
                            WriteItem(output, database, "/sitecore/layout/Sublayouts");
                            WriteItem(output, database, "/sitecore/media library");
                            WriteItem(output, database, "/sitecore/system");
                            WriteItems(output, database, "/sitecore/system/field types");
                            WriteItems(output, database, "/sitecore/system/languages");
                            WriteItems(output, database, "/sitecore/templates");
                            break;

                        case "speak1":
                            WriteItems(output, database, "/sitecore/client/Speak");
                            WriteItem(output, database, "/sitecore/client/Business Component Library");
                            WriteItems(output, database, "/sitecore/client/Business Component Library/version 1", "/sitecore/client/Business Component Library/version 1/Content");
                            WriteItem(output, database, "/sitecore/client/Applications/Launchpad");
                            WriteItem(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Buttons");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Tiles");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Renderings");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Templates");
                            break;

                        case "speak2":
                            WriteItems(output, database, "/sitecore/client/Speak");
                            WriteItem(output, database, "/sitecore/client/Business Component Library");
                            WriteItems(output, database, "/sitecore/client/Business Component Library/version 2", "/sitecore/client/Business Component Library/version 2/Content");
                            WriteItem(output, database, "/sitecore/client/Applications/Launchpad");
                            WriteItem(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Buttons");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Tiles");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Renderings");
                            WriteItems(output, database, "/sitecore/client/Applications/Launchpad/PagesSettings/Templates");
                            break;
                    }

                    output.WriteEndElement();
                }
            }
        }

        private void WriteItem(XmlTextWriter output, Database database, string itemPath)
        {
            var item = database.GetItem(itemPath);
            if (item == null)
            {
                return;
            }

            WriteItem(output, item);
        }

        private void WriteItem(XmlTextWriter output, Item item)
        {
            if (item.TemplateID == TemplateIDs.Template)
            {
                output.WriteStartElement("Template");
                output.WriteAttributeString("Id", item.ID.ToString());
                output.WriteAttributeString("Path", item.Paths.Path);

                var baseTemplates = string.Join("|", item[FieldIDs.BaseTemplate]);
                if (!string.IsNullOrEmpty(baseTemplates))
                {
                    output.WriteAttributeString("BaseTemplates", baseTemplates);
                }

                foreach (Item templateSection in item.Children.OfType<Item>().Where(c => c.TemplateID == TemplateIDs.TemplateSection))
                {
                    output.WriteStartElement("Section");
                    output.WriteAttributeString("Id", templateSection.ID.ToString());
                    output.WriteAttributeString("Name", templateSection.Name);

                    foreach (Item field in templateSection.Children.OfType<Item>().Where(c => c.TemplateID == TemplateIDs.TemplateField))
                    {
                        output.WriteStartElement("Field");
                        output.WriteAttributeString("Id", field.ID.ToString());
                        output.WriteAttributeString("Name", field.Name);

                        if (!string.Equals(field["Type"], "text", StringComparison.OrdinalIgnoreCase) && !string.Equals(field["Type"], "Single-Line Text", StringComparison.OrdinalIgnoreCase))
                        {
                            output.WriteAttributeString("Type", field["Type"]);
                        }

                        if (field["Shared"] != "1")
                        {
                            output.WriteAttributeString("Sharing", field["Unversioned"] == "1" ? "Unversioned" : "Versioned");
                        }

                        output.WriteEndElement();
                    }

                    output.WriteEndElement();
                }

                output.WriteEndElement();
            }
            else
            {
                output.WriteStartElement("Item");
                output.WriteAttributeString("Id", item.ID.ToString());
                output.WriteAttributeString("Template", item.Template.InnerItem.ID.ToString());
                output.WriteAttributeString("Path", item.Paths.Path);
                output.WriteEndElement();
            }
        }

        private void WriteItems(XmlTextWriter output, Database database, string itemPath, params string[] exclude)
        {
            var item = database.GetItem(itemPath);
            if (item == null)
            {
                return;
            }

            WriteItems(output, item, exclude);
        }

        private void WriteItems(XmlTextWriter output, Item item, string[] exclude)
        {
            WriteItem(output, item);

            foreach (Item child in item.GetChildren())
            {
                if (exclude.Contains(child.Paths.Path))
                {
                    continue; 
                }

                if (child.TemplateID != TemplateIDs.TemplateSection)
                {
                    WriteItems(output, child, exclude);
                }
            }
        }
    }
}
