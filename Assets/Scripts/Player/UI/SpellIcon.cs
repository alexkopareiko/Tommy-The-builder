using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Com.NikfortGames.MyGame{
    [CreateAssetMenu(fileName = "New Spell Icon", menuName = "Spell Icons")]
    public class SpellIcon : ScriptableObject
    {
        [Tooltip("Also refer to Attack.cs script for keyCode using per spell")]
        public KeyCode keyCode;
        public Sprite sprite;
    }
}


