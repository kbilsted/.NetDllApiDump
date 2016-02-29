using System;
using System.Reflection;
using DotNetDllApiDump;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
    public class Class1
    {
		[Test]
		public void TestDumpType()
		{
			var dumper = new Dumper(new[] { "System." });

			var dump = dumper.Dump(typeof(MustFindClass));

			Assert.True(dump.Replace("\r", "").Contains(@"
*Tests.MustFindClass*
M: String MustFindMethod1(String x, Tests.Class1 c)
M: String MustFindGenericMethod(String x, T c)
M: String ToString()
M: Boolean Equals(Object obj)
M: Boolean Equals(Object objA, Object objB)
M: Boolean ReferenceEquals(Object objA, Object objB)
M: Int32 GetHashCode()
M: Type GetType()
C: .ctor()
E: Int32 MustFindEvent(Int32 i)
"));
		}


		[Test]
		public void TestDumpAssembly()
		{
			var dumper = new Dumper(new[] { "System." });

			var dump = dumper.Dump(Assembly.GetExecutingAssembly());

			Assert.True(dump.Replace("\r", "").Contains(@"
*Tests.MustFindClass+MustFindInnerClass*
M: String MustFindMethod2(String x, Tests.Class1 c)
M: String ToString()
M: Boolean Equals(Object obj)
M: Boolean Equals(Object objA, Object objB)
M: Boolean ReferenceEquals(Object objA, Object objB)
M: Int32 GetHashCode()
M: Type GetType()
C: .ctor()

*Tests.MustFindClass+MustFindDelegate*
M: Int32 Invoke(Int32 i)
M: IAsyncResult BeginInvoke(Int32 i, AsyncCallback callback, Object object)
M: Int32 EndInvoke(IAsyncResult result)
M: Void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context)
M: Boolean Equals(Object obj)
M: Delegate[] GetInvocationList()
M: Boolean op_Equality(MulticastDelegate d1, MulticastDelegate d2)
M: Boolean op_Inequality(MulticastDelegate d1, MulticastDelegate d2)
M: Int32 GetHashCode()
M: Object DynamicInvoke(Object[] args)
M: Delegate Combine(Delegate a, Delegate b)
M: Delegate Combine(Delegate[] delegates)
M: Reflection.MethodInfo get_Method()
M: Object get_Target()
M: Delegate Remove(Delegate source, Delegate value)
M: Delegate RemoveAll(Delegate source, Delegate value)
M: Object Clone()
M: Delegate CreateDelegate(Type type, Object target, String method)
M: Delegate CreateDelegate(Type type, Object target, String method, Boolean ignoreCase)
M: Delegate CreateDelegate(Type type, Object target, String method, Boolean ignoreCase, Boolean throwOnBindFailure)
M: Delegate CreateDelegate(Type type, Type target, String method)
M: Delegate CreateDelegate(Type type, Type target, String method, Boolean ignoreCase)
M: Delegate CreateDelegate(Type type, Type target, String method, Boolean ignoreCase, Boolean throwOnBindFailure)
M: Delegate CreateDelegate(Type type, Reflection.MethodInfo method, Boolean throwOnBindFailure)
M: Delegate CreateDelegate(Type type, Object firstArgument, Reflection.MethodInfo method)
M: Delegate CreateDelegate(Type type, Object firstArgument, Reflection.MethodInfo method, Boolean throwOnBindFailure)
M: Boolean op_Equality(Delegate d1, Delegate d2)
M: Boolean op_Inequality(Delegate d1, Delegate d2)
M: Delegate CreateDelegate(Type type, Reflection.MethodInfo method)
M: String ToString()
M: Boolean Equals(Object objA, Object objB)
M: Boolean ReferenceEquals(Object objA, Object objB)
M: Type GetType()
C: .ctor(Object object, IntPtr method)
P: Reflection.MethodInfo Method
P: Object Target
"));
		}
	}


	public class MustFindClass
	{
		public class MustFindInnerClass
		{
			public string MustFindMethod2(string x, Class1 c)
			{
				return null;
			}
		}

		public delegate int MustFindDelegate(int i);

		public event  MustFindDelegate MustFindEvent;

		public string MustFindMethod1(string x, Class1 c)
		{
			return null;
		}

		public string MustFindGenericMethod<T>(string x, T c)
		{
			return null;
		}
	}
}
