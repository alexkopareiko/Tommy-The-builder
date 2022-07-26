using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 
namespace Com.NikfortGames.MyGame {
    public class CMFreelookOnlyWhenRightMouseDown : MonoBehaviour {

        #region MonoBehaviour Callbacks

        void Start(){
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }

        #endregion

        #region Custom

        public float GetAxisCustom(string axisName){
            if(axisName == "Mouse X"){
                if (Input.GetMouseButton(1)){
                    return UnityEngine.Input.GetAxis("Mouse X");
                } else{
                    return 0;
                }
            }
            else if (axisName == "Mouse Y"){
                if (Input.GetMouseButton(1)){
                    return UnityEngine.Input.GetAxis("Mouse Y");
                } else{
                    return 0;
                }
            }
            return UnityEngine.Input.GetAxis(axisName);
        }

        #endregion
    }
}
