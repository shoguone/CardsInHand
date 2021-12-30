using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CardsInHand.Scripts.Entity
{
    [Serializable]
    public class Card : INotifyPropertyChanged
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

        [SerializeField]
        private Texture2D _portrait;


        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value == _description)
                {
                    return;
                }

                _description = value;
                OnPropertyChanged();
            }
        }

        public int Hp
        {
            get => _hp;
            set
            {
                if (value == _hp)
                {
                    return;
                }

                _hp = value > 0 ? value : 0;
                OnPropertyChanged();
            }
        }

        public int Attack
        {
            get => _attack;
            set
            {
                if (value == _attack)
                {
                    return;
                }

                _attack = value;
                OnPropertyChanged();
            }
        }

        public int Mana
        {
            get => _mana;
            set
            {
                if (value == _mana)
                {
                    return;
                }

                _mana = value;
                OnPropertyChanged();
            }
        }

        public Texture2D Portrait
        {
            get => _portrait;
            set
            {
                if (value == _portrait)
                {
                    return;
                }

                _portrait = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public int GetParameter(CardParameter cardParameter)
        {
            switch (cardParameter)
            {
                case CardParameter.Hp: return Hp;
                case CardParameter.Attack: return Attack;
                case CardParameter.Mana: return Mana;
                default: return -1;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum CardParameter
    {
        Hp = 0,
        Attack,
        Mana,
    }
}
