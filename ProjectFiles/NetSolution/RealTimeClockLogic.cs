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

public class RealTimeClockLogic : BaseNetLogic
{
    public OptixMiscFunctions funcs;
    private Label lblRealTimeClock;
    private Label lblRealTimeDate;
    //private string lblRealTimeClockStr = "UI/MainWindow/Main/TopBar/LblRealTimeClock";
    //private string dateFormat = "yyyy-MM-dd HH:mm:ss";
    public override void Start()
    {
        funcs = new OptixMiscFunctions();
        // Get the label object
        lblRealTimeClock = funcs.GetLblObjectFromName(Project.Current, Constants.lblRealTimeClockStr);
        lblRealTimeDate=funcs.GetLblObjectFromName(Project.Current,Constants.lblRealTimeDateStr);
        // Start the clock periodical task
        var clockTask = new PeriodicTask(RefreshClock, 1000, LogicObject);
        clockTask.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    private void RefreshClock(PeriodicTask task)
    {
        //
        // Get current date and time
        //
        DateTime dt = DateTime.Now;
        //
        // Update this label with the current time.
        //
        lblRealTimeClock.Text = dt.ToString(Constants.TimeFormat);
		lblRealTimeDate.Text = dt.ToString(Constants.dateFormat);

	}
}
