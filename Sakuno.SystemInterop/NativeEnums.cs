using System;

namespace Sakuno.SystemInterop
{
    public static partial class NativeEnums
    {
        [Flags]
        public enum WindowStyle : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_CAPTION = 0x00C00000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        }

        [Flags]
        public enum ExtendedWindowStyle
        {
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,

            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,

            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
            WS_EX_LAYERED = 0x00080000,
            WS_EX_NOINHERITLAYOUT = 0x00100000,
            WS_EX_LAYOUTRTL = 0x00400000,
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_NOACTIVATE = 0x08000000,
        }

        [Flags]
        public enum SetWindowPosition
        {
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOREDRAW = 0x0008,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020,
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,

            SWP_DRAWFRAME = SWP_FRAMECHANGED,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,

            SWP_DEFERERASE = 0x2000,
            SWP_ASYNCWINDOWPOS = 0x4000,

            SWP_NOSIZEORMOVE = SWP_NOSIZE | SWP_NOMOVE,
        }

        [Flags]
        public enum CACHEENTRYTYPE
        {
            NORMAL_CACHE_ENTRY = 1,
            STICKY_CACHE_ENTRY = 4,
            EDITED_CACHE_ENTRY = 8,
            TRACK_OFFLINE_CACHE_ENTRY = 0x10,
            TRACK_ONLINE_CACHE_ENTRY = 0x20,
            SPARSE_CACHE_ENTRY = 0x10000,
            COOKIE_CACHE_ENTRY = 0x100000,
            URLHISTORY_CACHE_ENTRY = 0x200000
        }

        [Flags]
        public enum DWM_TNP
        {
            DWM_TNP_RECTDESTINATION = 1,
            DWM_TNP_RECTSOURCE = 2,
            DWM_TNP_OPACITY = 4,
            DWM_TNP_VISIBLE = 8,
            DWM_TNP_SOURCECLIENTAREAONLY = 16,
        }

        [Flags]
        public enum SLGP
        {
            SLGP_SHORTPATH = 1,
            SLGP_UNCPRIORITY = 2,
            SLGP_RAWPATH = 4,
            SLGP_RELATIVEPRIORITY = 8,
        }

        [Flags]
        public enum FLASHW
        {
            FLASHW_STOP,
            FLASHW_CAPTION = 1,
            FLASHW_TRAY = 2,
            FLASHW_ALL = 3,
            FLASHW_TIMER = 4,
            FLASHW_TIMERNOFG = 12,
        }

        [Flags]
        public enum SND
        {
            SND_SYNC,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_MEMORY = 0x0004,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,

            SND_NOWAIT = 0x00002000,
            SND_ALIAS = 0x00010000,
            SND_ALIAS_ID = 0x00110000,
            SND_FILENAME = 0x00020000,
            SND_RESOURCE = 0x00040004,

            SND_PURGE = 0x0040,
            SND_APPLICATION = 0x0080,

            SND_SENTRY = 0X00080000,
            SND_RING = 0X00100000,
            SND_SYSTEM = 0X00200000,
        }

        [Flags]
        public enum MF
        {
            MF_BYCOMMAND,
            MF_ENABLED = 0,
            MF_GRAYED,
            MF_DISABLED = 0x0002,
            MF_BYPOSITION = 0x0400,
        }

        [Flags]
        public enum AR_STATE
        {
            AR_ENABLED,
            AR_DISABLED = 0x01,
            AR_SUPPRESSED = 0x02,
            AR_REMOTESESSION = 0x04,
            AR_MULTIMON = 0x08,
            AR_NOSENSOR = 0x10,
            AR_NOT_SUPPORTED = 0x20,
            AR_DOCKED = 0x40,
            AR_LAPTOP = 0x80,
        }

        public enum FILEOPENDIALOGOPTIONS : uint
        {
            FOS_OVERWRITEPROMPT = 0x2,
            FOS_STRICTFILETYPES = 0x4,
            FOS_NOCHANGEDIR = 0x8,
            FOS_PICKFOLDERS = 0x20,
            FOS_FORCEFILESYSTEM = 0x40,
            FOS_ALLNONSTORAGEITEMS = 0x80,
            FOS_NOVALIDATE = 0x100,
            FOS_ALLOWMULTISELECT = 0x200,
            FOS_PATHMUSTEXIST = 0x800,
            FOS_FILEMUSTEXIST = 0x1000,
            FOS_CREATEPROMPT = 0x2000,
            FOS_SHAREAWARE = 0x4000,
            FOS_NOREADONLYRETURN = 0x8000,
            FOS_NOTESTFILECREATE = 0x10000,
            FOS_HIDEMRUPLACES = 0x20000,
            FOS_HIDEPINNEDPLACES = 0x40000,
            FOS_NODEREFERENCELINKS = 0x100000,
            FOS_DONTADDTORECENT = 0x2000000,
            FOS_FORCESHOWHIDDEN = 0x10000000,
            FOS_DEFAULTNOMINIMODE = 0x20000000,
            FOS_FORCEPREVIEWPANEON = 0x40000000,
            FOS_SUPPORTSTREAMABLEITEMS = 0x80000000,
        }

        [Flags]
        public enum PROPDESC_TYPE_FLAGS : uint
        {
            PDTF_DEFAULT,
            PDTF_MULTIPLEVALUES = 0x1,
            PDTF_ISINNATE = 0x2,
            PDTF_ISGROUP = 0x4,
            PDTF_CANGROUPBY = 0x8,
            PDTF_CANSTACKBY = 0x10,
            PDTF_ISTREEPROPERTY = 0x20,
            PDTF_INCLUDEINFULLTEXTQUERY = 0x40,
            PDTF_ISVIEWABLE = 0x80,
            PDTF_ISQUERYABLE = 0x100,
            PDTF_CANBEPURGED = 0x200,
            PDTF_SEARCHRAWVALUE = 0x400,
            PDTF_ISSYSTEMPROPERTY = 0x80000000,
            PDTF_MASK_ALL = 0x800007FF,
        }
        [Flags]
        public enum PROPDESC_VIEW_FLAGS
        {
            PDVF_DEFAULT,
            PDVF_CENTERALIGN = 0x1,
            PDVF_RIGHTALIGN = 0x2,
            PDVF_BEGINNEWGROUP = 0x4,
            PDVF_FILLAREA = 0x8,
            PDVF_SORTDESCENDING = 0x10,
            PDVF_SHOWONLYIFPRESENT = 0x20,
            PDVF_SHOWBYDEFAULT = 0x40,
            PDVF_SHOWINPRIMARYLIST = 0x80,
            PDVF_SHOWINSECONDARYLIST = 0x100,
            PDVF_HIDELABEL = 0x200,
            PDVF_HIDDEN = 0x800,
            PDVF_CANWRAP = 0x1000,
            PDVF_MASK_ALL = 0x1BFF,
        }
        [Flags]
        public enum PROPDESC_FORMAT_FLAGS
        {
            PDFF_DEFAULT,
            PDFF_PREFIXNAME = 0x1,
            PDFF_FILENAME = 0x2,
            PDFF_ALWAYSKB = 0x4,
            PDFF_RESERVED_RIGHTTOLEFT = 0x8,
            PDFF_SHORTTIME = 0x10,
            PDFF_LONGTIME = 0x20,
            PDFF_HIDETIME = 0x40,
            PDFF_SHORTDATE = 0x80,
            PDFF_LONGDATE = 0x100,
            PDFF_HIDEDATE = 0x200,
            PDFF_RELATIVEDATE = 0x400,
            PDFF_USEEDITINVITATION = 0x800,
            PDFF_READONLY = 0x1000,
            PDFF_NOAUTOREADINGORDER = 0x2000,
        }
    }
}
