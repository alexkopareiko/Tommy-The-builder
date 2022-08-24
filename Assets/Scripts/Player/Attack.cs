using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public bool isCasting = false;
        public IEnumerator currentSpell;
        public SpellProgress spellProgressPrefab;



        [Header("Staff Attack Spell")]
        [Tooltip("No need initialize")]
        public Image abilityImage1;
        public float coolDown1 = 1;
        public bool isCoolDown1 = false;
        [Tooltip("No need initialize")]
        public KeyCode keyCode1;
        public int attackDamageStaff = 5;
        public LayerMask staffAttackLayerMask;
        public Vector3 staffAttackPointSize;
        public AnimationClip attackStaff;


        [Header("Spear Spell")]
        [Tooltip("No need initialize")]
        public Image abilityImage2;
        public float coolDown2 = 5;
        public bool isCoolDown2 = false;
        [Tooltip("No need initialize")]
        public KeyCode keyCode2;
        public int attackDamageSpear = 20;
        public GameObject vfxSpearSpell;
        public GameObject vfxSpearSpellPrepare;



        [Header("Heal Spell")]
        [Tooltip("No need initialize")]
        public Image abilityImage3;
        public float coolDown3 = 5;
        public bool isCoolDown3 = false;
        [Tooltip("No need initialize")]
        public KeyCode keyCode3;
        public GameObject vfxHealSpell;
        public float castingTime3;
        public int manaCost3 = 30;



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
        private BoxCollider staffHitPointCollider;
        private bool resetCasting;
        private SpellProgress spellProgress;

        int SPEAR_ATTACK = 1;
        int HEAL_SPELL = 2;
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
            staffHitPointCollider = staffHitPoint.GetComponent<BoxCollider>();
            staffHitPointCollider.size = staffAttackPointSize;
            staffHitPointCollider.enabled = false;
        }

        private void Update() {
            if(photonView.IsMine) {
                AttackFunc();
                RunCoolDowns();
            }
            VFXSpellCheck();
            if(resetCasting) {
                ResetCasting();
            }
            staffHitPointCollider.enabled = attackNumber == STAFF_ATTACK;

        }

        #endregion


        #region Private Methods

        /// <summary>
        /// For sure, need to refactor, but for now...
        /// </summary>

        void SpellProgressIntantiate(float time) {
            spellProgress = Instantiate(spellProgressPrefab);
            spellProgress.slider.maxValue = time;
        }
        /// <summary>
        /// For sure, need to refactor, but for now...
        /// </summary>
        void RunCoolDowns() {
            if(isCoolDown1) {
                abilityImage1.fillAmount -= 1 / coolDown1 * Time.deltaTime;
                if(abilityImage1.fillAmount <= 0) {
                    abilityImage1.fillAmount = 0;
                    isCoolDown1 = false;
                }
            }
            if(isCoolDown2) {
                abilityImage2.fillAmount -= 1 / coolDown2 * Time.deltaTime;
                if(abilityImage2.fillAmount <= 0) {
                    abilityImage2.fillAmount = 0;
                    isCoolDown2 = false;
                }
            }
            if(isCoolDown3) {
                abilityImage3.fillAmount -= 1 / coolDown3 * Time.deltaTime;
                if(abilityImage3.fillAmount <= 0) {
                    abilityImage3.fillAmount = 0;
                    isCoolDown3 = false;
                }
            }
        }

        /// <summary>
        /// General Attack mathod
        /// </summary>
        void AttackFunc()
        {
            if(player.currentHealth <= 0) return; 
            animator.SetInteger("attack_num", attackNumber);
            notMoving = controller.velocity.magnitude == 0 && thirdPersonMovement.groundedPlayer;
            // ableToAttack = Time.time >= timeToFire && !isCasting;
            ableToAttack = !isCasting;
            animator.SetBool("able_to_attack", ableToAttack);
            /// <summary>
            /// Staff Kick
            /// </summary>
            if(Input.GetKeyDown(keyCode1) && ableToAttack && !isCoolDown1) {
                Debug.Log("Hit with staff");
                Player _target = GetComponent<Focus>().focus;
                if(_target == null) {
                    GetComponent<InstantiateUI>().ShowMessage("No target selected.");
                    return;
                }
                animator.SetLayerWeight(animator.GetLayerIndex("Attack_Torso"), 1);
                attackNumber = STAFF_ATTACK;
                currentSpell = StaffAttack(attackStaff.length);
                StartCoroutine(currentSpell);
            }
            /// <summary>
            /// Spear Spell
            /// </summary>
            if(Input.GetKeyDown(keyCode2) && notMoving && ableToAttack && !isCoolDown2) {
                Player _target = GetComponent<Focus>().focus;
                if(_target == null || _target.currentHealth <= 0) {
                    GetComponent<InstantiateUI>().ShowMessage("No target selected.");
                    return;
                }
                Spell spell = vfxSpearSpell.GetComponent<Spell>();
                timeToFire = Time.time + spell.castingTime;
                if(player.currentMana >= spell.manaCost) {
                    animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
                    attackNumber = SPEAR_ATTACK;
                    animator.SetInteger("attack_num", attackNumber);
                    isCasting = true;
                    animator.SetBool("isCasting", isCasting);
                    currentSpell = SpearAttack(spell);
                    StartCoroutine(currentSpell);
                } else {
                    GetComponent<InstantiateUI>().ShowMessage("Not enough mana.");
                }
            }
            /// <summary>
            /// Heal Spell
            /// </summary>
            if(Input.GetKeyDown(keyCode3) && notMoving && ableToAttack && !isCoolDown3) {
                if(player.currentMana >= manaCost3) {
                    animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
                    attackNumber = HEAL_SPELL;
                    animator.SetInteger("attack_num", attackNumber);
                    isCasting = true;
                    animator.SetBool("isCasting", isCasting);
                    currentSpell = HealSpell();
                    StartCoroutine(currentSpell);
                } else {
                    GetComponent<InstantiateUI>().ShowMessage("Not enough mana.");
                }
            }
            if(!notMoving && attackNumber != STAFF_ATTACK) {
                resetCasting = true;
            }
        }

        void VFXSpellCheck() {
            vfxHealSpell.SetActive(attackNumber == HEAL_SPELL);
            vfxSpearSpellPrepare.SetActive(attackNumber == SPEAR_ATTACK);
        }

        /// <summary>
        /// Spear Spell Attack
        /// </summary>
        IEnumerator SpearAttack(Spell spell) {
            SpellProgressIntantiate(spell.castingTime);
            yield return new WaitForSeconds(spell.castingTime);
            if(firepoint != null && !resetCasting) {
                Player target = GetComponent<Focus>().focus;
                if(IsAbleToAttack(target) != Vector3.zero && !isCoolDown2 ) {
                    isCoolDown2 = true;
                    abilityImage2.fillAmount = 1;
                    transform.forward = IsAbleToAttack(target);
                    player.SpendMana(spell.manaCost);
                    photonView.RPC("ThrowSpearSpell", RpcTarget.AllViaServer);
                } else {
                    GetComponent<InstantiateUI>().ShowMessage("No target selected.\nOut of angle range.");
                }
                resetCasting = true;
            } else {
                Debug.Log("No Fire Point for Spear Attack");
            }
        }

        /// <summary>
        /// Heal Spell
        /// </summary>
        IEnumerator HealSpell() {
            SpellProgressIntantiate(castingTime3);
            yield return new WaitForSeconds(castingTime3);
            if(!resetCasting) {
                if(!isCoolDown3 ) {
                    isCoolDown3 = true;
                    abilityImage3.fillAmount = 1;
                    player.SpendMana(manaCost3);
                    player.Heal(manaCost3);
                } else {
                    GetComponent<InstantiateUI>().ShowMessage("Cooldown is not over");
                }
                resetCasting = true;
            } else {
                Debug.Log("No Fire Point for Spear Attack");
            }
        }


        /// <summary>
        /// Check if target is in attack range
        /// </summary>
        Vector3 IsAbleToAttack(Player _target) {
            if(_target != null) {
                Vector3 diff = _target.transform.position - transform.position;
                float angle = Helpers.GetXZAngle(diff, transform.forward);
                if(angle <= 90) {
                    return new Vector3(diff.x, 0, diff.z);
                } else return Vector3.zero;
            }
            return Vector3.zero;
        }


        /// <summary>
        /// Photon RPC Spear Spell
        /// </summary>
        [PunRPC]
        void ThrowSpearSpell(){
            Player target = GetComponent<Focus>().focus;
            Spell projectile = vfxSpearSpell.GetComponent<Spell>();
            if(photonView.IsMine)  {
                if(target != null) {
                    Spell _projectile = Instantiate(projectile, firepoint.transform.position, player.transform.rotation);
                    _projectile.SetTarget(target);
                }
            } else {
                int playerUITopFocus = GetComponent<Focus>().focusId;
                int EMPTY_FOCUS = GetComponent<Focus>().EMPTY_FOCUS;
                if(playerUITopFocus != EMPTY_FOCUS) {
                    var _target = PhotonNetwork.CurrentRoom.GetPlayer(playerUITopFocus);
                    if(_target != null) {
                        Spell _projectile = Instantiate(projectile, firepoint.transform.position, player.transform.rotation);
                        GameObject _t = _target.TagObject as GameObject;
                        _projectile.SetTarget(_t.GetComponent<Player>());
                    }
                }
            }
        }

        /// <summary>
        /// Staff Attack
        /// </summary>
        IEnumerator StaffAttack(float castingTime) {
            yield return new WaitForSeconds(castingTime);
            isCasting = true;
            isCoolDown1 = true;
            abilityImage1.fillAmount = 1;
            resetCasting = true;
        }

        void OnDrawGizmosSelected() {
            if(staffHitPoint != null) {
                Gizmos.DrawWireCube(staffHitPoint.transform.position, staffAttackPointSize);
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
                
                timeToFire = 0;
                attackNumber = 0;
                if(spellProgress != null) {
                    spellProgress.DestroySlider();
                }
            }
        }

        /// <summary>
        /// It's called fromInstantiateUI script
        /// Need to refactor that
        /// </summary>
        public void FindAndInitializeSpells(List<SpellSlot> spellSlots) {
            if(spellSlots != null) {
                int i = 0;
                foreach (SpellSlot slot in spellSlots)
                {
                    if(i == 0) {
                        abilityImage1 = slot.disabledIcon.GetComponent<Image>();
                        keyCode1 = slot.spellIcon.keyCode;
                    } else if(i == 1) {
                        abilityImage2 = slot.disabledIcon.GetComponent<Image>();
                        keyCode2 = slot.spellIcon.keyCode;
                    } else if(i == 2) {
                        abilityImage3 = slot.disabledIcon.GetComponent<Image>();
                        keyCode3 = slot.spellIcon.keyCode;
                    }
                    i++;
                }
                abilityImage1.fillAmount = 0;
                abilityImage2.fillAmount = 0;
                abilityImage3.fillAmount = 0;
            } else {
                Debug.LogError("Missed <Color=Red>spellSlots</Color> in FindAndInitializeSpells method");
            }
            
        }

        #endregion
 
    }

}
