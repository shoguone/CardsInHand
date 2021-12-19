using System.ComponentModel;
using CardsInHand.Scripts.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace CardsInHand.Scripts.Game
{
    public class CardHandler : MonoBehaviour
    {
        [SerializeField]
        private Card _card;


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


        public Card Card { get => _card; set => _card = value; }


        public (int width, int height) GetPortraitSize() =>
            GetRectTransformSize(_portraitImage.rectTransform);


        private void Awake()
        {
            AssertReferences();

            Card.PropertyChanged += Card_PropertyChanged;
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
                    ReloadCardHp();
                    break;
                case nameof(Card.Attack):
                    ReloadCardAttack();
                    break;
                case nameof(Card.Mana):
                    ReloadCardMana();
                    break;
                case nameof(Card.Portrait):
                    ReloadCardPortrait();
                    break;
                default:
                    Reload();
                    break;
            }
        }

        private void OnValidate()
        {
            Reload();
        }

        private void Reload()
        {
            ReloadCardTitle();
            ReloadCardDescription();
            
            ReloadCardHp();
            ReloadCardAttack();
            ReloadCardMana();

            ReloadCardPortrait();
        }

        private void ReloadCardTitle() => _titleText.text = Card.Title;

        private void ReloadCardDescription() => _descriptionText.text = Card.Description;

        private void ReloadCardHp() => _hpText.text = Card.Hp.ToString();

        private void ReloadCardAttack() => _attackText.text = Card.Attack.ToString();

        private void ReloadCardMana() => _manaText.text = Card.Mana.ToString();

        private void ReloadCardPortrait()
        {
            if (Card.Portrait != null)
            {
                _portraitImage.sprite = Sprite.Create(Card.Portrait, CreateRectFromImage(_portraitImage), new Vector2(.5f, .5f));
            }
        }


        private bool AssertReferences()
        {
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

        private (int width, int height) GetRectTransformSize(RectTransform rectTransform) =>
            ((int)rectTransform.rect.width, (int)rectTransform.rect.height);

        private Rect CreateRectFromImage(Image image)
        {
            var (w, h) = GetRectTransformSize(image.rectTransform);
            return new Rect(0, 0, w, h);
        }
    }
}