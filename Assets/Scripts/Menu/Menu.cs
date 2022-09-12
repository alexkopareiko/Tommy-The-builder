using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public class Menu : MonoBehaviour
    {
        #region Public Fields

        public static Menu instance;

        #endregion

        #region Private Fields

        [Header("Sound")]
        [SerializeField] GameObject soundMenu;
        [SerializeField] Button soundButton;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            instance = this;
            soundButton.onClick.AddListener(TriggerSoundMenu);
        }

        private void Start() {
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods 



        void CloseOtherMenus(GameObject except) {
            if(except != soundMenu) soundMenu.CloseMenu();
        }
        void TriggerSoundMenu() {
            soundMenu.TriggerMenu();
            CloseOtherMenus(soundMenu);
        }
        
        #endregion
    }
}

