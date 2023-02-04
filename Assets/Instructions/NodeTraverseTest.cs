using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTraverseTest : MonoBehaviour
{
    [SerializeField] Node startNode;
    Node currentNode;

    void Awake(){
        currentNode = startNode;
        this.transform.position = currentNode.transform.position + new Vector3(1,1,0);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentNode = currentNode.GetNextNode();
            if(currentNode == null) {
                currentNode = startNode;
            }


            this.transform.position = currentNode.transform.position + new Vector3(1,1,0);
        }
    }
}
