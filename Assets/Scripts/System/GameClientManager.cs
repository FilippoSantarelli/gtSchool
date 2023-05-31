using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientSystem
{
    public class GameClientManager
    {
        public BlockDataManager blockDataManager;

        private static GameClientManager instance = null;

        public GameClientManager()
        {
            blockDataManager = BlockDataManager.Instance;
        }

        public static GameClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameClientManager();
                }
                return instance;
            }
        }
    }
}
