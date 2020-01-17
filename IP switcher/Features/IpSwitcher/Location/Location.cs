using System.Collections.ObjectModel;

namespace TTech.IP_Switcher.Features.IpSwitcher.Location
{
    public class Location
    {
        public string Description { get; set; } = string.Empty;

        public uint ID { get; set; }

        public bool DHCPEnabled { get; set; }

        public ObservableCollection<IPDefinition> IPList { get; set; } = new ObservableCollection<IPDefinition>();

        public ObservableCollection<IPv4Address> Gateways { get; set; } = new ObservableCollection<IPv4Address>();

        public ObservableCollection<IPv4Address> DNS { get; set; } = new ObservableCollection<IPv4Address>();

        public Location Clone()
        {
            return (Location)MemberwiseClone();
        }
    }
}