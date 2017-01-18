using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sakuno
{
    public static unsafe class Memory
    {
        static MemorySet r_MemorySet;
        static MemoryCopy r_MemoryCopy;

        static Memory()
        {
            EmitMemorySet();
            EmitMemoryCopy();
        }
        static void EmitMemorySet()
        {
            var rMethod = new DynamicMethod(nameof(MemorySet), MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(void), new[] { typeof(void*), typeof(byte), typeof(int) }, typeof(Memory), true);

            var rGenerator = rMethod.GetILGenerator();
            rGenerator.Emit(OpCodes.Ldarg_0);
            rGenerator.Emit(OpCodes.Ldarg_1);
            rGenerator.Emit(OpCodes.Ldarg_2);
            rGenerator.Emit(OpCodes.Initblk);
            rGenerator.Emit(OpCodes.Ret);

            r_MemorySet = (MemorySet)rMethod.CreateDelegate(typeof(MemorySet));
        }
        static void EmitMemoryCopy()
        {
            var rMethod = new DynamicMethod(nameof(MemoryCopy), MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(void), new[] { typeof(void*), typeof(void*), typeof(int) }, typeof(Memory), true);

            var rGenerator = rMethod.GetILGenerator();
            rGenerator.Emit(OpCodes.Ldarg_0);
            rGenerator.Emit(OpCodes.Ldarg_1);
            rGenerator.Emit(OpCodes.Ldarg_2);
            rGenerator.Emit(OpCodes.Cpblk);
            rGenerator.Emit(OpCodes.Ret);

            r_MemoryCopy = (MemoryCopy)rMethod.CreateDelegate(typeof(MemoryCopy));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(void* rpBuffer, int rpCount) => r_MemorySet(rpBuffer, 0, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(IntPtr rpBuffer, int rpCount) => r_MemorySet((void*)rpBuffer, 0, rpCount);
        public static void Clear(byte[] rpBuffer, int rpCount)
        {
            var rHandle = GCHandle.Alloc(rpBuffer, GCHandleType.Pinned);
            try
            {
                Clear(rHandle.AddrOfPinnedObject(), rpCount);
            }
            finally
            {
                rHandle.Free();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(void* rpSource, void* rpDestination, int rpCount) => r_MemoryCopy(rpDestination, rpSource, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(IntPtr rpSource, IntPtr rpDestination, int rpCount) => r_MemoryCopy((void*)rpDestination, (void*)rpSource, rpCount);
        public static void Copy(byte[] rpSource, byte[] rpDestination, int rpCount)
        {
            var rSourceHandle = GCHandle.Alloc(rpSource, GCHandleType.Pinned);
            var rDestinationHandle = GCHandle.Alloc(rpDestination, GCHandleType.Pinned);
            try
            {
                Copy(rSourceHandle.AddrOfPinnedObject(), rDestinationHandle.AddrOfPinnedObject(), rpCount);
            }
            finally
            {
                rSourceHandle.Free();
                rDestinationHandle.Free();
            }
        }

        delegate void MemorySet(void* rpAddress, byte rpValue, int rpCount);
        delegate void MemoryCopy(void* rpDestination, void* rpSource, int rpCount);
    }
}
