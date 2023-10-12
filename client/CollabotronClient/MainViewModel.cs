using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel;
using System.Windows;

namespace CollabotronClient
{
    public class MainViewModel : BindableBase
    {
        private string _serverDetailsInput;
        private string _accessCodeInput;
        private string _statusMessage;
        private CollabMappingSession session;

        public string ServerDetailsInput
        {
            get { return _serverDetailsInput; }
            set { SetProperty(ref _serverDetailsInput, value); }
        }

        public string AccessCodeInput
        {
            get { return _accessCodeInput; }
            set { SetProperty(ref _accessCodeInput, value); }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            private set { SetProperty(ref _statusMessage, value); }
        }

        public DelegateCommand AuthenticationCommmand { get; }
        public DelegateCommand UploadBeatmapCommand { get; }
        public DelegateCommand ManualRefreshCommand { get; }
        public DelegateCommand ExitSessionCommand { get; }

        public MainViewModel()
        {
            AuthenticationCommmand = new DelegateCommand(async () => await AuthenticateAndStart());
            UploadBeatmapCommand = new DelegateCommand(async () => await UploadBeatmap());
            ManualRefreshCommand = new DelegateCommand(async () => await ManualRefresh());
            ExitSessionCommand = new DelegateCommand(ExitSession);

            _serverDetailsInput = "";
            _accessCodeInput = "";
            session = null;
            _statusMessage = "No active session.";
        }

        private async Task AuthenticateAndStart()
        {
            if (session != null)
            {
                MessageBox.Show("Already in active collab session.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_serverDetailsInput == "" || _accessCodeInput == "")
            {
                MessageBox.Show("Please enter server details and access code first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string[] serverInfo = _serverDetailsInput.Split(":");

            if (serverInfo.Length != 2)
            {
                MessageBox.Show("Invalid server details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string serverAddr = serverInfo[0];
            string serverPort = serverInfo[1];

            session = new CollabMappingSession(serverAddr, serverPort);

            if (!session.IsReady())
            {
                if (!ShowSongsFolderPopup())
                {
                    MessageBox.Show("You must enter a valid path to your osu! songs folder before continuing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    session = null;
                    return;
                }
            }
            session.CollabEvent += HandleCollabEvent;

            await session.Authenticate(_accessCodeInput);

            await session.BeginGetStateLoop();

            session = null;

            SetProperty(ref _statusMessage, "No active session.");

        }

        private async Task UploadBeatmap()
        {
            if (session == null)
            {
                MessageBox.Show("You need to be in an active collab session to upload collab parts.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool res = await session.UploadBeatmap();

            if (res)
            {
                MessageBox.Show("Collab part uploaded succesfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Collab part upload forbidden by server. Are you sure you are the current host?", "Forbbiden", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ManualRefresh()
        {
            if (session == null)
            {
                MessageBox.Show("You need to be in an active collab session to update your map.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await session.RefreshMapData();
        }

        private void ExitSession()
        {
            if (session == null)
            {
                return;
            }
            session.StopSession();
        }

        private bool ShowSongsFolderPopup()
        {
            var popup = new SongsFolderWindow();
            if (popup.ShowDialog() == true)
            {
                var input = popup.UserInput;
                if (input.Length == 0)
                {
                    return false;
                }

                session.SetSongsFolder(input);
                return true;
            }

            return false;
        }

        private void HandleCollabEvent(object sender, CollabEventArgs e)
        {
            StatusMessage = e.IsHost ? "You are now the host!" : "Host is mapping...";
        }

    }
}
