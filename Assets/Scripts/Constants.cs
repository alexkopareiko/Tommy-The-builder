using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.NikfortGames.MyGame {
    public static class Constants
    {
        #region Public Fields

        public class PLAYER_PREFS {
            public const string SOUND_MUSIC = "SOUND_MUSIC";
            public const string SOUND_EFFECTS = "SOUND_EFFECTS";
            
        }

        public class MSG {
            public const string NICKNAME_LENGTH = "name should be 4-14 characters";
            public const string NICKNAME_SYMBOLS = "name should consist letters/numbers/underscore";
            public const string OK = "OK";
        }

        public class COLORS {
            public static Color PURPLE = new Color(0.7843138f, 0.9960785f, 0.1843137f);
            public static Color GREEN = new Color(0.7411765f, 0.5019608f, 0.9921569f);
            public static Color GRAY = new Color(0.7f, 0.8f, 0.9f);

        }

        #endregion
    }

}
