using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMagician
{
    public enum GameState
    {
        PAUSED = 1 << 0,
        GAMEPLAY = 1 << 1,
        //DIALOGUE = 1 << 2, 5/23 - refactoring out
        CREDITS = 1 << 2,
        //GAME_MODE = GAMEPLAY | DIALOGUE
    }
}
