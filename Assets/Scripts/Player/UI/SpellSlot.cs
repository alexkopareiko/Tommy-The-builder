using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Com.NikfortGames.MyGame{
    public class SpellSlot : MonoBehaviour
    {
        #region Public Fields

        [Tooltip("No need initialization")]
        public SpellIcon spellIcon;
        public bool isDisabled = false;
        public GameObject disabledIcon;
        public TMP_Text numberText;
        
        #endregion

        #region Private Fields
        [SerializeField] GameObject highlightIcon;
        [SerializeField] Image image;
        [SerializeField] Button button;

        #endregion

        #region MonoBehaviour

        private void Start() {
            if(spellIcon != null) image.sprite = spellIcon.sprite;
        }

        private void Update() {
            if(Input.GetKeyDown(spellIcon.keyCode)) {
                highlightIcon.SetActive(true);
            } else if(Input.GetKeyUp(spellIcon.keyCode)) {
                highlightIcon.SetActive(false);
            }
            // disabledIcon.SetActive(isDisabled);
            button.interactable = !isDisabled;
        }

        #endregion

        #region Public Methods

        public void SetNumberText(int number) {
            if(numberText != null) {
                numberText.text = number.ToString();
            } else {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> numberText reference on SpellSlot Prefab.", this);
            }
        }

        public void SetSpellIcon(SpellIcon _spellIcon) {
            if(_spellIcon != null) {
                spellIcon = _spellIcon;
            } else {
                Debug.LogWarning("<Color=Red><b>_spellIcon</b></Color> _spellIcon reference on SpellSlot Prefab.", this);
            }
        }

        #endregion
        
    }

}
