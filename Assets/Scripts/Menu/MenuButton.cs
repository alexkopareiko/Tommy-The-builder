using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Com.NikfortGames.MyGame {
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        #region Private Field

        CanvasGroup canvasGroup;
        float alpha = 0.8f;

        #endregion

        private void Awake() {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = alpha;
        }

        public void OnPointerEnter( PointerEventData ped ) {
            canvasGroup.alpha = 1f;
        }
        public void OnPointerExit( PointerEventData ped ) {
            canvasGroup.alpha = alpha;
        }
 
        public void OnPointerDown( PointerEventData ped ) {
            SoundManager.instance.PlayMenuButtonClick();

        }
    }

}

