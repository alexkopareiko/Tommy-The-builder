using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.NikfortGames.MyGame {
    public class GameManager : MonoBehaviour
    {

        #region Public Fields

        public static GameManager instance;

        #endregion
        
        #region Private Fields
        private Player player;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        #endregion
        
    }
}

