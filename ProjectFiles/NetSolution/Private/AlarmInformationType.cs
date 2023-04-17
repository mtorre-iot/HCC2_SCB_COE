using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HCC2_SCB_2", Guid = "64d3ac9520bd32511b9ec06ee747fc78")]
public class AlarmInformationType : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public UAManagedCore.NodeId alarmNodeId
    {
        get
        {
            return (UAManagedCore.NodeId)Refs.GetVariable("alarmNodeId").Value.Value;
        }
        set
        {
            Refs.GetVariable("alarmNodeId").SetValue(value);
        }
    }
    public IUAVariable alarmNodeIdVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("alarmNodeId");
        }
    }
#endregion
}
