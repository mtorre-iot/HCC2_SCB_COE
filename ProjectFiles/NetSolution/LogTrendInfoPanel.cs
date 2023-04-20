#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.DataLogger;
using FTOptix.Alarm;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.EventLogger;
using FTOptix.Store;
using FTOptix.SQLiteStore;
using FTOptix.OPCUAServer;
using FTOptix.Modbus;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.UI;
using FTOptix.Core;
using HCC2_SCB_2;
#endregion

public class LogTrendInfoPanel : BaseNetLogic
{
	

	//public Label lblLastExportDate;
	//private OptixMiscFunctions funcs;

	public override void Start()
    {
		
	 //   funcs = new OptixMiscFunctions(Project.Current);
		//lblLastExportDate = funcs.GetLblObjectFromName(Owner, Constants.lblLastExportDate);
		//lblLastExportDate.Text= DateTime.Now.ToString();

		// Insert code to be executed when the user-defined logic is started
	}

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
