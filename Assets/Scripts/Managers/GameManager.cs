using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Com.NikfortGames.MyGame {
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        public static GameManager instance;

        [Header("Circle Selection")]
        [Tooltip("lower down the selection for correct texture drawing")]
        [SerializeField] 
        public  Vector3 circleSelectVector3 = new Vector3(0f, -1.2f, 0f);
        public  CharacterSelection circleSelectPrefab;

        [Header("Others")]
        [Tooltip("Menu when pressed ESC")]
        public GameObject canvasMenu;
        public bool gameIsPaused = false;
        public Transform mainCanvas;

        #endregion
        
        #region Private Fields
        private Player player;

        private  CharacterSelection circleSelect;

        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            instance = this;
            canvasMenu.SetActive(false);
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                TriggerMenu();
            }
        }

        #endregion

        #region Pun Callbacks

        public override void OnDisconnected(DisconnectCause cause)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Launcher");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        #endregion
        
        #region Public Methods

        public void EndGame() {
            PhotonNetwork.LeaveRoom();
        }



        public void TriggerMenu(){
            
            gameIsPaused = !gameIsPaused;
            if(gameIsPaused)
            {
                canvasMenu.SetActive(true);
            }
            else 
            {
                canvasMenu.SetActive(false);
            }
        }

        public void SetCircleSelection(Player focus) {
            if(focus) {
                if(circleSelect == null ) {
                    if(circleSelectPrefab == null) {
                        Debug.LogWarning("<Color=Red><b>Missing</b></Color> circleSelectPrefab reference on GameManager.");
                        return;
                    }
                    circleSelect = Instantiate(circleSelectPrefab);
                }
                if(!circleSelect.gameObject.activeSelf) circleSelect.gameObject.SetActive(true);
                Vector3 pos = focus.transform.position + circleSelectVector3;
                circleSelect.transform.position = pos;
            } else if(circleSelect != null){
                circleSelect.gameObject.SetActive(false);
            }
        }

        #endregion

        
        
    }
}

