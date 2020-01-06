// ***********************************************************************
// Assembly         : ETL
// Author           : Mariusz
// Created          : 01-03-2020
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="ExportCSV.cs" company="">
//     Copyright ©  2019 Mariusz Owczarski
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;

namespace ETL.Helpers
{
    /// <summary>
    /// Class ExportCSV. This class cannot be inherited.
    /// </summary>
    public sealed class ExportCSV
    {
        // JEST TO KLASA UMOZLIWIAJCA SERIALIZACJE OBIEKTU DO POSTACI PLIKA .SVG

        /// <summary>
        /// Class CSVExport.
        /// </summary>
        private static class CSVExport
        {
            /// <summary>
            /// Froms the date.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>String.</returns>
            public static String FromDate(DateTime value) => value.TimeOfDay.TotalSeconds == 0 ? value.ToString("yyyy-MM-dd") : value.ToString("yyyy-MM-dd HH:mm:ss");
            /// <summary>
            /// Froms the nullable date.
            /// </summary>
            /// <param name="cell">The cell.</param>
            /// <returns>String.</returns>
            public static String FromNullableDate(DateTime? cell) => cell.HasValue ? FromDate(cell.Value) : String.Empty;
            /// <summary>
            /// Froms the string.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>String.</returns>
            public static String FromString(String value)
            {
                var comma = value.Contains(",");
                var quote = value.Contains("\"");

                return (comma || quote) ?
                    QuoteString(quote ? EscapeQuotes(value) : value) : value;
            }
            /// <summary>
            /// Froms the specified object.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="obj">The object.</param>
            /// <returns>String.</returns>
            public static String From<T>(T obj) => FromString(Convert.ToString(obj, CultureInfo.InvariantCulture));
            /// <summary>
            /// Quotes the string.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>String.</returns>
            private static String QuoteString(String value) =>  '"' + value + '"';
            /// <summary>
            /// Escapes the quotes.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>String.</returns>
            private static String EscapeQuotes(String value) => value.Replace("\"", "\"\"");
        }

        /// <summary>
        /// Class SerializerCSV.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class SerializerCSV<T>
        {
            /// <summary>
            /// Gets or sets the property value converters.
            /// </summary>
            /// <value>The property value converters.</value>
            private IEnumerable<Tuple<PropertyInfo, Func<Object, String>>> PropertyValueConverters { get; set; }
            /// <summary>
            /// Initializes a new instance of the <see cref="SerializerCSV{T}"/> class.
            /// </summary>
            public SerializerCSV()
            {
                this.PropertyValueConverters = typeof(T).GetProperties().Select(x =>
                {
                    if (x.PropertyType == typeof(DateTime))
                        return Tuple.Create(x, (Func<Object, String>)(o => CSVExport.FromDate((DateTime)o)));
                    else if (x.PropertyType == typeof(DateTime?))
                        return Tuple.Create(x, (Func<Object, String>)(o => CSVExport.FromNullableDate((DateTime?)o)));
                    else
                        return Tuple.Create(x, (Func<Object, String>)CSVExport.From);
                }).ToList();
            }
            /// <summary>
            /// Extracts the property values.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>IEnumerable&lt;String&gt;.</returns>
            private IEnumerable<String> ExtractPropertyValues(T obj) => this.PropertyValueConverters.Select(x => x.Item2(x.Item1.GetValue(obj, null)));
            /// <summary>
            /// Extracts the headers.
            /// </summary>
            /// <returns>IEnumerable&lt;String&gt;.</returns>
            private IEnumerable<String> ExtractHeaders() => this.PropertyValueConverters.Select(x => x.Item1.Name);
            /// <summary>
            /// Converts to csvrow.
            /// </summary>
            /// <param name="values">The values.</param>
            /// <returns>String.</returns>
            private String ToCsvRow(IEnumerable<String> values)=> String.Join(",", values);

            /// <summary>
            /// Serializes the specified entries.
            /// </summary>
            /// <param name="entries">The entries.</param>
            /// <returns>String.</returns>
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

        /// <summary>
        /// Serializes the specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns>String.</returns>
        public static String Serialize<T>(IEnumerable<T> values, string movieId = null)
        {
            if (values == null) return String.Empty;

            var serializer = new SerializerCSV<T>();
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            if (movieId != null)
                System.IO.File.WriteAllText(path + $@"\MovieId-{movieId}.csv", serializer.Serialize(values));
            else
                System.IO.File.WriteAllText(path + $@"\db.csv", serializer.Serialize(values));

            return serializer.Serialize(values);
        }
    }
}
