
using NetTools;
using System.Net;
using System.Text.RegularExpressions;
using Task_2.Models;

namespace Task_2
{
    public class IPRangeChecker
    {
        public IPRange ip = new();
        public List<IPRange> IpRanges { get; set; }

        readonly Regex validateIPv4Regex = new("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

        readonly Regex extractIPv4Regex = new("(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");

        readonly Regex cidrRegex = new("^([0-9]{1,3}\\.){3}[0-9]{1,3}($|/(16|24))$");


        private void ParseCIDRIPNetwork(string cidr)
        {
            var result = IPNetwork.Parse(cidr);
            ip.Start = result.FirstUsable;
            ip.End = result.LastUsable;
        }

        public IPRangeChecker(List<IPRange> ipRanges)
        {
            IpRanges = ipRanges;
        }

        public bool Execute(string whatsYourIp)
        {
            try
            {
                AssignIPs(whatsYourIp);

                if (ip.Start != null && ip.End != null)
                {
                    return CheckIpRange();
                }           
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return false;

        }


        private bool CheckIpRange()
        {
            var res = IpRanges.Select(m => m).ToList();

            foreach(var line in res)
            {
                var range = IPAddressRange.Parse(line.Start + " - " + line.End);
                if (range.Contains(ip.Start) && range.Contains(ip.End))
                {
                    return true;
                }
            }
            return false;
        }

        private void AssignIPs(string whatsYourIp)
        {

            if (cidrRegex.IsMatch(whatsYourIp))
            {
                ParseCIDRIPNetwork(whatsYourIp);
            }

            if (validateIPv4Regex.IsMatch(whatsYourIp))
            {
                string[] extracted = extractIPv4Regex.Matches(whatsYourIp)
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToArray();

                if (extracted.Select(m => m).Count() > 1)
                {
                    ip.Start = IPAddress.Parse(extracted[0]);
                    ip.End = IPAddress.Parse(extracted[extracted.Count()]);
                }
                else
                {
                    ip.Start = IPAddress.Parse(extracted[0]);
                    ip.End = IPAddress.Parse(extracted[0]);
                }
            }
            else
            {
                var range = IPAddressRange.Parse(whatsYourIp);
                ip.Start = range.Begin;
                ip.End = range.End;
            }
        }

    }
}