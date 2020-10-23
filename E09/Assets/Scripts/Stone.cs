using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [Header("ROUTES")]
    public Route commonRoute; //Outer Route
    public Route finalRoute; //final route

    public List<Node> fullRoute = new List<Node>();

    [Header("NODES")]
    public Node startNode; // this is the node the stone will move to when it leaves it's base.
    public Node baseNode; // this is the node the stone is starting at.
    public Node currentNode; //this is the node we are currently on
    public Node goalNode; //this is the node we want to end on. 

    int routePosition; // the position at which our stone is currently at. 
    int startNodeIndex;

    [Header("Booleans")]
    public bool isOut;
    bool isMoving;

    int steps; // rolled dice amount 
    int doneSteps;

    bool HasTurn; //It is for human input 

    [Header("Selector")]
    public GameObject selector;

    private void Start()
    {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute(); // fill/create the full game route when the game starts
    }

    void CreateFullRoute()
    {
        for (int i = 0; i < commonRoute.childNodeList.Count; i++)
        {
            int tempPos = startNodeIndex + i;
            tempPos %= commonRoute.childNodeList.Count;

            fullRoute.Add(commonRoute.childNodeList[tempPos].GetComponent<Node>());
        }

        for (int i = 0; i < finalRoute.childNodeList.Count; i++)
        {
            fullRoute.Add(finalRoute.childNodeList[i].GetComponent<Node>());
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Dice number is = " + steps);
            if(doneSteps + steps < fullRoute.Count)
            {
                StartCoroutine(Move());
            }
            else
            {
                Debug.Log("Number is too high");
            }
        }
    }
    IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        while(steps > 0)
        {
            routePosition++; // update current route position 

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            while (MoveToNextNode(nextPos, 8f)) { yield return null; }
            yield return new WaitForSeconds(0.1f);
            steps--;
            doneSteps++;
        }

        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goalPos, float speed)
    {
        return goalPos != (transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime));
    }
}
