using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class SpawnPlayers : MonoBehaviour
    {
        #region  Public Fields

        public GameObject playerPrefab;
        public static float minX;
        public static float maxX;
        public static float minZ;
        public static float maxZ;
        public static float y;

        public static float delayAfterDeath = 3f;
        
        #endregion



        #region MonoBehaviour Callbacks

        private void Start() {
            PhotonNetwork.Instantiate(playerPrefab.name, GetSpawnPosition(), Quaternion.identity);
        }

        #endregion

        #region Public Methods

        public static Vector3 GetSpawnPosition(){
            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
            return randomPosition;
        }

        public static IEnumerator RespawnMe(Player player) {
            yield return new WaitForSeconds(delayAfterDeath);
            player.currentHealth = player.maxHealth;
            player.currentMana = player.maxMana;
            player.transform.position = GetSpawnPosition();
        }

        #endregion


    }
}

