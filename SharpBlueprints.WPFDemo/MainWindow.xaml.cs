using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpBlueprints.Graph;
using SharpBlueprints.WPF.ViewModels;

namespace SharpBlueprints.WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var nodeGraph = new NodeGraph();
            nodeGraph.Nodes.Add(new Node() { Name = "Start", PositionX = 10, PositionY = 10 });
            nodeGraph.Nodes.Add(new Node() { Name = "Do Stuff", PositionX = 210, PositionY = 110 });
            nodeGraph.Nodes.Add(new Node() { Name = "End", PositionX = 410, PositionY = 50 });
            DataContext = new NodeGraphViewModel(nodeGraph);
        }
    }
}