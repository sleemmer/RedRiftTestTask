using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Items.Helpers
{
    public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> StartDragEvent;
        public event Action<PointerEventData> EndDragEvent;

        [SerializeField]
        private RectTransform _draggableRoot;

        private Vector3 _startPosition;
        private Vector2 _lastMousePosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.localPosition;
            _lastMousePosition = eventData.position;

            StartDragEvent?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - _lastMousePosition;
            Vector3 newPosition = _draggableRoot.localPosition + new Vector3(diff.x, diff.y, 0);
            _draggableRoot.localPosition = newPosition;
            _lastMousePosition = currentMousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localPosition = _startPosition;

            EndDragEvent?.Invoke(eventData);
        }
    }
}