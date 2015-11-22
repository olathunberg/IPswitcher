﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NativeWifi;

namespace Deucalion.IP_Switcher.Features.WiFiManager
{
    public class InterfaceModel : INotifyPropertyChanged
    {
        internal WlanClient.WlanInterface interFace;

        public InterfaceModel(WlanClient.WlanInterface interFace)
        {
            this.interFace = interFace;

            UpdateInformation();
        }

        public void UpdateInformation()
        {
            try
            {
                IsConnected = interFace.InterfaceState != Wlan.WlanInterfaceState.Disconnected;
                ProfileName = IsConnected ? interFace.CurrentConnection.profileName : null;
                SignalQuality = IsConnected ? interFace.CurrentConnection.wlanAssociationAttributes.wlanSignalQuality : 0;
                InterfaceState = interFace.InterfaceState;
                Channel = IsConnected ? interFace.Channel : default(int?);
                CurrentOperationMode = IsConnected ? interFace.CurrentOperationMode : Wlan.Dot11OperationMode.Unknown;
                RSSI = IsConnected ? interFace.RSSI : default(int?);
                BssType = interFace.BssType;
                Autoconf = interFace.Autoconf;
                InterfaceName = interFace.InterfaceName;
                InterfaceDescription = interFace.InterfaceDescription;

                foreach (var item in this.GetType().GetProperties())
                {
                    NotifyPropertyChanged(item.Name);
                }
            }
            catch(System.Exception ex)
            {
                Helpers.ShowWindow.Show.Message(ex.Message);
            }
        }

        public bool IsConnected { get; private set; }

        public string ProfileName { get; private set; }

        public uint SignalQuality { get; private set; }

        public Wlan.WlanInterfaceState InterfaceState { get; private set; }

        public int? Channel { get; private set; }

        public Wlan.Dot11OperationMode CurrentOperationMode { get; private set; }

        public int? RSSI { get; private set; }

        public Wlan.Dot11BssType BssType { get; private set; }

        public bool Autoconf { get; private set; }

        public string InterfaceName { get; private set; }

        public string InterfaceDescription { get; private set; }

        public string[] GetProfiles()
        {
            if (interFace == null)
                return new string[] { };

            return interFace.GetProfiles().Select(x => x.profileName).ToArray();
        }

        public List<Wlan.WlanProfileInfo> GetProfileInfos()
        {
            if (interFace == null)
                return new List<Wlan.WlanProfileInfo>();

            return interFace.GetProfiles().ToList();
        }

        public string GetProfileXml(string profileName)
        {
            if (interFace == null)
                return string.Empty;

            return interFace.GetProfileXml(profileName);
        }

        public IEnumerable<Wlan.WlanAvailableNetwork> GetAvailableNetworkList()
        {
            return interFace.GetAvailableNetworkList(Wlan.WlanGetAvailableNetworkFlags.IncludeAllAdhocProfiles).Where(x => x.flags != 0);
        }

        public override string ToString()
        {
            if (interFace == null)
                return string.Empty;

            return InterfaceName;
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
