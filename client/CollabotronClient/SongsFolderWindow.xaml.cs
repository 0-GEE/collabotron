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
using System.Windows.Shapes;

namespace CollabotronClient
{
    /// <summary>
    /// Interaction logic for SongsFolderWindow.xaml
    /// </summary>
    public partial class SongsFolderWindow : Window
    {
        public string UserInput { get; private set; } = "";
        public SongsFolderWindow()
        {
            InitializeComponent();
        }

        private void SubmitSongsFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            UserInput = SongsFolderInput.Text;
            DialogResult = true;
        }
    }
}
