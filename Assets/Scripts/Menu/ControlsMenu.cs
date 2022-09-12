using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public class ControlsMenu : MonoBehaviour
    {
        #region MonoBehaviour Callbacks

            private void Awake() {
                gameObject.CloseMenu();
            }

        #endregion
        
    }
}
