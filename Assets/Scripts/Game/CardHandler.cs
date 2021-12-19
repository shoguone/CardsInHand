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

        private string _portraitSpriteName;


        public Card Card { get => _card; set => _card = value; }


        public (int width, int height) GetPortraitSize() =>
            GetRectTransformSize(_portraitImage.rectTransform);


        private void Awake()
        {
            AssertReferences();

            _portraitSpriteName = _portraitImage.sprite.name;
        }

        private void Update()
        {
            _titleText.text = _card.Title;
            _descriptionText.text = _card.Description;
            _hpText.text = _card.Hp.ToString();
            _attackText.text = _card.Attack.ToString();
            _manaText.text = _card.Mana.ToString();

            if (_card.Portrait != null && _portraitImage.sprite.name == _portraitSpriteName)
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
                Debug.LogWarning("some of references is null");
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