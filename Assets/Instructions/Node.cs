using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour,  IDragHandler, IBeginDragHandler
{
    [SerializeField] private bool draggable = true;
    [SerializeField] private NodeChannel[] inputChannels;
    [SerializeField] private NodeChannel[] outputChannels;

    Vector3 pointerOffset = Vector3.zero;

    public Node GetPrevNode(int inIndex = 0)
    {
        if(inIndex >= 0 && inIndex < inputChannels.Length)
            return inputChannels[inIndex].ConnectedNode;
        return null;
    }

    public Node GetNextNode(int outIndex = 0)
    {
        if(outIndex >= 0 && outIndex < outputChannels.Length)
            return outputChannels[outIndex].ConnectedNode;
        return null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = 0;
        pointerOffset = this.transform.position - pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!draggable)
            return;
            
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = 0;
        this.transform.position = pos + pointerOffset;
    }
}
