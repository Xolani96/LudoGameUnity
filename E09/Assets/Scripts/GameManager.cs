using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*the game manager keeps track on:
     * who has the next turn
     * who are the active players
     * etc
     */
    [System.Serializable]
    public class Player
    {
        public string playerName;
        public Stone[] myStones;
        public bool hasTurn;
        public enum PlayerType
        {
            HUMAN,
            CPU,
            NO_PLAYER
        }

        public PlayerType playerType;
        public bool hasWon;
    }

    public List<Player> playerList = new List<Player>();

    //StateMachine

    public enum States
    {
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }

    public States state;

    public void Update()
    {
        switch (state)
        {
            case States.ROLL_DICE:
                break;

            case States.WAITING:
                break;
            
            case States.SWITCH_PLAYER:
                break;
        }
    }
}
