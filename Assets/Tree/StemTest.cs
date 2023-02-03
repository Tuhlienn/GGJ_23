using UnityEngine;

namespace Tree
{
    public class StemTest : MonoBehaviour
    {
        [SerializeField] private float tickTime = 1;

        private float _timer;
        private Stem _stem;

        private void Start()
        {
            _stem = new Stem();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < tickTime)
                return;

            _stem.Tick();
            _timer -= tickTime;
        }
    }
}
