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
#endregion

public class InitializeDBTables : BaseNetLogic
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
    public void InitializeAlarmsOperationsTable()
    {
        // transfer all collected info to the local DB
        var project = FTOptix.HMIProject.Project.Current;
        // 
        // Check if table can be read
        //
        string storeNameStr = "DataStores/AlarmsConfig";
        string tableNameStr = "AlarmsOperations";
        string[] columns = { "AlarmOperation"};
        string[] operations = { "Bypass", "Log", "Stop", "Stop & Log" }; 
        //
        // Get access to  DB store object
        //
        var store = project.GetObject(storeNameStr) as Store.Store;
        if (store == null)
        {
            throw new Exception("Datastore name: " + storeNameStr + " NOT found");
        }
        //
        // clear any previous state
        //
        var dbo = new DBOperations(store, tableNameStr, columns);
        dbo.CleanDBTable(); 
        //
        var table = store.Tables.Get<Store.Table>(tableNameStr);
        if (table == null)
        {
            throw new Exception("Table name: " + tableNameStr + " NOT found");
        }
        //
        // Save the names of the columns of the table to an array
        int numCols = columns.Length;
        int numOperations = operations.Length;

        var rawValues = new object[numOperations, numCols];
        for (int i=0; i < numOperations; i++)
        {
            rawValues[i,0] = i+1;
            rawValues[i,1] = operations[i];
        }
        //
        // Insert New record in table
        //
        var table1 = store.Tables.Get<Store.Table>(tableNameStr);
        // Insert values into table1
        table1.Insert(columns, rawValues);

        //object[,] resultSet; // test
        //GetAllRowsFromTable(store, tableNameStr, out resultSet); // Test
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

    // void GetAllRowsFromTable(Store.Store store, string tableNameStr, out object[,] resultSet)
    // {
    //     string[] header;
    //     string queryStr = "SELECT * FROM \"" + tableNameStr + "\"";
    //     store.Query(queryStr, out header, out resultSet);
    //     // Get Table from myStore
    //     var table = store.Tables.Get<Store.Table>(tableNameStr);
    //     if (table == null)
    //     {
    //         throw new Exception("Table name: " + tableNameStr + " NOT found");
    //     } 
    // }
}
