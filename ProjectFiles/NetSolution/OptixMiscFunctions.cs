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
public class OptixMiscFunctions
{
    private ProjectFolder project_current;
    public Label GetLblObjectFromId(IUAObject logicObject, NodeId lblNodeId)
    {
        Label lbl = (Label) logicObject.Context.GetObject(lblNodeId);
        if (lbl == null)
        {
            throw new Exception("GetTbxObjectFromId - Label with Id: " + lblNodeId.ToString() + " was not found");
        }
        return lbl;
    }

    public Label GetLblObjectFromName(ProjectFolder project_current, string lblName)
    {
        Label lbl = (Label) project_current.GetObject(lblName);
        if (lbl == null)
        {
            throw new Exception("GetTbxObjectFromName - Label with name: " + lblName + " was not found");
        }
        return lbl;
    }

 public Label GetLblObjectFromName(IUANode owner, string lblName)
    {
        Label lbl = (Label) owner.GetObject(lblName);
        if (lbl == null)
        {
            throw new Exception("GetTbxObjectFromName - Label with name: " + lblName + " was not found");
        }
        return lbl;
    }

}
