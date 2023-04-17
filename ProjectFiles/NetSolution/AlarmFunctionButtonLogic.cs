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

public class AlarmFunctionButtonLogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void SendAlarmInfoToContext(NodeId contextNodeId, NodeId alarmInfomationNodeId, string alarmType)
    {
        // 
        // Get the AlarmInformation object from NodeId
        //
        AlarmInformationType alarmInformation = (AlarmInformationType) LogicObject.Context.GetObject(alarmInfomationNodeId);
        if (alarmInformation == null)
        {
            throw new Exception("AlarmInformation object with Id: " + alarmInfomationNodeId.ToString() + " NOT found");
        }
        NodeId alarmNodeId = alarmInformation.alarmNodeId;
        var alarmObject = LogicObject.Context.GetObject(alarmNodeId); 
        if (alarmObject == null) 
        {
            throw new Exception("Object with NodeId: " + alarmNodeId.ToString() + " NOT found");
        }
        // get context object
        var contextObject = (AlarmActionContextType) LogicObject.Context.GetObject(contextNodeId); 
        if (contextObject == null) 
        {
            throw new Exception("Object with NodeId: " + contextNodeId.ToString() + " NOT found");
        }
        //
        // Put the alarm name into the context object
        //
        contextObject.AlarmName = alarmObject.BrowseName;
        contextObject.AlarmNodeId = alarmNodeId;
        contextObject.AlarmType = alarmType;
    }   
}
