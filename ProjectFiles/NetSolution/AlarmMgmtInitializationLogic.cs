#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.DataLogger;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.NativeUI;
using Store = FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.OPCUAServer;
using FTOptix.Modbus;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using System.Threading;
using System.Collections.Generic;
using FTOptix.EventLogger;
#endregion

public class AlarmMgmtInitializationLogic : BaseNetLogic
{
    private string storeNameStr = "DataStores/AlarmsConfig";

    private string tableNameStr = "AlarmsParams";
    private string[] columns = { "AlarmName", "AlarmType", "AlarmOperation", "Setpoint", "TripTime", "Restarts", "RestartDelay"};
    private string basePathStr =  "Alarms/DHT/";

    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
