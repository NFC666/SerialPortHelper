using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using SerialHelper.Model;

namespace SerialHelper.ViewModel;

public class MainVM : BindableBase
{
    #region 收发数据的数据模型

    //接收区数据
    private string _receiveAreaText;

    public string ReceiveAreaText
    {
        get => _receiveAreaText;
        set
        {
            _receiveAreaText = value;
            RaisePropertyChanged();
        }
    }

    //发送区数据
    private string _senderAreaText;

    public string SenderAreaText
    {
        get => _senderAreaText;
        set
        {
            _senderAreaText = value;
            RaisePropertyChanged();
        }
    }

    #endregion


    #region 自定义类的定义与初始化
    
    
    

    //定义SerialPort类
    private SerialPort _serialPort;

    //初始化我的串口数据类据类
    private SerialPortData _mySerialPortData = new SerialPortData();

    public SerialPortData MySerialPortData
    {
        get => _mySerialPortData;
        set
        {
            _mySerialPortData = value;
            RaisePropertyChanged();
        }
    }
    
    // private BufferData _bufferData = new BufferData();
    //
    // public BufferData BufferData
    // {
    //     get => _bufferData;
    //     set
    //     {
    //         _bufferData = value;
    //         RaisePropertyChanged();
    //     }
    // }

    #endregion


    #region 处理按钮事件命令

    public DelegateCommand<Button> OpenSerialPortCommand { get; set; }
    public DelegateCommand SendMessageCommand { get; set; }
    // public DelegateCommand SelectFileCommand { get; set; }
    // public DelegateCommand ClearReceiveBufferCommand { get; set; }
    // public DelegateCommand ClearSendBufferCommand { get; set; }

    #endregion


    public MainVM()
    {
        InitSerialPortList();
        OpenSerialPortCommand = new DelegateCommand<Button>(Btn_OpenSerialPort);
        // SelectFileCommand = new DelegateCommand(SelectFile);
        SendMessageCommand = new DelegateCommand(SendMessage);
        // ClearReceiveBufferCommand = new DelegateCommand(ClearReceiveBuffer);
        // ClearSendBufferCommand = new DelegateCommand(ClearSendBuffer);
    }


    #region 串口操作相关方法

    /// <summary>
    /// 初始化Ports列表
    /// </summary>
    private void InitSerialPortList()
    {
        var ports = SerialPort.GetPortNames();

        MySerialPortData.PortList = new ObservableCollection<string>(ports);
        
    }


    /// <summary>
    /// 与前端交互控制串口
    /// </summary>
    /// <param name="btnOpenSerialPort"></param>
    private void Btn_OpenSerialPort(Button btnOpenSerialPort)
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            //关闭
            if (CloseSerialPort())
            {
                btnOpenSerialPort.Content = "打开串口";
                btnOpenSerialPort.Background = Brushes.Red;
            }
        }
        else
        {
            if (OpenSerialPort())
            {
                btnOpenSerialPort.Content = "关闭串口";
                btnOpenSerialPort.Background = Brushes.Green;
            }
        }
    }

    /// <summary>
    /// 关闭串口
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private bool CloseSerialPort()
    {
        bool flag = false;

        try
        {
            _serialPort.Close();
            flag = true;
        }
        catch (Exception ex)
        {
            throw;
        }


        _serialPort.DataReceived -= MySerialPortOnDataReceived;

        return flag;
    }

    /// <summary>
    /// 实际打开串口
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private bool OpenSerialPort()
    {
        bool flag = false;
        try
        {
            _serialPort = new SerialPort();
            _serialPort.BaudRate = MySerialPortData.BaudRate;
            _serialPort.StopBits = MySerialPortData.ConvertToStopBits(MySerialPortData.StopBits);
            _serialPort.Parity = MySerialPortData.ConvertToParity(MySerialPortData.Parity);
            _serialPort.PortName = MySerialPortData.PortName;
            _serialPort.DataBits = MySerialPortData.DataBits;
            _serialPort.Open();
            flag = true;

            _serialPort.DataReceived += MySerialPortOnDataReceived;
            
        }
        catch (Exception ex)
        {
            throw;
            return false;
        }

        return flag;
    }



    #endregion


    #region 缓冲区操作

    /// <summary>
    /// 清除发送缓冲区
    /// </summary>
    private void ClearSendBuffer()
    {
        _serialPort.DiscardInBuffer();
        // BufferData.SendBufferSize = 0;
    }

    
    /// <summary>
    /// 清除接收缓冲区
    /// </summary>
    private void ClearReceiveBuffer()
    {
        ReceiveAreaText = string.Empty;
        _serialPort.DiscardInBuffer();
        // BufferData.ReceiveBufferSize = 0;
    }

    #endregion


    #region 收发数据方法和事件

    /// <summary>
    /// 接收数据的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MySerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        //获取缓冲区字节数
        int count = _serialPort!.BytesToRead;
        byte[] buffer = new byte[count];
        //串口数据读取到缓冲区
        _serialPort.Read(buffer, 0, count);

        ReceiveAreaText += System.Text.Encoding.UTF8.GetString(buffer) + "\r\n";

        // BufferData.ReceiveBufferSize += count;
    }
    
    /// <summary>
    /// 无参发数据——获得输入框的数据并发送
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void SendMessage()
    {
        try
        {
            //获取文本框中获取的数据
            byte[] bytes = Encoding.UTF8.GetBytes(SenderAreaText);
            _serialPort.Write(bytes, 0, bytes.Length);
            // BufferData.SendBufferSize += bytes.Length;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 有参发送数据——txt根据文件发送
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="Exception"></exception>
    // private async void SendMessage(string message)
    // {
    //     try
    //     {
    //         //获取文本框中获取的数据
    //         byte[] bytes = Encoding.UTF8.GetBytes(message);
    //         _serialPort.Write(bytes, 0, bytes.Length);
    //         Thread.Sleep(100);
    //         // BufferData.SendBufferSize += bytes.Length;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw;
    //     }
    // }

    /// <summary>
    /// 选择文件方法
    /// </summary>
    // private void SelectFile()
    // {
    //     OpenFileDialog openFileDialog = new OpenFileDialog
    //     {
    //         Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
    //         Title = "选择一个TXT文件"
    //     };
    //
    //     if (openFileDialog.ShowDialog() == true)
    //     {
    //         string filePath = openFileDialog.FileName;
    //         ParseFile(filePath);
    //     }
    // }

    /// <summary>
    /// 解析文件并发送
    /// </summary>
    /// <param name="filePath"></param>
    // private async void ParseFile(string filePath)
    // {
    //     try
    //     {
    //         if (filePath.EndsWith("txt"))
    //         {
    //             string showContent = "确认发送以下内容到下位机吗？\r\n";
    //             using (StreamReader reader = new StreamReader(filePath))
    //             {
    //                 if (await reader.ReadLineAsync() != null)
    //                 {
    //                     string line = await reader.ReadLineAsync();
    //                     for (int i = 0; i < 3; i++)
    //                     {
    //                         showContent += line + "\r\n";
    //                     }
    //                 }
    //
    //             }
    //
    //             var res = MessageBox.Show(showContent, "确认发送？", MessageBoxButton.YesNo, MessageBoxImage.Question);
    //             if (res == MessageBoxResult.Yes)
    //             {
    //                 using StreamReader reader = new StreamReader(filePath);
    //                 string line;
    //                 while ((line = await reader.ReadLineAsync()) != null)
    //                 {
    //                     SendMessage(line);
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             MessageBox.Show("请选择txt格式文件","错误",MessageBoxButton.OK, MessageBoxImage.Error);
    //         }
    //         // string content = File.ReadAllText(filePath);
    //         // MessageBox.Show("文件内容:\n" + content, "文件解析", MessageBoxButton.OK, MessageBoxImage.Information);
    //     }
    //     catch (Exception ex)
    //     {
    //         MessageBox.Show("读取文件失败: " + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
    //     }
    // }
    #endregion





}