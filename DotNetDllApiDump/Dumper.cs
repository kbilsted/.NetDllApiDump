using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetDllApiDump
{
	public class Dumper
	{
		private readonly string[] filters;

		public Dumper(string[] filters)
		{
			this.filters = filters ?? new string[0];
		}

		public string Dump(Assembly assembly)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("Dumping '{0}'\n", assembly.Location);
			foreach (var type in assembly.GetExportedTypes())
			{
				DumpType(sb, type);
			}

			return sb.ToString();
		}

		public string Dump(Type type)
		{
			var sb = new StringBuilder();
			DumpType(sb, type);
			return sb.ToString();
		}

		private void DumpType(StringBuilder sb, Type type)
		{
			sb.AppendFormat("\n*{0}*\n", type.FullName);
			var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			foreach (var member in members)
				DumpType(sb, member, filters);
		}

		string Smallify(object type)
		{
			return Smallify(type.ToString());
		}

		string Smallify(string typename)
		{
			var first = filters.FirstOrDefault(typename.StartsWith);
			return typename.Substring((first ?? "").Length);
		}

		string Params(ParameterInfo[] parameters)
		{
			return string.Join(", ", parameters.Select(Smallify));
		}

		void DumpType(StringBuilder sb, MemberInfo member, string[] filters)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Constructor:
					var ctor = (ConstructorInfo)member;
					if (ctor.GetParameters().All(x => x.ParameterType.IsPublic))
					{
						sb.AppendFormat("C: {0}({1})\n", ctor.Name, Params(ctor.GetParameters()));
					}
					break;

				case MemberTypes.Event:
					var ei = (EventInfo)member;
					PrintFunc(sb, ei.Name, ei.EventHandlerType.GetMethod("Invoke"), "E");
					break;

				case MemberTypes.Field:
					var fi = (FieldInfo)member;
					sb.AppendFormat("F: {0} {1}\n", Smallify(fi.FieldType), fi.Name);
					break;

				case MemberTypes.Method:
					PrintFunc(sb, member, "M");
					break;

				case MemberTypes.Property:
					var pi = (PropertyInfo)member;
					if (pi.PropertyType.IsPublic && pi.GetAccessors(false).Any())
					{
						sb.AppendFormat("P: {0} {1}\n", Smallify(pi.PropertyType), pi.Name);
					}
					break;

				case MemberTypes.NestedType:
					// delegates and inner types - are also handled in the other cases
					break;

				case MemberTypes.TypeInfo:
				case MemberTypes.Custom:
				case MemberTypes.All:
					throw new Exception("Not understood - please raise an issue on github for: " + member.MemberType);

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void PrintFunc(StringBuilder sb, MemberInfo member, string kind)
		{
			var mi = (MethodInfo) member;
			PrintFunc(sb, mi, kind, mi.Name);
		}

		void PrintFunc(StringBuilder sb, string name, MethodInfo member, string kind)
		{
			PrintFunc(sb, member, kind, name);
		}

		void PrintFunc(StringBuilder sb, MethodInfo mi, string kind, string name)
		{
			if (mi.ReturnType.IsPublic && mi.GetParameters().All(x => x.ParameterType.IsPublic))
			{
				sb.AppendFormat("{0}: {1} {2}({3})\n", kind, Smallify(mi.ReturnType), name, Params(mi.GetParameters()));
			}
		}
	}
}
