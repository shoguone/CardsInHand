using System.Collections.Generic;
using System.ComponentModel;
using CardsInHand.Scripts.Entity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardsInHand.Scripts.Game
{
    public class CardHandler : MonoBehaviour
    {
        [SerializeField]
        private Card _card;


        [Header("UI objects")]
        [SerializeField]
        private Text _titleText;

        [SerializeField]
        private Text _descriptionText;

        [SerializeField]
        private Text _hpText;

        [SerializeField]
        private Text _attackText;

        [SerializeField]
        private Text _manaText;

        [SerializeField]
        private Image _portraitImage;
        

        [Header("Animation")]
        [SerializeField]
        private float _counterDuration = 1f;

        [SerializeField]
        private float _counterScale = 3f;

        [SerializeField]
        private Color _counterColorIncrease = Color.green;

        [SerializeField]
        private Color _counterColorReduce = Color.red;


        private Dictionary<CardParameter, (Text text, int lastValue)> _cardParamsDict;
        private Dictionary<CardParameter, (Text text, int lastValue)> CardParamsDict
        {
            get
            {
                return _cardParamsDict ??= new Dictionary<CardParameter, (Text text, int lastValue)>
                {
                    {CardParameter.Hp, (_hpText, Card.Hp)},
                    {CardParameter.Attack, (_attackText, Card.Attack)},
                    {CardParameter.Mana, (_manaText, Card.Mana)},
                };
            }
        }

        public Card Card { get => _card; set => _card = value; }


        public (int width, int height) GetPortraitSize() =>
            GetRectTransformSize(_portraitImage.rectTransform);

        public void Die()
        {
            Destroy(gameObject);
        }


        private void Awake()
        {
            AssertReferences();

            Card.PropertyChanged += Card_PropertyChanged;
        }

        private void OnValidate()
        {
            // if we run Reload immediate, we get a Warning:
            // "SendMessage cannot be called during Awake, CheckConsistency, or OnValidate"
            // if we delay this call, Text.text is not updated in editor until the next change occurred
            // UnityEditor.EditorApplication.delayCall += () => Reload();
            Reload();
        }


        private void Reload()
        {
            if (!AssertReferences())
            {
                return;
            }

            ReloadCardTitle();
            ReloadCardDescription();

            DieOrReloadCardHp();
            ReloadCardParameterWithCounter(CardParameter.Attack);
            ReloadCardParameterWithCounter(CardParameter.Mana);

            ReloadCardPortrait();
        }

        private void Card_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Card.Title):
                    ReloadCardTitle();
                    break;
                case nameof(Card.Description):
                    ReloadCardDescription();
                    break;
                case nameof(Card.Hp):
                    DieOrReloadCardHp();
                    break;
                case nameof(Card.Attack):
                    ReloadCardParameterWithCounter(CardParameter.Attack);
                    break;
                case nameof(Card.Mana):
                    ReloadCardParameterWithCounter(CardParameter.Mana);
                    break;
                case nameof(Card.Portrait):
                    ReloadCardPortrait();
                    break;
                default:
                    Reload();
                    break;
            }
        }

        private void ReloadCardTitle() => _titleText.text = Card.Title;

        private void ReloadCardDescription() => _descriptionText.text = Card.Description;

        private void DieOrReloadCardHp()
        {
            if (Card.Hp < 1)
            {
                Die();
            }
            else
            {
                ReloadCardParameterWithCounter(CardParameter.Hp);
            }
        }

        private void ReloadCardPortrait()
        {
            if (Card.Portrait != null)
            {
                _portraitImage.sprite = Sprite.Create(Card.Portrait, CreateRectFromImage(_portraitImage), new Vector2(.5f, .5f));
            }
        }

        private void ReloadCardParameterWithCounter(CardParameter cardParameter)
        {
            var (text, lastValue) = CardParamsDict[cardParameter];
            var newValue = Card.GetParameter(cardParameter);

            if (newValue == lastValue)
            {
                if (text.text != newValue.ToString())
                {
                    if (!int.TryParse(text.text, out lastValue))
                    {
                        text.text = newValue.ToString();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            if (Application.isPlaying)
            {
                AnimateCardParameterChange();
            }
            else
            {
                text.text = newValue.ToString();
            }

            void AnimateCardParameterChange()
            {
                text.DOCounter(lastValue, newValue, _counterDuration);
                CardParamsDict[cardParameter] = (text, newValue);

                var usualColor = text.color;
                var usualScale = text.rectTransform.localScale;
                var newColor = newValue < lastValue
                    ? _counterColorReduce
                    : _counterColorIncrease;
                DOTween.Sequence()
                    .Append(text.DOColor(newColor, _counterDuration))
                    .Join(text.rectTransform.DOScale(_counterScale, _counterDuration))
                    .Append(text.DOColor(usualColor, _counterDuration))
                    .Join(text.rectTransform.DOScale(usualScale, _counterDuration));
            }

        }


        private bool AssertReferences()
        {
            if (this == null)
            {
                return false;
            }

            if (_titleText == null
                || _descriptionText == null
                || _hpText == null
                || _attackText == null
                || _manaText == null
                || _portraitImage == null)
            {
                Debug.LogWarning("some of references are null");
                enabled = false;
                return false;
            }

            return true;
        }

        private static (int width, int height) GetRectSize(Rect rect) =>
            ((int)rect.width, (int)rect.height);

        private static (int width, int height) GetRectTransformSize(RectTransform rectTransform) =>
            GetRectSize(rectTransform.rect);

        private static Rect CreateRectFromImage(Graphic image)
        {
            var (w, h) = GetRectTransformSize(image.rectTransform);
            return new Rect(0, 0, w, h);
        }
    }
}