using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.NikfortGames.MyGame {
    public class SaveThis : MonoBehaviour
    {
        #region MonoBehaviour Callbacks

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }   
}

