using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.NikfortGames.MyGame {
    public class PlayerUIBottomPanel : MonoBehaviour
    {

        #region Public Fields

        public List<SpellSlot> spellSlots = new List<SpellSlot>();

        #endregion


        #region Private Fields

        [SerializeField] SpellSlot spellSlotPreafb;
        [SerializeField] List<SpellIcon> spellIcons;
        [SerializeField] GameObject itemsParent;

        #endregion


        #region MonoBehaviour Callbacks

        private void Awake() {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            int i = 0;
            foreach (SpellIcon spellIcon in spellIcons)
            {
                SpellSlot _sI = Instantiate(spellSlotPreafb);
                if(_sI == null) return;
                _sI.transform.SetParent(itemsParent.GetComponent<Transform>(), false);
                _sI.SetSpellIcon(spellIcon);
                _sI.SetNumberText(i+1);
                spellSlots.Add(_sI);
                i++;
            }
        }

        #endregion
    }

}
