using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Tooltip("offset between camera and target")] 
    public Vector3 offset;
    
    [Tooltip("change this value to get desired smoothness")] 
    public float smoothTime = 0.3f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    [Tooltip("Rotation speed for moving the camera")]
    public float RotationSpeed = 200f;

    float m_CameraVerticalAngle = 0f;
 
    [Tooltip("This value will change at the runtime depending on target movement. Initialize with zero vector.")] 
    private Vector3 velocity = Vector3.zero;

    [Tooltip("Middle point of characher or where camera need look to")]
    private Transform target;


    private void Start()
    {
        target = FindObjectOfType<ThirdPersonMovement>().lookTo;
    }
    
    private void Update() {
        if(Input.GetMouseButton(1)) {
            MouseLookFunc();
        }
    }

    private void LateUpdate()
    {
     
        m_CameraVerticalAngle = transform.eulerAngles.x;
        // update position
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        if(!Input.GetMouseButton(1)) {
            // update rotation
            transform.LookAt(target);
        }

    }

    void MouseLookFunc() {
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(
                new Vector3(0f, (Input.GetAxis("Mouse X") * RotationSpeed),
                    0f), Space.Self);
        }

        // vertical camera rotation
        {

            // add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle += Input.GetAxis("Mouse Y") * RotationSpeed * (-1f);  

            // limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, minimumVert, maximumVert);

            Debug.Log(m_CameraVerticalAngle);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            transform.eulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
        }
    }
}
