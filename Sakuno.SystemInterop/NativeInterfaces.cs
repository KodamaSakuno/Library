using System;
using System.Runtime.InteropServices;

namespace Sakuno.SystemInterop
{
    public static partial class NativeInterfaces
    {
        [ComImport]
        [Guid("00000000-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IUnknown
        {
            [PreserveSig]
            IntPtr QueryInterface(ref Guid riid, ref IntPtr pVoid);
            [PreserveSig]
            ulong AddRef();
            [PreserveSig]
            ulong Release();
        }

        [ComImport]
        [Guid("0000010d-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IViewObject
        {
            [PreserveSig]
            int Draw([MarshalAs(UnmanagedType.U4)] int dwDrawAspect, int lindex, IntPtr pvAspect, ref NativeStructs.DVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, ref NativeStructs.RECT lprcBounds, ref NativeStructs.RECT lprcWBounds, IntPtr pfnContinue, IntPtr dwContinue);
        }
        [ComImport]
        [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IServiceProvider
        {
            [PreserveSig]
            int QueryService(ref Guid guidService, ref Guid riid, [Out][MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        }

        [ComImport]
        [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyStore
        {
            [PreserveSig]
            int GetCount([Out] out uint cProps);
            [PreserveSig]
            int GetAt([In] uint iProp, out NativeStructs.PROPERTYKEY pkey);
            [PreserveSig]
            int GetValue([In] ref NativeStructs.PROPERTYKEY key, [Out] NativeStructs.PROPVARIANT propvar);
            [PreserveSig]
            int SetValue([In] ref NativeStructs.PROPERTYKEY key, [In] NativeStructs.PROPVARIANT propvar);
            [PreserveSig]
            int Commit();
        }

        [ComImport]
        [Guid("6F79D558-3E96-4549-A1D1-7D75D2288814")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyDescription
        {
            [PreserveSig]
            int GetPropertyKey([Out] out NativeStructs.PROPERTYKEY pkey);
            [PreserveSig]
            int GetCanonicalName([Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            [PreserveSig]
            int GetPropertyType([Out] out VarEnum pvartype);
            [PreserveSig]
            int GetDisplayName([Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            [PreserveSig]
            int GetEditInvitation([Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszInvite);
            [PreserveSig]
            int GetTypeFlags([In] NativeEnums.PROPDESC_TYPE_FLAGS mask, [Out] out NativeEnums.PROPDESC_TYPE_FLAGS ppdtFlags);
            [PreserveSig]
            int GetViewFlags([Out] out NativeEnums.PROPDESC_VIEW_FLAGS ppdvFlags);
            [PreserveSig]
            int GetDefaultColumnWidth([Out] out uint pcxChars);
            [PreserveSig]
            int GetDisplayType([Out] out NativeConstants.PROPDESC_DISPLAYTYPE pdisplaytype);
            [PreserveSig]
            int GetColumnState([Out] out NativeEnums.SHCOLSTATE pcsFlags);
            [PreserveSig]
            int GetGroupingRange([Out] out NativeConstants.PROPDESC_GROUPING_RANGE pgr);
            [PreserveSig]
            int GetRelativeDescriptionType([Out] out NativeConstants.PROPDESC_RELATIVEDESCRIPTION_TYPE prdt);
            [PreserveSig]
            int GetRelativeDescription([In] NativeStructs.PROPVARIANT propvar1, [In] NativeStructs.PROPVARIANT propvar2, [Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1, [Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);
            [PreserveSig]
            int GetSortDescription([Out] out NativeConstants.PROPDESC_SORTDESCRIPTION psd);
            [PreserveSig]
            int GetSortDescriptionLabel([In] bool fDescending, out IntPtr ppszDescription);
            [PreserveSig]
            int GetAggregationType([Out] out NativeConstants.PROPDESC_AGGREGATION_TYPE paggtype);
            [PreserveSig]
            int GetConditionType([Out] out NativeConstants.PROPDESC_CONDITION_TYPE pcontype, [Out] out NativeConstants.CONDITION_OPERATION popDefault);
            [PreserveSig]
            int GetEnumTypeList([In] ref Guid riid, [Out][MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);
            [PreserveSig]
            int CoerceToCanonicalValue([In][Out] NativeStructs.PROPVARIANT propvar);
            [PreserveSig]
            int FormatForDisplay([In] NativeStructs.PROPVARIANT propvar, [In] NativeEnums.PROPDESC_FORMAT_FLAGS pdfFlags, [Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
            [PreserveSig]
            int IsValueCanonical([In] NativeStructs.PROPVARIANT propvar);
        }

        [ComImport]
        [Guid("1F9FC1D0-C39B-4B26-817F-011967D3440E")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyDescriptionList
        {
            [PreserveSig]
            int GetCount([Out] out uint pcElem);
            [PreserveSig]
            int GetAt([In] uint iElem, [In] ref Guid riid, [Out][MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
        }

        [ComImport]
        [Guid("11E1FBF9-2D56-4A6B-8DB3-7CD193A471F2")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyEnumType
        {
            [PreserveSig]
            int GetEnumType([Out] out NativeConstants.PROPENUMTYPE penumtype);
            [PreserveSig]
            int GetValue([Out] NativeStructs.PROPVARIANT ppropvar);
            [PreserveSig]
            int GetRangeMinValue([Out] NativeStructs.PROPVARIANT ppropvar);
            [PreserveSig]
            int GetRangeSetValue([Out] NativeStructs.PROPVARIANT ppropvar);
            [PreserveSig]
            int GetDisplayText([Out][MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
        }

        [ComImport]
        [Guid("A99400F4-3D84-4557-94BA-1242FB2CC9A6")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyEnumTypeList
        {
            [PreserveSig]
            int GetCount([Out] out uint pctypes);
            [PreserveSig]
            int GetAt([In] uint itype, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);
            [PreserveSig]
            int GetConditionAt([In] uint nIndex, [In] ref Guid riid, [Out][MarshalAs(UnmanagedType.Interface)] out IntPtr ppv);
            [PreserveSig]
            int FindMatchingIndex([In] NativeStructs.PROPVARIANT propvarCmp, [Out] out uint pnIndex);
        }

        [ComImport]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumIDList
        {
            [PreserveSig]
            int Next(uint celt, [Out] out IntPtr rgelt, out uint pceltFetched);
            [PreserveSig]
            int Skip([In] uint celt);
            [PreserveSig]
            int Reset();
            [PreserveSig]
            int Clone([Out][MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
        }
    }
}
