using UAManagedCore;

public class Config
{
    //
    // Model configuration
    //
    public readonly string modelDigitalInputsStr = "Model/GPIO/DI/DI{0}";
    public readonly string modelDigitalOutputsStr = "Model/GPIO/DO/DO{0}";
    public readonly string modelMeasurementsStr = "Model/MEAS/{0}";
    public readonly string[] measModelNameStr = {"CPUTemp", "SystemTemp"};
    // 
    /// UI Configuration
    //
    public readonly string stateOnColor = "4294901760";  //#FFFF0000 (RED)
    public readonly string stateOffColor = "4278255360"; // #FF00FF00 (GREEN)
    public readonly string DigitalTypeStr = "UI/Panel1/DIOValue1/LblsDtype";
    public readonly string DigitalTypeNumr = "UI/Panel1/DIOValue1/LblDtypeNum";
    public readonly string DigitalValueStr = "UI/Panel1/DIOValue1/LblValue";

    //
    // Misc configuration
    //
    public readonly string mutexName = "advantech_mutex";
    //
    // Application configuration
    //
    public readonly string ScanPollPeriodStr = "ScanPeriod";
    public readonly string ReportPeriodStr = "ReportPeriod";
    public readonly int MinimumScanPollPeriod = 100;
    public readonly int MinimumReportPeriod = 100;
    public IUAVariable ScanPeriodVariable;
    public IUAVariable ReportPeriodVariable;
    public void ReadConfigurationVariables(IUAObject logicObject)
    {
        ScanPeriodVariable = logicObject.GetVariable(ScanPollPeriodStr);
        if (ScanPeriodVariable == null)
        {
            throw new CoreConfigurationException("Minimum Scan Period variable not found");
        }
        ReportPeriodVariable = logicObject.GetVariable(ReportPeriodStr);
        if (ReportPeriodVariable == null)
        {
            throw new CoreConfigurationException("Minimum Report Period variable not found");
        }
    }

}