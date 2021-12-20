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

        private (int from, int to) _paramBoundaries = (2, 9);

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
                switch (param)
                {
                    case CardParameter.Hp:
                        cardHandler.Card.Hp = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                        break;
                    case CardParameter.Attack:
                        cardHandler.Card.Attack = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                        break;
                    case CardParameter.Mana:
                        cardHandler.Card.Mana = RandomExtensions.RangeInclusive(_paramBoundaries.from, _paramBoundaries.to);
                        break;
                    default:
                        Debug.LogWarning($"Unknown {nameof(CardParameter)} value: {param}");
                        break;
                }
            }
        }
    }
}
