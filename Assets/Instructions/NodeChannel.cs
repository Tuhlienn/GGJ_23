using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class NodeChannel : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler
{
    public enum Type{IN, OUT};

    [SerializeField] private NodeConnectionVisualizer connectionPrefab;
    private Node parentNode;
    
    private NodeChannel currentConnection;
    private NodeConnectionVisualizer currentConnectionVisualizer;
    private NodeConnectionVisualizer tempConnectionVisualizer;

    public Type type = Type.IN;

    public Node ParentNode => parentNode;

    void Awake()
    {
        this.parentNode = GetComponentInParent<Node>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, 1, Vector2.zero, 0);
        NodeChannel other = null;
        foreach(var h in hits)
        {
            NodeChannel channel = h.collider.gameObject.GetComponent<NodeChannel>();
            if(channel != null && channel.type != this.type && channel.ParentNode != this.ParentNode)
            {
                other = channel;
                break;
            }
        }

        if(other != null && other != currentConnection)
        {
            CreateConnection(other, tempConnectionVisualizer);
        }
        else
        {
            Destroy(tempConnectionVisualizer.gameObject);
        }
        tempConnectionVisualizer =  null;
    }

    public void CreateConnection(NodeChannel target, NodeConnectionVisualizer visualizer)
    {
        if(currentConnection == target)
            return;

        RemoveConnection();
        this.currentConnection = target;
        this.currentConnectionVisualizer = visualizer;
        visualizer.p1 = new NodeConnectionVisualizer.Connection(
            this.transform,
            NodeChannel.GetConnectionDirection(this.type)
        );
        visualizer.p2 = new NodeConnectionVisualizer.Connection(
            target.transform,
            NodeChannel.GetConnectionDirection(target.type)
        );
        target.CreateConnection(this, visualizer);
    }

    public void RemoveConnection()
    {
        if(currentConnection == null)
            return;

        var temp = currentConnection;
        currentConnection = null;
        temp.RemoveConnection();

        if(currentConnectionVisualizer) {
            Destroy(currentConnectionVisualizer.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempConnectionVisualizer = Instantiate(connectionPrefab);
        tempConnectionVisualizer.p1 = new NodeConnectionVisualizer.Connection(
            this.transform,
            NodeChannel.GetConnectionDirection(this.type)
        );
        tempConnectionVisualizer.p2 = new NodeConnectionVisualizer.Connection(
            FindObjectOfType<CursorAnchor>().transform,
            NodeChannel.GetConnectionDirection(this.type == Type.IN ? Type.OUT : Type.IN)
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        //TODO on hover von anderen channel visualisieren?
    }

    private static Vector3 GetConnectionDirection(Type channelType)
    {
        return channelType == Type.IN ? Vector3.up : Vector3.down;
    }
}
