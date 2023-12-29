using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*代表currency changed*/
public delegate void CurrencyChanged();

public class GameManager : singleTon<GameManager> {

    /*an event that is trigger when the currency changed*/
    public event CurrencyChanged Changed;

    public TowerButton ClickedBtn; /*the tower button that I click*/

    [SerializeField]
    private long currency;

    public Text currencyText;
    public Text livesText;

    [SerializeField]
    private GameObject gameOverMenu;

    public GameObject upgradePanel;

    public Text sellText;

    private Tower selectedTower; //current tower

    [SerializeField]
    private GameObject waveBtn;/*next wave button*/

    private List<Monster> activeMonsters = new List<Monster>(); /*count for number of monster in the map*/

    private int wave; /*current number of the wave*/

    public int lives;

    private bool gameOver = false;

    private int health = 15;

    [SerializeField]
    private Text waveText;

    public Vector3 hoverScale;

    public ObjectPool Pool; /*the monster Pool*/

    public GameObject statsPanel;

    [SerializeField]
    private Text statText;

    public Text upgradePrice;

    public GameObject inGameMenu; /*pasue menu*/

    public GameObject optionsMenu;

    //return true if there still has monster is active
    public bool WaveActive
    {
        get
        {
            return activeMonsters.Count > 0;
        }
    }

    public long Currency
    {
        get
        {
            return currency;
        }

        set
        {
            currency = value;
            OnCurrencyChanged();
        }
    }

    /*Awake用于在游戏开始之前初始化变量或游戏状态。*/
    /*Start仅在Update函数第一次被调用前调用*/
    private void Awake() {
        Pool = GetComponent<ObjectPool>();
    }

   
    public void Start()
    {
        /*set the initial lives to 10 and current to 100*/
        lives = 10;
        Currency = 100L;
    }

    /*pick a tower from tower panel*/
    /*this will be called in tower button*/
    public void PickTower(TowerButton towerButton)
    {

        //delete !WaveActive allow us to put tower when wave active
        if(Currency >= towerButton.price && !WaveActive)
        {
            this.ClickedBtn = towerButton;
            hoverScale = towerButton.TowerPrefab.transform.localScale; /*改变hover的scale*/

            /*dont show the hover range if it is block*/
            Hover.Instance.Activate(towerButton.Sprite, towerButton.isTower); /*让hover是tower的图像*/
        }
    }

    /*buy a tower -- place a tower to tile == buy a tower*/ 
    public void BuyTower()
    {
        if (Currency >= ClickedBtn.price)
        {
            Currency -= ClickedBtn.price;
            Hover.Instance.Deativate(); /*取消hover的图像*/
        }
    }

    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {
            Changed();
        }
    }

    /**/
    public void Update()
    {
        HandleEscape(); /*当按escape，取消选择的tower*/
        currencyText.text = Currency + "<color=lime>$</color>";
        livesText.text = "" + lives;

        /*IF live less than and equal to 0, gameOver*/
        if(lives <= 0)
        {
            lives = 0;
            GameOver();
        }


    }

    /*选择这个塔就会出现他的攻击方位，同时取消其他塔的攻击方位*/
    public void SelectTower(Tower tower)
    {
        /*If we select tower alaready, then deselecte the tower before select next tower*/
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        sellText.text = "+ " + (selectedTower.price/2).ToString() + "$";
        upgradePanel.SetActive(true);
    }

    /*如果鼠标在空地点击，取消当前塔的攻击范围*/
    public void DeselectTower()
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;

        upgradePanel.SetActive(false);
    }

    /*当按escape，取消选择的tower*/
    public void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if(selectedTower == null && !Hover.Instance.IsVisible)
            {
                ShowIngameMenu();
            }
            else if (Hover.Instance.IsVisible)
            {
                DropTower();
            }
            else if(selectedTower != null)
            {
                DeselectTower();
            }
        }
    }
    
    public void openSetting()
    {
        if (selectedTower == null && !Hover.Instance.IsVisible)
        {
            ShowIngameMenu();
        }
        else if (Hover.Instance.IsVisible)
        {
            DropTower();
        }
        else if (selectedTower != null)
        {
            DeselectTower();
        }
    }

    /*start next wave*/
    public void StartWave()
    {
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);

        StartCoroutine(SpawnWave()); /*generate monster to this wave*/

        waveBtn.SetActive(false); /*hide the next wave button*/
    }

    /*generate monster to this wave*/
    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath(); /*generate new path depend on the tower in the map*/

        if (wave % 3 == 0)
        {
            health += 5;
        }
        //Debug.Log("Max health: " + health);

        //每一波都有和wave数字一样的怪物
        for (int i = 0; i < wave; i++)
        {
            LevelManager.Instance.GeneratePath();

            int monterIndex = Random.Range(0, 11); // generate number from 0.0 to 11.0
            string type = string.Empty;

            switch (monterIndex)
            {
                case 0:
                    type = "man-1";
                    break;
                case 1:
                    type = "studentZombie";
                    break;
                case 2:
                    type = "OrangeShibin";
                    break;
                case 3:
                    type = "grassGirl";
                    break;
                case 4:
                    type = "WhiteMonster";
                    break;
                case 5:
                    type = "man-2";
                    break;
                case 6:
                    type = "man-3";
                    break;
                case 7:
                    type = "girlZombie2";
                    break;
                case 8:
                    type = "man-4";
                    break;
                case 9:
                    type = "man-5";
                    break;
            }

           type = "man-3";

            Monster monster = Pool.GetObject(type).GetComponent<Monster>(); /*get the monster from the pool*/
            monster.Spwan(health);

            activeMonsters.Add(monster); /*add the monster to the activeMonster */

            yield return new WaitForSeconds(2.5f);
        }
    }

    /*remove monster from the active monster*/
    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        /*if wave is not active and not gameover, the show the next wave button*/
        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
    }

    /*show the gameOver menu*/
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    /*relaod the scene again to play the game again*/
    public void Restart()
    {
        SoundManager.Instance.PlaySFX("PressButton");

        StartCoroutine(waitForASecondInRestart());
    }

    IEnumerator waitForASecondInRestart()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*quit from the game*/
    public void QuitGame()
    {
        SoundManager.Instance.PlaySFX("PressButton");
        StartCoroutine(waitForASecondInQuit());
    }

    IEnumerator waitForASecondInQuit()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }

    public void SellTower()
    {
        if(selectedTower != null)
        {
            Currency += selectedTower.price / 2;
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            selectedTower.GetComponentInParent<TileScript>().walkAble = true;

            Destroy(selectedTower.transform.parent.gameObject);
            DeselectTower();
        }
    }

    /*show and hide the description of tower*/
    public void ShowStats()
    {
        /*If the panel is enable, then unenable the panel
         if the panel is unenable, then enable it*/
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void ShowSelectedTowerStats()
    {
        UpdateUpgradeTip();
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetTooltipText(string txt)
    {
        statText.text = txt;
    }

    public void UpdateUpgradeTip()
    {
        if (selectedTower != null)
        {
            sellText.text = "+ " + (selectedTower.price / 2).ToString() + "$";
            SetTooltipText(selectedTower.GetStats());

            if(selectedTower.NextUpgrade != null)
            {
                upgradePrice.text = "$" + selectedTower.NextUpgrade.Price.ToString();
            }
            else /*no more upgrade*/
            {
                upgradePrice.text = string.Empty;
            }
        }
    }

    public void UpgradeTower()
    {
        if(selectedTower != null)
        {
            if(selectedTower.level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    public void ShowIngameMenu()
    {
        if (optionsMenu.activeSelf)
        {
            ShowMain();
        }
        else
        {
            SoundManager.Instance.PlaySFX("PressButton");

            inGameMenu.SetActive(!inGameMenu.activeSelf);

            if (!inGameMenu.activeSelf)
            {
                Time.timeScale = 1; /*run the game*/
            }
            else
            {
                Time.timeScale = 0; /*stop the game*/
            }
        }

        if(optionsMenu.activeSelf || inGameMenu.activeSelf)
        {
            CameraMovement.Instance.cameraSpeed = 0;
        }
        else
        {
            CameraMovement.Instance.cameraSpeed = 10;
        }
    }

    private void DropTower()
    {
        ClickedBtn = null;

        Hover.Instance.Deativate();
    }

    public void ShowOptions()
    {
        SoundManager.Instance.PlaySFX("PressButton");
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);

    }

    public void ShowMain()
    {
        SoundManager.Instance.PlaySFX("PressButton");

        inGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
