using System;
using UnityEngine;

namespace CardsInHand.Scripts.Entity
{
    [Serializable]
    public class Card
    {
        [SerializeField]
        private string _title = "Egg";

        [SerializeField]
        private string _description = "Rolling on the floor";

        [SerializeField]
        private int _hp = 6;

        [SerializeField]
        private int _attack = 6;

        [SerializeField]
        private int _mana = 6;

        //private string _back;

        //private string _portrait;


        public string Title { get => _title; set => _title = value; }

        public string Description { get => _description; set => _description = value; }

        public int Hp { get => _hp; set => _hp = value > 0 ? value : 0; }

        public int Attack { get => _attack; set => _attack = value > 0 ? value : 0; }

        public int Mana { get => _mana; set => _mana = value > 0 ? value : 0; }

    }
}
