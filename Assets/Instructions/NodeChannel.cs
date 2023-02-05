namespace Instructions
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NodeChannel : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler
    {
        public enum Type{IN, OUT};

        [SerializeField] private NodeConnectionVisualizer connectionPrefab;
        private Node parentNode;
        
        private NodeChannel currentConnection;
        public NodeConnectionVisualizer ConnectionVisualizer {get; private set;}
        private NodeConnectionVisualizer tempConnectionVisualizer;

        public Type type = Type.IN;

        public Node ParentNode => parentNode;
        public Node ConnectedNode => currentConnection?.ParentNode;

        [Header("PREFIXED")]
        public NodeChannel forcedConnection;

        void Awake()
        {
            this.parentNode = GetComponentInParent<Node>();
            if(forcedConnection != null)
            {
                CreateForceConnection();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(tempConnectionVisualizer == null)
                return;

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

            if(other != null && other != currentConnection && other.forcedConnection == null)
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
            this.ConnectionVisualizer = visualizer;
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

            if(ConnectionVisualizer) {
                Destroy(ConnectionVisualizer.gameObject);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(parentNode.Locked || forcedConnection != null)
                return;

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
    
    
        private void CreateForceConnection()
        {
            if(forcedConnection != null)
            {
                connectionPrefab = Instantiate(connectionPrefab);
                this.CreateConnection(forcedConnection, connectionPrefab);
                forcedConnection.forcedConnection = this;
                connectionPrefab.MarkAsLocked();
                

                var selfAnimation =this.GetComponent<HoverTween>();
                if(selfAnimation != null)
                    Destroy(selfAnimation);

                var otherAnimation =forcedConnection.GetComponent<HoverTween>();
                if(otherAnimation != null)
                    Destroy(otherAnimation);
            }
        }

        public void OnDrawGizmos()
        {
            if(forcedConnection != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(this.transform.position, forcedConnection.transform.position);
            }
        }
    }
}