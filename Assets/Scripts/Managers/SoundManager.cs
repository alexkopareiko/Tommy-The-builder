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
        [SerializeField] float initialMusicVolume = 0.5f;
        [SerializeField] float initialEffectsVolume = 0.5f;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            instance = this;
            if(allEffectsAudioSources.Count > 0) {
                audioSourceEffects = allEffectsAudioSources[0];
            }
            SoundInitialization();
        }

        #endregion

        #region Public Methods
        

        public void SetMusicVolume(float volume) {
            PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC, volume);
            foreach (var audioSource in SoundManager.instance.allMusicAudioSources)
            {
                audioSource.volume = volume;
            }
        }

        public void SetEffectsVolume(float volume) {
            PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS, volume);
            foreach (var audioSource in SoundManager.instance.allEffectsAudioSources)
            {
                audioSource.volume = volume;
            }
        }

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

        #region Private Fields

        void SoundInitialization() {
            float _musicVolume = initialMusicVolume;
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS.SOUND_MUSIC)) {
                _musicVolume = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC);
            } else {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_MUSIC, _musicVolume);
            }
            SetMusicVolume(_musicVolume);

            float _effectVolume = initialEffectsVolume;
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS.SOUND_EFFECTS)) {
                _effectVolume = PlayerPrefs.GetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS);
            } else {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS.SOUND_EFFECTS, _effectVolume);
            }
            SetEffectsVolume(_effectVolume);

        }

        #endregion
    }

}
