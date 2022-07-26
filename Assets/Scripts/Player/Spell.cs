using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.NikfortGames.MyGame {
    public class Spell : MonoBehaviour
    {

        #region Public Fields

        public float speed;
        public string spellName;
        public int damage;
        public int manaCost;
        public float castingTime;

        [Tooltip("time after casting")]
        public float coolDown;
        public GameObject muzzlePrefab;
        public GameObject hitPrefab;

        #endregion

        #region Private Fields

        private void Start() {
            Destroy(gameObject, 5);
            if(muzzlePrefab != null) {
                GameObject muzzleVFX = Instantiate(muzzlePrefab, transform.position, transform.rotation); 
                DestroyGO(muzzleVFX);
            }
        }

        private void Update() {
            if(speed != 0) {
                transform.position += transform.forward * (speed * Time.deltaTime);
            } else {
                Debug.Log("No speed");
            }
        }

        private void OnCollisionEnter(Collision other) {
            speed = 0;
            if(other.gameObject.CompareTag("Player")){
                Player enemy = other.gameObject.GetComponent<Player>();
                enemy.TakeDamage(damage);
                enemy.PlayerHit(transform.forward, other.transform.forward);
            }
            ContactPoint contact = other.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            if(hitPrefab != null) {
                GameObject hitVFX = Instantiate(hitPrefab, pos, rot);
                DestroyGO(hitVFX);
            }
            Destroy(gameObject);    
        }

        #endregion

        #region Private Methods

        void DestroyGO(GameObject vfx) {
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

