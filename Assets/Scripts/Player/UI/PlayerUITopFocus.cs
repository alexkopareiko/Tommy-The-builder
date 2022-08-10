using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Com.NikfortGames.MyGame {
    public class PlayerUITopFocus : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField] private TMP_Text playerNameText;

        [Tooltip("UI Text to display Player's Mana")]
        [SerializeField] private TMP_Text playerManaText;

        [Tooltip("UI Text to display Player's Health")]
        [SerializeField] private TMP_Text playerHealthText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private Slider playerManaSlider;

        private Player target;
        CanvasGroup _canvasGroup;


        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update() {
            if(target != null) {
                
            }
            // Reflect the Player Health
            if(playerHealthSlider != null) {
                playerHealthSlider.value = target.currentHealth;
            }
            // Reflect the Player Mana
            if(playerManaSlider != null) {
                playerManaSlider.value = target.currentMana;
            }

            if(target != null) {
                playerHealthText.text = target.currentHealth.ToString() + "/" + target.maxHealth.ToString();
                playerManaText.text = target.currentMana.ToString() + "/" + target.maxMana.ToString();
            }

            //Destroy itself if the target is null. It's a fail safe when Photon is destroying Instances of a Player over the network
            if(target == null) {
                Destroy(gameObject);
                return;
            }
        }

        #endregion


        #region Public Methods

        public void SetTarget(Player _target) {
            if(_target == null) {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Player _target target for PlayerUITopFocus.SetTarget", this);
                return;
            }
            // Cache references for efficiency
            target = _target;

            if(playerNameText != null) {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }
        
        #endregion
        
    }
    
}

