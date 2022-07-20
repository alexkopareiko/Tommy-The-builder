using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
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



    private GameObject effectToSpawn;
    private Animator animator;
    private float timeToFire;
    private bool ableToAttack;
    private float lastVelocity;
    private CharacterController controller;
    private GameObject player;
    private ThirdPersonMovement thirdPersonMovement;


    void Start()
    {
        player = gameObject.GetComponentInParent<Player>().gameObject;
        thirdPersonMovement = gameObject.GetComponent<ThirdPersonMovement>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        animator.SetBool("able_to_attack", ableToAttack);
        if(Input.GetKeyDown(KeyCode.Alpha1) && Time.time >= timeToFire && ableToAttack) {
            timeToFire = Time.time + 1 / vfx[0].GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_spear");
            Invoke("SpearAttack", attackSpear.length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / vfx[0].GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_staff");
            Invoke("StaffAttack", attackStaff.length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / vfx[1].GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_fireball");
            Invoke("FireballAttack", attackFireball.length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / vfx[1].GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_shield");
            // Invoke("SpawnVFX", attackShield.length);
        } else {
        }
    }

    private void LateUpdate() {
        if(lastVelocity == 0 && controller.velocity.magnitude == 0)
        {
            ableToAttack = thirdPersonMovement.groundedPlayer ;
        }
        else
        {
            lastVelocity = controller.velocity.magnitude;
            ableToAttack = false;
        }
    }


    void SpearAttack() {
        if(firepoint != null && ableToAttack) {
            if(ableToAttack) {
                Instantiate(vfx[0], firepoint.transform.position, player.transform.rotation);
            }
        } else {
            Debug.Log("No Fire Point");
        }
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }

    void StaffAttack() {
        if(staffHitPoint != null) {
            if(ableToAttack) {
                Collider[] hitEnemies = Physics.OverlapSphere(staffHitPoint.transform.position, staffAttackPointRadius, staffAttackLayerMask);
                foreach(Collider enemy in hitEnemies) {
                    enemy.GetComponent<Player>().TakeDamage(attackDamageStaff);
                }
            }
        } else {
            Debug.Log("No Fire Point");
        }
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }

    void FireballAttack() {
        if(LeftHandPoint != null) {
            if(ableToAttack) {
                Instantiate(vfx[1], LeftHandPoint.transform.position, player.transform.rotation);
            }
        } else {
            Debug.Log("No Fire Point"); 
        }
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }

    void OnDrawGizmosSelected() {
        if(staffHitPoint != null) {
            Gizmos.DrawWireSphere(staffHitPoint.transform.position, staffAttackPointRadius);
        }
    }
}
