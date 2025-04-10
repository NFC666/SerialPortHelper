using System.Collections.ObjectModel;
using System.IO.Ports;
using Prism.Mvvm;

namespace SerialHelper.Model;

public class SerialPortData : BindableBase
{
    public string PortName { get; set; } = "COM1";
    

    public int BaudRate { get; set; } = 115200;
    public string Parity { get; set; } = "无校验";
    public int DataBits { get; set; } = 8;
    public string StopBits { get; set; } = "One";
    
    public Parity ConvertToParity(string parity)
    {
        switch (parity)
        {
            case "无校验":
                return System.IO.Ports.Parity.None;
            case "奇校验":
                return System.IO.Ports.Parity.Odd;
            case "偶校验":
                return System.IO.Ports.Parity.Even;
            default:
                return System.IO.Ports.Parity.None;
        }
    }
    public StopBits ConvertToStopBits(string stopBits)
    {
        switch (stopBits)
        {
            case "One":
                return System.IO.Ports.StopBits.One;
            case "Two":
                return System.IO.Ports.StopBits.Two;
            case "None":
                return System.IO.Ports.StopBits.None;
            default:
                return System.IO.Ports.StopBits.One;
        }
    }
    
    
    
    
    public ObservableCollection<int> BaudRateOptions { get; } = new ObservableCollection<int>
    {
        9600, 19200, 38400, 57600, 115200
    };
    public ObservableCollection<int> DataBitsOptions { get; } = new ObservableCollection<int>
    {
        8, 7, 6, 5
    };
    
    public ObservableCollection<string> ParityOptions { get; } = new ObservableCollection<string>
    {
        "无校验",
        "奇校验",
        "偶校验"
    };
    public ObservableCollection<string> StopBitsOptions { get; } = new ObservableCollection<string>
    {
        "One",
        "Two",
        "None"
    };
    
    
    //让PortList在UI变化时做出反应

    private ObservableCollection<string> _portList;

    public ObservableCollection<string> PortList
    {
        get => _portList;
        set
        {
            _portList = value;
            RaisePropertyChanged();
        }
    }
}