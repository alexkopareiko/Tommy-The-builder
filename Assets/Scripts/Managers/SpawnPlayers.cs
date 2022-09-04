using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    public class SpawnPlayers : MonoBehaviour
    {

        public static SpawnPlayers instance;

        #region  Public Fields

        public GameObject playerPrefab;
        public  float minX;
        public  float maxX;
        public  float minZ;
        public  float maxZ;
        public  float y;

        public float delayAfterDeath = 3f;
        
        #endregion



        #region MonoBehaviour Callbacks

        private void Awake()
        {
            instance = this;
        }

        private void Start() {
            PhotonNetwork.Instantiate(playerPrefab.name, GetSpawnPosition(), Quaternion.identity);
        }

        #endregion

        #region Public Methods

        public Vector3 GetSpawnPosition(){
            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
            return randomPosition;
        }

        public IEnumerator RespawnMe(Player player) {
            yield return new WaitForSeconds(delayAfterDeath);
            player.currentHealth = player.maxHealth;
            player.currentMana = player.maxMana;
            player.transform.position = GetSpawnPosition();
        }

        #endregion


    }
}

