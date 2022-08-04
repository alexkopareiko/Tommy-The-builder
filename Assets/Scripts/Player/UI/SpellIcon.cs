using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Com.NikfortGames.MyGame{
    [CreateAssetMenu(fileName = "New Spell Icon", menuName = "Spell Icons")]
    public class SpellIcon : ScriptableObject
    {
        public KeyCode keyCode;
        public Sprite sprite;
    }
}


