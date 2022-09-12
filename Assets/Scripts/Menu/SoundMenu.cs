using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public class SoundMenu : MonoBehaviour
    {
        #region Public Fields

        #endregion

        #region Private Fields


        [SerializeField] float initialMusicVolume = 0.5f;
        [SerializeField] float initialEffectsVolume = 0.5f;
        
        [SerializeField] Slider soundMusicSlider;
        [SerializeField] Slider soundEffectsSlider;


        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            gameObject.CloseMenu();
        }

        private void Start() {
            SoundInitialization();
            SliderAttachListeners();
        }

        #endregion

        #region Public Methods

        
        #endregion

        #region Private Methods

        void SliderAttachListeners() {
            soundMusicSlider.onValueChanged.AddListener (delegate {
                EventManager.m_onSoundMusicSliderChanged.Invoke();
            });
            soundEffectsSlider.onValueChanged.AddListener (delegate {
                EventManager.m_onSoundEffectsSliderChanged.Invoke();
            });
            EventManager.m_onSoundMusicSliderChanged.AddListener(SetMusicVolume);
            EventManager.m_onSoundEffectsSliderChanged.AddListener(SetEffectsVolume);
        } 

        
        void SoundInitialization() {
            float _musicVolume = initialMusicVolume;
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS.SOUND_MUSIC)) {
                _musicVolume = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC);
            } else {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC, _musicVolume);
            }
            soundMusicSlider.value = _musicVolume;

            float _effectVolume = initialEffectsVolume;
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS.SOUND_EFFECTS)) {
                _effectVolume = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS);
            } else {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS, _effectVolume);
            }
            soundEffectsSlider.value = _effectVolume;

            SetMusicVolume();
            SetEffectsVolume();
        }

        void SetMusicVolume() {
            PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC, soundMusicSlider.value);
            foreach (var audioSource in SoundManager.instance.allMusicAudioSources)
            {
                audioSource.volume = soundMusicSlider.value;
            }
        }

        void SetEffectsVolume() {
            PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS, soundEffectsSlider.value);
            foreach (var audioSource in SoundManager.instance.allEffectsAudioSources)
            {
                audioSource.volume = soundEffectsSlider.value;
            }
        }

        

        #endregion`
    }
}

