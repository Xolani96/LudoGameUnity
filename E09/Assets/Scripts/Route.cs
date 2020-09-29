using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childNodes;
    public List<Transform> childNodeList = new List<Transform>();
    void Start()
    {
        FillNodes();   
    }
    void FillNodes(){
        childNodeList.Clear(); 
        childNodes = GetComponentsInChildren<Transform>();

        foreach(Transform child in childNodes){
            if(child != this.transform){
                childNodeList.Add(child);
            }
        }
    }
}
