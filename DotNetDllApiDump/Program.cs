using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetDllApiDump
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(".NetDllApiDump v1.0 by Kasper Graversen");
			if (args.Length == 0)
			{
				Console.WriteLine("Arguments");
				Console.WriteLine("DllApiDumpForILMerge [--filter filterlist] <dll1> [dll2...dlln]");
				Console.WriteLine("");
				Console.WriteLine("--filter with a 'filterlist' which is a comma-separated list of starting names to filter away to create less verbose output. Eg. use 'System' ");
			}

			string[] filters = new string[0];
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].ToLowerInvariant() == "--filter")
				{
					filters = args[++i].Split(',').Select(x => x + ".").ToArray();
					continue;
				}

				var assembly = TryLoadAssembly(args[i]);
				if (assembly != null)
					Console.WriteLine(new Dumper(filters).Dump(assembly));
			}
		}

		private static Assembly TryLoadAssembly(string path)
		{
			var fullPath = Path.GetFullPath(path);

			if (!File.Exists(fullPath))
			{
				Console.WriteLine("Cannot open file '{0}'", fullPath);
				return null;
			}

			return Assembly.UnsafeLoadFrom(fullPath);
		}
	}
}
