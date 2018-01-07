using System;
using Sharp.Core;

namespace Sharp
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (EngineMgr game = new EngineMgr())
            {
                game.Run();
            }
        }
    }
}

