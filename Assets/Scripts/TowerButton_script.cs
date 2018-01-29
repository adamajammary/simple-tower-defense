using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton_script : MonoBehaviour
{
    [SerializeField]
    private GameObject _towerPrefab;

    public GameObject TowerPrefab
    {
        get { return this._towerPrefab; }
    }

    [SerializeField]
    private Sprite _sprite;

    public Sprite Sprite
    {
        get { return this._sprite; }
    }

    [SerializeField]
    private int _price;

    public int Price
    {
        get { return this._price; }
    }

    [SerializeField]
    private Text _priceText;

    private void Start()
    {
        this._priceText.text = string.Format("$ {0}", this._price);
	}
	
	private void Update()
    {
        if ((Game_script.Instance.Currency < this._price) || Game_script.Instance.IsWaveActive) {
            this._priceText.color = Color.gray;
        } else {
            this._priceText.color = Color.white;
        }
    }
}
