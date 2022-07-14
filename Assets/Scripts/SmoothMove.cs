using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{

    public float velocity = 10f;
    private void Start() {
        GetComponent<Rigidbody>().velocity = transform.forward * velocity;
    }
}
