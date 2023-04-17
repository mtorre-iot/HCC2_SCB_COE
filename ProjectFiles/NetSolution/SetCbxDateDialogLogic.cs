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

public class SetCbxDateDialogLogic : BaseNetLogic
{
    //private string cbxYearStr = "CbxYear";
    //private string cbxMonthStr = "CbxMonth";
    //private string cbxDayStr = "CbxDay";
    //private string daysBase = "Model/InternalConfiguration/Days";
    //private string dialogContextBaseStr = "UI/Screens/LogExportPanel/LogTrendDatedExport1/SetDialogUnitPanelFrom/";
    //private string setDialogContextStr = "SetDateDialogContext";
    //private string varNewFromDateStr = "NewFromDate";
    //private string varNewToDateStr = "NewToDate";
    private ComboBox cbxYear, cbxMonth, cbxDay;
    private TextBox tbxDateField, tbxYearField, tbxMonthField, tbxDayField;
    private IUAObject setDateDialogContextObj;
    //private int[] daysOfMonth = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    //private int[] daysOfMonthLeap = {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

    
    public override void Start()
    {   
        setDateDialogContextObj = GetContext(Constants.dialogContextBaseStr + Constants.setDialogContextStr);
        string dateStr = Constants.varNewFromDateStr;
        string newDate =  setDateDialogContextObj.GetVariable(dateStr).Value;
        DateTime date =  ConvertStringToDateTime(newDate);
        
        cbxYear = (ComboBox) Owner.GetObject(Constants.cbxYearStr);
        if (cbxYear == null) 
        {
            throw new Exception("Combobox object with name: " + Constants.cbxYearStr + " NOT found");
        }
        cbxMonth = (ComboBox) Owner.GetObject(Constants.cbxMonthStr);
        if (cbxMonth == null) 
        {
            throw new Exception("Combobox object with name: " + Constants.cbxMonthStr + " NOT found");
        }
        cbxDay = (ComboBox) Owner.GetObject(Constants.cbxDayStr);
        if (cbxDay == null) 
        {
            throw new Exception("Combobox object with name: " + Constants.cbxDayStr + " NOT found");
        }
        var daysObj = Project.Current.GetObject("Model/InternalConfiguration/Days" + Constants.daysOfMonth[date.Month-1].ToString());
        if (daysObj == null) 
        {
            throw new Exception("Error trying to calculate the number of days in a month. Check configuration.");
        }
        cbxYear.SelectedValue = (object) date.Year.ToString();
        cbxMonth.SelectedValue = (object) date.Month.ToString();
        cbxDay.Model = daysObj.NodeId;
        cbxDay.SelectedValue = (object) date.Day.ToString();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void CbxMonthSelectionChanged()
    {
        // Try to build the number of days depending on the selected month
        int selectedMonth;
        try
        {
            selectedMonth = Convert.ToInt32(((LocalizedText) cbxMonth.SelectedValue).Text);
        }
        catch (Exception e) 
        {
            throw new Exception("Errors in the month to day conversion. Error: " + e.Message);
        }
        var daysObj = Project.Current.GetObject("Model/InternalConfiguration/Days" + Constants.daysOfMonth[selectedMonth-1].ToString());
        if (daysObj == null) 
        {
            throw new Exception("Error trying to calculate the number of days in a month. Check configuration.");
        }
        // create a new set of number into CbxDay according to the selected month.
        cbxDay.Model = daysObj.NodeId;
    }

[ExportMethod]
    public void SetPropagateNewDate()
    {
        setDateDialogContextObj = GetContext(Constants.dialogContextBaseStr + Constants.setDialogContextStr);
        string dateStr = Constants.varNewFromDateStr;
 
        var newDate =  setDateDialogContextObj.GetVariable(dateStr).Value;
        setDateDialogContextObj.GetVariable(dateStr).Value = ConcatenateCbxToDate();
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

    private string ConcatenateCbxToDate()
    {
        //return cbxYear.SelectedValue + "-" + cbxMonth.SelectedValue + "-" + cbxDay.SelectedValue;
        string year, month, day;
        year = ConvertCbxSelectedValueToString(cbxYear).PadLeft(4, '0');
        month = ConvertCbxSelectedValueToString(cbxMonth).PadLeft(2, '0');
        day = ConvertCbxSelectedValueToString(cbxDay).PadLeft(2, '0');


        return year + "-" + month + "-" + day;
    }
    private string ConvertCbxSelectedValueToString(ComboBox cbx)
    {
    string rtn;
    try
        {
            rtn = (string) cbx.SelectedValue;
        }
        catch 
        {
            rtn = ((LocalizedText) cbx.SelectedValue).Text;
        }
        return rtn;
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

}
