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
            bool isEnabled = p1?.transform && p2?.transform;
            spriteRenderer.enabled = isEnabled;

            if (!isEnabled)
                return;

            float yd = Mathf.Abs(p2.transform.position.y - p1.transform.position.y);
            float tangentFactor = Mathf.Clamp(yd, 1, 10);

            spriteShapeController.spline.SetPosition(0, p1.transform.position);
            spriteShapeController.spline.SetLeftTangent(0, p1.direction  * tangentFactor);
            spriteShapeController.spline.SetRightTangent(0, p1.direction  * tangentFactor);

            spriteShapeController.spline.SetPosition(1, p2.transform.position);
            spriteShapeController.spline.SetLeftTangent(1, p2.direction * tangentFactor);
            spriteShapeController.spline.SetRightTangent(1, p2.direction * tangentFactor);
        }
    }
}
