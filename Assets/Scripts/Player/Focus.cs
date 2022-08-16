using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using Photon.Pun;


namespace Com.NikfortGames.MyGame {
    public class Focus : MonoBehaviourPunCallbacks, IPunObservable
    {

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if(stream.IsWriting) {
                // we own this player: send the others our data
                stream.SendNext(focusId);
            }
            else {
                // Network player, receive data
                this.focusId = (int)stream.ReceiveNext();
            }
        }

        #endregion

        #region Public Fields

        public Material matOfTerrainWithCircleShader;
        public float focusDistance = 60f;

        [Tooltip("No need initialize")]
        public Player focus;

        [Tooltip("No need initialize. The view ID of focus character")]
        public int focusId;

        [Tooltip("Focus character prefab near mine")]
        public GameObject playerUITopFocusPrefab;
        
        public int EMPTY_FOCUS = -1;

        #endregion

        #region Private Fields

        GameObject playerUITopFocus;


        #endregion

        #region MonoBehavoiur Callbacks

        private void Update() {
            if(photonView.IsMine) {
                // Debug.LogError(GetComponent<Player>().ownerId);
                InteractWith();
                GameManager.instance.SetCircleSelection(focus);
                DeFocusOnDistance();               
            } 
        }


        #endregion

        #region Private Methods

        void SetPlayerUITopFocus() {
            if(playerUITopFocusPrefab != null) {
                if(focus != null) {
                    if(playerUITopFocus != null) Destroy(playerUITopFocus.gameObject);
                    playerUITopFocus = Instantiate(playerUITopFocusPrefab);
                    playerUITopFocus.SendMessage("SetTarget", focus, SendMessageOptions.RequireReceiver);
                } else if (focus == null && playerUITopFocus != null) {
                    Destroy(playerUITopFocus.gameObject);
                    playerUITopFocus = null; 
                } 
            } else {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUITopFocus reference on player Prefab.", this);
            }
        }

        void DeFocusOnDistance(){
            if(focus == null) return;
            float dist = Vector3.Distance(focus.transform.position, Camera.main.transform.position);
            if(dist >= focusDistance) focus = null;
        }

        void InteractWith() {
            if (EventSystem.current.IsPointerOverGameObject())
            return;

            if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, focusDistance))
                {
                    Player _enemy = hit.collider.GetComponent<Player>();
                    int _myId = GetComponent<Player>().ownerId;
                    if (_enemy != null && _myId != _enemy.ownerId)
                    {
                        focus = _enemy;
                        focusId = _enemy.ownerId;
                    } else {
                        focus = null;
                        focusId = EMPTY_FOCUS;
                    }
                    SetPlayerUITopFocus();
                }
            }
        }

        #endregion
        
    }
}

