using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Com.NikfortGames.MyGame{
    public class CMFindPlayer : MonoBehaviour
    {
        #region Private Fields

        CinemachineFreeLook cinemachineFreeLook;

        private void Start() {
            cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
            cinemachineFreeLook.m_Follow = FindObjectOfType<ThirdPersonMovement>().cameraLookHere;
            cinemachineFreeLook.m_LookAt = cinemachineFreeLook.m_Follow;
        }

        #endregion

    }
}

