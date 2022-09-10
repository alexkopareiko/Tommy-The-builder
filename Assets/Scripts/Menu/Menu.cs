using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public class Menu : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] int initialDefaultSound = 70;
        [SerializeField] Slider soundSlider;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            SoundInitialization();
            soundSlider.onValueChanged.AddListener (delegate {SetSoundVolume ();});
        }

        #endregion

        #region Public Methods

        public void SetSoundVolume() {
            PlayerPrefs.SetInt(Constants.PLAYER_PREFS.SOUND, (int)soundSlider.value);
        }

        #endregion

        #region Private Methods 

        
        void SoundInitialization() {
            int soundValue = initialDefaultSound;
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS.SOUND)) {
                soundValue = PlayerPrefs.GetInt(Constants.PLAYER_PREFS.SOUND);
            } else {
                PlayerPrefs.SetInt(Constants.PLAYER_PREFS.SOUND, soundValue);
            }
            soundSlider.value = soundValue;
        }

        #endregion
    }
}

