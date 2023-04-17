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
using Store = FTOptix.Store;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.EventLogger;
#endregion
public class DBOperations 
{
    public Store.Store store {get; set; }
    public string tableNameStr {get; set; }
    public string[] columns {get; set; }

    public DBOperations(Store.Store store, string tableNameStr, string[] columns)
    {   
        this.store = store;
        this.tableNameStr = tableNameStr;
        this.columns = columns;
    }

    public DBOperations(Store.Store store, string tableNameStr)
    {   
        this.store = store;
        this.tableNameStr = tableNameStr;
        this.columns = new string[0];
    }

    public void GetAllRowsFromTable(out object[,] resultSet)
    {
        string[] header;
        string queryStr = "SELECT * FROM \"" + this.tableNameStr + "\"";
        store.Query(queryStr, out header, out resultSet);
    }

    public void GetAllRowsBetweenDates(DateTime from, DateTime to, out string[] header, out object[,] resultSet)
    {
        //
        // Create the SQL query string
        //
        string queryStr = GenerateQueryBetweenDatesString(this.tableNameStr, from, to);
        // Execute query on store of the current project
        try
        {
            store.Query(queryStr, out header, out resultSet);
        }
        catch (Exception e)
        {
            throw new Exception("Error trying to query table: " + this.tableNameStr + ". Error: " + e.Message);
        }
    }

    public int GetOneRowFromTable(string alarmName, string alarmType, out object[,] resultSet)
    {
        string[] header;
        string queryStr = "SELECT";
        foreach (string col in this.columns)
        {
            queryStr += " " + col + ",";
        }
        queryStr = queryStr.TrimEnd(',');
        queryStr += " FROM \"" + this.tableNameStr + "\"";
        queryStr += " WHERE AlarmName = \"" + alarmName + "\" AND AlarmType= \"" + alarmType + "\"";
        try
        {
            store.Query(queryStr, out header, out resultSet);
        }
        catch (Exception e)
        {
            throw new Exception("Error trying to query table: " + this.tableNameStr + ". Error: " + e.Message);
        }
        // Get Table from myStore
        var table = this.store.Tables.Get<Store.Table>(this.tableNameStr);
        if (table == null)
        {
            throw new Exception("Table name: " + this.tableNameStr + " NOT found");
        }
        return resultSet.GetLength(0); 
    }

    public int UpdateOneRowFromTable(AlarmConfigurationData outputObj, out object[,] resultSet)
    {
        // "Setpoint", "TripTime", "Restarts", "RestartDelay"
        //
        string alarmName = outputObj.alarmName;
        string alarmType = outputObj.alarmType;
        string[] header;
        string updateStr = "UPDATE \"" + this.tableNameStr + "\"";
        updateStr += " SET " + this.columns[2] + " = \"" + outputObj.alarmOperation + "\", ";
        updateStr += this.columns[3] + " = \"" + outputObj.setpoint.ToString() + "\", ";
        updateStr += this.columns[4] + " = \"" + outputObj.tripTime.ToString() + "\", ";
        updateStr += this.columns[5] + " = \"" + outputObj.restarts.ToString() + "\", ";
        updateStr += this.columns[6] +" = \"" + outputObj.restartDelay.ToString() + "\"";
        updateStr += " WHERE " + this.columns[0] + " = \"" + alarmName + "\" AND " + this.columns[1] +" = \"" + alarmType + "\"";
        try
        {
            this.store.Query(updateStr, out header, out resultSet);
        }
        catch (Exception e)
        {
            throw new Exception("Error trying to query table: " + this.tableNameStr + ". Error: " + e.Message);
        }
        return resultSet.GetLength(0); 
    }

private string GenerateQueryBetweenDatesString(string browseName, DateTime dtFromValue, DateTime dtToValue)
    {
        string dateFormat = "yyyy-MM-ddTHH:mm:ss";
        //
        // add 23:59:59 to end date, to include the full end date.
        //
        TimeSpan fullCurrentDay = new TimeSpan(23,59,59);
        dtToValue = dtToValue.Add(fullCurrentDay);
        string rtn = "SELECT * FROM \"" + browseName + "\"";
        rtn += " WHERE LocalTimestamp >= \"" + dtFromValue.ToString(dateFormat) + "\" AND LocalTimestamp <= \"" + dtToValue.ToString(dateFormat) + "\"";
        return rtn;
    }
    public void CleanDBTable()
    {
        object[,] resultSet;
        string[] header;
        string truncateStr = "DELETE FROM \"" + this.tableNameStr + "\"";
        try
        {
            this.store.Query(truncateStr, out header, out resultSet);
        }
        catch (Exception e)
        {
            throw new Exception("Error on Table truncate. Error: " + e.Message);
        }
    }
}
