using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClientSystem
{ 
    public static class GameClient
    {
        private static GameClientManager manager = null;

        public static GameClientManager GetManager()
        {
            return manager;
        }

        public static void SetManager(GameClientManager gameClientManager)
        {
            if (manager != null)
            {
                Console.WriteLine("Manager was already assigned");
                return;
            }

            if (gameClientManager == null)
            {
                Console.WriteLine("game client manager is null");
                return;
            }

            manager = gameClientManager;
        }
    }
}
