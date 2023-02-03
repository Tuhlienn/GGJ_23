using UnityEngine;
using UnityEngine.U2D;


[RequireComponent(typeof(SpriteShapeController))]
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

    void Awake()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
    }

    void Update()
    {
        if(p1 != null)
        {
            spriteShapeController.spline.SetPosition(0, p1.transform.position);
            spriteShapeController.spline.SetLeftTangent(0, p1.direction * 10);
            spriteShapeController.spline.SetRightTangent(0, p1.direction * 10);
        }    
        if(p2 != null)
        {
            spriteShapeController.spline.SetPosition(1, p2.transform.position);
            spriteShapeController.spline.SetLeftTangent(1, p2.direction * 10);
            spriteShapeController.spline.SetRightTangent(1, p2.direction * 10);
        }    
    }
}
