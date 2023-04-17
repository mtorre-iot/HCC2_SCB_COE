#region Using directives
using System;
using UAManagedCore;
using FTOptix.UI;
using FTOptix.NetLogic;
using HMIProject = FTOptix.HMIProject;
using System.IO;
using FTOptix.Alarm;
using FTOptix.EventLogger;
using Store = FTOptix.Store;

#endregion

public class ExportTableData : BaseNetLogic
{
    private string sep;
    private string usbPath;
    private string lblMessageStr, dtFromStr, dtToStr;
    private  Label lblMessage;
    private DateTimePicker dtFrom, dtTo;
    private DateTime dtValueFrom, dtValueTo;
    private DelayedTask GenerateDelayedTask;
    private int delay;
    private string dateFormat;
    private string  csvFileNameFormat;
    private string csvFileNameDateFormat;
    public override void Start()
    {
        sep = ",";
        usbPath = "D:\\";
        lblMessageStr = "UI/Screens/DownloadLogsPanel/DownloadMessage";
        dtFromStr = "UI/Screens/DownloadLogsPanel/DownloadFrom"; 
        dtToStr = "UI/Screens/DownloadLogsPanel/DownloadTo"; 
        delay = 5000;
        dateFormat = "yyyy-MM-ddTHH:mm:ss";
        csvFileNameDateFormat = "yyyyMMddHHmm";
        csvFileNameFormat = "CSVExport-{0}-{1}.csv";
        //
        // Setup from / to controls
        //
        dtFrom = (DateTimePicker) HMIProject.Project.Current.GetObject(dtFromStr);

        if (dtFrom == null) 
        {
            throw new Exception("DateTimePicker <" + dtFromStr + "> field NOT found");
        }
        dtTo = (DateTimePicker) HMIProject.Project.Current.GetObject(dtToStr);
        if (dtTo == null) 
        {
            throw new Exception("DateTimePicker <" + dtToStr + "> field NOT found");
        }
        dtValueTo = dtTo.Value = DateTime.Now;
        dtValueFrom = dtFrom.Value = DateTime.Now.Subtract(new TimeSpan(1,0,0));
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void ExportCSV(UAManagedCore.NodeId tableNodeId, NodeId dtFrom, NodeId dtTo)
    {
        // Getting the current project
        var project = HMIProject.Project.Current;

        // Getting the object table from its NodeId
        var tableObject = LogicObject.Context.GetObject(tableNodeId);

        if (tableObject == null)
        {
             throw new Exception("Object with NodeId: " + tableNodeId.ToString() + " NOT found");
        }
        // Setup the message object
        lblMessage = (Label) project.GetObject(lblMessageStr);

        if (lblMessage == null)
        {
            throw new Exception("Label <" + lblMessageStr + "> label field NOT found");
        }
        var dtFromObj = LogicObject.Context.GetObject(dtFrom);
        if (dtFromObj == null)
        {
            throw new Exception("'From' DateTimePciker object is invalid or was not found");
        }
        DateTime dtFromValue = dtFromObj.GetVariable("Value").Value;
         var dtToObj = LogicObject.Context.GetObject(dtTo);
        if (dtTo == null)
        {
            throw new Exception("'To' DateTimePciker object is invalid or was not found");
        }
        DateTime dtToValue = dtToObj.GetVariable("Value").Value;
        //
        // Getting the Tables collection
        //
        var tablesCollection = tableObject.Owner;

        if (tablesCollection == null)
        {
            throw new Exception("TableCollection object is invalid or was not found");
        }

        // Getting the Store
        var storeObject = tablesCollection.Owner as Store.Store;
        if (storeObject == null)
        {
            throw new Exception("Store object is invalid or was not found");
        }

        object[,] resultSet;
        string[] header;
        //
        // Create query string
        //
        // print from / to times  , dtValueTo;
        string queryStr = GenerateQueryString(tableObject.BrowseName, dtValueFrom, dtValueTo);
        // Execute query on store of the current project
        storeObject.Query(queryStr, out header, out resultSet);
        // Check if the resultSet is a bidimensional array
        if (resultSet.Rank != 2)
            return;

        // Getting the number of rows and columns of the matrix
        var rowCount = resultSet != null ? resultSet.GetLength(0) : 0;
        var columnCount = header != null ? header.Length : 0;

        // create delayed task to clean the message label
        GenerateDelayedTask = new DelayedTask(CleanMessage, delay, LogicObject);

        // Check if usb Path exists (i.e. the USB drive is mounted)
        if (Directory.Exists(usbPath)== false)
        {
            lblMessage.Text = "USB Disk " + usbPath + " is not Mounted. Please verify";
            // fire delayed task to clean the message label
            GenerateDelayedTask.Start();
            return;
        }
        // Output file path
        string filePath = Path.Combine(usbPath, string.Format(csvFileNameFormat, tableObject.BrowseName, DateTime.UtcNow.ToString(csvFileNameDateFormat)));
        Log.Info(filePath);
        
        // Writing CSV to file
        try
        {
            using (StreamWriter csvFileStream =
            new StreamWriter(filePath))
            {
                // Table header
                for (UInt32 i = 0; i < columnCount; i++)
                {
                    csvFileStream.Write(header[i]);

                    if (i < columnCount - 1)
                        csvFileStream.Write(sep);
                }

                csvFileStream.Write("\n");

                // Table content
                for (UInt32 i = 0; i < rowCount; i++)
                {
                    for (UInt32 j = 0; j < columnCount; j++)
                    {
                        if (resultSet[i, j] == null)
                            csvFileStream.Write("");

                        csvFileStream.Write(resultSet[i, j]);

                        if (j < columnCount - 1)
                            csvFileStream.Write(sep);
                    }
                    csvFileStream.Write("\n");
                }
            }
            lblMessage.Text = "DataLog succesfully saved in file " + filePath +". Total: " + rowCount.ToString() + " rows.";
            // fire delayed task to clean the message label
            GenerateDelayedTask.Start();
        }
        catch(Exception e)
        {
            // Write the error in the log
            string errorMes = "Unable to export datalog to CSV file '" + filePath + "': " + e.Message;
            lblMessage.Text = errorMes;
            UAManagedCore.Logging.LogManager.CoreLogger.Log(
                UAManagedCore.Logging.LogLevel.Error, "", 0, 0, errorMes, "");
        }
    }

    private string GenerateQueryString(string browseName, DateTime dtFromValue, DateTime dtToValue)
    {
        string rtn = "SELECT * FROM \"" + browseName + "\"";
        rtn += " WHERE LocalTimestamp >= \"" + dtFromValue.ToString(dateFormat) + "\" AND LocalTimestamp <= \"" + dtToValue.ToString(dateFormat) + "\"";
        return rtn;
    }

    public void CleanMessage()
    {
        lblMessage.Text = "";
    }
    private string GetBrowseName (string fullBrowseName)
    {
        string[] split = fullBrowseName.Split("/");
        return split[split.Length-1];
    }

    [ExportMethod]
    public void DateTimeFromPickerChange(NodeId datePickerNodeId)
    {
        var datePickerObj = LogicObject.Context.GetObject(datePickerNodeId);
        if (datePickerObj == null)
        {
            return;
        }
         var datePickerValue = datePickerObj.GetVariable("Value");
        if (datePickerObj.BrowseName.Contains(GetBrowseName(dtFromStr)))
        {
            Log.Info("From: " + datePickerValue.Value.ToString());
            dtValueFrom = datePickerValue.Value;
            Log.Info(">>>> From: " + dtValueFrom.ToString());
        }
        else {
             Log.Info("To: " + datePickerValue.Value.ToString());
            dtValueTo = datePickerValue.Value;
            Log.Info(">>>> To: " + dtValueTo.ToString());
        }
    }

    [ExportMethod]
    public void DateTimeToPickerChange(NodeId datePickerNodeId)
    {
        var datePickerObj = LogicObject.Context.GetObject(datePickerNodeId);
        if (datePickerObj == null)
        {
            return;
        }
        var datePickerValue = datePickerObj.GetVariable("Value");
        Log.Info("To: " + datePickerValue.Value.ToString());
        dtValueTo = datePickerValue.Value;
        Log.Info(">>>> To: " + dtValueTo.ToString());
    }
}
