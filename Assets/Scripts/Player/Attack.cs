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
        public float coolDown1 = 1;
        [HideInInspector]
        public Image abilityImage1;
        public bool isCoolDown1 = false;
        [HideInInspector]
        public KeyCode keyCode1;
        public int attackDamageStaff = 5;
        public LayerMask staffAttackLayerMask;
        public Vector3 staffAttackPointSize;
        public AnimationClip attackStaff;


        [Header("Spear Spell")]
        public float coolDown2 = 3;
        [HideInInspector]
        public Image abilityImage2;
        public bool isCoolDown2 = false;
        [HideInInspector]
        public KeyCode keyCode2;
        public int attackDamageSpear = 30;
        public GameObject vfxSpearSpell;
        public GameObject vfxSpearSpellPrepare;
        public float castingTime2 = 2f;
        public int manaCost2 = 30;




        [Header("Heal Spell")]
        public float coolDown3 = 3;
        [HideInInspector]
        public Image abilityImage3;
        public bool isCoolDown3 = false;
        [HideInInspector]
        public KeyCode keyCode3;
        public GameObject vfxHealSpell;
        public float castingTime3;
        public int manaCost3 = 30;


        [Header("Teleport Spell")]
        public float coolDown4 = 5;
        [HideInInspector]
        public Image abilityImage4;
        public bool isCoolDown4 = false;
        [HideInInspector]
        public KeyCode keyCode4;
        public GameObject vfxTeleportSpellPrefab;
        [HideInInspector]
        public GameObject vfxTeleportSpell;
        public float castingTime4 = 0.5f;
        public int manaCost4 = 30;
        public float distanceToTeleport = 30f;
        public Vector3 teleportTo;
        public GameObject vfxTeleportPlaceSelection;



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
        private float minMovementDistance = 0.00001f;

        int SPEAR_ATTACK = 1;
        int HEAL_SPELL = 2;
        int TELEPORT_SPELL = 3;
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
            if(!photonView.IsMine) return;
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
            if(isCoolDown4) {
                abilityImage4.fillAmount -= 1 / coolDown4 * Time.deltaTime;
                if(abilityImage4.fillAmount <= 0) {
                    abilityImage4.fillAmount = 0;
                    isCoolDown4 = false;
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
            notMoving = controller.velocity.magnitude <= minMovementDistance && thirdPersonMovement.groundedPlayer;
            ableToAttack = !isCasting;
            animator.SetBool("able_to_attack", ableToAttack);
            /// <summary>
            /// Staff Kick
            /// </summary>
            if(Input.GetKeyDown(keyCode1) && ableToAttack && !isCoolDown1) {
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
                timeToFire = Time.time + castingTime2;
                if(player.currentMana >= manaCost2) {
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
            /// <summary>
            /// Teleport Spell
            /// </summary>
            if(Input.GetKeyDown(keyCode4) && notMoving && ableToAttack && !isCoolDown4) {
                if(player.currentMana >= manaCost4) {
                    animator.SetLayerWeight(animator.GetLayerIndex("Attack_Torso"), 1);
                    attackNumber = TELEPORT_SPELL;
                    animator.SetInteger("attack_num", attackNumber);
                    isCasting = true;
                    animator.SetBool("isCasting", isCasting);
                    RaycastHit hit;
                    if (Physics.Raycast(firepoint.transform.position, transform.forward, out hit, distanceToTeleport))
                    {
                        teleportTo = new Vector3(hit.point.x, hit.point.y, hit.point.z); 
                    }
                    else {
                        teleportTo = transform.position + transform.forward * distanceToTeleport;
                    }
                    photonView.RPC("TeleportSpellRPC", RpcTarget.AllViaServer, teleportTo);
                    if(vfxTeleportPlaceSelection != null) {
                        GameObject _vfxTPS = Instantiate(vfxTeleportPlaceSelection, teleportTo, vfxTeleportPlaceSelection.transform.rotation);
                        Destroy(_vfxTPS, 0.2f);
                    }
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
            SpellProgressIntantiate(castingTime2);
            yield return new WaitForSeconds(castingTime2);
            if(firepoint != null && !resetCasting) {
                Player target = GetComponent<Focus>().focus;
                if(IsAbleToAttack(target) != Vector3.zero && !isCoolDown2 ) {
                    isCoolDown2 = true;
                    abilityImage2.fillAmount = 1;
                    transform.forward = IsAbleToAttack(target);
                    player.SpendMana(manaCost2);
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
        /// Photon RPC Teleport Spell
        /// </summary>
        [PunRPC]
        void TeleportSpellRPC(Vector3 _teleportTo){ 
            currentSpell = TeleportSpell(_teleportTo);
            StartCoroutine(currentSpell);
            vfxTeleportSpell = Instantiate(vfxTeleportSpellPrefab, thirdPersonMovement.cameraLookHere.position, transform.rotation);
        }

        /// <summary>
        /// Teleport Spell
        /// </summary>
        IEnumerator TeleportSpell(Vector3 _teleportTo) {
            SpellProgressIntantiate(castingTime4);
            yield return new WaitForSeconds(castingTime4);
            if(!resetCasting) {
                if(!isCoolDown4 ) {
                    if(photonView.IsMine) {
                        isCoolDown4 = true;
                        abilityImage4.fillAmount = 1;
                        player.SpendMana(manaCost4);
                    }
                    if(vfxTeleportSpell != null)
                    {
                        vfxTeleportSpell.GetComponent<TeleportOrb>().Activate(_teleportTo);
                    }
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
            projectile.owner = player;
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
                if(vfxTeleportSpell != null) {
                    if(vfxTeleportSpell.GetComponent<TeleportOrb>().isActive != true) {
                        Destroy(vfxTeleportSpell);
                    }
                }
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
                    } else if(i == 3) {
                        abilityImage4 = slot.disabledIcon.GetComponent<Image>();
                        keyCode4 = slot.spellIcon.keyCode;
                    }
                    i++;
                }
                abilityImage1.fillAmount = 0;
                abilityImage2.fillAmount = 0;
                abilityImage3.fillAmount = 0;
                abilityImage4.fillAmount = 0;
            } else {
                Debug.LogError("Missed <Color=Red>spellSlots</Color> in FindAndInitializeSpells method");
            }
            
        }

        #endregion
 
    }

}
