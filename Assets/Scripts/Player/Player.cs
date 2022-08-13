using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class Player : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
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

        #region Photon Callbacks

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = this.gameObject;
        }


        #endregion

        #region Public Fields

        public int maxHealth = 100;
        public int currentHealth;
        public int maxMana = 100;
        public int currentMana;
        public int ownerId;

        public float delayManaInc = 1f;

        #endregion

        #region Private Fields
        private Animator animator;

        protected float timer;

        [SerializeField] private AnimationClip getDamageAC;

        #endregion



        #region MonoBehaviour Callbacks

        void Start()
        {
            ownerId = PhotonNetwork.LocalPlayer.ActorNumber;
            currentHealth = maxHealth;
            currentMana = maxMana;
            animator = GetComponentInChildren<Animator>();
        }

        private void Update() {
            if(photonView.IsMine) {
                CheckIfDeadAndReAssign();
                ManaIncrement();
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

        void ManaIncrement(){
            timer += Time.deltaTime;
            if (timer >= delayManaInc)
            {
                timer = 0f;
                currentMana++;
                if(currentMana > maxMana) currentMana = maxMana;
            }
        }

        void Die() {
            animator.SetLayerWeight(animator.GetLayerIndex("Die"), 1);
            animator.SetBool("isDead", true);
            StartCoroutine(SpawnPlayers.RespawnMe(this));
        }

        void CheckIfDeadAndReAssign(){
            if(animator.GetBool("isDead") && currentHealth > 0) {
                animator.SetLayerWeight(animator.GetLayerIndex("Die"), 0);
                animator.SetBool("isDead", false);
            }
        }

        

        
        void PlayerHitAnimation(Vector3 bulletDir) {
            float angle = Helpers.GetXZAngle(bulletDir, transform.forward);
            animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 2);
            if(angle <= 90) {
                animator.SetBool("get_damage_back", true);
            } else{
                animator.SetBool("get_damage_front", true);
            }
            GetComponent<Attack>().ResetCasting();
            Invoke("ReturnAnimation", getDamageAC.length);
        }

        void ReturnAnimation() {
            animator.SetBool("get_damage_back", false);
            animator.SetBool("get_damage_front", false);
            animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 0);
        }

        #endregion

    }

}

