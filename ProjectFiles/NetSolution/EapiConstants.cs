using System.Runtime.InteropServices;

namespace EAPI
{
    internal class EapiConstants
    {
        public const int UNDEFINED = -1;
        public const uint EAPI_STATUS_SUCCESS = 0;
        public const uint EAPI_STATUS_ERROR = 0xFFFFF0FF;
        public const uint EAPI_STATUS_UNSUPPORTED = 0xFFFFFCFF;
        public const uint EAPI_STATUS_MORE_DATA = 0xFFFFF9FF;

        public const uint EAPI_ID_GET_NAME_MASK = 0xF0000000;

        public static uint EAPI_ID_GET_NAME_BASE(uint Id)
        {
            return Id & EAPI_ID_GET_NAME_MASK;
        }

        public const int NAME_MAXIMUM_LENGTH = 256;

        public const uint EAPI_ID_BASE_GET_NAME_INFO = 0x00000000;
        public const uint EAPI_ID_BASE_GET_NAME_HWMON = 0x10000000;

        public const uint EAPI_ID_BOARD_NAME_STR = 1;           /* Board Name String */
        public const uint EAPI_ID_BOARD_BIOS_REVISION_STR = 4;  /* Board Bios Revision String*/

        public const uint EAPI_ID_BOARD_EC_REVISION_STR = 0x101;    /* EC version */
        public const uint EAPI_ID_BOARD_OS_REVISION_STR = 0x102;    /* OS version */
        public const uint EAPI_ID_BOARD_CPU_MODEL_NAME_STR = 0x103; /* OS version */
    }

    internal class BoardInfoValues
    {
        public static uint EAPI_ID_GET_INDEX(uint Id)
        {
            return Id & 0xFF;
        }

        public static uint EAPI_ID_GET_TYPE(uint Id)
        {
            return Id & 0x000FF000;
        }
    }

    internal class Brightness
    {
        public const int EAPI_ID_BACKLIGHT_MAX = 3;
        public const int EAPI_ID_BACKLIGHT_1 = 0;
        public const int EAPI_ID_BACKLIGHT_2 = 1;
        public const int EAPI_ID_BACKLIGHT_3 = 2;

        public const int EAPI_ID_DISPLAY_BRIGHTNESS_MAXIMUM = 0x00010000;
        public const int EAPI_ID_DISPLAY_BRIGHTNESS_MINIMUM = 0x00010001;
        public const int EAPI_ID_DISPLAY_AUTO_BRIGHTNESS = 0x00010002;

        public const int EAPI_BACKLIGHT_SET_ON = 1;
        public const int EAPI_BACKLIGHT_SET_OFF = 0;
        public const int EAPI_AUTO_BRIGHTNESS_SET_ON = 1;
        public const int EAPI_AUTO_BRIGHTNESS_SET_OFF = 0;

        public const int MaxFunction = 3;

        public static uint g_BrightnessMax = 0;
        public static uint g_BrightnessMin = 0xFF;
        public static uint g_AutoBrightnessConfig = 0;
        public static uint g_BrightnessValue = 0;
        public static bool g_IsAutoBrightnessAvailable = true;
    }

    public class GPIO
    {
        public static uint EAPI_GPIO_GPIO_ID(int GPIO_NUM)
        {
            return (uint)GPIO_NUM;
        }

        public const uint EAPI_GPIO_GPIO_BITMASK = 1;

        public static uint EAPI_ID_GPIO_GPIO00 = EAPI_GPIO_GPIO_ID(0);  /* (Optional) */
        public static uint EAPI_ID_GPIO_GPIO01 = EAPI_GPIO_GPIO_ID(1);  /* (Optional) */
        public static uint EAPI_ID_GPIO_GPIO02 = EAPI_GPIO_GPIO_ID(2);  /* (Optional) */
        public static uint EAPI_ID_GPIO_GPIO03 = EAPI_GPIO_GPIO_ID(3);  /* (Optional) */

        public const uint BANK_MAX = 4;
        public const uint EAPI_ID_GPIO_BANK_BASE = 0x10000;

        public static uint EAPI_ID_GPIO_BANK(uint BANK_NUM)
        {
            return EAPI_ID_GPIO_BANK_BASE | BANK_NUM;
        }

        public static uint EAPI_ID_GPIO_PIN_BANK(uint GPIO_NUM)
        {
            return EAPI_ID_GPIO_BANK_BASE | (GPIO_NUM >> 5);
        }

        public static uint EAPI_GPIO_PIN_BANK_MASK(int GPIO_NUM)
        {
            return (uint)(1 << (GPIO_NUM & 0x1F));
        }

        public static bool EAPI_GPIO_PIN_BANK_TEST_STATE(int GPIO_NUM, int TState, int TValue)
        {
            return ((TValue >> (GPIO_NUM & 0x1F)) & 1) == TState;
        }

        public static uint EAPI_ID_GPIO_BANK00()
        {
            return EAPI_ID_GPIO_BANK(0);
        }

        public static uint EAPI_ID_GPIO_BANK01()
        {
            return EAPI_ID_GPIO_BANK(1);
        }

        public static uint EAPI_ID_GPIO_BANK02()
        {
            return EAPI_ID_GPIO_BANK(2);
        }

        public static uint EAPI_ID_GPIO_BANK03()
        {
            return EAPI_ID_GPIO_BANK(3);
        }

        /* Bit mask Bit States */
        public const uint EAPI_GPIO_BITMASK_SELECT = 1;
        public const uint EAPI_GPIO_BITMASK_NOSELECT = 0;

        /* Levels */
        public const uint EAPI_GPIO_LOW = 0;
        public const uint EAPI_GPIO_HIGH = 1;

        /* Directions */
        public const uint EAPI_GPIO_INPUT = 1;
        public const uint EAPI_GPIO_OUTPUT = 0;

        public enum FunctionIndex
        {
            funcPin = 0,
            funcIdType,
            funcDirection,
            funcLevel,
            funcGet,
            MaxFunction
        };

        public static int[] functions = new int[(int)FunctionIndex.MaxFunction];

        [StructLayout(LayoutKind.Sequential)]
        public struct GPIOInfo
        {
            public byte supPinNum;
            public uint supInput;
            public uint supOutput;
        }

        public static GPIOInfo[] info = new GPIOInfo[BANK_MAX];

        [StructLayout(LayoutKind.Sequential)]
        public struct GPIOConfig
        {
            public uint idType;
            public uint directionVal;
            public uint levelVal;
        }

        public enum ID_TYPE
        {
            idtypeSingle = 0,
            idtypeMulti,
            idtypeMax
        };

        public static GPIOConfig config;
        public static byte PinNumber = 0;
    }

    public class HWMonitor
    {
        public const int EAPI_ID_HWMON_TEMP_MAX = 10;
        public const int EAPI_ID_HWMON_TEMP_BASE = 0x00050000;
        public const int EAPI_ID_HWMON_TEMP = EAPI_ID_HWMON_TEMP_BASE;
        public const int EAPI_ID_HWMON_TEMP_CPU = EAPI_ID_HWMON_TEMP_BASE + 0;              /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_CHIPSET = EAPI_ID_HWMON_TEMP_BASE + 1;          /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_SYSTEM = EAPI_ID_HWMON_TEMP_BASE + 2;           /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_CPU2 = EAPI_ID_HWMON_TEMP_BASE + 3;             /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_OEM0 = EAPI_ID_HWMON_TEMP_BASE + 4;             /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_OEM1 = EAPI_ID_HWMON_TEMP_BASE + 5;             /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_OEM2 = EAPI_ID_HWMON_TEMP_BASE + 6;             /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_OEM3 = EAPI_ID_HWMON_TEMP_BASE + 7;             /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_OEM4 = EAPI_ID_HWMON_TEMP_BASE + 8;             /* 0.1 Kelvins */
        public const int EAPI_ID_HWMON_TEMP_OEM5 = EAPI_ID_HWMON_TEMP_BASE + 9;             /* 0.1 Kelvins */

        public const int EAPI_ID_HWMON_VOLTAGE_MAX = 24;
        public const int EAPI_ID_HWMON_VOLTAGE_BASE = 0x00051000;
        public const int EAPI_ID_HWMON_VOLTAGE = EAPI_ID_HWMON_VOLTAGE_BASE;
        public const int EAPI_ID_HWMON_VOLTAGE_VCORE = EAPI_ID_HWMON_VOLTAGE_BASE + 0;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_VCORE2 = EAPI_ID_HWMON_VOLTAGE_BASE + 1;     /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_2V5 = EAPI_ID_HWMON_VOLTAGE_BASE + 2;        /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_3V3 = EAPI_ID_HWMON_VOLTAGE_BASE + 3;        /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_5V = EAPI_ID_HWMON_VOLTAGE_BASE + 4;         /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_12V = EAPI_ID_HWMON_VOLTAGE_BASE + 5;        /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_5VSB = EAPI_ID_HWMON_VOLTAGE_BASE + 6;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_3VSB = EAPI_ID_HWMON_VOLTAGE_BASE + 7;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_VBAT = EAPI_ID_HWMON_VOLTAGE_BASE + 8;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_5NV = EAPI_ID_HWMON_VOLTAGE_BASE + 9;        /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_12NV = EAPI_ID_HWMON_VOLTAGE_BASE + 10;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_VTT = EAPI_ID_HWMON_VOLTAGE_BASE + 11;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_24V = EAPI_ID_HWMON_VOLTAGE_BASE + 12;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_DC = EAPI_ID_HWMON_VOLTAGE_BASE + 13;        /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_DCSTBY = EAPI_ID_HWMON_VOLTAGE_BASE + 14;    /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_VBATLI = EAPI_ID_HWMON_VOLTAGE_BASE + 15;    /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_OEM0 = EAPI_ID_HWMON_VOLTAGE_BASE + 16;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_OEM1 = EAPI_ID_HWMON_VOLTAGE_BASE + 17;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_OEM2 = EAPI_ID_HWMON_VOLTAGE_BASE + 18;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_OEM3 = EAPI_ID_HWMON_VOLTAGE_BASE + 19;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_1V05 = EAPI_ID_HWMON_VOLTAGE_BASE + 20;      /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_1V5 = EAPI_ID_HWMON_VOLTAGE_BASE + 21;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_1V8 = EAPI_ID_HWMON_VOLTAGE_BASE + 22;       /* millivolts */
        public const int EAPI_ID_HWMON_VOLTAGE_DC2 = EAPI_ID_HWMON_VOLTAGE_BASE + 23;       /* millivolts */

        public const int EAPI_ID_HWMON_FAN_MAX = 10;
        public const int EAPI_ID_HWMON_FAN_BASE = 0x00052000;
        public const int EAPI_ID_HWMON_FAN_CPU = EAPI_ID_HWMON_FAN_BASE + 0;                /* RPM */
        public const int EAPI_ID_HWMON_FAN_SYSTEM = EAPI_ID_HWMON_FAN_BASE + 1;             /* RPM */
        public const int EAPI_ID_HWMON_FAN_CPU2 = EAPI_ID_HWMON_FAN_BASE + 2;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM0 = EAPI_ID_HWMON_FAN_BASE + 3;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM1 = EAPI_ID_HWMON_FAN_BASE + 4;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM2 = EAPI_ID_HWMON_FAN_BASE + 5;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM3 = EAPI_ID_HWMON_FAN_BASE + 6;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM4 = EAPI_ID_HWMON_FAN_BASE + 7;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM5 = EAPI_ID_HWMON_FAN_BASE + 8;               /* RPM */
        public const int EAPI_ID_HWMON_FAN_OEM6 = EAPI_ID_HWMON_FAN_BASE + 9;               /* RPM */

        public const int EAPI_ID_HWMON_CURRENT_MAX = 3;
        public const int EAPI_ID_HWMON_CURRENT_BASE = 0x00053000;
        public const int EAPI_ID_HWMON_CURRENT_OEM0 = EAPI_ID_HWMON_CURRENT_BASE + 0;       /* milliampere */
        public const int EAPI_ID_HWMON_CURRENT_OEM1 = EAPI_ID_HWMON_CURRENT_BASE + 1;       /* milliampere */
        public const int EAPI_ID_HWMON_CURRENT_OEM2 = EAPI_ID_HWMON_CURRENT_BASE + 2;       /* milliampere */

        public const int EAPI_ID_HWMON_POWER_MAX = 1;
        public const int EAPI_ID_HWMON_POWER_BASE = 0x00054000;
        public const int EAPI_ID_HWMON_POWER_OEM0 = EAPI_ID_HWMON_POWER_BASE + 0;           /* millwatt */

        public const int EAPI_ID_HWMON_MAX = EAPI_ID_HWMON_VOLTAGE_MAX;

        public const uint EAPI_KELVINS_OFFSET = 2731;

        public static float EAPI_ENCODE_CELCIUS(float Celsius)
        {
            return ((Celsius) * 10) + EAPI_KELVINS_OFFSET;
        }

        public static float EAPI_DECODE_CELCIUS(float Kelvins)
        {
            return (Kelvins - EAPI_KELVINS_OFFSET) / 10;
        }

        public static float EAPI_CELCIUS_TO_FAHRENHEIT(float Celsius)
        {
            return 32 + (Celsius * 9 / 5);
        }

        public static float EAPI_DECODE_HWMON_VALUE(float RawValue)
        {
            return RawValue / 1000;
        }

        public static string[] funcStr = new string[]
        {
            "Exit",
            "Temperature",
            "Voltage",
            "Fan speed",
            "Current",
            "Power"
        };

        public enum FunctionIndex
        {
            exitFunction = 0,
            funcTemperature,
            funcVoltage,
            funcFanSpeed,
            funcCurrent,
            funcPower,
            MaxFunction,
        };

        public static int[] temperature = new int[EAPI_ID_HWMON_TEMP_MAX];
        public static int[] voltage = new int[EAPI_ID_HWMON_VOLTAGE_MAX];
        public static int[] fanspeed = new int[EAPI_ID_HWMON_FAN_MAX];
        public static int[] current = new int[EAPI_ID_HWMON_CURRENT_MAX];
        public static int[] power = new int[EAPI_ID_HWMON_POWER_MAX];
        public static int[] functions = new int[(int)FunctionIndex.MaxFunction];
        // 
        // Added by MT 
        //
        public static readonly string[] MeasurementsStrArray = {"Temp.CPU", "Temp.System"};
        public enum BoardTemp
    {
        Unknown = -1,
        CPU = 0,
        System = 2

    };
    
    }

    internal class LED
    {
        public const uint EAPI_EXT_FUNC_LED_MAX = 16;
        public const uint EAPI_ID_EXT_FUNC_LED_BASE = 0;
        public static uint EAPI_ID_EXT_FUNC_LED_MIN = EAPI_ID_EXT_FUNC_LED(0);
        public static uint EAPI_ID_EXT_FUNC_LED_MAX = EAPI_ID_EXT_FUNC_LED(EAPI_EXT_FUNC_LED_MAX - 1);

        public static uint EAPI_ID_EXT_FUNC_LED(uint N)
        {
            return EAPI_ID_EXT_FUNC_LED_BASE + N;
        }

        public static uint EAPI_EXT_FUNC_LED_ID_TO_INDEX(uint ID)
        {
            return ID - EAPI_ID_EXT_FUNC_LED_MIN;
        }

        public static byte[] LEDTable = new byte[EAPI_EXT_FUNC_LED_MAX];
    }

    internal class RedundantPowerStatus
    {
        public const uint EAPI_EXT_FUNC_POWER_VIN_MAX = 2;
        public const uint EAPI_ID_EXT_FUNC_POWER_STATUS_BASE = 0x10;
        public static uint EAPI_ID_EXT_FUNC_POWER_STATUS_VIN_MIN = EAPI_ID_EXT_FUNC_POWER_STATUS_VIN(0);
        public static uint EAPI_ID_EXT_FUNC_POWER_STATUS_VIN_MAX = EAPI_ID_EXT_FUNC_POWER_STATUS_VIN(EAPI_EXT_FUNC_POWER_VIN_MAX - 1);

        public static uint EAPI_ID_EXT_FUNC_POWER_STATUS_VIN(uint N)
        {
            return EAPI_ID_EXT_FUNC_POWER_STATUS_BASE + N;
        }

        public static uint EAPI_EXT_FUNC_POWER_STATUS_VIN_ID_TO_INDEX(uint ID)
        {
            return ID - EAPI_ID_EXT_FUNC_POWER_STATUS_VIN_MIN;
        }
    }

    internal class AuxIO
    {
        public const byte AUXIO_PIN_MAX = 8;
        public const double AUXIO_OUTPUTVOLTAGE_MAX = 5.0;

        public enum AuxIO_Func
        {
            Reset = 0,
            GPIO,
            ADC,
            DAC
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AuxIOConfig
        {
            public AuxIO_Func func;
            public uint GPIOLevel;
            public uint GPIODir;
            public double ADCValue;
            public double DACValue;
        }
        public static AuxIOConfig[] pinInfo = new AuxIOConfig[AUXIO_PIN_MAX];
        /* GPIO Bank mode */
        public static uint idType = 0;
    }
}