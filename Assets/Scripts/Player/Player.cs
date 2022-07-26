using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.NikfortGames.MyGame {
    public class Player : MonoBehaviour
    {

        #region Public Fields

        public int maxHealth = 100;
        public int currentHealth;
        public int maxMana = 100;
        public int currentMana;

        #endregion

        #region Private Fields
        private Animator animator;

        #endregion



        #region MonoBehaviour Callbacks

        void Start()
        {
            currentHealth = maxHealth;
            currentMana = maxMana;
            animator = GetComponentInChildren<Animator>();
        }

        #endregion


        #region Public Methods

        public void TakeDamage(int damage){
            if(currentHealth > 0) {
                currentHealth -= damage;
                Debug.Log("currentHealth " + currentHealth);
                if(currentHealth <= 0) {
                    Die();
                }
            }
        }

        public void PlayerHit(Vector3 bulletDir, Vector3 myDir) {
            Vector2 bulletVector = new Vector2(bulletDir.x, bulletDir.z);
            Vector2 myVector = new Vector2(myDir.x, myDir.z);
            float angle = Vector2.Angle(bulletVector, myVector);
            animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 2);
            if(angle <= 90) {
                animator.SetTrigger("get_damage_back");
            } else{
                animator.SetTrigger("get_damage_front");
            }
            GetComponent<Attack>().ResetCasting();
        }
        public void SpendMana(int mana){
            currentMana -= mana;
            Debug.Log("currentMana " + currentMana);
        }

        #endregion


        #region Private Methods

        void Die() {
            Debug.Log("Player died");
            animator.SetLayerWeight(animator.GetLayerIndex("Die"), 1);
            animator.SetBool("isDead", true);

            // Die animation

            // Disable the enemy
        }

        #endregion





    }

}

