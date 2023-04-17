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
#endregion

public class CreateRealtimeGaugesLogic : BaseNetLogic
{
    [ExportMethod]
    public void CreateRealtimeGauges()
    {
        Log.Info("Create RealtimeGauges started");
        for (int j = 1; j <= 3; j++)
        {
            for (int i = 1; i <= 6; i++)
            {
                var realtimeGaugeWidgetType = Project.Current.Get("UI/Widgets/RealtimeGaugeType");
                if (realtimeGaugeWidgetType == null)
                {
                    throw new Exception("\"UI/Widgets/RealtimeGauge\" NOT found");
                }
                var realtimeGaugeWidgetInstance = InformationModel.MakeObject("RealtimeGauge" +j + "_" + i, realtimeGaugeWidgetType.NodeId);
                Project.Current.Get("UI/Screens/SummaryPanel").Children.Add(realtimeGaugeWidgetInstance);
                var realtimeGaugeWidget = Project.Current.Get<Panel>("UI/Screens/SummaryPanel/RealtimeGauge" +j + "_" + i);
                //realtimeGaugeWidget.SetAlias("RealtimeGaugeAlias", Project.Current.Get("Model/MotorInstances/Motor" + i));
                realtimeGaugeWidget.TopMargin = (realtimeGaugeWidget.Height + 1) * (j - 1) + 2; 
                realtimeGaugeWidget.LeftMargin = (realtimeGaugeWidget.Width + 1) * (i - 1);
            }
        }
        Log.Info("Create RealtimeGauges ended");
    }
}
