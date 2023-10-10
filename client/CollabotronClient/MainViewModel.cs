using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.ComponentModel;

namespace CollabotronClient
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string _serverDetailsInput;
        private string _accessCodeInput;
        private CollabMappingSession session;

        public string ServerDetailsInput
        {
            get { return _serverDetailsInput; }
            set
            {
                if (_serverDetailsInput != value)
                {
                    _serverDetailsInput = value;
                    OnPropertyChanged(nameof(ServerDetailsInput));
                }
            }
        }

        public string AccessCodeInput
        {
            get { return _accessCodeInput; }
            set
            {
                if (_accessCodeInput != value)
                {
                    _accessCodeInput = value;
                    OnPropertyChanged(nameof(AccessCodeInput));
                }
            }
        }

        public DelegateCommand AuthenticationCommmand { get; }
        public DelegateCommand UploadBeatmapCommand { get; }
        public DelegateCommand ManualRefreshCommand { get; }
        public DelegateCommand ExitSessionCommand { get; }

        public MainViewModel()
        {
            AuthenticationCommmand = new DelegateCommand(Authenticate);
            UploadBeatmapCommand = new DelegateCommand(UploadBeatmap);
            ManualRefreshCommand = new DelegateCommand(ManualRefresh);
            ExitSessionCommand = new DelegateCommand(ExitSession);
            session = null;
        }

        private void Authenticate()
        {
            // TODO
        }

        private void UploadBeatmap()
        {
            // TODO
        }

        private void ManualRefresh()
        {
            // TODO
        }

        private void ExitSession()
        {
            // TODO
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
