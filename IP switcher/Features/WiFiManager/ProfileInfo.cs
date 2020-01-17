using System.Collections.Generic;

namespace TTech.IP_Switcher.Features.WiFiManager
{
    public class ProfileInfo
    {
        public ProfileInfo(string Header)
        {
            this.Header = Header;
            Children = new List<ProfileInfo>();
        }

        public string Header { get; set; }

        public List<ProfileInfo> Children { private get; set; }

        public bool HasChildren { get { return Children.Count > 0; } }
    }
}
