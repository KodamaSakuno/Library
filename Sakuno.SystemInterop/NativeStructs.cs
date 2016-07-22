using System;
using System.IO;
using System.Net;
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
            public NativeConstants.ShowCommand showCmd;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_BATTERY_STATE
        {
            [MarshalAs(UnmanagedType.I1)]
            public bool AcOnLine;
            [MarshalAs(UnmanagedType.I1)]
            public bool BatteryPresent;
            [MarshalAs(UnmanagedType.I1)]
            public bool Charging;
            [MarshalAs(UnmanagedType.I1)]
            public bool Discharging;

            public byte Spare1;
            public byte Spare2;
            public byte Spare3;
            public byte Spare4;

            public uint MaxCapacity;
            public uint RemainingCapacity;
            public uint Rate;
            public uint EstimatedTime;
            public uint DefaultAlert1;
            public uint DefaultAlert2;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public int cbSize;
            public IntPtr hwnd;
            public NativeEnums.FLASHW dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct COMDLG_FILTERSPEC
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszSpec;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TASKDIALOGCONFIG
        {
            public int cbSize;
            public IntPtr hwndParent;
            public IntPtr hInstance;
            public NativeEnums.TASKDIALOG_FLAGS dwFlags;
            public TaskDialogCommonButtons dwCommonButtons;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszWindowTitle;
            public TaskDialogIcon hMainIcon;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszMainInstruction;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszContent;
            public int cButtons;
            public IntPtr pButtons;
            public int nDefaultButton;
            public int cRadioButtons;
            public IntPtr pRadioButtons;
            public int nDefaultRadioButton;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszVerificationText;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszExpandedInformation;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszExpandedControlText;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszCollapsedControlText;
            public TaskDialogIcon hFooterIcon;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszFooter;
            public NativeDelegates.TaskDialogCallbackProc pfCallback;
            public IntPtr lpCallbackData;
            public uint cxWidth;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TASKDIALOG_BUTTON
        {
            public int nButtonID;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszButtonText;

            public TASKDIALOG_BUTTON(int rpID, string rpText)
            {
                nButtonID = rpID;
                pszButtonText = rpText;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPTABLE
        {
            public uint dwNumEntries;
            public MIB_TCPROW_OWNER_PID table;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public NativeConstants.MIB_TCP_STATE state;
            public int dwLocalAddr;
            public int dwLocalPort;
            public int dwRemoteAddr;
            public int dwRemotePort;
            public int dwOwningPid;

            public IPAddress LocalAddress => new IPAddress(dwLocalAddr);
            public int LocalPort => ((dwLocalPort & 0xFF) << 8 & 0xFF00) + ((dwLocalPort & 0xFF00) >> 8);

            public IPAddress RemoteAddress => new IPAddress(dwRemoteAddr);
            public int RemotePort => ((dwRemotePort & 0xFF) << 8 & 0xFF00) + ((dwRemotePort & 0xFF00) >> 8);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public FileAttributes dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public NativeConstants.PROCESSOR_ARCHITECTURE wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public NativeEnums.PAGE AllocationProtect;
            public int RegionSize;
            public NativeEnums.MEM State;
            public NativeEnums.PAGE Protect;
            public NativeEnums.MEM Type;
        }
    }
}
