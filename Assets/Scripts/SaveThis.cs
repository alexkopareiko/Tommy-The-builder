using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.NikfortGames.MyGame {
    public class SaveThis : MonoBehaviour
    {
        #region MonoBehaviour Callbacks

        ///<summary>
        /// make sure you destroy duplicates
        /// look ScoreboardOverview.cs for exmaple
        ///<summary>
        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }   
}

