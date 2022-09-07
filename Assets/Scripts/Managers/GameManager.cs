using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.NikfortGames.MyGame {
    public class GameManager : MonoBehaviour
    {

        #region Public Fields

        public static GameManager instance;

        [Header("Circle Selection")]
        [Tooltip("lower down the selection for correct texture drawing")]
        [SerializeField] 
        public  Vector3 circleSelectVector3 = new Vector3(0f, -1.2f, 0f);
        public  CharacterSelection circleSelectPrefab;

        [Header("UI")]
        public ScoreboardOverview scoreboardOverview;

        #endregion
        
        #region Private Fields
        private Player player;

        private  CharacterSelection circleSelect;

        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void Update() {
            ToggleScoreboard();
        }

        #endregion

        #region Public Methods

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

        #region Private Methods

        private void ToggleScoreboard(){
            if(Input.GetKeyDown(KeyCode.Tab)){
                scoreboardOverview.GetComponent<CanvasGroup>().alpha = 1;
            } else if(Input.GetKeyUp(KeyCode.Tab)){
                scoreboardOverview.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        #endregion
        
    }
}

