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
using HCC2_SCB_2;
#endregion

public class AlarmUnitListPanelLogic : BaseNetLogic
{
    private ExclusiveLevelAlarmController alarmObj;
    private AlarmConfigurationData alarmData;
    private Label lblSignal;
    private Label lblAlarmType;
    private Label lblValueSignal;
    private Button btnAlarmFunctionButton;
    private string alarmType;
    //private string lblSignalStr = "LblSignal";
    //private string lblValueSignalStr = "LblValueSignal";
    //private string lblAlarmTypeStr = "LblAlarmType";
    //private string btnAlarmFunctionButtonStr = "AlarmFunctionButton";
    //private string alarmInformationStr = "AlarmInformation";
    //private string storeNameStr = "DataStores/AlarmsConfig";
    //private string tableNameStr = "AlarmsParams";
    //private string[] columns = { "AlarmName", "AlarmType", "AlarmOperation", "Setpoint", "TripTime", "Restarts", "RestartDelay"};
    //private string[] operations = { "Bypass", "Log", "Stop", "Stop & Log" }; 

    public override void Start()
    {
        alarmData = new AlarmConfigurationData();
        //
        // Get the alarm NodeId from the AlarmInformation object
        //
        AlarmInformationType alarmInformation = (AlarmInformationType) Owner.GetObject(Constants.alarmInformationStr);
        if (alarmInformation == null)
        {
            throw new Exception("AlarmInformation object with name: " + Constants.alarmInformationStr + " NOT found");
        }
        NodeId alarmNodeId = alarmInformation.alarmNodeId;
        //
        // Now go grab the alarm itself
        //
        alarmObj = (ExclusiveLevelAlarmController) LogicObject.Context.GetObject(alarmNodeId); 
        if (alarmObj == null) 
        {
            throw new Exception("Object with NodeId: " + alarmNodeId.ToString() + " NOT found");
        }
        ///////////////////////////////////////////////////////
        // Grab the value that is stored in the AlarmDB
        ///////////////////////////////////////////////////////
        //
        // Figure out what type of alarm is configured
        //
        lblAlarmType = (Label) Owner.GetObject(Constants.lblAlarmTypeStr);
        if (lblAlarmType == null) 
        {
            throw new Exception("Label with name: " + Constants.lblAlarmTypeStr + " NOT found");
        }
        alarmType = lblAlarmType.Text;
        var outputObj =  new AlarmConfigurationData();
        var store = Project.Current.GetObject(Constants.storeNameStr) as Store.Store;
        if (store == null)
        {
            throw new Exception("Datastore name: " + Constants.storeNameStr + " NOT found");
        }
        var dbo = new DBOperations(store, Constants.tableNameStr, Constants.columns);
        object[,] resultSet;
        int numrows = dbo.GetOneRowFromTable(alarmObj.BrowseName, alarmType, out resultSet);
        
        switch (numrows)
        {
            case 0:
                // No previous value. Setup default values
                string selectedOperation = Constants.operations[0]; // assume Bypass
                outputObj.InitializeData(alarmObj.BrowseName, alarmType, selectedOperation);
                break;
            case 1:
                // Get all values from table
                outputObj.LoadOneRowTableData(resultSet);
                break;
            default:
                throw new Exception("Cannot be more that 1 entry in Alarm Configuration Database. Please check configuration.");
        }
        //
        // Write down the alarm Description to local label
        //
        lblSignal = (Label) Owner.GetObject(Constants.lblSignalStr);
        if (lblSignal == null) 
        {
            throw new Exception("Label with name: " + Constants.lblSignalStr + " NOT found");
        }
        //lblSignal.Text = alarmObj.Description.Text;
        lblSignal.Text = outputObj.alarmName;
        //
        // Setup the Setpoint label
        //
        lblValueSignal = (Label) Owner.GetObject(Constants.lblValueSignalStr);
        if (lblValueSignal == null) 
        {
            throw new Exception("Label with name: " + Constants.lblValueSignalStr + " NOT found");
        }
        //
        // Get the action Button as well
        //
        btnAlarmFunctionButton = (Button) Owner.GetObject(Constants.btnAlarmFunctionButtonStr);
        if (btnAlarmFunctionButton == null) 
        {
            throw new Exception("Button with name: " + Constants.btnAlarmFunctionButtonStr + " NOT found");
        }
        btnAlarmFunctionButton.Text = outputObj.alarmOperation;
        //
        // Get the appropriate value from Alarm configuration based on the alam type
        //
        double? alarmSetpoint = outputObj.setpoint;
        //
        // write it down the setpoint in the form
        //
        lblValueSignal.Text = alarmSetpoint.ToString();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void ReturnFromDialogOnChange()
    {
        var project = FTOptix.HMIProject.Project.Current;
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
        // Let's check if there's already an enrty for this Alarm + type
        //
        object[,] resultSet;
        int numrows = dbo.GetOneRowFromTable(alarmObj.BrowseName, alarmType, out resultSet);
        if (numrows == 1)
        {
            //
            // Get stored data in DB and update the form
            //
            lblSignal.Text = resultSet[0,0] as string;
            lblAlarmType.Text = resultSet[0,1] as string;
            btnAlarmFunctionButton.Text = resultSet[0,2] as string;
            lblValueSignal.Text = resultSet[0,3] as string;
            //
            // Now store the values in the alarm objet itself!
            //
            double? val;
            try
            {
                val = Convert.ToDouble(lblValueSignal.Text);
            }
            catch (Exception)
            {
                val = null;
            }
            alarmData.SetAlarmSetpoint(alarmObj, lblAlarmType.Text, val);
        }
        else if (numrows > 1) 
        {
            throw new Exception("Error, there's more than one instance of an alarm in the AlarmDB. Pleasde check.");
        }
        
    }
}
