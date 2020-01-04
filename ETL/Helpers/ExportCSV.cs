using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;

namespace ETL.Helpers
{
    public sealed class ExportCSV
    {
        // JEST TO KLASA UMOZLIWIAJCA SERIALIZACJE OBIEKTU DO POSTACI PLIKA .SVG
        private static class CsvValue
        {
            public static String FromDate(DateTime value)
            {
                return value.TimeOfDay.TotalSeconds == 0 ?
                    value.ToString("yyyy-MM-dd") : value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            public static String FromNullableDate(DateTime? cell)
            {
                return cell.HasValue ? FromDate(cell.Value) : String.Empty;
            }

            public static String FromString(String value)
            {
                var comma = value.Contains(",");
                var quote = value.Contains("\"");

                return (comma || quote) ?
                    QuoteString(quote ? EscapeQuotes(value) : value) : value;
            }

            public static String From<T>(T obj)
            {
                return FromString(Convert.ToString(obj, CultureInfo.InvariantCulture));
            }

            private static String QuoteString(String value)
            {
                return '"' + value + '"';
            }

            private static String EscapeQuotes(String value)
            {
                return value.Replace("\"", "\"\"");
            }
        }

        private class CsvSerializer<T>
        {
            private IEnumerable<Tuple<PropertyInfo, Func<Object, String>>> PropertyValueConverters { get; set; }

            public CsvSerializer()
            {
                this.PropertyValueConverters = typeof(T).GetProperties().Select(x =>
                {
                    if (x.PropertyType == typeof(DateTime))
                        return Tuple.Create(x, (Func<Object, String>)(o => CsvValue.FromDate((DateTime)o)));
                    else if (x.PropertyType == typeof(DateTime?))
                        return Tuple.Create(x, (Func<Object, String>)(o => CsvValue.FromNullableDate((DateTime?)o)));
                    else
                        return Tuple.Create(x, (Func<Object, String>)CsvValue.From);
                }).ToList();
            }

            private IEnumerable<String> ExtractPropertyValues(T obj)
            {
                return this.PropertyValueConverters.Select(x => x.Item2(x.Item1.GetValue(obj, null)));
            }

            private IEnumerable<String> ExtractHeaders()
            {
                return this.PropertyValueConverters.Select(x => x.Item1.Name);
            }

            private String ToCsvRow(IEnumerable<String> values)
            {
                return String.Join(",", values);
            }

            public String Serialize(IEnumerable<T> entries)
            {
                using (var stream = new StringWriter() { NewLine = "\n" })
                {
                    stream.WriteLine(ToCsvRow(ExtractHeaders()));
                    foreach (var entry in entries)
                        stream.WriteLine(ToCsvRow(ExtractPropertyValues(entry)));
                    return stream.ToString();
                }
            }
        }

        public static String Serialize<T>(IEnumerable<T> values, string movieId = null)
        {
            if (values == null) return String.Empty;

            var serializer = new CsvSerializer<T>();
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            if (movieId != null)
                System.IO.File.WriteAllText(path + $@"\MovieId-{movieId}.csv", serializer.Serialize(values));
            else
                System.IO.File.WriteAllText(path + $@"\db.csv", serializer.Serialize(values));

            return serializer.Serialize(values);
        }
    }
}
