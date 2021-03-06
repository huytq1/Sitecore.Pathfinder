﻿// © 2015-2017 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Extensions;

namespace Sitecore.Pathfinder.Configuration.ConfigurationModel.Json
{
    public class JsonConfigurationSource : ConfigurationSource
    {
        public JsonConfigurationSource([NotNull] string path) : this(path, false)
        {
        }

        public JsonConfigurationSource([NotNull] string path, bool optional)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(Json.Resources.Error_InvalidFilePath, nameof(path));
            }

            Optional = optional;
            Path = JsonPathResolver.ResolveAppRelativePath(path);
        }

        public bool Optional { get; }

        [NotNull]
        public string Path { get; }

        public override void Load()
        {
            if (!File.Exists(Path))
            {
                if (!Optional)
                {
                    throw new FileNotFoundException(string.Format(Json.Resources.Error_FileNotFound, Path), Path);
                }

                Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                using (var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    Load(fileStream);
                }
            }
        }

        internal void Load([NotNull] Stream stream)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (var jsonTextReader = new JsonTextReader(new StreamReader(stream)))
            {
                var num = 0;
                jsonTextReader.DateParseHandling = DateParseHandling.None;
                jsonTextReader.Read();
                SkipComments(jsonTextReader);
                if (jsonTextReader.TokenType != JsonToken.StartObject)
                {
                    throw new FormatException(Json.Resources.FormatError_RootMustBeAnObject(jsonTextReader.Path, jsonTextReader.LineNumber, jsonTextReader.LinePosition));
                }

                do
                {
                    SkipComments(jsonTextReader);
                    switch (jsonTextReader.TokenType)
                    {
                        case JsonToken.None:
                            throw new FormatException(Json.Resources.FormatError_UnexpectedEnd(jsonTextReader.Path, jsonTextReader.LineNumber, jsonTextReader.LinePosition));
                        case JsonToken.StartArray:
                            var arrayKey = GetKey(jsonTextReader.Path);
                            if (dictionary.ContainsKey(arrayKey))
                            {
                                throw new FormatException(Json.Resources.FormatError_KeyIsDuplicated(arrayKey));
                            }

                            var arrayValues = new List<string>();
                            while(jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndArray)
                            {
                                SkipComments(jsonTextReader);
                                switch (jsonTextReader.TokenType)
                                {
                                    case JsonToken.Raw:
                                    case JsonToken.Integer:
                                    case JsonToken.Float:
                                    case JsonToken.String:
                                    case JsonToken.Boolean:
                                    case JsonToken.Null:
                                    case JsonToken.Bytes:
                                        arrayValues.Add(jsonTextReader.Value.ToString().Escape(','));
                                        break;
                                    case JsonToken.EndArray:
                                        break;
                                    default:
                                        throw new FormatException(Json.Resources.FormatError_UnsupportedJSONToken(jsonTextReader.TokenType, jsonTextReader.Path, jsonTextReader.LineNumber, jsonTextReader.LinePosition));
                                }
                            }

                            dictionary[arrayKey] = string.Join(",", arrayValues);
                            goto case JsonToken.PropertyName;
                        case JsonToken.StartObject:
                            ++num;
                            goto case JsonToken.PropertyName;
                        case JsonToken.PropertyName:
                            jsonTextReader.Read();
                            continue;

                        case JsonToken.Raw:
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.String:
                        case JsonToken.Boolean:
                        case JsonToken.Null:
                        case JsonToken.Bytes:
                            var propertyKey = GetKey(jsonTextReader.Path);
                            if (dictionary.ContainsKey(propertyKey))
                            {
                                throw new FormatException(Json.Resources.FormatError_KeyIsDuplicated(propertyKey));
                            }

                            dictionary[propertyKey] = jsonTextReader.Value.ToString();
                            goto case JsonToken.PropertyName;
                        case JsonToken.EndObject:
                            --num;
                            goto case JsonToken.PropertyName;
                        default:
                            throw new FormatException(Json.Resources.FormatError_UnsupportedJSONToken(jsonTextReader.TokenType, jsonTextReader.Path, jsonTextReader.LineNumber, jsonTextReader.LinePosition));
                    }
                }
                while (num > 0);
            }

            Data = dictionary;
        }

        [NotNull]
        private string GetKey([NotNull] string jsonPath)
        {
            if (jsonPath.IndexOf("['", StringComparison.Ordinal) < 0)
            {
                return jsonPath.Replace('.', ':');
            }

            var stringList = new List<string>();

            int n;
            for (var startIndex1 = 0; startIndex1 < jsonPath.Length; startIndex1 = n + 2)
            {
                var startIndex2 = jsonPath.IndexOf("['", startIndex1, StringComparison.Ordinal);
                if (startIndex2 < 0)
                {
                    stringList.Add(jsonPath.Substring(startIndex1));
                    break;
                }

                if (startIndex2 > startIndex1)
                {
                    stringList.Add(jsonPath.Substring(startIndex1, startIndex2 - startIndex1));
                }

                n = jsonPath.IndexOf("']", startIndex2, StringComparison.Ordinal);
                stringList.Add(jsonPath.Substring(startIndex2 + 2, n - startIndex2 - 2));
            }

            return string.Join(":", stringList);
        }

        private void SkipComments([NotNull] JsonReader reader)
        {
            while (reader.TokenType == JsonToken.Comment)
            {
                reader.Read();
            }
        }
    }
}
