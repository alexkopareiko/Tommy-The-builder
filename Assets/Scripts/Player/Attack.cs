using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class Attack : MonoBehaviourPunCallbacks, IPunObservable
    {

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if(stream.IsWriting) {
                // we own this player: send the others our data
                stream.SendNext(ableToAttack);
                stream.SendNext(attackNumber);
                stream.SendNext(isCasting);
                stream.SendNext(notMoving);
            }
            else {
                // Network player, receive data
                this.ableToAttack = (bool)stream.ReceiveNext();
                this.attackNumber = (int)stream.ReceiveNext();
                this.isCasting = (bool)stream.ReceiveNext();
                this.notMoving = (bool)stream.ReceiveNext();
            }
        }

        #endregion

        #region Public Fields

        public GameObject firepoint;
        public GameObject LeftHandPoint;
        public GameObject staffHitPoint;
        public int attackDamageStaff = 5;
        public int attackDamageSpear = 20;
        public int attackDamageFireball = 40;
        public LayerMask staffAttackLayerMask;
        public float staffAttackPointRadius = 2f;
        public List<GameObject> vfx = new List<GameObject> ();

        #region AnimationClips
        public AnimationClip attackFireball;
        public AnimationClip attackShield;
        public AnimationClip attackSpear;
        public AnimationClip attackStaff;
        #endregion

        public bool isCasting = false;
        public IEnumerator currentSpell;


        #endregion


        #region Private Fields

        private Animator animator;
        private float timeToFire;
        private bool ableToAttack;
        private bool notMoving;
        private CharacterController controller;
        private Player player;
        private ThirdPersonMovement thirdPersonMovement;
        private int attackNumber;
        private SphereCollider staffHitPointCollider;
        private bool resetCasting;

        int SPEAR_ATTACK = 1;
        // int FIREBALL_ATTACK = 2;
        // int SHIELD_ATTACK = 3;
        int STAFF_ATTACK = 4;

        #endregion


        #region MonoBehaviour Callbacks

        void Start()
        {
            player = GetComponent<Player>();
            thirdPersonMovement = GetComponent<ThirdPersonMovement>();
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();
            staffHitPointCollider = staffHitPoint.GetComponent<SphereCollider>();
            staffHitPointCollider.radius = staffAttackPointRadius;
            staffHitPointCollider.enabled = false;
        }

        private void Update() {

            if(photonView.IsMine) {
                AttackFunc();
            }
            if(resetCasting) {
                ResetCasting();
            }

            staffHitPointCollider.enabled = attackNumber == STAFF_ATTACK;
        }

        #endregion


        #region Private Methods

        void AttackFunc()
        {
            if(player.currentHealth <= 0) return; 
            animator.SetInteger("attack_num", attackNumber);
            notMoving = controller.velocity.magnitude == 0 && thirdPersonMovement.groundedPlayer;
            // ableToAttack = Time.time >= timeToFire && !isCasting;
            ableToAttack = true;
            animator.SetBool("able_to_attack", ableToAttack);
            if(Input.GetKeyDown(KeyCode.Alpha1) && notMoving && ableToAttack) {
                Spell spell = vfx[0].GetComponent<Spell>();
                timeToFire = Time.time + spell.castingTime;
                if(player.currentMana >= spell.manaCost) {
                    animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
                    attackNumber = SPEAR_ATTACK;
                    animator.SetInteger("attack_num", attackNumber);
                    isCasting = true;
                    animator.SetBool("isCasting", isCasting);
                    currentSpell = SpearAttack(spell);
                    StartCoroutine(currentSpell);
                }
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2) && ableToAttack) {
                animator.SetLayerWeight(animator.GetLayerIndex("Attack_Torso"), 1);
                attackNumber = STAFF_ATTACK;
                currentSpell = StaffAttack(attackStaff.length);
                StartCoroutine(currentSpell);
            }
            if(!notMoving) {
                resetCasting = true;
            }
        }

        IEnumerator SpearAttack(Spell spell) {
            yield return new WaitForSeconds(spell.castingTime);
            if(firepoint != null && !resetCasting) {
                player.SpendMana(spell.manaCost);
                photonView.RPC("ThrowSpell", RpcTarget.AllViaServer, 0);
                resetCasting = true;
            } else {
                Debug.Log("No Fire Point for Spear Attack");
            }
        }

        [PunRPC]
        void ThrowSpell(int _vfxIndex){
            Spell projectile = vfx[_vfxIndex].GetComponent<Spell>();
            Instantiate(projectile, firepoint.transform.position, player.transform.rotation);
        }

        IEnumerator StaffAttack(float castingTime) {
            yield return new WaitForSeconds(castingTime);
            isCasting = true;
            resetCasting = true;
        }

        // void FireballAttack() {
        //     if(LeftHandPoint != null) {
        //         if(ableToAttack) {
        //             Instantiate(vfx[1], LeftHandPoint.transform.position, player.transform.rotation);
        //         }
        //     } else {
        //         Debug.Log("No Fire Point"); 
        //     }
        //     animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 0);
        // }

        void OnDrawGizmosSelected() {
            if(staffHitPoint != null) {
                Gizmos.DrawWireSphere(staffHitPoint.transform.position, staffAttackPointRadius);
            }
        }
    
        #endregion


        #region Public Methods

        public void ResetCasting() {
            resetCasting = false;
             if(currentSpell != null) {
                StopCoroutine(currentSpell);
                StopAllCoroutines();
                currentSpell = null;
                isCasting = false;
                animator.SetBool("isCasting", isCasting);
                animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 0);
                animator.SetLayerWeight(animator.GetLayerIndex("Attack_Torso"), 0);
                // animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 0);
                timeToFire = 0;
                attackNumber = 0;
            }
        }

        #endregion
 
    }

}
