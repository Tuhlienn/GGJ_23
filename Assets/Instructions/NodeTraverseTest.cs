namespace Instructions
{
    using UnityEngine;
    using UnityEngine.U2D;

    public class NodeTraverseTest : MonoBehaviour
    {
        [SerializeField] Node startNode;
        Node currentNode;

        private Spline spline;
        private float t = 0;

        void Awake(){
            currentNode = startNode;
            this.transform.position = currentNode.transform.position + new Vector3(1,1,0);
        }


        void Update()
        {

            if(spline != null)
            {
                Vector3 pos = BezierUtility.BezierPoint(
                    spline.GetRightTangent(0), spline.GetPosition(0),
                    spline.GetPosition(1), spline.GetLeftTangent(1),
                    t
                );
                this.transform.position = pos;
                t += Time.deltaTime;
                Debug.Log($"{t}  {pos}");
                if(t >= 1)
                {
                    spline = null;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                var nextNode = currentNode.GetNextNode();
                if(nextNode == null) {
                    currentNode = startNode;
                }
                else
                {
                    var connection = currentNode.GetNextNodeConnection();
                    spline = connection.Spline;
                    currentNode = nextNode;
                    t = 0;
                }
            }
        }
    }
}