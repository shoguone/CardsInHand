using UnityEngine;

namespace CardsInHand.Scripts.Utility
{
    public static class RandomExtensions
    {
        public static int RangeInclusive(int fromInclusive, int toInclusive) =>
            Random.Range(fromInclusive, toInclusive + 1);
    }
}
