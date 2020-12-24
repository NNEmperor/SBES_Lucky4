using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerMonitor
{
    class Program
    {
        enum endpointName { first, second }

        static void Main(string[] args)
        {

            IServerState serverOne = null;
            IServerState serverTwo = null;

            if (ConnectToServer(endpointName.first, out serverOne, EServerState.Primary))
            {
                ConnectToServer(endpointName.second, out serverTwo, EServerState.Secondary);
            }
            else
                ConnectToServer(endpointName.second, out serverTwo, EServerState.Primary);


            while (true)
            {

                EServerState stateOne = EServerState.Unkonwn;
                EServerState stateTwo = EServerState.Unkonwn;

                try
                {
                    stateOne = serverOne.CheckServerState();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with server One : " + e.Message);
                }

                try
                {
                    stateTwo = serverTwo.CheckServerState();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with server Two : " + e.Message);
                }


                Console.WriteLine("Server One state : " + stateOne);
                Console.WriteLine("Server Two state : " + stateTwo);


                if (stateOne == EServerState.Unkonwn || stateTwo == EServerState.Unkonwn)
                {
                    if (stateOne == EServerState.Primary)
                    {
                        ConnectToServer(endpointName.second, out serverTwo, EServerState.Secondary);
                    }
                    else if (stateTwo == EServerState.Primary)
                    {
                        ConnectToServer(endpointName.first, out serverOne, EServerState.Secondary);
                    }
                    else if (stateTwo == EServerState.Secondary)
                    {
                        serverTwo.UpdateServerState(EServerState.Primary);
                        ConnectToServer(endpointName.first, out serverOne, EServerState.Secondary);
                    }
                    else if (stateOne == EServerState.Primary)
                    {
                        serverOne.UpdateServerState(EServerState.Primary);
                        ConnectToServer(endpointName.second, out serverTwo, EServerState.Secondary);
                    }
                    else
                    {
                        if (ConnectToServer(endpointName.first, out serverOne, EServerState.Primary))
                        {
                            ConnectToServer(endpointName.second, out serverTwo, EServerState.Secondary);
                        }
                        else
                            ConnectToServer(endpointName.second, out serverTwo, EServerState.Primary);

                    }
                }

                //vise manje?

                Thread.Sleep(5000);
            }



        }

        static bool ConnectToServer(endpointName endpointName, out IServerState server, EServerState serverState = EServerState.Unkonwn)
        {
            server = null;
            try
            {
                ChannelFactory<IServerState> channelFactory = new ChannelFactory<IServerState>(endpointName.ToString());
                server = channelFactory.CreateChannel();
                server.UpdateServerState(serverState);

                return true;
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
