using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Com.NikfortGames.MyGame {
    [RequireComponent(typeof(Image))]
    public class RoomChatArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,  IPointerDownHandler, IBeginDragHandler, IEndDragHandler 
    {
        #region Private Fields

        [SerializeField] Image image;
		[SerializeField] private float hoverOpacity = 0.2f;
		[SerializeField] private float initialOpacity = 0.05f;
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private bool isBeingDrag;


        #endregion

        #region MonoBehavior Callbacks

        private void Awake() {
            scrollbar = GetComponent<ScrollRect>().verticalScrollbar;
            Hide();
        }

        private void Start() {
            RoomChat.instance.OnPointerExit();
            // slider.gameObject.CloseMenu();
        }

        #endregion

        #region Event Callbacks

        public void OnPointerEnter(PointerEventData eventData) {
			Higlight();
            RoomChat.instance.OnPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData) {
            if(!isBeingDrag && eventData.fullyExited) {
                Hide();
                RoomChat.instance.OnPointerExit();
            }
			
        }

        public void OnPointerDown(PointerEventData eventData) {
            RoomChat.instance.OnPointerDown();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            isBeingDrag = true;
        }

        public void OnEndDrag(PointerEventData eventData) {
            isBeingDrag = false;
        }

        #endregion

        #region Private Callbacks

        void Higlight() {
            var tmpColor = image.color;
			tmpColor.a = hoverOpacity;
			image.color = tmpColor;
            scrollbar.gameObject.OpenMenu();
        }

        void Hide() {
            var tmpColor = image.color;
			tmpColor.a = initialOpacity;
			image.color = tmpColor;
            scrollbar.gameObject.CloseMenu();
        }

        #endregion
    }

}
