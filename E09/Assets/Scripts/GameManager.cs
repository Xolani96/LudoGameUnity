using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*the game manager keeps track on:
     * who has the next turn
     * who are the active players
     * etc
     */
    public static GameManager instance; 

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

    public int activePlayer;
    bool swtchingPlayer;

    //human input
    //Game object for our button
    //int rolledHumanDice;

    private void Awake()
    {
        instance = this; 
    }

    public void Update()
    {
        if(playerList[activePlayer].playerType == Player.PlayerType.CPU)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    {
                        StartCoroutine(RolledDiceDelay());
                        state = States.WAITING;
                    }
                    break;

                case States.WAITING:
                    break;

                case States.SWITCH_PLAYER:
                    break;
            }
        }
    }

    void RollDice()
    {
        int rolledDice = Random.Range(1, 7);

        if(rolledDice == 6)
        {
            //check the start node

        }
        if(rolledDice < 6)
        {
            //check for kick
        }
        Debug.Log("Dice number rolled " + rolledDice);
    }

    IEnumerator RolledDiceDelay()
    {
        yield return new WaitForSeconds(2); // the next cpu will have to wait for 2 seconds before it rolls the dice. 
        RollDice();
    }
}
