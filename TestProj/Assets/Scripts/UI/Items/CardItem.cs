using System;
using System.Collections;
using DG.Tweening;
using GameMechanics;
using TMPro;
using UI.Items.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI.Items
{
    public class CardItem : AnimatedItem
    {
        public event Action<CardItem> StartDragEvent;
        public event Action<PointerEventData> EndDragEvent;

        [Header("Common")]
        [SerializeReference]
        private RectTransform _itemRoot;

        [SerializeReference]
        private ItemDragHandler _dragHandler;

        [Header("Card Data")]
        [SerializeReference]
        private Image _icon;

        [SerializeReference]
        private RectTransform _iconRect;

        [SerializeReference]
        private TextMeshProUGUI _titleText;

        [SerializeReference]
        private TextMeshProUGUI _descriptionText;

        [SerializeReference]
        private CanvasGroup _highlight;

        [Header("Card Counters")]
        [SerializeReference]
        private GameObject _counterGroup;

        [SerializeReference]
        private CardCounterItem _manaCounter;

        [SerializeReference]
        private CardCounterItem _attackCounter;

        [SerializeReference]
        private CardCounterItem _healthCounter;

        private Sequence _highlightSequence;

        private Quaternion _startRotation;
        private Vector3 _startScale;

        public void Init(bool isDragAvailable = true)
        {
            _dragHandler.enabled = isDragAvailable;

            if (isDragAvailable)
            {
                _dragHandler.StartDragEvent += OnStartDrag;
                _dragHandler.EndDragEvent += OnEndDrag;
            }
        }

        public void SetData(CardData data)
        {
            _titleText.text = data.Title;
            _descriptionText.text = data.Description;

            UpdatePlayableData(data);

            EnableHighlight(false);

            LoadIcon(data.IconPath);
        }

        public void UpdatePlayableData(CardData data, bool isAnimated = false, Action callback = null)
        {
            _manaCounter.SetValue(data.Mana, isAnimated);
            _attackCounter.SetValue(data.Attack, isAnimated);
            _healthCounter.SetValue(data.Health, isAnimated);

            if (!isAnimated)
            {
                return;
            }

            PlayHighlightBlink(callback);
        }

        private void PlayHighlightBlink(Action callback = null)
        {
            ResetHighlightSequence();
            _highlightSequence = DOTween.Sequence()
                .OnStart(() =>
                {
                    _highlight.alpha = 0;
                    EnableHighlight(true);
                })
                .Append(_highlight.DOFade(0.3f, 0.15f))
                .Append(_highlight.DOFade(0, 0.15f))
                .Append(_highlight.DOFade(0.3f, 0.15f))
                .Append(_highlight.DOFade(0, 0.15f))
                .OnComplete(() =>
                {
                    EnableHighlight(false);
                    callback?.Invoke();
                });
        }


        private void ResetHighlightSequence()
        {
            if (_highlightSequence != null)
            {
                _highlightSequence.Kill();
                _highlightSequence = null;
            }
        }

        public void PlayDeath(Action callback)
        {
            Move(0, 1f, _itemRoot.localPosition + new Vector3(0, 300, 0), () =>
            {
                Hide(0.5f, callback);
            });
        }

        public RectTransform GetRect()
        {
            return _itemRoot;
        }

        private void LoadIcon(string url)
        {
            StartCoroutine(GetTextureRequest(url, SetIcon));
        }

        IEnumerator GetTextureRequest(string url, Action<Sprite> callback)
        {
            using (var www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.isDone)
                    {
                        var texture = DownloadHandlerTexture.GetContent(www);
                        var rect = new Rect(0, 0, _iconRect.rect.width, _iconRect.rect.height);
                        var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                        callback(sprite);
                    }
                }
            }
        }

        private void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void EnableHighlight(bool isEnable)
        {
            _highlight.gameObject.SetActive(isEnable);
        }

        private void OnStartDrag(PointerEventData eventData)
        {
            _startRotation = transform.localRotation;
            _startScale = transform.localScale;

            
            var targetRot = Quaternion.identity;
            Rotate(0, 0.15f, targetRot, () =>
            {
                Scale(0, 0.15f, new Vector3(1.2f, 1.2f, 1.2f));
            });

            ResetHighlightSequence();
            _highlightSequence = DOTween.Sequence()
                .OnStart(() =>
                {
                    _highlight.alpha = 0;
                    EnableHighlight(true);
                })
                .Append(_highlight.DOFade(0.5f, 0.3f));

            StartDragEvent?.Invoke(this);
        }

        private void OnEndDrag(PointerEventData eventData)
        {
            ResetSequence();
            transform.localRotation = _startRotation;
            transform.localScale = _startScale;

            ResetHighlightSequence();
            _highlightSequence = DOTween.Sequence()
                .Append(_highlight.DOFade(0, 0.3f))
                .OnComplete(() =>
                {
                    EnableHighlight(false);
                });

            EndDragEvent?.Invoke(eventData);
        }

        public void Dispose()
        {
            _dragHandler.StartDragEvent -= OnStartDrag;
            _dragHandler.EndDragEvent -= OnEndDrag;
        }
    }
}