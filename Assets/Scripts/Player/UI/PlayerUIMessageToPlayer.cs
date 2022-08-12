using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Com.NikfortGames.MyGame{
    public class PlayerUIMessageToPlayer : MonoBehaviour
    {

        #region Public Fields

        public float timeToDestroy = 4;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
                transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
                Destroy(gameObject, timeToDestroy);
            }

        #endregion

        #region Public Methods

        public void SetText(string text) {
            GetComponent<TMP_Text>().text = text;
        }

        #endregion
    }
}
