using System.Collections.Generic;

namespace MultiserviceIPQuery.Common
{
    public class Constants
    {
        public const string GeoIPBaseURL = "GeoIP:BaseURL";
        public const string GeoIPAccept = "GeoIP:Accept";
        public const string RDAPBaseURL = "RDAP:BaseURL";
        public const string RDAPAccept = "RDAP:Accept";
        public const string VirusTotalBaseURL = "VirusTotal:BaseURL";
        public const string VirusTotalAccept = "VirusTotal:Accept";
        public const string VirusTotalAPIKey = "virustotal:APIKey";
        public const string IPAddressRegEx = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        public static readonly List<string> SupportedServices = new List<string>() { "geoip", "rdap", "reversedns", "ping", "virustotal" };
        public static readonly List<string> DefaultServices = new List<string>(){ "geoip", "rdap", "reversedns", "ping" };
        public const string GeoIPService = "geoip";
        public const string RDAPService = "rdap";
        public const string ReverseDNSService = "reversedns";
        public const string PingService = "ping";
        public const string VirusTotalService = "virustotal";
    }
}
