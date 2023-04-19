using System;
using System.Runtime.InteropServices;

namespace EAPI
{
    public class MethodClass
    {
        public const string EAPIdll = "EAPI.dll";
        public const string AuxIOdll = "AuxIO.dll";

        internal class NativeMethods
        {
            // Check EAPI.dll exists or not
            // Reference: https://stackoverflow.com/questions/2292578/check-if-a-dll-is-present-in-the-system
            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport(EAPIdll, EntryPoint = "EApiLibInitialize", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiLibInitialize();

            [DllImport(EAPIdll, EntryPoint = "EApiLibUnInitialize", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiLibUnInitialize();

            #region General

            [DllImport(EAPIdll, EntryPoint = "EApiBoardGetStringA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiBoardGetStringA(uint Id, [Out] char[] pBuffer, ref uint pBufferLen);

            [DllImport(EAPIdll, EntryPoint = "EApiBoardGetValue", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiBoardGetValue(uint Id, ref uint pValue);

            [DllImport(EAPIdll, EntryPoint = "EApiBoardGetValues", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiBoardGetValues(uint Id, [Out] char[] pBuffer, ref uint pBufferLen);

            #endregion General

            #region Brightness

            [DllImport(EAPIdll, EntryPoint = "EApiDisplayGetBacklightBrightness", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiDisplayGetBacklightBrightness(uint Id, ref uint pBright);

            [DllImport(EAPIdll, EntryPoint = "EApiDisplaySetBacklightBrightness", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiDisplaySetBacklightBrightness(uint Id, uint Bright);

            [DllImport(EAPIdll, EntryPoint = "EApiDisplayGetCap", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiDisplayGetCap(uint Id, uint CapId, ref uint pValue);

            [DllImport(EAPIdll, EntryPoint = "EApiDisplaySetCap", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiDisplaySetCap(uint Id, uint CapId, uint Value);

            #endregion Brightness

            #region ETP

            [StructLayout(LayoutKind.Sequential)]
            public struct ETP_DATA
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
                public string DeviceOrderText;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
                public string DeviceOrderNumber;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
                public string DeviceIndex;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
                public string DeviceSerialNumber;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
                public string OperatingSystem;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
                public string Image;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 92)]
                public string Reverse;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ETP_DATA_Std
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 240)]
                public string uBuffer;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ETP_DATA_DeviceSpace
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string dsModelName;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
                public string dsSerialNumber;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
                public string dsBoardVersion;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
                public string dsUUIDs;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 108)]
                public string dsReserve;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
                public string dsVersion;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ETP_USER_DATA
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string UserSpace1;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string UserSpace2;
            }

            [DllImport(EAPIdll, EntryPoint = "EApiGetLockStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiGetLockStatus(int slaveaddr, ref uint LockStatus);

            [DllImport(EAPIdll, EntryPoint = "EApiSetEepromProtect", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiSetEepromProtect(int SalveAddr, bool bProtect, string pBuffer, int BufLen);

            [DllImport(EAPIdll, EntryPoint = "EApiETPReadDeviceData", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiETPReadDeviceData(ref ETP_DATA pOutBuffer);

            [DllImport(EAPIdll, EntryPoint = "EApiETPReadDeviceData", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiETPReadDeviceData(ref ETP_DATA_Std pOutBuffer);

            [DllImport(EAPIdll, EntryPoint = "EApiETPReadDeviceData", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiETPReadDeviceData(ref ETP_DATA_DeviceSpace pOutBuffer);

            [DllImport(EAPIdll, EntryPoint = "EApiETPReadUserData", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiETPReadUserData(ref ETP_USER_DATA pOutBuffer);

            [DllImport(EAPIdll, EntryPoint = "EApiETPWriteUserData", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiETPWriteUserData(ref ETP_USER_DATA pOutBuffer);

            #endregion ETP

            #region GPIO

            [DllImport(EAPIdll, EntryPoint = "EApiGPIOGetDirectionCaps", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiGPIOGetDirectionCaps(uint Id, ref uint pInputs, ref uint pOutputs);

            [DllImport(EAPIdll, EntryPoint = "EApiGPIOGetDirection", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiGPIOGetDirection(uint Id, uint Bitmask, ref uint pDirection);

            [DllImport(EAPIdll, EntryPoint = "EApiGPIOSetDirection", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiGPIOSetDirection(uint Id, uint Bitmask, uint Direction);

            [DllImport(EAPIdll, EntryPoint = "EApiGPIOGetLevel", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiGPIOGetLevel(uint Id, uint Bitmask, ref uint pLevel);

            [DllImport(EAPIdll, EntryPoint = "EApiGPIOSetLevel", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiGPIOSetLevel(uint Id, uint Bitmask, uint Level);

            #endregion GPIO

            #region LED

            [DllImport(EAPIdll, EntryPoint = "EApiExtFunctionSetStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiExtFunctionSetStatus(uint Id, uint Status);

            #endregion LED

            #region Redundant Power Status

            [DllImport(EAPIdll, EntryPoint = "EApiExtFunctionGetStatus", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiExtFunctionGetStatus(uint Id, ref uint pStatus);

            #endregion Redundant Power Status

            #region AuxIO

            [DllImport(AuxIOdll, EntryPoint = "EApiGetGPIOMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGetGPIOMask(ref uint pGPIOMask);

            [DllImport(AuxIOdll, EntryPoint = "EApiGPIOGetDirectionCaps", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGPIOGetDirectionCaps(uint Id, ref uint pInputs, ref uint pOutputs);

            [DllImport(AuxIOdll, EntryPoint = "EApiGPIOGetDirection", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGPIOGetDirection(uint Id, uint Bitmask, ref uint pDirection);

            [DllImport(AuxIOdll, EntryPoint = "EApiGPIOSetDirection", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGPIOSetDirection(uint Id, uint Bitmask, uint Direction);

            [DllImport(AuxIOdll, EntryPoint = "EApiGPIOGetLevel", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGPIOGetLevel(uint Id, uint Bitmask, ref uint pLevel);

            [DllImport(AuxIOdll, EntryPoint = "EApiGPIOSetLevel", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGPIOSetLevel(uint Id, uint Bitmask, uint Level);

            [DllImport(AuxIOdll, EntryPoint = "EApiAIGetValue", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOAIGetValue(uint Id, ref uint pValue);

            [DllImport(AuxIOdll, EntryPoint = "EApiAOSetValue", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOAOSetValue(uint Id, uint Value);

            [DllImport(AuxIOdll, EntryPoint = "EApiAOGetValue", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOAOGetValue(uint Id, ref uint pValue);

            [DllImport(AuxIOdll, EntryPoint = "EApiGetAIMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGetAIMask(ref uint pAIMask);

            [DllImport(AuxIOdll, EntryPoint = "EApiGetAOMask", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOGetAOMask(ref uint pAOMask);

            [DllImport(AuxIOdll, EntryPoint = "EApiSetDefault", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint EApiAuxIOSetDefault(uint Id);

            #endregion AuxIO
        }
    }
}