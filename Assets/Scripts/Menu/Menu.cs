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

        [Header("Controls")]
        [SerializeField] GameObject controlsMenu;
        [SerializeField] Button controlsButton;

        [Header("Other")]
        [SerializeField] Button exitButton;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            instance = this;
            soundButton.onClick.AddListener(TriggerSoundMenu);
            controlsButton.onClick.AddListener(TriggerControlsMenu);
            if(exitButton != null)
                exitButton.onClick.AddListener(ExitGame);
        }

        private void Start() {
        }

        #endregion

        #region Public Methods

        public void CloseOtherMenus(GameObject except = null) {
            if(except != soundMenu) soundMenu.CloseMenu();
            if(except != controlsMenu) controlsMenu.CloseMenu();
        }

        #endregion

        #region Private Methods 
        
        void TriggerSoundMenu() {
            soundMenu.TriggerMenu();
            CloseOtherMenus(soundMenu);
        }
        void TriggerControlsMenu() {
            controlsMenu.TriggerMenu();
            CloseOtherMenus(controlsMenu);
        }
        void ExitGame() {
            Application.Quit();
        }
        
        #endregion
    }
}

