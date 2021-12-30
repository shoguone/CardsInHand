using System;
using UnityEngine;

namespace CardsInHand.Scripts.Utility
{
    [Serializable]
    public struct ParamsInterval
    {
        [SerializeField]
        private int _from;

        [SerializeField]
        private int _to;

        public ParamsInterval(int from, int to)
        {
            _from = from;
            _to = to;
        }

        public int From
        {
            get => _from;
            set => _from = value;
        }

        public int To
        {
            get => _to;
            set => _to = value;
        }
    }
}