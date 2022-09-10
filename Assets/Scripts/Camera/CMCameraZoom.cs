using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Com.NikfortGames.MyGame 
{
    public class CMCameraZoom : MonoBehaviour
    {
        #region Private Fields

        CinemachineFreeLook cinemachineFreeLook;
        float cameraDistance;
        [SerializeField] float sensitivity = 1f;
        [SerializeField] float minFieldOfView = 1f;
        [SerializeField] float maxFieldOfView = 100f;

        #endregion


        #region MonoBehaviour Callbacks

        private void Start() {
            cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        }

        private void Update() {
            if(GameManager.instance.gameIsPaused) return;
            if(cinemachineFreeLook && Input.GetAxis("Mouse ScrollWheel") != 0) {
                cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity;
                cinemachineFreeLook.m_CommonLens = true;
                cinemachineFreeLook.m_Lens.FieldOfView -= cameraDistance;
                cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Clamp(cinemachineFreeLook.m_Lens.FieldOfView, minFieldOfView, maxFieldOfView);
            }
        }

        #endregion
    }
}


