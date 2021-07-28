using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Items
{
	public class AnimatedItem : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		public CanvasGroup CanvasGroup => _canvasGroup;
		private Sequence _sequence;

		public void Show(float delay, float time)
		{
			ResetSequence();
			_sequence = DOTween.Sequence()
				.SetDelay(delay)
				.Append(_canvasGroup.DOFade(1f, time));
		}

		public void Hide(float time, Action callback = null)
		{
			ResetSequence();
			_sequence = DOTween.Sequence()
				.Append(_canvasGroup.DOFade(0f, time))
				.OnComplete(() => { callback?.Invoke(); });
		}

		public void Move(float delay, float time, Vector3 targetPos, Action callback = null)
		{
			ResetSequence();
			_sequence = DOTween.Sequence()
				.SetDelay(delay)
				.Append(_canvasGroup.transform.DOLocalMove(targetPos, time))
				.OnComplete(() => { callback?.Invoke(); });
		}

		public void Scale(float delay, float time, Vector3 targetScale, Action callback = null)
		{
			ResetSequence();
			_sequence = DOTween.Sequence()
				.SetDelay(delay)
				.Append(_canvasGroup.transform.DOScale(targetScale, time))
				.OnComplete(() => { callback?.Invoke(); });
		}

		public void Rotate(float delay, float time, Quaternion targetRotation, Action callback = null)
		{
			ResetSequence();
			_sequence = DOTween.Sequence()
				.SetDelay(delay)
				.Append(_canvasGroup.transform.DORotateQuaternion(targetRotation, time))
				.OnComplete(() => { callback?.Invoke(); });
		}

		public virtual void ResetSequence(bool isComplete = false)
		{
			if (_sequence == null)
				return;

			_sequence.Kill(isComplete);
			_sequence = null;
		}
	}
}