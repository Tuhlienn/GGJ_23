using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour,  IDragHandler
{
    [SerializeField] private bool draggable = true;
    [SerializeField] private NodeChannel[] inputChannels;
    [SerializeField] private NodeChannel[] outputChannels;


    public void OnDrag(PointerEventData eventData)
    {
        if(!draggable)
            return;
            
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = 0;
        this.transform.position = pos;
    }
}
