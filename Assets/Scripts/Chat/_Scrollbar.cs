using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Com.NikfortGames.MyGame {
    [RequireComponent(typeof(Scrollbar))]
    public class _Scrollbar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,  IPointerDownHandler
    {
        #region Public Fields
        [HideInInspector] public bool onPointerEnter = false;

        #endregion

        #region Pointer Callbacks

        public void OnPointerEnter(PointerEventData eventData) {
            // Debug.Log("_Scrollbar OnPointerEnter");
            onPointerEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            // Debug.Log("_Scrollbar OnPointerExit");
            onPointerEnter = false;
        }

        public void OnPointerDown(PointerEventData eventData) {

        }

        #endregion
    }

}
