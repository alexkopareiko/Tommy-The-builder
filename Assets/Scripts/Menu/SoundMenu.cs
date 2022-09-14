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


        
        
        [SerializeField] Slider soundMusicSlider;
        [SerializeField] Slider soundEffectsSlider;


        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            gameObject.CloseMenu();
        }

        private void Start() {
            Sliderslization();
            SliderAttachListeners();
        }

        #endregion

        #region Public Methods

        
        #endregion

        #region Private Methods

        void Sliderslization() {
            soundMusicSlider.value = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC);
            soundEffectsSlider.value = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS);
        }

        void SliderAttachListeners() {
            soundMusicSlider.onValueChanged.AddListener (delegate {
                SoundManager.instance.SetMusicVolume(soundMusicSlider.value);
            });
            soundEffectsSlider.onValueChanged.AddListener (delegate {
                SoundManager.instance.SetEffectsVolume(soundEffectsSlider.value);
            });
        } 

        
        

        

        #endregion`
    }
}

