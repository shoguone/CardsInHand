using CardsInHand.Scripts.Entity;
using CardsInHand.Scripts.Game;
using CardsInHand.Scripts.Utility;
using UnityEngine;

namespace CardsInHand.Scripts.ManageScene
{
    public class CardParamsSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Transform _handContainer;

        private (int from, int to) _paramBoundaries = (-2, 9);

        private int _lastChildIndex = -1;

        public void Switch()
        {
            if (_handContainer == null)
            {
                Debug.LogWarning("Some of references are null");
                enabled = false;
                return;
            }

            if (_handContainer.childCount < 1)
            {
                Debug.LogWarning("No cards in hand!");
                return;
            }

            _lastChildIndex = (_lastChildIndex + 1) % _handContainer.childCount;
            var child = _handContainer.GetChild(_lastChildIndex);
            var cardHandler = child.GetComponent<CardHandler>();
            if (cardHandler == null)
            {
                Debug.LogWarning($"{nameof(CardHandler)} was not found on game object `{gameObject.name}`!");
            }
            else
            {
                var param = (CardParameter)RandomExtensions.RangeInclusive((int)CardParameter.Hp, (int)CardParameter.Mana);
                var value = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                switch (param)
                {
                    case CardParameter.Hp:
                        cardHandler.Card.Hp = GenerateRandomParam(cardHandler.Card.Hp);
                        break;
                    case CardParameter.Attack:
                        cardHandler.Card.Attack = GenerateRandomParam(cardHandler.Card.Attack);
                        break;
                    case CardParameter.Mana:
                        cardHandler.Card.Mana = GenerateRandomParam(cardHandler.Card.Mana);
                        break;
                    default:
                        Debug.LogWarning($"Unknown {nameof(CardParameter)} value: {param}");
                        break;
                }
            }

            int GenerateRandomParam(int current)
            {
                while (true)
                {
                    var value = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                    if (current != value)
                    {
                        return value;
                    }
                }
            }
        }
    }
}
