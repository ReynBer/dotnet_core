using System.Linq;

namespace Core.Common
{
    public static class UriCombinerHelper
	{
		private static string CleanIfNecessaryEnd(string path)
			=> path.TrimEnd('/');

		private static string CleanIfNecessaryStart(string path)
			=> path.TrimStart('/');

		public static string Combine(string baseUri, string path)
			=> $"{CleanIfNecessaryEnd(baseUri)}/{CleanIfNecessaryStart(path)}";

		public static string Combine(string baseUri, params string[] paths)
			=> paths.Aggregate(baseUri, (prev, current) => Combine(prev, current));
	}
}
