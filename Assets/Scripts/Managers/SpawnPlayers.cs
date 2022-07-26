using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class SpawnPlayers : MonoBehaviour
    {
        #region  Public Fields

        public GameObject playerPrefab;
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;
        public float y;
        
        #endregion



        #region MonoBehaviour Callbacks

        private void Start() {
            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        }

        #endregion


    }
}

