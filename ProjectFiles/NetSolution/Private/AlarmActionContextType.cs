using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HCC2_SCB_2", Guid = "7f3d540f6f6b851037cdee82c8e59311")]
public class AlarmActionContextType : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public FTOptix.CoreBase.MethodInvocation OnConfirm
    {
        get
        {
            return (FTOptix.CoreBase.MethodInvocation)Refs.GetObject("OnConfirm");
        }
    }
    public FTOptix.CoreBase.MethodInvocation OnCancel
    {
        get
        {
            return (FTOptix.CoreBase.MethodInvocation)Refs.GetObject("OnCancel");
        }
    }
    public string AlarmName
    {
        get
        {
            return (string)Refs.GetVariable("AlarmName").Value.Value;
        }
        set
        {
            Refs.GetVariable("AlarmName").SetValue(value);
        }
    }
    public IUAVariable AlarmNameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("AlarmName");
        }
    }
    public UAManagedCore.NodeId AlarmNodeId
    {
        get
        {
            return (UAManagedCore.NodeId)Refs.GetVariable("AlarmNodeId").Value.Value;
        }
        set
        {
            Refs.GetVariable("AlarmNodeId").SetValue(value);
        }
    }
    public IUAVariable AlarmNodeIdVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("AlarmNodeId");
        }
    }
    public string AlarmType
    {
        get
        {
            return (string)Refs.GetVariable("AlarmType").Value.Value;
        }
        set
        {
            Refs.GetVariable("AlarmType").SetValue(value);
        }
    }
    public IUAVariable AlarmTypeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("AlarmType");
        }
    }
#endregion
}
