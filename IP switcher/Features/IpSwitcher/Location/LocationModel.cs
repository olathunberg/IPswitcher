﻿using System;

namespace TTech.IP_Switcher.Features.IpSwitcher.Location
{
    public class LocationModel
    {
        public string Description { get; set; } = string.Empty;

        public string DHCPEnabled { get; set; } = string.Empty;

        public string Ip { get; set; } = string.Empty;

        public string Gateways { get; set; } = string.Empty;

        public string Dns { get; set; } = string.Empty;

        public LocationModel()
        { }

        public LocationModel(Location location)
        {
            Description = location.Description;
            DHCPEnabled = ActiveTextFromBool(location.DHCPEnabled);

            var temporaryString = string.Empty;
            foreach (var ip in location.IPList)
                temporaryString += String.Format("{0}/{1}{2}", ip.IP, ip.NetMask, Environment.NewLine);
            Ip = temporaryString.Trim();

            temporaryString = string.Empty;
            foreach (var dns in location.DNS)
                temporaryString += dns.IP + Environment.NewLine;
            Dns = temporaryString.Trim();

            temporaryString = string.Empty;
            foreach (var gateway in location.Gateways)
                temporaryString += gateway.IP + Environment.NewLine;
            Gateways = temporaryString.Trim();
        }

        private static string ActiveTextFromBool(bool state)
        {
            if (state)
                return Resources.LocationModelLoc.Active;
            else
                return Resources.LocationModelLoc.Inactive;
        }
    }
}