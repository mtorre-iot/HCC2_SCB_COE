using System;
using System.Threading;
using EAPI;
using FTOptix.HMIProject;
using FTOptix.UI;
using UAManagedCore;

public class UIUpdate
{
    private Config cfg;
    private Eapi eapi;
    private Mutex mtx;
    private ProjectFolder project_current;
    private OptixMiscFunctions fn;
    private PeriodicTask ModelUpdate;
    public UIUpdate(Config cfg, Eapi eapi)
    {
        this.cfg = cfg;
        this.eapi = eapi;
        this.project_current = Project.Current;
        this.fn = new OptixMiscFunctions(project_current);
        this.mtx = new Mutex(false, cfg.mutexName);
    }   
    public void StartReportTask(int period, IUAObject logicObject)
        {
                ModelUpdate = new PeriodicTask(Report, period, logicObject);  
                ModelUpdate.Start();
        }

    private void Report(PeriodicTask task)
    {
        // Get control of the Mutex
        try
        {
            mtx.WaitOne();
        }
        catch (Exception e)
        {
            Log.Error("Report() - error trying to get hold of mutex. Error: " + e.Message);
        }
        //
        // Update the FT Optix model
        //
        UpdateUI();
        //
        // Return mutex
        //
        try
        {
            mtx.ReleaseMutex();
        }
        catch (Exception e)
        {
            Log.Error ("Report() - error trying to release mutex. Error: " + e.Message);
        }
        
    }

    private void UpdateUI()
    {
        //
        // Update Discrete 
        //
        foreach (DiscreteIOInfo dio in eapi.discreteIOsList)
        {
            string diName = "";
            //Log.Info("Num: " + dio.num.ToString() + " Bit Position: " + dio.bitPosition.ToString() + " Iotype: " + dio.ioType.ToString() + 
            // "value: " + dio.value.ToString());
            if (dio.ioType == IoType.dInput)
            {
                diName = cfg.modelDigitalInputsStr;
                string fullName = string.Format(diName, dio.num.ToString("D2"));
                fn.UpdateVariableModelValue(fullName, dio.value.ToString());
            }
            else if (dio.ioType == IoType.dOutput)
            {
                diName = cfg.modelDigitalOutputsStr;   
                string fullName = string.Format(diName, dio.num.ToString("D2"));
                UAValue oldVal = fn.GetVariableModelValue(fullName);
                if ((uint) oldVal != dio.value)
                {
                    eapi.SetLevel((byte)dio.bitPosition, dio.value.ToString(), (uint)dio.idType);
                }
                fn.UpdateVariableModelValue(fullName, dio.value.ToString());
            }
        }
        //
        // Update Analogs
        //
        foreach (AnalogMeasInfo ami in eapi.measurementsList)
        {
            string aiName = cfg.modelMeasurementsStr;
            string fullName = string.Format(aiName, ami.modelName);
            fn.UpdateVariableModelValue(fullName, ami.value.ToString());
        }
    }

    public void ProcessSwitch(IUAObject logicObject, NodeId swNodeId, NodeId modelNodeId)
    {
        // Get the switch object
        Switch swt = fn.GetSwtObjectFromId(logicObject, swNodeId);
        // get the state
        bool val = swt.Checked;
        // Get the Model data object
        IUAVariable doVar = fn.GetVariableModelFromId (modelNodeId);
        //
        // set the state into rgew SCANDB
        //
        int num = 0;
        try
        {
            num = Convert.ToInt32(doVar.BrowseName.Substring(2));
        }
        catch (Exception e)
        {
            Log.Error("ProcessSwitch() - error trying to extract number on " + doVar.BrowseName + " Error: " + e.Message);
        }
        foreach (DiscreteIOInfo dio in eapi.discreteIOsList)
        {
            if ((dio.ioType == IoType.dOutput) && (dio.num == num))
            {
                dio.value = Convert.ToUInt32(val);
            }
        }
        
    }
}