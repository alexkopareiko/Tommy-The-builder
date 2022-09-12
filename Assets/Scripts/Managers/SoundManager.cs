using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.NikfortGames.MyGame {
    public  class SoundManager: MonoBehaviour
    {
        #region Public Fields

        public static SoundManager instance;
        public  List<AudioSource> allEffectsAudioSources;
        public  List<AudioSource> allMusicAudioSources;
        [HideInInspector]
        public AudioSource audioSourceEffects;
        

        #endregion

        #region Private Fields
        [Header("Menu Sounds")]
        [SerializeField]  AudioClip menuButtonHover;
        [SerializeField]  AudioClip menuButtonClick;
        [SerializeField]  AudioClip menuPlayButtonClick;
        [SerializeField]  AudioClip menuPlayerNameInputField;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            instance = this;
            if(allEffectsAudioSources.Count > 0) {
                audioSourceEffects = allEffectsAudioSources[0];
            }
        }

        #endregion

        #region Public Methods

        public void PlayMenuButtonHover(){
            if(audioSourceEffects != null && menuButtonHover != null)
                audioSourceEffects.PlayOneShot(menuButtonHover);
        }
        public void PlayMenuButtonClick(){
            if(audioSourceEffects != null && menuButtonClick != null)
                audioSourceEffects.PlayOneShot(menuButtonClick);
        }
        public void PlayMenuPlayButtonClick(){
            if(audioSourceEffects != null && menuPlayButtonClick != null)
                audioSourceEffects.PlayOneShot(menuPlayButtonClick);
        }
        public void PlayMenuPlayerNameInput(){
            if(audioSourceEffects != null && menuPlayerNameInputField != null)
                audioSourceEffects.PlayOneShot(menuPlayerNameInputField);
        }

        #endregion
    }

}
