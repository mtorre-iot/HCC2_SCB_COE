using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HCC2_SCB_2", Guid = "3d0f8362fced1c68a4c1b6871cf17b18")]
public class SetDateDialogContextType : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public FTOptix.CoreBase.MethodInvocation onConfirm
    {
        get
        {
            return (FTOptix.CoreBase.MethodInvocation)Refs.GetObject("onConfirm");
        }
    }
    public FTOptix.CoreBase.MethodInvocation onCancel
    {
        get
        {
            return (FTOptix.CoreBase.MethodInvocation)Refs.GetObject("onCancel");
        }
    }
    public UAManagedCore.LocalizedText DateType
    {
        get
        {
            return (UAManagedCore.LocalizedText)Refs.GetVariable("DateType").Value.Value;
        }
        set
        {
            Refs.GetVariable("DateType").SetValue(value);
        }
    }
    public IUAVariable DateTypeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("DateType");
        }
    }
    public string NewFromDate
    {
        get
        {
            return (string)Refs.GetVariable("NewFromDate").Value.Value;
        }
        set
        {
            Refs.GetVariable("NewFromDate").SetValue(value);
        }
    }
    public IUAVariable NewFromDateVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("NewFromDate");
        }
    }
    public string NewToDate
    {
        get
        {
            return (string)Refs.GetVariable("NewToDate").Value.Value;
        }
        set
        {
            Refs.GetVariable("NewToDate").SetValue(value);
        }
    }
    public IUAVariable NewToDateVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("NewToDate");
        }
    }
#endregion
}
