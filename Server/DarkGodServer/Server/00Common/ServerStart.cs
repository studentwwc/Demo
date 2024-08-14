using PENet;
using Protocal;
using Server._00Common;
using System.Threading;

namespace Server
{
    internal class ServerStart
    {
        static void Main(string[] args)
        {
            GameRoot.Instance.Init();
            while (true) {
                GameRoot.Instance.Update();
                Thread.Sleep(100);
            }
        }
    }
}
