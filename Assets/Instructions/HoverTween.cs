namespace Instructions
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using DG.Tweening;

    public class HoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SpriteRenderer sprite;
        private Sequence currentSequence;

        [SerializeField] private Color targetColor;
        [SerializeField] private float targetScale;

        Color normalColor;
        private float normalScale;

        void Awake()
        {
            this.sprite = GetComponent<SpriteRenderer>();
            this.normalScale = this.transform.localScale.x;
            this.normalColor = sprite.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(currentSequence != null)
                currentSequence.Kill();
            
            currentSequence = DOTween.Sequence();
            currentSequence.Join(sprite.DOColor(targetColor, 0.15f));
            currentSequence.Join(this.transform.DOScale(targetScale, 0.15f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(currentSequence != null)
                currentSequence.Kill();
            
            currentSequence = DOTween.Sequence();
            currentSequence.Join(sprite.DOColor(normalColor, 0.15f));
            currentSequence.Join(this.transform.DOScale(normalScale, 0.15f));
        }
    }
}