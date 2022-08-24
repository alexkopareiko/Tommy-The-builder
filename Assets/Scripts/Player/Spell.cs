using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.NikfortGames.MyGame {
    public class Spell : MonoBehaviour
    {

        #region Public Fields

        public float speed;
        public string spellName;
        public int manaCost;
        public float castingTime;

        [Tooltip("time after casting")]
        public float coolDown;
        public GameObject muzzlePrefab;
        public GameObject hitPrefab;
        public Player target;

        #endregion

        #region MonoBehavior Callbacks

        private void Start() {
            Destroy(gameObject, 5);
            if(muzzlePrefab != null) {
                GameObject muzzleVFX = Instantiate(muzzlePrefab, transform.position, transform.rotation); 
                DestroyMuzzle(muzzleVFX);
            }
        }

        private void Update() {
            if(speed != 0) {
                
                transform.position += transform.forward * (speed * Time.deltaTime);
            } else {
                Debug.Log("No speed");
            }
        }

        private void LateUpdate() {
            if(speed != 0) {
                if(target != null) {
                    Transform lookAt = target.GetComponent<ThirdPersonMovement>().cameraLookHere;
                    transform.LookAt(lookAt.position);
                }
            }
        }

        private void OnTriggerEnter(Collider other) {
            // if(hitPrefab != null) {
            //     GameObject hitVFX = Instantiate(hitPrefab, transform.position, transform.rotation); 
            // }
            Destroy(gameObject, 0.5f);
        }

        #endregion

        #region Private Methods

        void DestroyMuzzle(GameObject vfx) {
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            if(ps != null) {
                    Destroy(vfx, ps.main.duration);
            }
            else {
                ParticleSystem psChild = vfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(vfx, psChild.main.duration);
            }
        }

        #endregion

        #region Public Methods

        public void SetTarget(Player _target) {
            if(_target == null) {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Player _target target for Spell.SetTarget", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
        }

        #endregion

    }

}

