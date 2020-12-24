using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServer
{
    class ServerConfig
    {
        private EServerState serverState;

        public EServerState ServerState
        {
            get => serverState;
            set => serverState = value;
        }

        public ServerConfig()
        {
            this.serverState = Properties.Settings.Default.ServerState;
            //mozda ispis?
        }
    }
}
