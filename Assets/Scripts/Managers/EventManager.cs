using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Com.NikfortGames.MyGame {
    public class EventManager : MonoBehaviour
    {

        #region Public Fields
        public static UnityEvent m_onSoundSliderChanged;

        public Slider soundSlider;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            if (m_onSoundSliderChanged == null)
                m_onSoundSliderChanged = new UnityEvent();
        }
        
        #endregion


    }
}


