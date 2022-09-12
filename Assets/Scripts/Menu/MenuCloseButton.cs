using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.NikfortGames.MyGame {
    [RequireComponent(typeof(Button))]
    public class MenuCloseButton : MonoBehaviour
    {

        #region Private Fields

        private Button btn;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            btn = GetComponent<Button>();
            GameObject menu = transform.parent.gameObject;
            btn.onClick.AddListener(menu.CloseMenu);
        }



        #endregion
    }

}
