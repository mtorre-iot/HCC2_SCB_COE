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
using System.IO;
using FTOptix.EventLogger;
using HCC2_SCB_2;
#endregion

public class LogTrendExportLogic : BaseNetLogic
{
    private string sep;
    private string[] usbPaths;
    private Label lblExportMessage, lblFromDate, lblToDate, lblEstimatedExportTime, lblExportProgress, lblLastExportDate;
    private DelayedTask GenerateDelayedTask;
    private int delay;
    //private string lblExportMessageStr = "LblExportMessage";
    //private string lblFromDateStr = "SetDialogUnitPanelFrom/LblDate";
    //private string lblToDateStr = "SetDialogUnitPanelTo/LblDate";
    //private string dialogContextBaseStr = "UI/Screens/LogExportPanel/LogTrendDatedExport1/SetDialogUnitPanelFrom/";
    //private string lblEstimatedExportTimeStr = "LblEstimatedExportTime";
    //private string lblExportProgressStr = "LblExportProgress";
    //private string setDialogContextStr = "SetDateDialogContext";
    //private string varNewFromDateStr = "NewFromDate";
    //private string varNewToDateStr = "NewToDate";
    private ProjectFolder project_current;
    private OptixMiscFunctions funcs;
    public override void Start()
    {
        sep = ",";
        usbPaths = new string[] {"D:\\", "E:\\", "F:\\"};
        delay = 5000;
        funcs = new OptixMiscFunctions(Project.Current);
        //
        // Get from/to data as selected by customer
        //
        lblFromDate = funcs.GetLblObjectFromName(Owner, Constants.lblFromDateStr);
        lblToDate = funcs.GetLblObjectFromName(Owner, Constants.lblToDateStr);
        lblExportMessage = funcs.GetLblObjectFromName(Owner, Constants.lblExportMessageStr);
        lblEstimatedExportTime = funcs.GetLblObjectFromName(Owner, Constants.lblEstimatedExportTimeStr);
        lblExportProgress = funcs.GetLblObjectFromName(Owner, Constants.lblExportProgressStr);
		//lblLastExportDate = funcs.GetLblObjectFromName(Project.Current, Constants.lblLastExportDate);

		GenerateDelayedTask = new DelayedTask(CleanMessage, delay, LogicObject);
        //
        // Set default From / To Dates to today!
        //
        lblFromDate.Text = ConcatenateDateToString(DateTime.Now);
        lblToDate.Text = ConcatenateDateToString(DateTime.Now);
		
	}
    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void QueryLogger(NodeId dataLoggerNodeId)
    {
        string selectedUsbPath = "";

		
		bool found = false;
        foreach (string usbPath in usbPaths)
        {
            if (Directory.Exists(usbPath) == true)
            {
                found = true;
                selectedUsbPath = usbPath;
                break;
            }
        }
		//found = true;
		//selectedUsbPath = "C:\\FTlogs";
		if (found == false)
        {
            lblExportMessage.Text = "USB Device is not Mounted. Please verify.";
            // fire delayed task to clean the message label
            GenerateDelayedTask.Start();
            return;
        }
        //
        // get a hold of the datalogger object
        //
        var dataLoggerObj = LogicObject.Context.GetObject(dataLoggerNodeId);
        if (dataLoggerObj == null)
        {
            throw new Exception("Selected DataGrid Object (NodeId: " + dataLoggerNodeId.ToString() + ") is invalid or not found");
        }
        //
        // convert the dates to something usable for the query
        //
        DateTime fromDate = ConvertStringToDateTime(lblFromDate.Text);
        DateTime toDate = ConvertStringToDateTime(lblToDate.Text);
        //
        // Prepare for the query
        //
        var tablesCollection = dataLoggerObj.Owner;
        if (tablesCollection == null)
        {
            throw new Exception("Datalogger object is invalid or was not found");
        }
        // Getting the Store
        var store = tablesCollection.Owner as Store.Store;
        if (store == null)
        {
            throw new Exception("Store object is invalid or was not found");
        }
        // create the DB operation artifact

        var dbo = new DBOperations(store, dataLoggerObj.BrowseName);
        object[,] resultSet;
        string[] header;
        //
        // Issue the query
        //
        dbo.GetAllRowsBetweenDates(fromDate, toDate, out header, out resultSet);   
        //
        // Store to file
        //
        try
        {
            string filePath = ExportCSV (selectedUsbPath, header, resultSet, lblEstimatedExportTime, lblExportProgress);   
            lblExportMessage.Text = "DataLog succesfully saved in file " +  filePath +". Total: " + resultSet.GetLength(0).ToString() + " rows.";
            //Added by Krishna
			//lblLastExportDateStr.Text = ConcatenateDateToString(DateTime.Now);
			//DateTime dateTime= DateTime.Now;
			//lblFromDate.Text= dateTime.ToString();
			//lblLastExportDate.Text= dateTime.ToString();
			// fire delayed task to clean the message label
			//LogTrendInfoPanel test= new LogTrendInfoPanel();
			//         test.Start();
			//         test.lblLastExportDate.Text = dateTime.ToString();
			//GenerateDelayedTask.Start();
		}
        catch (Exception e)
        {
            lblExportMessage.Text = e.Message;
            GenerateDelayedTask.Start();
        }
    }

    private DateTime ConvertStringToDateTime(string dateStr)
    {
        int year, month, day;
        try
        {
            year = Convert.ToInt32(dateStr.Substring(0,4));
            month = Convert.ToInt32(dateStr.Substring(5,2));
            day = Convert.ToInt32(dateStr.Substring(8,2));
        }
        catch (Exception e)
        {
             throw new Exception("Error trying to convert string text to Datetime. Error: " + e.Message);
        }
        DateTime rtn = new DateTime(year, month, day);
        return rtn;
    }

    private void CleanMessage()
    {
        lblExportMessage.Text = "";
    }


    [ExportMethod]
    private string ExportCSV(string usbPath, string[] header, object[,] resultSet, Label lblEstimatedExportTime, Label lblExportProgress)
    {
        //
        // Output file path
        //
        string filePath = Path.Combine(usbPath, "CSVExport-"  + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmm") + ".csv");

        var rowCount = resultSet != null ? resultSet.GetLength(0) : 0;
        var columnCount = header != null ? header.Length : 0;
        
        if (rowCount == 0 || columnCount == 0)
        {
            throw new Exception ("No Records were found. Review the From/To Dates.");
        }
        //
        // Setup the progress counters
        //
        int progress_count = 0; 
        int countToReport = rowCount/10; // this 10 should be configurable...
        //
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
                    progress_count++;
                    //
                    // Report
                    //
                    if (((progress_count % countToReport) == 0) || (progress_count == rowCount))
                    {
                        lblExportProgress.Text = (progress_count*100/rowCount).ToString();
                    }
                }
            }
        }
        catch(Exception e)
        {
            throw new Exception("Unable to export datalog to CSV file '" + filePath + "': " + e.Message);
        }
        return filePath;
    }

    [ExportMethod]
    public void ClearTable(NodeId dataLoggerNodeId)
    {
        var dataLoggerObj = LogicObject.Context.GetObject(dataLoggerNodeId);
        if (dataLoggerObj == null)
        {
            throw new Exception("Selected DataGrid Object (NodeId: " + dataLoggerNodeId.ToString() + ") is invalid or not found");
        }
        //
        // Prepare for the query
        //
        var tablesCollection = dataLoggerObj.Owner;
        if (tablesCollection == null)
        {
            throw new Exception("Datalogger object is invalid or was not found");
        }
        // Getting the Store
        var store = tablesCollection.Owner as Store.Store;
        if (store == null)
        {
            throw new Exception("Store object is invalid or was not found");
        }
        var dbo = new DBOperations(store, dataLoggerObj.BrowseName);
        dbo.CleanDBTable();
    }

    private string ConcatenateDateToString(DateTime dt)
    {
        string year, month, day;
        year = dt.Year.ToString().PadLeft(4, '0');
        month = dt.Month.ToString().PadLeft(2, '0');
        day = dt.Day.ToString().PadLeft(2, '0');

        return year + "-" + month + "-" + day;
    }

    [ExportMethod]
    public void PropagateDateToDialog(string source)
    {
        var setDateDialogContextObj = GetContext(Constants.dialogContextBaseStr + Constants.setDialogContextStr);
        string dateStr = Constants.varNewFromDateStr;
        source = source.ToUpper();
        string outp = "";
        switch (source)
        {
            case "FROM":
                outp = lblFromDate.Text;
                break;
            case "TO":
                outp = lblToDate.Text;
                break;
            default:
                 throw new Exception("Source parameter is invalid. Must be \"FROM\" or \"TO\". Check configuration.");
        }
        setDateDialogContextObj.GetVariable(dateStr).Value = outp;
    }

        private IUAObject GetContext(string path)
        {
          var obj = Project.Current.GetObject(path);
          if (obj == null) 
          {
            throw new Exception("Error trying to calculate the number of days in a month. Check configuration.");
          }
          return obj;
        }
}
