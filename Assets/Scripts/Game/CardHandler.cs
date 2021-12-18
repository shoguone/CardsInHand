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


        //public Text TitleText { get => _titleText; set => _titleText = value; }

        //public Text DescriptionText { get => _descriptionText; set => _descriptionText = value; }

        //public Text HpText { get => _hpText; set => _hpText = value; }

        //public Text AttackText { get => _attackText; set => _attackText = value; }

        //public Text ManaText { get => _manaText; set => _manaText = value; }


        public Card Card { get => _card; set => _card = value; }


        private void Awake()
        {
            AssertReferences();
        }

        private void Update()
        {
            _titleText.text = _card.Title;
            _descriptionText.text = _card.Description;
            _hpText.text = _card.Hp.ToString();
            _attackText.text = _card.Attack.ToString();
            _manaText.text = _card.Mana.ToString();

            //TitleText.text = card.Title;
            //DescriptionText.text = card.Description;
            //HpText.text = card.Hp.ToString();
            //AttackText.text = card.Attack.ToString();
            //ManaText.text = card.Mana.ToString();
        }

        private bool AssertReferences()
        {
            if (_titleText == null
                || _descriptionText == null
                || _hpText == null
                || _attackText == null
                || _manaText == null)
            {
                Debug.LogWarning("some of references is null");
                enabled = false;
                return false;
            }

            return true;
        }
    }
}