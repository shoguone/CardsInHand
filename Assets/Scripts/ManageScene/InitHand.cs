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
        private const string _loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";


        [SerializeField]
        private GameObject _cardPrefab;

        [SerializeField]
        private Transform _handContainer;


        private readonly string[] _words = Regex.Replace(_loremIpsum, @"[^\w\s]", string.Empty).Split(' ');
        private (int from, int to) _cardsInHand = (4, 6);
        private (int from, int to) _titleBoundaries = (1, 2);
        private (int from, int to) _descrBoundaries = (3, 7);
        private (int from, int to) _paramBoundaries = (2, 9);

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

            var cardsCount = RandomExtensions.RangeInclusive(_cardsInHand.from, _cardsInHand.to);
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
                cardHandler.Card.Title = PickWords(_titleBoundaries.from, _titleBoundaries.to);
                cardHandler.Card.Description = PickWords(_descrBoundaries.from, _descrBoundaries.to);
                cardHandler.Card.Hp = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                cardHandler.Card.Attack = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                cardHandler.Card.Mana = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                SetUpCardPortrait(cardHandler);
            }

            string PickWords(int from, int to)
            {
                var take = RandomExtensions.RangeInclusive(from, to);
                var skip = RandomExtensions.RangeInclusive(0, _words.Length - take - 1);
                return string.Join(" ", _words.ToList().Skip(skip).Take(take));
            }

            void ClearHand()
            {
                var children = new List<GameObject>();
                for (var i = 0; i < _handContainer.childCount; i++)
                {
                    children.Add(_handContainer.GetChild(i).gameObject);
                }

                _handContainer.DetachChildren();
                children.ForEach(c => Destroy(c));
                children.Clear();
            }

            void SetUpCardPortrait(CardHandler cardHandler)
            {
                var imageSize = cardHandler.GetPortraitSize();
                var url = $"https://picsum.photos/{imageSize.width}/{imageSize.height}";
                WebRequestProvider.GetTexture(
                    url,
                    (err) => Debug.LogWarning(err),
                    (tx2) =>
                    {
                        cardHandler.Card.Portrait = tx2;
                    });
            }

        }

    }
}