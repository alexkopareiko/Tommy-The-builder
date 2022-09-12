using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Com.NikfortGames.MyGame {
    public class EventManager : MonoBehaviour
    {

        #region Public Fields
        
        public static UnityEvent m_onSoundMusicSliderChanged;
        public static UnityEvent m_onSoundEffectsSliderChanged;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            if (m_onSoundMusicSliderChanged == null)
                m_onSoundMusicSliderChanged = new UnityEvent();
            if (m_onSoundEffectsSliderChanged == null)
                m_onSoundEffectsSliderChanged = new UnityEvent();
        }

        #endregion


    }
}


