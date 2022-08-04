using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame{
    public class SpellSlot : MonoBehaviour
    {
        #region Public Fields
        public SpellIcon spellIcon;
        public bool isDisabled = false;
        
        #endregion

        #region Private Fields
        [SerializeField] GameObject highlightIcon;
        [SerializeField] GameObject disabledIcon;
        [SerializeField] Image image;
        [SerializeField] Button button;

        #endregion

        #region MonoBehaviour

        private void Awake() {
            image.sprite = spellIcon.sprite;
        }

        private void Update() {
            if(Input.GetKeyDown(spellIcon.keyCode)) {
                highlightIcon.SetActive(true);
            } else if(Input.GetKeyUp(spellIcon.keyCode)) {
                highlightIcon.SetActive(false);
            }
            disabledIcon.SetActive(isDisabled);
            button.interactable = !isDisabled;
        }

        #endregion
        
    }

}
