namespace Sakuno.SystemInterop
{
    partial class NativeConstants
    {
        public enum FDE_OVERWRITE_RESPONSE
        {
            FDEOR_DEFAULT,
            FDEOR_ACCEPT,
            FDEOR_REFUSE,
        }
        public enum FDE_SHAREVIOLATION_RESPONSE
        {
            FDESVR_DEFAULT,
            FDESVR_ACCEPT,
            FDESVR_REFUSE,
        }
        public enum SIATTRIBFLAGS
        {
            SIATTRIBFLAGS_AND = 1,
            SIATTRIBFLAGS_OR,
            SIATTRIBFLAGS_APPCOMPAT,
            SIATTRIBFLAGS_MASK = 3,
            SIATTRIBFLAGS_ALLITEMS = 0x4000,
        }
    }
}
