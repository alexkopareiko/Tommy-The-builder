using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class InstantiateUI : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField]
        private GameObject playerUiLabelPrefab;

        [SerializeField]
        private GameObject playerUiTopLeftPrefab;

        [SerializeField]
        private GameObject playerUiBottomPanelPrefab;

        [SerializeField]
        private GameObject playerUiMessageToPlayerPrefab;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start() {
            Player player = GetComponent<Player>();
             if(!photonView.IsMine) {
                if (playerUiLabelPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiLabelPrefab);
                    _uiGo.SendMessage("SetTarget", player, SendMessageOptions.RequireReceiver);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUilabelPrefab reference on InstantiateUI .", this);
                }
            }
            else {
                if (playerUiTopLeftPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiTopLeftPrefab);
                    _uiGo.SendMessage("SetTarget", player, SendMessageOptions.RequireReceiver);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUiTopLeftPrefab reference on InstantiateUI.", this);
                }

                if (playerUiBottomPanelPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiBottomPanelPrefab);
                    List<SpellSlot> spellSlots = _uiGo.GetComponent<PlayerUIBottomPanel>().spellSlots;
                    GetComponent<Attack>().FindAndInitializeSpells(spellSlots);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUiBottomPanelPrefab reference on InstantiateUI.", this);
                }
            }    
        }

        #endregion

        #region Public Methods

        public void ShowMessage(string message) {
            if(playerUiMessageToPlayerPrefab != null) {
                PlayerUIMessageToPlayer[] _uis = FindObjectsOfType<PlayerUIMessageToPlayer>();
                if(_uis.Length > 0) {
                    foreach (var ui in _uis)
                    {
                        Destroy(ui.gameObject);
                    }
                }
                GameObject _uiGo = Instantiate(playerUiMessageToPlayerPrefab);
                _uiGo.GetComponent<PlayerUIMessageToPlayer>().SetText(message);
            }
            else {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUiMessageToPlayerPrefab reference on InstantiateUI.", this);
            }
        }

        #endregion
        
    }
}
