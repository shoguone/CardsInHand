using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
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


    public string Title { get => _title; set => _title = value; }

    public string Description { get => _description; set => _description = value; }

    public int Hp { get => _hp; set => _hp = value > 0 ? value : 0; }

    public int Attack { get => _attack; set => _attack = value > 0 ? value : 0; }

    public int Mana { get => _mana; set => _mana = value > 0 ? value : 0; }


    public Text TitleText { get => _titleText; set => _titleText = value; }

    public Text DescriptionText { get => _descriptionText; set => _descriptionText = value; }

    public Text HpText { get => _hpText; set => _hpText = value; }

    public Text AttackText { get => _attackText; set => _attackText = value; }

    public Text ManaText { get => _manaText; set => _manaText = value; }


    private void Awake()
    {
        AssertReferences();
    }

    private void Update()
    {
        TitleText.text = Title;
        DescriptionText.text = Description;
        HpText.text = Hp.ToString();
        AttackText.text = Attack.ToString();
        ManaText.text = Mana.ToString();
    }

    private bool AssertReferences()
    {
        if (_titleText == null
            || _descriptionText == null
            || _hpText == null
            || _attackText == null
            || _manaText == null)
        {
            enabled = false;
            return false;
        }

        return true;
    }
}
