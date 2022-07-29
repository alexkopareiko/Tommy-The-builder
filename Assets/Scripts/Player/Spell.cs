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

        private void OnTriggerEnter(Collider other) {
            Destroy(gameObject);
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

    }

}

