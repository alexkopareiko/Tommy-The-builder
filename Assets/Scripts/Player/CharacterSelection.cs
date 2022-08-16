using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.NikfortGames.MyGame {
    public class CharacterSelection : MonoBehaviour
    {
        #region Public Methods

        public float rotationSpeed;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update() {
            transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        #endregion

    }
}

