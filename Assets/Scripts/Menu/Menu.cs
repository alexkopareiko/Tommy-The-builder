using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public class Menu : MonoBehaviour
    {
        #region Public Fields

        public static Menu instance;
        public Slider soundSlider;

        #endregion
        #region Private Fields
        [SerializeField] float initialDefaultSound = 0.5f;
        [SerializeField] List<AudioSource> allAudioSources;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            instance = this;
            SoundInitialization();
            soundSlider.onValueChanged.AddListener (delegate {
                EventManager.m_onSoundSliderChanged.Invoke();
            });
            EventManager.m_onSoundSliderChanged.AddListener(SetSoundVolume);
        }

        private void Start() {
        }

        #endregion

        #region Public Methods

        public void SetSoundVolume() {
            PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND, soundSlider.value);
            foreach (var audioSource in allAudioSources)
            {
                audioSource.volume = soundSlider.value;
            }
        }

        #endregion

        #region Private Methods 

        
        void SoundInitialization() {
            float soundValue = initialDefaultSound;
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS.SOUND)) {
                soundValue = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND);
            } else {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND, soundValue);
            }
            soundSlider.value = soundValue;
        }

        #endregion
    }
}

