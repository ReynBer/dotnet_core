using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Common
{
	public static class CsvHelper
	{
		public static IEnumerable<string[]> ReadFrom(string file, string separator)
		{
			var Separators = new[] { separator };
			string line;
			using (var reader = File.OpenText(file))
				while ((line = reader.ReadLine()) != null)
					yield return line.Split(Separators, StringSplitOptions.None);
		}

		public static IEnumerable<string[]> ReadRows(IEnumerable<string> rows, string separator)
		{
			var Separators = new[] { separator };
			foreach (var row in rows)
				yield return row.Split(Separators, StringSplitOptions.None);
		}

		public static IEnumerable<IDictionary<string, string>> CreateValues(this IEnumerable<string[]> rowsWithHeader)
			=> CreateValues(rowsWithHeader.First(), rowsWithHeader.Skip(1));

		public static IEnumerable<IDictionary<string, string>> CreateValues(string[] header, IEnumerable<string[]> rows)
		{
			foreach (var row in rows)
			{
				var input = new Dictionary<string, string>(header.Length);
				for (int i = 0; i < row.Length; i++)
					input[header[i]] = row[i];
				yield return input;
			}
		}
	}
}
