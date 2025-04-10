using Prism.Mvvm;

namespace SerialHelper.Model;

public class BufferData : BindableBase
{
    private int _sendBufferSize;
    
    public int SendBufferSize
    {
        get => _sendBufferSize;
        set
        {
            _sendBufferSize = value;
            RaisePropertyChanged();
        }
    }
    private int _receiveBufferSize;
    
    public int ReceiveBufferSize
    {
        get => _receiveBufferSize;
        set
        {
            _receiveBufferSize = value;
            RaisePropertyChanged();
        }
    }

}