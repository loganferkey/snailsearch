using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class RelicUtil : IRelicUtil
    {
        public RelicUtil()
        {
        }

        public void ScrubEncoding(string field)
        {
            Dictionary<string, char> encodingMap = new Dictionary<string, char>()
            {
            };
            // Scrub encoding from messages, like apostrophe replacement etc.

        }
    }
}