using UnityEngine;

public class FlowerAnimator : MonoBehaviour
{
    private const string SHOWBUD = "ShowBud";
    private const string SHOWFLOWER = "ShowFlower";

    [SerializeField] Animator animator;

    public void ShowBud() => animator.SetBool(SHOWBUD, true);
    public void ShowFlower() => animator.SetBool(SHOWFLOWER, true);
}
