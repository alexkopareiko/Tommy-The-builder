using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject firepoint;
    public GameObject LeftHandPoint;
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
        effectToSpawn = vfx[0];
        player = gameObject.GetComponentInParent<Player>().gameObject;
        thirdPersonMovement = gameObject.GetComponentInParent<ThirdPersonMovement>();
        animator = GetComponentInChildren<Animator>();
        controller = thirdPersonMovement.controller;
    }

    void Update()
    {
        Debug.Log(controller.velocity.magnitude);
        animator.SetBool("able_to_attack", ableToAttack);
        if(Input.GetKeyDown(KeyCode.Alpha1) && Time.time >= timeToFire && ableToAttack) {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_spear");
            Invoke("SpawnVFX", attackSpear.length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_staff");
            Invoke("SpawnVFX", attackStaff.length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_fireball");
            Invoke("SpawnVFX_Fireball", attackFireball.length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
            animator.SetTrigger("attack_shield");
            Invoke("SpawnVFX", attackShield.length);
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


    void SpawnVFX() {
        if(firepoint != null && ableToAttack) {
            Instantiate(effectToSpawn, firepoint.transform.position, player.transform.rotation);
        } else {
            Debug.Log("No Fire Point");
        }
        //animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }
    void SpawnVFX_Fireball() {
        if(firepoint != null && ableToAttack) {
            Instantiate(vfx[1], LeftHandPoint.transform.position, player.transform.rotation);
        } else {
            Debug.Log("No Fire Point"); 
        }
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }
}
