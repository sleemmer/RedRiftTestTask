using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Items
{
    public class CardCounterItem : MonoBehaviour
    {
        [SerializeReference]
        private TextMeshProUGUI _counterText;

        private Vector3 _maxScale = new Vector3(1.15f, 1.15f, 1.15f);
        private Sequence _sequence;

        public void ResetSequence(bool isCompleted = false)
        {
            if (_sequence == null)
                return;

            _sequence.Kill(isCompleted);
            _sequence = null;
        }

        public void SetValue(int counter, bool isAnimated = false)
        {
            _counterText.text = counter.ToString();

            if (!isAnimated)
            {
                return;
            }

            ResetSequence();
            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(_maxScale, 0.15f))
                .Append(transform.DOScale(Vector3.one, 0.15f))
                .Append(transform.DOScale(_maxScale, 0.15f))
                .Append(transform.DOScale(Vector3.one, 0.15f));
        }
    }
}
