using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUilabelPrefab reference on player Prefab.", this);
                }
            }
            else {
                if (playerUiTopLeftPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiTopLeftPrefab);
                    _uiGo.SendMessage("SetTarget", player, SendMessageOptions.RequireReceiver);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUiTopLeftPrefab reference on player Prefab.", this);
                }

                if (playerUiBottomPanelPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiBottomPanelPrefab);
                    List<SpellSlot> spellSlots = _uiGo.GetComponent<PlayerUIBottomPanel>().spellSlots;
                    GetComponent<Attack>().FindAndInitializeSpells(spellSlots);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUiBottomPanelPrefab reference on player Prefab.", this);
                }
            }    
        }

        #endregion
        
    }
}
