using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using Photon.Pun;


namespace Com.NikfortGames.MyGame {
    public class Focus : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public Material matOfTerrainWithCircleShader;
        public float focusDistance = 60f;

        [Tooltip("No need initialize")]
        public Player focus;

        [Tooltip("Focus character prefab near mine")]
        public GameObject playerUITopFocusPrefab;
        #endregion

        #region Private Fields

        GameObject playerUITopFocus;

        #endregion

        #region MonoBehavoiur Callbacks

        private void Update() {
            if(photonView.IsMine) {
                InteractWith();
                SetCirclePosition();
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

        void SetCirclePosition() {
            if(focus) {
                matOfTerrainWithCircleShader.SetVector("_Center", new Vector4(focus.transform.position.x, focus.transform.position.y, focus.transform.position.z, 0f));
            } else {
                matOfTerrainWithCircleShader.SetVector("_Center", new Vector4(10000, 10000, 10000, 0f));
            }
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
                    } else {
                         focus = null;
                    }
                    SetPlayerUITopFocus();
                }
            }
        }

        #endregion
        
    }
}

