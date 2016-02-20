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
            try
            {
                var jsonArray = JArray.Parse(_jsonText);

                return GetTableFromJsonArray(jsonArray);
            }
            catch
            {
                var singleObject = JObject.Parse(_jsonText);

                var jsonArray = new JArray(singleObject);

                return GetTableFromJsonArray(jsonArray);
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
                    var value = property.Value.ToString();

                    row.AddCell(column, new Cell(value));
                }

                rows.Add(row);
            }

            return new Table(columns.Values.ToList(), rows);

        }
    }
}