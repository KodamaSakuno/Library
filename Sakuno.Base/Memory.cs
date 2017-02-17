using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

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
        public static void Clear(void* rpPointer, int rpCount) => r_MemorySet(rpPointer, 0, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(IntPtr rpPointer, int rpCount) => r_MemorySet((void*)rpPointer, 0, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(byte[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(bool[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(bool));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(char[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(char));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(short[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(short));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(int[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(int));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(uint[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(uint));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(long[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(long));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ulong[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(ulong));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(float[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(float));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(double[] rpArray, int rpCount)
        {
            fixed (void* rPointer = rpArray)
                r_MemorySet(rPointer, 0, rpCount * sizeof(double));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(void* rpSource, void* rpDestination, int rpCount) => r_MemoryCopy(rpDestination, rpSource, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(IntPtr rpSource, IntPtr rpDestination, int rpCount) => r_MemoryCopy((void*)rpDestination, (void*)rpSource, rpCount);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(byte[] rpSource, byte[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(bool[] rpSource, bool[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(bool));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(char[] rpSource, char[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(char));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(short[] rpSource, short[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(short));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(int[] rpSource, int[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(int));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(uint[] rpSource, uint[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(uint));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(long[] rpSource, long[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(long));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(ulong[] rpSource, ulong[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(ulong));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(float[] rpSource, float[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(float));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(double[] rpSource, double[] rpDestination, int rpCount)
        {
            fixed (void* rSource = rpSource)
            fixed (void* rDestination = rpDestination)
                r_MemoryCopy(rDestination, rSource, rpCount * sizeof(double));
        }

        delegate void MemorySet(void* rpAddress, byte rpValue, int rpCount);
        delegate void MemoryCopy(void* rpDestination, void* rpSource, int rpCount);
    }
}
