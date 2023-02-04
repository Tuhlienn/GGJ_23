namespace Instructions
{
    using UnityEngine;

    public class NodeArea : MonoBehaviour
    {
        [SerializeField] private float width;
        [SerializeField] private float height;

        public Rect Rect {get; private set;}

        void Awake(){
            Rect = new Rect(
                new Vector2(
                    this.transform.position.x - width/2.0f,
                    this.transform.position.y - height/2.0f
                ),
                new Vector2(width, height)
            );
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(this.transform.position, new Vector3(width, height, 0));
        }

        public Vector2 ClampPosition(Vector2 position, Vector2 size )
        {
            Vector2 clampedPos = new Vector2(position.x, position.y);
            clampedPos.x = Mathf.Clamp(clampedPos.x, Rect.xMin + size.x / 2.0f, Rect.xMax - size.x / 2.0f);
            clampedPos.y = Mathf.Clamp(clampedPos.y, Rect.yMin + size.y / 2.0f, Rect.yMax - size.y / 2.0f);
            return clampedPos;
        }
    }
}