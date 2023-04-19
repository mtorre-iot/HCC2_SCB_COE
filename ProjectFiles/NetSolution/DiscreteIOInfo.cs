using EAPI;

public class DiscreteIOInfo
{
    public IoType ioType;
    public GPIO.ID_TYPE idType;
    public int num;
    public int bitPosition; 
    public uint value;
}

public enum IoType 
{
    unknown = -1,
    dInput = 0,
    dOutput = 2
};
