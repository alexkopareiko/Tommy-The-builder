using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
namespace Com.NikfortGames.MyGame {
    public class ErrorMessage : MonoBehaviour
    {
        #region Private Fields

        TMP_Text tMP_Text;

        #endregion

        #region MonoBehaviour Callback

        private void Awake() {
            tMP_Text = GetComponentInChildren<TMP_Text>();
            gameObject.CloseMenu();
        }

        #endregion

        #region Public Methods

        public void SetMessage(string err) {
            tMP_Text.text = err;
            if(err == Constants.MSG.OK) {
                gameObject.CloseMenu();
            } else {
                gameObject.OpenMenu();
            }
        }

        #endregion
    }
}
