using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.NikfortGames.MyGame {
    public class TeleportOrb : MonoBehaviour
    {
        #region Public Fields

        public Vector3 teleportTo;
        public float timeToDestroy = 1f;
        public bool isActive {get; private set;}

        #endregion

        #region Private Fields

        private float timeStart;

        private Vector3 scaleStep;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake() {
            GetComponent<SphereCollider>().enabled = false;
        }

        private void Update() {
            GetComponent<SphereCollider>().enabled = isActive;
            if(!isActive) return;
            if(Time.time  >= timeStart) Destroy(gameObject);
            transform.localScale -= scaleStep;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")) {
                if(other.GetComponent<Player>().photonView.IsMine) {
                    StartCoroutine(other.GetComponent<ThirdPersonMovement>().TeleportMe(teleportTo));
                }
            }
        }

        #endregion

        #region Public Methods

        public void Activate(Vector3 _teleportTo){
            if(_teleportTo != null) {
                teleportTo = _teleportTo;
                isActive = true;
                timeStart = Time.time + timeToDestroy;
                scaleStep = transform.localScale / (timeToDestroy / Time.deltaTime);
            }
            else {
                Debug.LogError("Missed <Color=Red>_teleportTo</Color> in Activate method [TeleportOrb]");
            }
        }

        public void DestroyMe(){
            Destroy(gameObject);
        }

        #endregion
        
    }

}

