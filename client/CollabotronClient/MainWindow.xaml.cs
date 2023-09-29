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

namespace CollabotronClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HTTPHandler netHandler;
        private BeatmapHandler mapHandler;
        private string ServerUrlBase;

        public MainWindow()
        {
            netHandler = new HTTPHandler();
            mapHandler = new BeatmapHandler();
            ServerUrlBase = null;
            InitializeComponent();
        }

        private void ServerConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
