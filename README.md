# .NetDllApiDump
Dump all public api from a DLL to ensure ILMerge.exe does not internalize unintensionally

## 1. How to use
Before and after employing `ILMerge.exe` on your DLL, record the collective public API. Compare the outputs to see if any types or methods have been unintentionally internalized

A suggested work flow

  * `DotNetDllApiDump.exe MyDll > MyDll.before.ilmerge.txt`
  * Fiddle with `ILMerge.exe`
  * `DotNetDllApiDump.exe MyDll > MyDll.after.ilmerge.txt`
  * Compare in a diff tool (eg `kdiff3`)
  

## 2. Output

a class like

	public class MustFindClass
	{
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

	
results in the output
	

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

	
Notice the output has been created with the argument `--filter System` to shorten the type names.
	
	
