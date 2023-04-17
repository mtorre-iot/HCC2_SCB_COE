using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTOptix.UI;

namespace HCC2_SCB_2
{
	public static class Constants
	{
		public static string storeNameStr  = "DataStores/AlarmsConfig"; 
		public static string tableNameStr = "AlarmsParams";
		public static string[] columns = { "AlarmName", "AlarmType", "AlarmOperation", "Setpoint", "TripTime", "Restarts", "RestartDelay" };
		public static string[] operations = { "Bypass", "Log", "Stop", "Stop & Log" };
		public static string lblAlarmNameStr = "LblAlarmName";
		public static string lblAlarmTypeStr = "LblAlarmType";
		public static string tbxSetpointStr = "TbxSetpointValue";
		public static string tbxTripTimeStr = "TbxTripTime";
		public static string tbxRestartsStr = "TbxRestarts";
		public static string tbxRestartDelayStr = "TbxRestartDelay";
		public static string cbxAlarmsOperationsStr = "CbxAlarmsOperations";
		public static string basePathStr = "Alarms/DHT/";
		public static string lblSignalStr = "LblSignal";
		public static string lblValueSignalStr = "LblValueSignal";
		public static string btnAlarmFunctionButtonStr = "AlarmFunctionButton";
		public static string alarmInformationStr = "AlarmInformation";
		public static string lblExportMessageStr = "LblExportMessage";
		public static string lblFromDateStr = "SetDialogUnitPanelFrom/LblDate";
		public static string lblToDateStr = "SetDialogUnitPanelTo/LblDate";
		public static string dialogContextBaseStr = "UI/Screens/LogExportPanel/LogTrendDatedExport1/SetDialogUnitPanelFrom/";
		public static string lblEstimatedExportTimeStr = "LblEstimatedExportTime";
		public static string lblExportProgressStr = "LblExportProgress";
		public static string setDialogContextStr = "SetDateDialogContext";
		public static string varNewFromDateStr = "NewFromDate";
		public static string varNewToDateStr = "NewToDate";
		public static string lblRealTimeClockStr = "UI/MainWindow/Main/TopBar/LblRealTimeClock";
		public static string dateFormat = "yyyy-MM-dd HH:mm:ss";
		public static string cbxYearStr = "CbxYear";
		public static string cbxMonthStr = "CbxMonth";
		public static string cbxDayStr = "CbxDay";
		public static string daysBase = "Model/InternalConfiguration/Days";
		public static int[] daysOfMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
		public static int[] daysOfMonthLeap = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
		public static string LblNewDateStr = "LblDate";
	}
}
