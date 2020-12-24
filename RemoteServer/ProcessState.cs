using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServer
{
    class ProcessState : IServerState
    {
        private static ServerConfig config = new ServerConfig(); 

        public EServerState CheckServerState()
        {
            return config.ServerState;
        }

        public void UpdateServerState(EServerState state)
        {
            config.ServerState = state;
        }
    }
}
