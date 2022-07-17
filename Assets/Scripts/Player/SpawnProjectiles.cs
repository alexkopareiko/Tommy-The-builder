using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject firepoint;
    public List<GameObject> vfx = new List<GameObject> ();

    private GameObject effectToSpawn;
    private float timeToFire;

    private GameObject player;

    void Start()
    {
        effectToSpawn = vfx[0];
        player = gameObject.GetComponentInParent<Player>().gameObject;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && Time.time >= timeToFire) {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            SpawnVFX();
        }
    }

    void SpawnVFX() {
        GameObject vfx;

        if(firepoint != null) {
            vfx = Instantiate(effectToSpawn, firepoint.transform.position, player.transform.rotation);
        } else {
            Debug.Log("No Fire Point");
        }
    }
}
