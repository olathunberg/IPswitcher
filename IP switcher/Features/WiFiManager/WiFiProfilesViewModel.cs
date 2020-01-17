#nullable enable
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using NativeWifi;

namespace TTech.IP_Switcher.Features.WiFiManager
{
    public class WiFiProfilesViewModel : INotifyPropertyChanged
    {
        private System.Windows.Controls.UserControl? owner;
        private InterfaceModel? selectedInterface;
        private string selectedProfile = string.Empty;
        private ObservableCollection<InterfaceModel> interfaces;
        private bool effect;

        #region Constructors
        public WiFiProfilesViewModel()
        {
            interfaces = new ObservableCollection<InterfaceModel>(Client.Interfaces.Select(x => new InterfaceModel(x)).ToList());
            Profiles = new ObservableCollection<string>();
            ProfileTree = new List<ProfileInfo>();
            SelectedInterface = Interfaces.FirstOrDefault();
        }
        #endregion

        #region Eventhandlers
        void SelectedInterface_WlanReasonNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanReasonCode reasonCode)
        {
            SelectedInterface?.RefreshConnected();
        }

        void SelectedInterface_WlanNotification(Wlan.WlanNotificationData notifyData)
        {
            if (notifyData.notificationSource == Wlan.WlanNotificationSource.MSM)
            {
                SelectedInterface?.RefreshConnected();
            }
        }

        void SelectedInterface_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
            SelectedInterface?.RefreshConnected();
        }
        #endregion

        #region Public Properties
        public static WlanClient Client { get; private set; } = new WlanClient();

        public System.Windows.Controls.UserControl? Owner
        {
            get { return owner; }
            set
            {
                if (owner == value)
                    return;

                owner = value;

                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<InterfaceModel> Interfaces
        {
            get { return interfaces; }
            set
            {
                if (interfaces == value)
                    return;

                interfaces = value;
                NotifyPropertyChanged();
            }
        }

        public InterfaceModel? SelectedInterface
        {
            get { return selectedInterface; }
            set
            {
                selectedInterface = value;
                if (selectedInterface != null)
                {
                    RefreshProfiles();

                    selectedInterface.interFace.WlanConnectionNotification += SelectedInterface_WlanConnectionNotification;
                    selectedInterface.interFace.WlanNotification += SelectedInterface_WlanNotification;
                    selectedInterface.interFace.WlanReasonNotification += SelectedInterface_WlanReasonNotification;
                }

                NotifyPropertyChanged();
            }
        }

        public string SelectedProfile
        {
            get { return selectedProfile; }
            set
            {
                selectedProfile = value;
                NotifyPropertyChanged();
                Task.Run(() =>
                    {
                        ProfileTree = ParsePropertyXml(selectedInterface?.GetProfileXml(value));
                        NotifyPropertyChanged("ProfileTree");
                    });
            }
        }

        public bool Effect
        {
            get { return effect; }
            set
            {
                if (effect == value)
                    return;

                effect = value;

                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> Profiles { get; set; }

        public List<ProfileInfo> ProfileTree { get; set; }

        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Private methods
        private void RefreshProfiles()
        {
            Task.Run(() =>
            {
                Profiles = new ObservableCollection<string>(selectedInterface?.GetProfiles());
                SelectedProfile = Profiles.FirstOrDefault(x => x.Equals(selectedInterface?.ProfileName));
                NotifyPropertyChanged("Profiles");
            });
        }

        private List<ProfileInfo> ParsePropertyXml(string? propertyXml)
        {
            if (string.IsNullOrEmpty(propertyXml))
                return new List<ProfileInfo>();

            var xd = new XmlDocument();
            xd.LoadXml(propertyXml);
            foreach (var item in xd.ChildNodes.OfType<XmlNode>())
            {
                if (item.LocalName == "WLANProfile")
                    return GetChildNodes(item);
            }
            return new List<ProfileInfo>();
        }

        private List<ProfileInfo> GetChildNodes(XmlNode node)
        {
            var result = new List<ProfileInfo>();
            foreach (var item in node.ChildNodes.OfType<XmlNode>())
            {
                if (item.HasChildNodes)
                    result.Add(new ProfileInfo(item.Name) { Children = GetChildNodes(item) });
                else
                    result.Add(new ProfileInfo(item.Value));
            }

            return result;
        }

        private void DoExportProfiles()
        {
            if (selectedInterface == null)
                return;

            Effect = true;

            try
            {
                ProfileInfoExportExtension.WriteToFile(selectedInterface);
            }
            finally
            {
                Effect = false;
            }
        }

        private void DoImportPresets()
        {
            if (selectedInterface == null)
                return;

            Effect = true;

            try
            {
                ProfileInfoExportExtension.ReadFromFile(selectedInterface);

                RefreshProfiles();
            }
            finally
            {
                Effect = false;
            }
        }

        private void DoDeleteSelected()
        {
            Effect = true;

            try
            {
                selectedInterface?.interFace.DeleteProfile(selectedProfile);

                RefreshProfiles();
            }
            finally
            {
                Effect = false;
            }
        }
        #endregion

        #region Commands
        private RelayCommand? importProfilesCommand;
        public ICommand Import
        {
            get
            {
                return importProfilesCommand ?? (importProfilesCommand = new RelayCommand(
                    () => DoImportPresets(),
                    () => selectedInterface != null));
            }
        }

        private RelayCommand? exportProfilesCommand;
        public ICommand Export
        {
            get
            {
                return exportProfilesCommand ?? (exportProfilesCommand = new RelayCommand(
                    () => DoExportProfiles(),
                    () => selectedInterface != null));
            }
        }
        private RelayCommand? deleteSelectedCommand;
        public ICommand DeleteSelected
        {
            get
            {
                return deleteSelectedCommand ?? (deleteSelectedCommand = new RelayCommand(
                    () => DoDeleteSelected(),
                    () => SelectedProfile != null));
            }
        }
        #endregion
    }
}
