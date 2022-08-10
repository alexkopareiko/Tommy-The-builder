using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public class SpellProgress : MonoBehaviour
    {
        #region Public Fields

        [Tooltip("Initialize from inspector")]
        public Slider slider;

        public Image fillColor;

        public bool reset;


        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        private void Update() {
            if(!reset) {
                slider.value += Time.deltaTime;
                if(slider.value == slider.maxValue) Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void DestroySlider() {
            reset = true;
            slider.value = slider.maxValue;
            fillColor.color = new Color32(255,0,0,255);
            Destroy(gameObject, 1);
        }

        #endregion
            
    }
}

