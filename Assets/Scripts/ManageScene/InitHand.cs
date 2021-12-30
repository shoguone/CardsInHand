using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CardsInHand.Scripts.Game;
using CardsInHand.Scripts.Utility;
using CardsInHand.Scripts.Web;
using UnityEngine;

namespace CardsInHand.Scripts.ManageScene
{
    public class InitHand : MonoBehaviour
    {
        [SerializeField]
        private GameObject _cardPrefab;

        [SerializeField]
        private Transform _handContainer;

        [Header("Randomization")]
        [SerializeField]
        [TextArea(2, 5)]
        private string _loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

        [SerializeField]
        private ParamsInterval _cardsInHand = new ParamsInterval(4, 6);
        
        [SerializeField]
        private ParamsInterval _titleBoundaries = new ParamsInterval(1, 2);
        
        [SerializeField]
        private ParamsInterval _descrBoundaries = new ParamsInterval(3, 7);
        
        [SerializeField]
        private ParamsInterval _paramBoundaries = new ParamsInterval(2, 9);
        

        private string[] _words;

        private string[] Words => _words ??= Regex.Replace(_loremIpsum, @"[^\w\s]", string.Empty).Split(' ');

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_cardPrefab == null
                || _handContainer == null)
            {
                Debug.LogWarning("Some of references are null");
                enabled = false;
                return;
            }

            if (_handContainer.childCount > 0)
            {
                ClearHand();
            }

            var cardsCount = RandomExtensions.RangeInclusive(_cardsInHand.From, _cardsInHand.To);
            for (var i = 0; i < cardsCount; i++)
            {
                var cardHandler = CreateCardInHand();
                if (cardHandler == null)
                {
                    Debug.LogWarning($"{nameof(CardHandler)} was not found on {nameof(_cardPrefab)}!");
                }
                else
                {
                    SetUpCard(cardHandler);
                }
            }

            CardHandler CreateCardInHand()
            {
                var instance = Instantiate(_cardPrefab, _handContainer);
                return instance.GetComponent<CardHandler>();
            }

            void SetUpCard(CardHandler cardHandler)
            {
                cardHandler.Card.Title = PickWords(_titleBoundaries.From, _titleBoundaries.To);
                cardHandler.Card.Description = PickWords(_descrBoundaries.From, _descrBoundaries.To);
                cardHandler.Card.Hp = RandomExtensions.RangeInclusive(_paramBoundaries.From, _paramBoundaries.To);
                cardHandler.Card.Attack = RandomExtensions.RangeInclusive(_paramBoundaries.From, _paramBoundaries.To);
                cardHandler.Card.Mana = RandomExtensions.RangeInclusive(_paramBoundaries.From, _paramBoundaries.To);
                SetUpCardPortrait(cardHandler);
            }

            string PickWords(int from, int to)
            {
                var take = RandomExtensions.RangeInclusive(from, to);
                var skip = RandomExtensions.RangeInclusive(0, Words.Length - take - 1);
                return string.Join(" ", Words.ToList().Skip(skip).Take(take));
            }

            void ClearHand()
            {
                var children = new List<GameObject>();
                for (var i = 0; i < _handContainer.childCount; i++)
                {
                    children.Add(_handContainer.GetChild(i).gameObject);
                }

                _handContainer.DetachChildren();
                children.ForEach(Destroy);
                children.Clear();
            }

            void SetUpCardPortrait(CardHandler cardHandler)
            {
                var (width, height) = cardHandler.GetPortraitSize();
                var url = $"https://picsum.photos/{width}/{height}";
                WebRequestProvider.GetTexture(
                    url,
                    Debug.LogWarning,
                    (tx2) => cardHandler.Card.Portrait = tx2);
            }

        }

    }
}