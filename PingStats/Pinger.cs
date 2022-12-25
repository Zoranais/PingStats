using PingStats.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingStats
{
    public class Pinger
    {
        string _address;
        private Timer _timer = null;
        private List<PingModel> _pingList;
        public Pinger(string address)
        {
            _address = address;
            _pingList = new List<PingModel>();
        }
        public void StartTimer(int time) => _timer = new Timer(TimerCallback, null, 0, time);
        public void EndTimer() => _timer.Dispose();
        public List<PingModel> GetPingList() => _pingList;
        public float GetPingTime()
        {
            using (var pinger = new Ping())
            {
                var reply = pinger.Send(_address);
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine(reply.RoundtripTime);
                    return reply.RoundtripTime;
                }
            }
            return 999;
        }
        public static bool ValidateAddress(string address)
        {
            using var pinger = new Ping();
            try
            {
                var reply = pinger.Send(address);
            }
            catch
            {
                return false;
            }
            return true;
        }
        private void TimerCallback(Object o)
        {
            var ping = new PingModel() { Ping = GetPingTime(), Time = DateTime.Now.ToLongTimeString()};
            _pingList.Add(ping);
        }

    }
}
