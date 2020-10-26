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
    bool switchingPlayer;

    //human input
    //Game object for our button
    //int rolledHumanDice;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (playerList[activePlayer].playerType == Player.PlayerType.CPU)
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
                    StartCoroutine(SwitchPlayer());
                    state = States.WAITING;
                    break;
            }
        }
    }

    void RollDice()
    {
        int rolledDice = Random.Range(1, 7);
        //int rolledDice = 6;

        if (rolledDice == 6)
        {
            //check the start node
            ChecksStartNode(rolledDice);

        }
        if (rolledDice < 6)
        {
            //check for move
            MoveAStone(rolledDice);
        }
        Debug.Log("Dice number rolled " + rolledDice);
    }

    IEnumerator RolledDiceDelay()
    {
        yield return new WaitForSeconds(2); // the next cpu will have to wait for 2 seconds before it rolls the dice. 
        RollDice();
    }

    void ChecksStartNode(int rollednumber)
    {
        //is anyone on the start node
        bool startNodefull = false;
        for (int i = 0; i < playerList[activePlayer].myStones.Length; i++)
        {
            if(playerList[activePlayer].myStones[i].currentNode == playerList[activePlayer].myStones[i].startNode)
            {
                startNodefull = true;
                break; // We are done here we found a match
            }
        }

        if (startNodefull)
        {
            //Move the stone
            MoveAStone(rollednumber);
            Debug.Log("The start node is full");
        }
        else //Start Node Is empty
        {
            //if at least one stone is still in the base
            for (int i = 0; i < playerList[activePlayer].myStones.Length; i++)
            {
                if (!playerList[activePlayer].myStones[i].ReturnISOut())
                {
                    //move the stone out of the base
                    playerList[activePlayer].myStones[i].LeaveBase();
                    state = States.WAITING;
                    return;
                }
            }

            //Move the stone
            MoveAStone(rollednumber);
        }
    }

    void MoveAStone(int Dicenumber)
    {
        List<Stone> movableStones = new List<Stone>();
        List<Stone> moveKickStones = new List<Stone>();

        //fill the list
        for (int i = 0; i < playerList[activePlayer].myStones.Length; i++)
        {
            if (playerList[activePlayer].myStones[i].ReturnISOut())
            {
                //check for possible kick
                if(playerList[activePlayer].myStones[i].CheckPossibleKick(playerList[activePlayer].myStones[i].stoneId, Dicenumber))
                {
                    moveKickStones.Add(playerList[activePlayer].myStones[i]);
                    continue;
                }
                //check for possible move
                if (playerList[activePlayer].myStones[i].CheckPossibleMove(Dicenumber))
                {
                    movableStones.Add(playerList[activePlayer].myStones[i]);
                    continue;
                }
            }
        }

        //perform Kick if possible
        if(moveKickStones.Count > 0)
        {
            int num = Random.Range(0, moveKickStones.Count);
            moveKickStones[num].StartTheMove(Dicenumber);
            state = States.WAITING;
            return;
        }
        //perfome move if possible
        if (movableStones.Count > 0)
        {
            int num = Random.Range(0, movableStones.Count);
            movableStones[num].StartTheMove(Dicenumber);
            state = States.WAITING;
            return;
        }
        //if none is possible then switch player
        state = States.SWITCH_PLAYER;
        Debug.Log("Should switch players");
    }

    IEnumerator SwitchPlayer()
    {
        if (switchingPlayer)
        {
            yield break;
        }

        switchingPlayer = true;

        yield return new WaitForSeconds(2);
        //set next player
        SetNextActivePlayer();

        switchingPlayer = false;
    }

    void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;

        int available = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].hasWon)
            {
                available++;
            }
        }
        if(playerList[activePlayer].hasWon && available > 1)
        {
            SetNextActivePlayer();
            return;
        }
        else if (available < 2)
        {
            //Game over screen
            state = States.WAITING;
            return;
        }

        state = States.ROLL_DICE;

    }
}
