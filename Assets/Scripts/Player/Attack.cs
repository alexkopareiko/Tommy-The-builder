using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject firepoint;
    public GameObject LeftHandPoint;
    public GameObject staffHitPoint;
    public int attackDamageStaff = 5;
    public LayerMask staffAttackLayerMask;
    public float staffAttackPointRadius = 2f;
    public List<GameObject> vfx = new List<GameObject> ();
    public AnimationClip attackFireball;
    public AnimationClip attackShield;
    public AnimationClip attackSpear;
    public AnimationClip attackStaff;
    public bool isCasting = false;
    public IEnumerator currentSpell;

    private Animator animator;
    private float timeToFire;
    private bool ableToAttack;
    private bool notMoving;
    private CharacterController controller;
    private Player player;
    private ThirdPersonMovement thirdPersonMovement;


    void Start()
    {
        player = GetComponent<Player>();
        thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(player.currentHealth <= 0) return; 

        notMoving = controller.velocity.magnitude == 0 && thirdPersonMovement.groundedPlayer;
        ableToAttack = Time.time >= timeToFire && !isCasting;
        animator.SetBool("able_to_attack", ableToAttack);
        if(Input.GetKeyDown(KeyCode.Alpha1) && notMoving && ableToAttack) {
            Spell spell = vfx[0].GetComponent<Spell>();
            timeToFire = Time.time + spell.castingTime;
            int manaCost = spell.manaCost;
            if(player.currentMana >= manaCost) {
                isCasting = true;
                animator.SetBool("isCasting", isCasting);
                animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
                animator.SetTrigger("attack_spear");
                currentSpell = SpearAttack(spell);
                StartCoroutine(currentSpell);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && ableToAttack) {
            animator.SetLayerWeight(animator.GetLayerIndex("Attack_Torso"), 1);
            animator.SetTrigger("attack_staff");
            currentSpell = StaffAttack(attackStaff.length);
            StartCoroutine(currentSpell);
        }
        // else if(Input.GetKeyDown(KeyCode.Alpha2) && Time.time >= timeToFire) {
        //     timeToFire = Time.time + 1 / vfx[0].GetComponent<Spell>().fireRate;
        //     animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
        //     animator.SetTrigger("attack_staff");
        //     Invoke("StaffAttack", attackStaff.length);
        // }
        // else if(Input.GetKeyDown(KeyCode.Alpha3) && Time.time >= timeToFire) {
        //     timeToFire = Time.time + 1 / vfx[1].GetComponent<Spell>().fireRate;
        //     animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
        //     animator.SetTrigger("attack_fireball");
        //     Invoke("FireballAttack", attackFireball.length);
        // }
        // else if(Input.GetKeyDown(KeyCode.Alpha4) && Time.time >= timeToFire) {
        //     timeToFire = Time.time + 1 / vfx[1].GetComponent<Spell>().fireRate;
        //     animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 1);
        //     animator.SetTrigger("attack_shield");
        //     // Invoke("SpawnVFX", attackShield.length);
        // } else {
        // }

        if(!notMoving) {
            ResetCasting();
        }
    }

    void PlayerHit(Vector3 bulletDir, Vector3 myDir) {
        Vector2 bulletVector = new Vector2(bulletDir.x, bulletDir.z);
        Vector2 myVector = new Vector2(myDir.x, myDir.z);
        float angle = Vector2.Angle(bulletVector, myVector);
        animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 2);
        if(angle <= 90) {
            animator.SetTrigger("get_damage_back");
        } else{
            animator.SetTrigger("get_damage_front");
        }
        ResetCasting();
    }

    public void ResetCasting() {
        if(isCasting) {
            StopCoroutine(currentSpell);
            currentSpell = null;
            isCasting = false;
            animator.SetBool("isCasting", isCasting);
            animator.SetLayerWeight(animator.GetLayerIndex("Attack_Full_Body"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Attack_Torso"), 0);
            // animator.SetLayerWeight(animator.GetLayerIndex("Get Damage"), 0);
            timeToFire = 0;
        }
    }

    IEnumerator SpearAttack(Spell spell) {
        yield return new WaitForSeconds(spell.castingTime);
        if(firepoint != null) {
            player.SpendMana(spell.manaCost);
            Spell projectile = vfx[0].GetComponent<Spell>();
            Instantiate(projectile, firepoint.transform.position, player.transform.rotation);
            ResetCasting();
        } else {
            Debug.Log("No Fire Point");
        }
    }

    IEnumerator StaffAttack(float castingTime) {
        yield return new WaitForSeconds(castingTime);
        if(staffHitPoint != null) {
            Collider[] hitEnemies = Physics.OverlapSphere(staffHitPoint.transform.position, staffAttackPointRadius, staffAttackLayerMask);
            foreach(Collider enemy in hitEnemies) {
                enemy.GetComponent<Player>().TakeDamage(attackDamageStaff);
                enemy.GetComponent<Player>().PlayerHit(transform.forward, enemy.transform.forward);
            }
            isCasting = true;
            ResetCasting();
        } else {
            Debug.Log("No Fire Point");
        }
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
}
