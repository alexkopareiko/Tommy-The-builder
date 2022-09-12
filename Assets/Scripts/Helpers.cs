using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame {
    public static class Helpers
    {
        #region Public Methods

        public static float GetXZAngle(Vector3 v1, Vector3 v2) {
            Vector2 _v1 = new Vector2(v1.x, v1.z);
            Vector2 _v2 = new Vector2(v2.x, v2.z);
            float angle = Vector2.Angle(_v1, _v2);
            return angle;
        }

        public static void CloseMenu(this GameObject menu) {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public static void OpenMenu(this GameObject menu) {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        public static void TriggerMenu(this GameObject menu) {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
            canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
            canvasGroup.interactable = !canvasGroup.interactable;
        }

        #endregion
    }

}
