namespace Instructions
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(Collider2D))]
    public class Node : MonoBehaviour,  IDragHandler, IBeginDragHandler
    {
        [SerializeField] private bool draggable = true;
        [SerializeField] private NodeChannel[] inputChannels;
        [SerializeField] private NodeChannel[] outputChannels;
        private Vector2 size;

        [SerializeField] private InstructionType instruction;
        public InstructionType Instruction => instruction;

        Vector3 pointerOffset = Vector3.zero;
        NodeArea nodeArea;

        public void Awake()
        {
            nodeArea = FindObjectOfType<NodeArea>();
            Collider2D collider = GetComponent<Collider2D>();
            size = ((Vector2)collider.bounds.size) + new Vector2(0.5f,0.5f);
        }

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

        public NodeConnectionVisualizer GetNextNodeConnection(int outIndex = 0)
        {
            if(outIndex >= 0 && outIndex < outputChannels.Length)
                return outputChannels[outIndex].ConnectionVisualizer;
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

            if(nodeArea != null)
            {
                this.transform.position = nodeArea.ClampPosition(this.transform.position, this.size);
            }
        }
    }
}