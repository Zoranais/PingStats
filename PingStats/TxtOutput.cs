using PingStats.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingStats
{
    public class TxtOutput
    {
        private string _fileName;
        public TxtOutput(string fileName)
        {
            _fileName = fileName;
        }
        public async void WriteInFileAsync(List<PingModel> ping)
        {
            using StreamWriter file = new StreamWriter(_fileName);
            foreach (var item in ping)
            {
                await file.WriteLineAsync($"{item.Time} - {item.Ping}");
            }
        }
    }
}
