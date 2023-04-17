using System;
using System.Collections.Generic;
using FTOptix.Alarm;
using FTOptix.EventLogger;
using FTOptix.UI;

public class AlarmConfigurationData 
    {
        public string alarmName {get; set; }
        public string alarmType {get; set; }
        public string alarmOperation { get; set; }
        public double? setpoint { get; set; }
        public int tripTime { get; set; }
        public int restarts { get; set; }
        public int restartDelay { get; set; }
        public int startBypass { get; set; }
    
    public double? SelectAlarmSetpoint (ExclusiveLevelAlarmController alarmObj, string alarmType)
        {
            double? rtn;
            switch (alarmType)
            {
                case "HI":
                    rtn = alarmObj.HighLimit;
                    break;
                case "HH":
                    rtn = alarmObj.HighHighLimit;
                    break;
                case "LO":
                    rtn = alarmObj.LowLimit;
                    break;
                case "LL":
                    rtn = alarmObj.LowLowLimit;
                    break;
                default:
                    rtn = null;
                    break;
            }
            return rtn;
        }

    public void SetOperation(ExclusiveLevelAlarmController alarmObj, string operation)
    {
        switch (operation.ToUpper())
        {
            case "LOG":
            case "STOP & LOG":
            case "STOP":
                alarmObj.Enabled = true;
                break;
            case "BYPASS":
            default:
                alarmObj.Enabled = false;
                break;
        }
    }

    public void SetOperation(ExclusiveLevelAlarmController alarmObj)
    {
        SetOperation(alarmObj, this.alarmOperation);
    }

    public void SetAutoAcknowledge(ExclusiveLevelAlarmController alarmObj, bool state)
    {
        alarmObj.AutoAcknowledge = state;
        alarmObj.AutoConfirm = state;
    }

    public void SetAlarmSetpoint (ExclusiveLevelAlarmController alarmObj, string alarmType, double? newValue)
        {
            switch (alarmType)
            {
                case "HI":
                    alarmObj.HighLimit = newValue;
                    break;
                case "HH":
                    alarmObj.HighHighLimit = newValue;
                    break;
                case "LO":
                    alarmObj.LowLimit = newValue;
                    break;
                case "LL":
                    alarmObj.LowLowLimit = newValue;
                    break;
                default:
                    break;
            }
        }

    public void SetAlarmSetpoint(ExclusiveLevelAlarmController alarmObj) 
    {
        SetAlarmSetpoint(alarmObj, this.alarmType, this.setpoint);
    }
        public void InitializeData(string alarmName, string alarmType, string alarmOperation)
        {
            this.alarmName = alarmName;
            this.alarmType = alarmType;
            this.alarmOperation = alarmOperation;
            this.setpoint = null;
            this.tripTime = 0;
            this.restarts = 0;
            this.restartDelay = 0;
        }

        public void LoadOneRowTableData(object[,] resultSet)
        {
            this.alarmName = resultSet[0,0] as string;
            this.alarmType = resultSet[0,1] as string;
            this.alarmOperation = resultSet[0,2] as string;
            try
            {
                this.setpoint = Convert.ToDouble(resultSet[0,3] as string);
            }
            catch (Exception)
            {
                this.setpoint = null;
            }
            try
            {
                this.tripTime = Convert.ToInt32(resultSet[0,4] as string);
            }
            catch (Exception)
            {
                this.tripTime = 0;
            }
            try
            {
                this.restarts = Convert.ToInt32(resultSet[0,5] as string);
            }
            catch (Exception)
            {
                this.restarts = 0;
            }
            try
            {
                this.restartDelay = Convert.ToInt32(resultSet[0,6] as string);
            }
            catch (Exception)
            {
                this.restartDelay = 0;
            }
        }
        public static List<AlarmConfigurationData> LoadOneAllRowsTableData(object[,] resultSet)
        {
            List<AlarmConfigurationData> rtn = new List<AlarmConfigurationData>();

            for (int i=0; i<resultSet.GetLength(0) ; i++)
            {
                int a = resultSet.GetLength(0);
                int b = resultSet.GetLength(1);

                object[,] g = new object[1, resultSet.GetLength(1)];
                for (int j = 0; j <resultSet.GetLength(1); j++)
                {
                    g[0, j] = resultSet[i,j];
                }
                
                var alarmObj = new AlarmConfigurationData();
                alarmObj.LoadOneRowTableData(g);
                rtn.Add(alarmObj);
            }
            return rtn;
        }
    }
