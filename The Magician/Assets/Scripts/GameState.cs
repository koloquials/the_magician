using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public enum GameState
    {
        PAUSED = 1 << 0,
        GAMEPLAY = 1 << 1,
        DIALOGUE = 1 << 2,
        CREDITS = 1 << 3,
        GAME_MODE = GAMEPLAY | DIALOGUE
    }
}
