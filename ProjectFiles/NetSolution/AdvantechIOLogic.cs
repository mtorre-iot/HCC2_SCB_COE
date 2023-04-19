#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.Core;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using EAPI;
using System.Collections.Generic;

#endregion

public class AdvantechIOLogic : BaseNetLogic
{
    private Eapi eapi;
    private UIUpdate report; 
    private Config cfg;
   
    public override void Start()
    {
        // Load configuration
        cfg = new Config();
        cfg.ReadConfigurationVariables(LogicObject);
        eapi = new Eapi(cfg);
        report = new UIUpdate(cfg, eapi);
        eapi.LoadAll();
        Scan(); // start scanning
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void Scan()
    {
        if (eapi.IsInitialized == true)
        {
            Log.Info("Scan() - Library is initialized.");
            //
            // Get periods - check against minimums
            //
            int scanPeriod = cfg.ScanPeriodVariable.Value;
            if (scanPeriod < cfg.MinimumScanPollPeriod) 
            {
                scanPeriod = cfg.MinimumScanPollPeriod;
            }
            int reportPeriod = cfg.ReportPeriodVariable.Value;
            if (reportPeriod < cfg.MinimumReportPeriod) 
            {
                reportPeriod = cfg.MinimumReportPeriod;
            }
            //
            // Start all periodic tasks
            //
            try
            {
                eapi.StartScanTask(scanPeriod, LogicObject);
                report.StartReportTask(reportPeriod, LogicObject);
            }
            catch (Exception e)
            {
                Log.Error ("Scan() - unable to start periodic tasks - Error: " + e.Message);
            }
        }
        else
        {
            Log.Error("Scan() - Library was not initialized.");
        }
    }

    [ExportMethod]
    public void SwitchChanged(NodeId swNodeId, NodeId modNodeId)
    {
        report.ProcessSwitch(LogicObject, swNodeId, modNodeId);
    }

}
