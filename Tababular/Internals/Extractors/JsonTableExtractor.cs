using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tababular.Internals.Extensions;
using Tababular.Internals.TableModel;

namespace Tababular.Internals.Extractors
{
    class JsonTableExtractor : ITableExtractor
    {
        readonly string _jsonText;

        public JsonTableExtractor(string jsonText)
        {
            _jsonText = jsonText;
        }

        public Table GetTable()
        {
            var table = ParseArrayOrNull()
                        ?? ParseJsonlOrNull()
                        ?? ParseObjectOrNull();

            if (table == null)
            {
                throw new FormatException($"Could not interpret this as JSON: '{_jsonText}'");
            }

            return table;
        }

        Table ParseJsonlOrNull()
        {
            try
            {
                var lines = _jsonText
                    .Split(new[] {Environment.NewLine, "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim())
                    .ToList();

                var looksLikeJsonLines = lines
                    .All(line => line.StartsWith("{") && line.EndsWith("}"));

                if (!looksLikeJsonLines)
                {
                    return null;
                }

                var jsonObjects = lines.Select(JObject.Parse);

                var jsonArray = new JArray(jsonObjects);

                return GetTableFromJsonArray(jsonArray);
            }
            catch
            {
                return null;
            }
        }

        Table ParseObjectOrNull()
        {
            try
            {
                var singleObject = JObject.Parse(_jsonText);

                var jsonArray = new JArray(singleObject);

                return GetTableFromJsonArray(jsonArray);
            }
            catch
            {
                return null;
            }
        }

        Table ParseArrayOrNull()
        {
            try
            {
                var jsonArray = JArray.Parse(_jsonText);

                return GetTableFromJsonArray(jsonArray);
            }
            catch
            {
                return null;
            }
        }

        static Table GetTableFromJsonArray(JArray jsonArray)
        {
            var columns = new Dictionary<string, Column>();
            var rows = new List<Row>();

            foreach (var jsonToken in jsonArray)
            {
                var row = new Row();

                var jsonObject = jsonToken.Value<JObject>();

                foreach (var property in jsonObject.Properties())
                {
                    var name = property.Name;
                    var column = columns.GetOrAdd(name, _ => new Column(name));

                    var value = property.Value;

                    var stringValue = value.Type == JTokenType.Array 
                        ? GetAsLines(value) 
                        : value.ToString();

                    row.AddCell(column, new Cell(stringValue));
                }

                rows.Add(row);
            }

            return new Table(columns.Values.ToList(), rows);

        }

        static string GetAsLines(JToken value)
        {
            var lines = value.Values<string>();

            return string.Join(Environment.NewLine, lines);
        }
    }
}