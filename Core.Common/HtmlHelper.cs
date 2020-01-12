using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common
{
	public static class HtmlHelper
	{
		public static string GetUrlOnly(string url)
			=> url.Split(new[] { "?" }, StringSplitOptions.RemoveEmptyEntries)[0];

		public static int? GetValue(string key, IDictionary<string, string> queryString)
		{
			if (!queryString.ContainsKey(key))
				return null;
			return int.Parse(queryString[key]);
		}

		public static void SetValue(string key, int value, IDictionary<string, string> queryString)
		{
			queryString[key] = value.ToString();
		}

		public static string GetUrlWithQueryString(IDictionary<string, string> queryString)
			=> string.Join("&", queryString.Select(kv => $"{kv.Key}={kv.Value}"));

		public static IDictionary<string, string> GetQueryString(string url)
		{
			if (!url.Contains("?"))
				return new Dictionary<string, string>();
			return url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[1]
				.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(item =>
						{
							var keyValue = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
							return new { Key = keyValue[0], Value = keyValue.Length > 1 ? keyValue[1] : string.Empty };
						}
					)
					.ToDictionary(
						k => k.Key,
						v => v.Value);
		}
	}
}
