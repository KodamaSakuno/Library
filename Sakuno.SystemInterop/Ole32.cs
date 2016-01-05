using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    partial class NativeMethods
    {
        public static class Ole32
        {
            const string DllName = "ole32.dll";

            [DllImport(DllName, PreserveSig = false)]
            internal static extern void PropVariantClear([In][Out] NativeStructs.PROPVARIANT pvar);
        }
    }
}
