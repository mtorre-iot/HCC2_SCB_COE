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
using HCC2_SCB_2;
#endregion

public class UpdateNewTimeLogic : BaseNetLogic
{
    //private string dialogContextBaseStr = "UI/Screens/LogExportPanel/LogTrendDatedExport1/SetDialogUnitPanelFrom/";
    //private string setDialogContextStr = "SetDateDialogContext";
    //private string varNewFromDateStr = "NewFromDate";
    //private string varNewToDateStr = "NewToDate";
    //private string LblNewDateStr = "LblDate";
    private Label lblNewDate;
    public override void Start()
    {
        lblNewDate = (Label) Owner.GetObject(Constants.LblNewDateStr);
        if (lblNewDate == null) 
        {
            throw new Exception("Label with name: " +  Constants.LblNewDateStr + " NOT found");
        }
        DateTime today = DateTime.Now;
        lblNewDate.Text = today.Year.ToString("D4") + "-" + today.Month.ToString("D2") + "-" + today.Day.ToString("D2");
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

[ExportMethod]
    public void GetNewTimeInvoke()
    {
        var setDateDialogContextObj = GetContext(Constants.dialogContextBaseStr + Constants.setDialogContextStr);
        var newDate =  setDateDialogContextObj.GetVariable(Constants.varNewFromDateStr);
        // Got the value. Now store it into the corresponding label
        lblNewDate.Text = newDate.Value;
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
