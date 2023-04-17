#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.DataLogger;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.OPCUAServer;
using FTOptix.Modbus;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Alarm;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.EventLogger;
using Store = FTOptix.Store;
using HCC2_SCB_2;
#endregion

public class ClearTableButtonLogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void ClearTableButtonClick()
    {
        // Get the project
        var project = FTOptix.HMIProject.Project.Current;
        // 
        // Check if table can be read
        //
        //string storeNameStr = "DataStores/AlarmsConfig";
        //string tableNameStr = "AlarmsParams";
        //string[] columns = { "AlarmName", "AlarmType", "Setpoint", "TripTime", "Restarts", "RestartDelay"};
        //
        // Get access to  DB store object
        //
        var store = project.GetObject(Constants.storeNameStr) as Store.Store;
        if (store == null)
        {
            throw new Exception("Datastore name: " + Constants.storeNameStr + " NOT found");
        }
        // 
        // Clear the table!
        //
        var dbo = new DBOperations(store, Constants.tableNameStr, Constants.columns);
        dbo.CleanDBTable(); 
    }
    // void CleanDBTable(Store.Store store, string tableNameStr)
    // {
    //     object[,] resultSet;
    //     string[] header;
    //     string truncateStr = "DELETE FROM \"" + tableNameStr + "\"";
    //     try
    //     {
    //         store.Query(truncateStr, out header, out resultSet);
    //     }
    //     catch (Exception e)
    //     {
    //         throw new Exception("Error on Table truncate. Error: " + e.Message);
    //     }
    // }
}
