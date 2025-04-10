using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FastExpressionCompiler.LightExpression;
using Prism.Regions;
using SerialHelper.Extension;
using SerialHelper.Model;
using SerialHelper.ViewModel;

namespace SerialHelper.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        
        
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }


        
        


        
        
    }
}
