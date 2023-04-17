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

public class AlarmActionDialogLogic : BaseNetLogic
{
    private ExclusiveLevelAlarmController alarmObj;
    private AlarmConfigurationData outputObj;
    private ProjectFolder project;
    private Store.Store store;

    //private string storeNameStr = "DataStores/AlarmsConfig";
    //private string tableNameStr = "AlarmsParams";
    //private string[] columns = { "AlarmName", "AlarmType", "AlarmOperation", "Setpoint", "TripTime", "Restarts", "RestartDelay"};
    //private string[] operations = { "Bypass", "Log", "Stop", "Stop & Log" }; 

    //private string lblAlarmNameStr = "LblAlarmName";
    ////private string lblAlarmTypeStr = "LblAlarmType";
    //private string tbxSetpointStr = "TbxSetpointValue";
    //private string tbxTripTimeStr = "TbxTripTime";
    //private string tbxRestartsStr = "TbxRestarts";
    //private string tbxRestartDelayStr = "TbxRestartDelay";
    
    //private string cbxAlarmsOperationsStr = "CbxAlarmsOperations";
    //private string basePathStr =  "Alarms/DHT/";

    public override void Start()
    {
        //
        // Get the project
        //
        project = FTOptix.HMIProject.Project.Current;
        //
        // create the output object
        //
        outputObj = new AlarmConfigurationData();
        //
        // Get Alarm Name from Alarm Label
        //
        Label lblAlarmName = (Label) Owner.GetObject(Constants.lblAlarmNameStr);
        if (lblAlarmName == null) 
        {
            throw new Exception("Label with name: " + Constants.lblAlarmNameStr + " NOT found");
        }
        string alarmNameStr = Constants.basePathStr + lblAlarmName.Text;
        //
        // Get the alarm object with the provided name
        //
        alarmObj = (ExclusiveLevelAlarmController) Project.Current.GetObject(alarmNameStr);
        if (alarmObj == null) 
        {
            throw new Exception("Alarm object with name: " + alarmNameStr + " NOT found");
        }
        //
        // Get the Alarm Type (HI, LO, HH, LL)
        //
        Label lblAlarmType = (Label) Owner.GetObject(Constants.lblAlarmTypeStr);
        if (lblAlarmType == null) 
        {
            throw new Exception("Label with name: " + Constants.lblAlarmTypeStr + " NOT found");
        }
        //
        // Fill AlarmActionDialogObjects based on the Alarm 
        // Get the setpoint textbox
        //
        TextBox tbxSetpoint = (TextBox) Owner.GetObject(Constants.tbxSetpointStr);
        if (tbxSetpoint == null) 
        {
            throw new Exception("Textbox with name: " + Constants.tbxSetpointStr + " NOT found");
        }
        // Get the tripTime textbox
        //
        TextBox tbxTripTime = (TextBox) Owner.GetObject(Constants.tbxTripTimeStr);
        if (tbxTripTime == null) 
        {
            throw new Exception("Textbox with name: " + Constants.tbxTripTimeStr + " NOT found");
        }
        // Get the Restarts textbox
        //
        TextBox tbxRestarts = (TextBox) Owner.GetObject(Constants.tbxRestartsStr);
        if (tbxRestarts == null) 
        {
            throw new Exception("Textbox with name: " + Constants.tbxRestartsStr + " NOT found");
        }
        // Get the Restart Delay textbox
        //
        TextBox tbxRestartDelay = (TextBox) Owner.GetObject(Constants.tbxRestartDelayStr);
        if (tbxRestartDelay == null) 
        {
            throw new Exception("Textbox with name: " + Constants.tbxRestartDelayStr + " NOT found");
        }
        //
        // Now fill the AlarmOperations combobox  
        //
        ComboBox cbxAlarmsOperations = (ComboBox) Owner.GetObject(Constants.cbxAlarmsOperationsStr);
        if (cbxAlarmsOperations == null) 
        {
            throw new Exception("Combobox with name: " + Constants.cbxAlarmsOperationsStr + " NOT found");
        }
        string selectedOperation = ((LocalizedText) cbxAlarmsOperations.SelectedValue).Text;
        //
        //  Load the Alarm configuration data stored in database (if exists)
        //
        store = project.GetObject(Constants.storeNameStr) as Store.Store;
        if (store == null)
        {
            throw new Exception("Datastore name: " + Constants.storeNameStr + " NOT found");
        }
        // 
        // Let's check if there's already an entry for this Alarm + type
        //
        var dbo = new DBOperations(store, Constants.tableNameStr, Constants.columns);
        object[,] resultSet;
        int numrows = dbo.GetOneRowFromTable(alarmObj.BrowseName, lblAlarmType.Text, out resultSet);
        
        switch (numrows)
        {
            case 0:
                // No previous value. Setup default values
                outputObj.InitializeData(alarmObj.BrowseName, lblAlarmType.Text, selectedOperation);
                break;
            case 1:
                // Get all values from table
                outputObj.LoadOneRowTableData(resultSet);
                break;
            default:
                throw new Exception("Cannot be more that 1 entry in Alarm Configuration Database. Please check configuration.");
        }
        //
        // Store data into the form
        //
        tbxSetpoint.Text = outputObj.setpoint.ToString();
        tbxTripTime.Text = outputObj.tripTime.ToString();
        tbxRestarts.Text = outputObj.restarts.ToString();
        tbxRestartDelay.Text = outputObj.restartDelay.ToString();
        cbxAlarmsOperations.SelectedValue = outputObj.alarmOperation;
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void TbxSetpointChanged(NodeId tbxNodeId)
    {
        // Get the textbox obejct
        TextBox tbx = GetTbxObjectFromId(tbxNodeId);
        // check if text is valid
        double val;
        try
        {
            val = Convert.ToDouble(tbx.Text);
            outputObj.setpoint = val;
        }
        catch (Exception)
        {
            val = 0.0;
            tbx.Text = "";
        }
    }
    [ExportMethod]
    public void TbxTripTimeChanged(NodeId tbxNodeId)
    {
    // Get the textbox obejct
        TextBox tbx = GetTbxObjectFromId(tbxNodeId);
        // check if text is valid
        int val;
        try
        {
            val = Convert.ToInt32(tbx.Text);
            outputObj.tripTime = val;
        }
        catch (Exception)
        {
            val = 0;
            tbx.Text = "";
        }
    }

    [ExportMethod]
    public void TbxRestartsChanged(NodeId tbxNodeId)
    {
    // Get the textbox obejct
        TextBox tbx = GetTbxObjectFromId(tbxNodeId);
        // check if text is valid
        int val;
        try
        {
            val = Convert.ToInt32(tbx.Text);
            outputObj.restarts = val;
        }
        catch (Exception)
        {
            val = 0;
            tbx.Text = "";
        }
    }

    [ExportMethod]
    public void TbxRestartDelayChanged(NodeId tbxNodeId)
    {
    // Get the textbox obejct
        TextBox tbx = GetTbxObjectFromId(tbxNodeId);
        // check if text is valid
        int val;
        try
        {
            val = Convert.ToInt32(tbx.Text);
            outputObj.restartDelay = val;
        }
        catch (Exception)
        {
            val = 0;
            tbx.Text = "";
        }
    }


    private TextBox GetTbxObjectFromId(NodeId tbxNodeId)
    {
        TextBox tbx = (TextBox) LogicObject.Context.GetObject(tbxNodeId);
        if (tbx == null)
        {
            throw new Exception("Textbox with Id: " + tbxNodeId.ToString() + " NOT found");
        }
        return tbx;
    }

    private ComboBox GetCbxObjectFromId(NodeId cbxNodeId)
    {
        ComboBox cbx = (ComboBox) LogicObject.Context.GetObject(cbxNodeId);
        if (cbx == null)
        {
            throw new Exception("ComboBox with Id: " + cbxNodeId.ToString() + " NOT found");
        }
        return cbx;
    }

    [ExportMethod]
    public void ButtonSaveClick()
    {       
        object[,] resultSet; 
        // 
        // Let's check if there's already an enrty for this Alarm + type
        //
        var dbo = new DBOperations(store, Constants.tableNameStr, Constants.columns);

        int numrows = dbo.GetOneRowFromTable(outputObj.alarmName, outputObj.alarmType, out resultSet);
        
        if (numrows == 0)
        {
            var table = store.Tables.Get<Store.Table>(Constants.tableNameStr);
            if (table == null)
            {
                throw new Exception("Table name: " + Constants.tableNameStr + " NOT found");
            }
            //
            // Save the names of the columns of the table to an array
            var rawValues = new object[1, 7];
            rawValues[0,0] = outputObj.alarmName;
            rawValues[0,1] = outputObj.alarmType;
            rawValues[0,2] = outputObj.alarmOperation;
            rawValues[0,3] = outputObj.setpoint.ToString();
            rawValues[0,4] = outputObj.tripTime.ToString();
            rawValues[0,5] = outputObj.restarts.ToString();
            rawValues[0,6] = outputObj.restartDelay.ToString();
            //
            // Insert New record in table
            //
            var table1 = store.Tables.Get<Store.Table>(Constants.tableNameStr);
            // Insert values into table1
            table1.Insert(Constants.columns, rawValues);
        }
        else {
            //
            // Update the existing record
            //
            dbo.UpdateOneRowFromTable(outputObj, out resultSet);
        }
        dbo.GetAllRowsFromTable(out resultSet); // Test
        //
        // Save changes into the actual alarm as well
        //
        //
        // Store the changes in the alarm object itself!
        //
        outputObj.SetAlarmSetpoint(alarmObj);
        outputObj.SetOperation(alarmObj);
    }

    [ExportMethod]
    public void ComboBoxAlarmsOperationsChanged(NodeId cbxNodeId)
    {
    // Get the textbox obejct
        ComboBox cbx = GetCbxObjectFromId(cbxNodeId);
        outputObj.alarmOperation = ((LocalizedText) cbx.SelectedValue).Text;
    }
}
