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
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        FillNodes();

        for(int i = 0; i < childNodeList.Count; i++){
            Vector3 currrentPos = childNodeList[i].position;
            if(i>0){
                Vector3 prev = childNodeList[i-1].position;
                Gizmos.DrawLine(prev, currrentPos);
            }
        }
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

    public int RequestPosition(Transform nodeTransform)
    {
        return childNodeList.IndexOf(nodeTransform);
    }
}
