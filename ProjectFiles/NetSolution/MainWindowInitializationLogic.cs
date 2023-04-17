#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.DataLogger;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.SQLiteStore;
using FTOptix.OPCUAServer;
using FTOptix.Modbus;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using Store = FTOptix.Store;
using System.Collections.Generic;
using FTOptix.EventLogger;
using HCC2_SCB_2;
#endregion

public class MainWindowInitializationLogic : BaseNetLogic
{
    //private string storeNameStr = "DataStores/AlarmsConfig";

    //private string tableNameStr = "AlarmsParams";
    //private string[] columns = { "AlarmName", "AlarmType", "AlarmOperation", "Setpoint", "TripTime", "Restarts", "RestartDelay"};
    //private string basePathStr =  "Alarms/DHT/";
    public override void Start()
    {
            //
        // Try to connect with the AlarmsDB
        //
        var project = Project.Current;
        //
        // if enters here, means that the user pressed "Save" in the AlarmActionDialog
        //  Read the Information stored in DB
        //
        var store = project.GetObject(Constants.storeNameStr) as Store.Store;
        if (store == null)
        {
            throw new Exception("Datastore name: " + Constants.storeNameStr + " NOT found");
        }
        //
        // Create the DB Operations object
        //
        var dbo = new DBOperations(store, Constants.tableNameStr, Constants.columns);
        // 
        // Create the Alarm configuration Object
        //
        //
        // create the output object
        //
        var outputObj = new AlarmConfigurationData();
        //
        // Get all configuration data from table
        //
        object[,] resultSet;
        List<AlarmConfigurationData> alarmData;
        try
        {
            dbo.GetAllRowsFromTable(out resultSet);
            alarmData = AlarmConfigurationData.LoadOneAllRowsTableData(resultSet);
        }
        catch (Exception e)
        {
            throw new Exception("There was an error trying to query datastore name: " + Constants.storeNameStr + ". Error: " + e.Message);
        }
        //
        // Initialize all alarms from AlarmsDB to optix alarmProcessor
        //
        foreach (AlarmConfigurationData acd in alarmData)
        {
            string alarmNameStr = Constants.basePathStr + acd.alarmName;
            ExclusiveLevelAlarmController alarmObj = (ExclusiveLevelAlarmController) Project.Current.GetObject(alarmNameStr);
            if (alarmObj == null) 
            {
                throw new Exception("Alarm object with name: " + alarmNameStr + " NOT found");
            }
            //
            // Set this alarm
            //
            acd.SetAlarmSetpoint (alarmObj);
            //
            // Enable (or disable) depending on action state
            //
            acd.SetOperation(alarmObj);
            // 
            // Setup both auto acknowlege and auto confirm to true
            //
            acd.SetAutoAcknowledge(alarmObj, true);
        }
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
