using System;
using System.IO;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Sakuno.SystemInterop
{
    public static partial class NativeStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int X;
            public int Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width { get { return Right - Left; } }
            public int Height { get { return Bottom - Top; } }

            public RECT(int rpLeft, int rpTop, int rpRight, int rpBottom)
            {
                Left = rpLeft;
                Top = rpTop;
                Right = rpRight;
                Bottom = rpBottom;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;

            public MARGINS(int rpUniformValue)
                : this(rpUniformValue, rpUniformValue, rpUniformValue, rpUniformValue) { }
            public MARGINS(double rpLeft, double rpTop, double rpRight, double rpBottom)
                : this((int)rpLeft, (int)rpTop, (int)rpRight, (int)rpBottom) { }
            public MARGINS(int rpLeft, int rpTop, int rpRight, int rpBottom)
            {
                Left = rpLeft;
                Top = rpTop;
                Right = rpRight;
                Bottom = rpBottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public NativeEnums.SetWindowPosition flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public NativeConstants.ShowCommands showCmd;
            public POINT rpMinPosition;
            public POINT rpMaxPosition;
            public RECT rcNormalPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNET_CACHE_ENTRY_INFO
        {
            public uint dwStructSize;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszSourceUrlName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszLocalFileName;
            public NativeEnums.CACHEENTRYTYPE CacheEntryType;
            public uint dwUseCount;
            public uint dwHitRate;
            public uint dwSizeLow;
            public uint dwSizeHigh;
            public FILETIME LastModifiedTime;
            public FILETIME ExpireTime;
            public FILETIME LastAccessTime;
            public FILETIME LastSyncTime;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpHeaderInfo;
            public uint dwHeaderInfoSize;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszFileExtension;
            public IntPtr dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DVTARGETDEVICE
        {
            public int tdSize;
            public short tdDeviceNameOffset;
            public short tdDriverNameOffset;
            public short tdExtDevmodeOffset;
            public short tdPortNameOffset;
            public byte tdData;
        }

        #region Bitmap
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            public RGBQUAD[] bmiColors;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }
        #endregion

        #region DWM
        [StructLayout(LayoutKind.Sequential)]
        public struct DWM_THUMBNAIL_PROPERTIES
        {
            public NativeEnums.DWM_TNP dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fSourceClientAreaOnly;
        }
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct POWERBROADCAST_SETTING
        {
            public Guid PowerSetting;
            public uint DataLength;
            public int Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WIN32_FIND_DATAW
        {
            public FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            uint dwReserved0;
            uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct PROPERTYKEY
        {
            public Guid FormatID { get; }
            public int PropertyID { get; }

            public PROPERTYKEY(Guid rpFormatID, int rpPropertyID)
            {
                FormatID = rpFormatID;
                PropertyID = rpPropertyID;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public sealed class PROPVARIANT : IDisposable
        {
            [FieldOffset(0)]
            ushort r_ValueType;
            public VarEnum VarType
            {
                get { return (VarEnum)r_ValueType; }
                set { r_ValueType = (ushort)value; }
            }

            public bool IsNullOrEmpty => r_ValueType == (ushort)VarEnum.VT_EMPTY || r_ValueType == (ushort)VarEnum.VT_NULL;

            [FieldOffset(8)]
            IntPtr r_Pointer;

            public string StringValue => Marshal.PtrToStringUni(r_Pointer);

            public PROPVARIANT() { }
            public PROPVARIANT(string rpValue)
            {
                if (rpValue == null)
                    throw new ArgumentNullException(nameof(rpValue));

                VarType = VarEnum.VT_LPWSTR;
                r_Pointer = Marshal.StringToCoTaskMemUni(rpValue);
            }

            ~PROPVARIANT()
            {
                Dispose();
            }

            public void Dispose()
            {
                NativeMethods.Ole32.PropVariantClear(this);
                GC.SuppressFinalize(this);
            }
        }

    }
}
