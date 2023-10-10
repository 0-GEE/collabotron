using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabotronClient
{
    class CollabEventArgs: EventArgs
    {
        public bool IsHost { get; }
        public bool IsRefreshAvailable { get; }

        public CollabEventArgs(bool isHost, bool isRefreshAvailable)
        {
            IsHost = isHost;
            IsRefreshAvailable = isRefreshAvailable;
        }
    }
}
