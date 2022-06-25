using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Tooltip("offset between camera and target")] 
    public Vector3 offset;
    
    [Tooltip("change this value to get desired smoothness")] 
    public float smoothTime = 0.3f;
 
    [Tooltip("This value will change at the runtime depending on target movement. Initialize with zero vector.")] 
    private Vector3 velocity = Vector3.zero;
    public Transform target;


    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // update position
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
 
        // update rotation
        transform.LookAt(target);
    }
}
