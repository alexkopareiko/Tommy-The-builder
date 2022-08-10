using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.NikfortGames.MyGame {
    public class Helpers : MonoBehaviour
    {
        #region Public Methods

        public static float GetXZAngle(Vector3 v1, Vector3 v2) {
            Vector2 _v1 = new Vector2(v1.x, v1.z);
            Vector2 _v2 = new Vector2(v2.x, v2.z);
            float angle = Vector2.Angle(_v1, _v2);
            return angle;
        }

        #endregion
    }

}
