using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_script : Singleton<Game_script>
{
    public TowerButton_script ClickedButton { get; set; }
    public ObjectPool_script  ObjectPool    { get; private set; }

    private int                  _currency;
    private bool                 _gameOver;
    private int                  _lives;
    private Tower_script         _selectedTower;
    private int                  _waveNr;
    private List<Monster_script> _waveMonsters;

    [SerializeField]
    private Text _currencyText;

    [SerializeField]
    private GameObject _gameOverMenu;

    [SerializeField]
    private Text _livesText;

    [SerializeField]
    private GameObject _leftOpaque;

    [SerializeField]
    private GameObject _upgradePanel;

    [SerializeField]
    private Text _sellText;

    [SerializeField]
    private GameObject _waveButton;

    [SerializeField]
    private Text _waveNrText;

    public int Currency
    {
        get { return _currency; }
        set { this._currency = value; this._currencyText.text = string.Format(" <color=lime>$</color> {0}", value); }
    }

    public int Lives
    {
        get { return _lives; }
        set { this._lives = value; if (this._lives <= 0) { this._lives = 0; this.GameOver(); } this._livesText.text = this._lives.ToString(); }
    }

    public bool IsWaveActive
    {
        get { return (this._waveMonsters.Count > 0); }
    }

    private void Awake()
    {
        this.ObjectPool = GetComponent<ObjectPool_script>();
    }

    private void Start()
    {
        this.Currency      = 100;
        this.Lives         = 10;
        this._gameOver     = false;
        this._waveNr       = 1;
        this._waveMonsters = new List<Monster_script>();
    }

    private void Update()
    {
        this.Cancel();
	}

    private void Cancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Hover_script.Instance.SetSprite(null);
        }
    }

    public void DeselectTower()
    {
        this._leftOpaque.SetActive(false);
        this._upgradePanel.SetActive(false);

        if (this._selectedTower != null) {
            this._selectedTower.Select();
        }

        this._selectedTower = null;
    }

    public void GameOver()
    {
        if (!this._gameOver) {
            this._gameOver = true;
            this._gameOverMenu.SetActive(true);
        }
    }

    public void PickTower(TowerButton_script button)
    {
        if ((this.Currency >= button.Price) && !this.IsWaveActive) {
            this.ClickedButton = button;
            Hover_script.Instance.SetSprite(button.Sprite);
        }
    }

    public void PurchaseTower()
    {
        if (this.Currency >= this.ClickedButton.Price) {
            this.Currency -= this.ClickedButton.Price;
        }

        Hover_script.Instance.SetSprite(null);
    }

    public void PrepareWave()
    {
        this._waveNr *= 2;
        this._waveNrText.text = string.Format("Wave: <color=lime>{0}</color>", this._waveNr);
        StartCoroutine(SpawnWave());
        this._waveButton.SetActive(false);
    }

    public void RemoveMonster(Monster_script monster)
    {
        this._waveMonsters.Remove(monster);

        if (!this.IsWaveActive && !this._gameOver) {
            this._waveButton.SetActive(true);
        }
    }

    public void SelectTower(Tower_script tower)
    {
        this._sellText.text = string.Format("SELL: ${0}",(tower.Price / 2));
        this._leftOpaque.SetActive(true);
        this._upgradePanel.SetActive(true);

        if (this._selectedTower != null) {
            this._selectedTower.Select();
        }

        this._selectedTower = tower;
        this._selectedTower.Select();
    }

    public void SellTower()
    {
        if (this._selectedTower != null) {
            this.Currency += (this._selectedTower.Price / 2);
            this._selectedTower.GetComponentInParent<Tile_script>().IsAvailable = true;
            Destroy(this._selectedTower.transform.parent.gameObject);
            this.DeselectTower();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SpawnWave()
    {
        TileMap_script.Instance.GeneratePath();

        for (int i = 0; i < this._waveNr; i++)
        {
            int    monsterIndex = Random.Range(0, 3);
            string monsterType  = "";

            switch (monsterIndex) {
                case 0: monsterType = "monster_orc_001_prefab";    break;
                case 1: monsterType = "monster_spider_001_prefab"; break;
                case 2: monsterType = "monster_viking_001_prefab"; break;
            }

            TileMap_script.Instance.GeneratePath();

            Monster_script monster = (Monster_script)this.ObjectPool.GetObject(monsterType).GetComponent<Monster_script>();
            monster.Spawn();
            this._waveMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);
        }
    }
}
