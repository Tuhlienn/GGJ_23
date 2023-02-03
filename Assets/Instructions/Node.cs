using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour,  IDragHandler
{
    [SerializeField] private Transform[] inputChannels;
    [SerializeField] private Transform[] outputChannels;


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = 0;
        this.transform.position = pos;
    }
}
