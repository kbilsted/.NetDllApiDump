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

a class like in an assembly `A.dll`

	public class MustFindClass
	{
		public Func<int, int> MustFindField= x => 43;
		public delegate int MustFindDelegate(int i);
		public event  MustFindDelegate MustFindEvent;
		
		public string MustFindMethod1(string x, Class1 c) {
			return null;
		}

		public string MustFindGenericMethod<T>(string x, T c) {
			return null;
		}
	}

	
Results in the output using `DotNetDllApiDump.exe --filter System A.dll`:

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
	F: Func`2[System.Int32,System.Int32] MustFindField

	
Notice the output has been created with the argument `--filter System` to shorten the type names.
	
## 3. Download

Either pull and compile or download from https://github.com/kbilsted/.NetDllApiDump/releases

