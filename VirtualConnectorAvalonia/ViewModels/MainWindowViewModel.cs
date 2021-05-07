using Avalonia.Metadata;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtualConnectorAvalonia.Connections;
using VirtualConnectorAvalonia.UI.Connections;


namespace VirtualConnectorAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<IConnection>? connections;
        private DataRouter? selectedNetwork;
        private ConnectionCreatorWrapper? selectedConnectionCreator;
        private string? selectedNetworkName;
        private IConnection? selectedConnection;


        public ObservableCollection<IConnection>? Connections
        {
            get => connections;
            set
            {
                connections = value;
                OnPropertyChanged("Connections");
            }
        }
        public ObservableCollection<DataRouter> Networks { get; set; }

        public DataRouter? SelectedNetwork
        {
            get { return selectedNetwork; }
            set
            {
                if (selectedNetwork != value)
                {
                    selectedNetwork = value;
                    IsDeleteNetworkButtonEnabledUpdate();
                    IsCreateNetworkButtonEnabledUpdate();
                    if (selectedNetwork == null)
                        return;
                    SelectedNetworkName = selectedNetwork.Name;
                    OnPropertyChanged("SelectedNetwork");
                    Connections = SelectedNetwork?.connections;
                }

            }
        }


        public ConnectionCreatorWrapper? SelectedConnectionCreator
        {
            get { return selectedConnectionCreator; }
            set
            {
                if (selectedConnectionCreator != value)
                {
                    selectedConnectionCreator = value;
                    OnPropertyChanged("SelectedConnectionCreator");
                }
            }
        }

        public string? SelectedNetworkName
        {
            get
            {
                return selectedNetworkName;
            }
            set
            {
                if (selectedNetworkName != value)
                {
                    selectedNetworkName = value;
                    OnPropertyChanged("SelectedNetworkName");
                    IsCreateNetworkButtonEnabledUpdate();
                }
            }
        }


        IEnumerable<ConnectionCreatorWrapper> Channels { get; set; }

        private IConnection? SelectedConnection
        {
            get => selectedConnection;
            set
            {
                selectedConnection = value;
                OnPropertyChanged("SelectedConnection");
                IsDeleteConnectionButtonEnabledUpdate();
            }
        }

        public new event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        void IsCreateNetworkButtonEnabledUpdate()
        {
            string? Name = selectedNetworkName;
            if (Name == null)
            {
                IsCreateNetworkButtonEnabled = false;
                return;
            }
            foreach (var network in Networks)
            {
                if (network.Name == Name)
                {
                    //MessageBox.Show("Network with same name is exist");
                    IsCreateNetworkButtonEnabled = false;
                    return;
                }
            }
            IsCreateNetworkButtonEnabled = true;
        }

        //ObservableContainer<bool> isDeleteNetworkButtonEnabled;
        bool isCreateNetworkButtonEnabled;
        public bool IsCreateNetworkButtonEnabled
        {
            get => isCreateNetworkButtonEnabled;
            set
            {
                isCreateNetworkButtonEnabled = value;
                OnPropertyChanged("IsCreateNetworkButtonEnabled");
            }
        }


        void CreateNetworkButtonClicked()
        {
            string? Name = selectedNetworkName;
            if (Name == null)
                return;
            Networks.Add(new DataRouter(Name));
            IsCreateNetworkButtonEnabledUpdate();
        }


        void IsDeleteNetworkButtonEnabledUpdate()
        {
            IsDeleteNetworkButtonEnabled = (selectedNetwork != null);
        }

        bool isDeleteNetworkButtonEnabled;
        public bool IsDeleteNetworkButtonEnabled { 
            get => isDeleteNetworkButtonEnabled;
            set
            {
                isDeleteNetworkButtonEnabled = value;
                OnPropertyChanged("IsDeleteNetworkButtonEnabled");
            }
        }

        void DeleteNetworkButtonClicked()
        {
            if (SelectedNetwork == null)
                return;
            SelectedNetwork.Destroy();
            Networks.Remove(SelectedNetwork);
        }

        public MainWindowViewModel()
        {
            Networks = new ObservableCollection<DataRouter>();
            Channels = ConnectionsList.GetConnections().Select(connection => new ConnectionCreatorWrapper(connection));


            Networks.Add(new DataRouter("default channel"));
        }



        async void CreateConnectionButtonClicked()
        {
            try
            {
                IConnector? network = SelectedNetwork;
                if (network == null)
                    throw new Exception("No network selected");

                ConnectionCreatorWrapper? connectionCreator = (ConnectionCreatorWrapper?)SelectedConnectionCreator;
                if (connectionCreator == null)
                    return;

                IConnection connection = connectionCreator.CreateConnection(network);
                await Task.Run(() =>
                {
                    connection.Open();
                });
                network?.AddConnection(connection);
            }
            catch (Exception ex)
            {
                await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("error", ex.Message).Show();
            }
        }

        void IsDeleteConnectionButtonEnabledUpdate()
        {
            IsDeleteConnectionButtonEnabled = (selectedConnection != null);
        }

        //ObservableContainer<bool> isDeleteNetworkButtonEnabled;
        bool isDeleteConnectionButtonEnabled;
        public bool IsDeleteConnectionButtonEnabled
        {
            get => isDeleteConnectionButtonEnabled;
            set
            {
                if (isDeleteConnectionButtonEnabled == value)
                    return;
                isDeleteConnectionButtonEnabled = value;
                OnPropertyChanged("IsDeleteConnectionButtonEnabled");
            }
        }

        void DeleteConnectionButtonClicked()
        {
            selectedConnection?.Close();
        }

    }
}
