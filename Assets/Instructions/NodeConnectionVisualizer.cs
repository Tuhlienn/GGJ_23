namespace Instructions
{
    using UnityEngine;
    using UnityEngine.U2D;


    [RequireComponent(typeof(SpriteShapeController), typeof(SpriteShapeRenderer))]
    public class NodeConnectionVisualizer : MonoBehaviour
    {
        [System.Serializable]
        public class Connection{
            public Vector3 direction;
            public Transform transform;

            public Connection(Transform transform, Vector3 direction)
            {
                this.transform = transform;
                this.direction = direction;
            }
        }

        public Connection p1;
        public Connection p2;
        SpriteShapeController spriteShapeController;
        SpriteShapeRenderer spriteRenderer;

        [SerializeField] Color lockedColor;

        void Awake()
        {
            spriteShapeController = GetComponent<SpriteShapeController>();
            spriteRenderer = GetComponent<SpriteShapeRenderer>();
            spriteRenderer.enabled = false;
        }

        public Spline Spline => spriteShapeController.spline;

        public void MarkAsLocked()
        {
            spriteRenderer.color = lockedColor;
        }

        void Update()
        {
            float distance = Vector3.Distance(p1.transform.position, p2.transform.position);

            spriteRenderer.enabled = p1 != null && p2 != null;

            if (p1 == null || p2 == null)
                return;

            float distanceY = Mathf.Abs(p2.transform.position.y - p1.transform.position.y);

            float tangentFactor = Mathf.Clamp(distanceY * 0.5f, 1.5f, 5f);

            spriteShapeController.spline.SetPosition(0, p1.transform.position);
            spriteShapeController.spline.SetLeftTangent(0, p1.direction * -tangentFactor);
            spriteShapeController.spline.SetRightTangent(0, p1.direction * tangentFactor);

            spriteShapeController.spline.SetPosition(1, p2.transform.position);
            spriteShapeController.spline.SetLeftTangent(1, p2.direction * tangentFactor);
            spriteShapeController.spline.SetRightTangent(1, p2.direction * -tangentFactor);
        }
    }
}