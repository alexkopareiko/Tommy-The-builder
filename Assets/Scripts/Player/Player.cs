using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class Player : MonoBehaviourPunCallbacks, IPunObservable
    {

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if(stream.IsWriting) {
                // we own this player: send the others our data
                stream.SendNext(currentHealth);
                stream.SendNext(currentMana);
                stream.SendNext(ownerId);
            }
            else {
                // Network player, receive data
                this.currentHealth = (int)stream.ReceiveNext();
                this.currentMana = (int)stream.ReceiveNext();
                this.ownerId = (int)stream.ReceiveNext();
            }
        }

        #endregion

        #region Public Fields

        public int maxHealth = 100;
        public int currentHealth;
        public int maxMana = 100;
        public int currentMana;
 
        public int ownerId;

        #endregion

        #region Private Fields
        private Animator animator;

        [SerializeField]
        private GameObject playerUiLabelPrefab;

        [SerializeField]
        private GameObject playerUiTopLeftPrefab;

        #endregion



        #region MonoBehaviour Callbacks

        void Start()
        {
            // ownerId = PhotonNetwork.LocalPlayer.ActorNumber;
            ownerId = photonView.ViewID;
            currentHealth = maxHealth;
            currentMana = maxMana;
            animator = GetComponentInChildren<Animator>();
            if(!photonView.IsMine) {
                if (playerUiLabelPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiLabelPrefab);
                    _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUilabelPrefab reference on player Prefab.", this);
                }
            }
            else {
                if (playerUiTopLeftPrefab != null)
                {
                    GameObject _uiGo = Instantiate(playerUiTopLeftPrefab);
                    _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
                } else
                {
                    Debug.LogWarning("<Color=Red><b>Missing</b></Color> playerUiTopLeftPrefab reference on player Prefab.", this);
                }
            }            


        }

  
        /// <summary> 
        /// Player gets damage
        /// </summary>
        private void OnTriggerEnter(Collider other) {
            if(!photonView.IsMine) {
                return;
            }
            if(other.CompareTag("PlayerStaffHit")) {
                int damage = GetComponent<Attack>().attackDamageStaff;
                TakeDamage(damage);
                PlayerHitAnimation(other.transform.forward);
            } else if(other.CompareTag("SpearSpell")) {
                int damage = GetComponent<Attack>().attackDamageSpear;
                TakeDamage(damage);
                PlayerHitAnimation(other.transform.forward);
            } else if(other.CompareTag("FireballSpell")) {
                int damage = GetComponent<Attack>().attackDamageFireball;
                TakeDamage(damage);
                PlayerHitAnimation(other.transform.forward);
            }
            
        }

        #endregion


        #region Public Methods



        public void TakeDamage(int damage){
            if(currentHealth > 0) {
                currentHealth -= damage;
                if(currentHealth <= 0) {
                    Die();
                }
            }
        }

        public void SpendMana(int mana){
            currentMana -= mana;
        }

        #endregion


        #region Private Methods

        void Die() {
            animator.SetLayerWeight(animator.GetLayerIndex("Die"), 1);
            animator.SetBool("isDead", true);
        }

        
        void PlayerHitAnimation(Vector3 bulletDir) {
            Vector2 bulletVector = new Vector2(bulletDir.x, bulletDir.z);
            Vector2 myVector = new Vector2(transform.forward.x, transform.forward.z);
            float angle = Vector2.Angle(bulletVector, myVector);
            animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 2);
            if(angle <= 90) {
                animator.SetTrigger("get_damage_back");
            } else{
                animator.SetTrigger("get_damage_front");
            }
            GetComponent<Attack>().ResetCasting();
        }

        #endregion





    }

}

