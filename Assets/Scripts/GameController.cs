using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject[] levelPrefabs, star;
    public Level[] level;
    public GameObject levelPlay, onMusic, offMusic, complete, start, end, startPoint, shop, ui_play, listLevel, setting, infor, onBuy, offBuy, choice, itemInBalo, useHp, _swapHp, upDamage, bomb;
    public Chest_ chest;
    public EnemyControl enemy;
    public PlayerControl player;
    public int indexLevel, coin, starLevelPlay, idxItem, _price;
    public TextMeshProUGUI[] countItem, countItemPlay, priceItem;
    public TextMeshProUGUI countCoin, scoreText;
    public bool isMusic, isPlay, isEnough, isOpenChest, isOpenBalo, isUpDamage;
    //Json
    public class PlayerData
    {
        public bool _isMusic;
        public int playerCoin;
        public int[] levels;
        public int[] items;
    }

    private string filePath;
    public PlayerData loadedData;
    // Start is called before the first frame update
    void Start()
    {
        // Đặt đường dẫn của tệp JSON, ví dụ: "data.json" trong thư mục gốc của ứng dụng di động
        filePath = Path.Combine(Application.persistentDataPath, "data.json");

        //Ghi dữ liệu JSON cho lần chạy đầu tiên
        if (!File.Exists(filePath))
        {
            PlayerData dataToSave = new PlayerData
            {
                _isMusic = true,
                playerCoin = 10000,
                levels = new int[] { 7, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                items = new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            SaveDataToJson(dataToSave);
        }

        // Đọc dữ liệu JSON
        loadedData = LoadDataFromJson();
        if (loadedData != null)
        {
            Debug.Log("Is Music On: " + loadedData._isMusic);
            Debug.Log("Player Coin: " + loadedData.playerCoin);
            Debug.Log("Levels: " + string.Join(", ", loadedData.levels));
            Debug.Log("Items: " + string.Join(", ", loadedData.items));
        }
        /////////////////////////////////////////////////////////////////////////////////////////
        setActiveLevel();
        start.SetActive(true);
        isMusic = loadedData._isMusic;
        if (isMusic)
            offMusic.SetActive(false);
        else
            onMusic.SetActive(false);
        isPlay = false;
        isEnough = false;
        isOpenChest = false;
        isOpenBalo = false;
        isUpDamage = false;
        setCountItem();
    }
    public void getBomb()
    {
        if (enemy)
        {
            if (loadedData.items[3] > 0)
            {
                loadedData.items[3]--;
                Instantiate(bomb, enemy.transform.position - new Vector3(0, 1.2f, 0), transform.rotation);
                setCountItem();
                saveSetting();
            }
        }
    }    
    public void getUpDamage()
    {
        if (!isUpDamage && loadedData.items[4] > 0)
        {
            player.setDamage(true);
            upDamage.SetActive(true);
            isUpDamage = true;
            loadedData.items[4]--;
            setCountItem();
            saveSetting();
        }    
    }    
    public void resetDamage()
    {
        player.setDamage(false);
        upDamage.SetActive(false);
        isUpDamage = false;
    }    
    public void setMusic(bool sta)
    {
        isMusic = sta;
        loadedData._isMusic = sta;
        if (sta)
        {
            offMusic.SetActive(false);
            onMusic.SetActive(true);
        }
        else
        {
            onMusic.SetActive(false);
            offMusic.SetActive(true);
        }

    }
    public void setCountItem()
    {
        countCoin.text = "" + loadedData.playerCoin;
        for (int i = 0; i < countItem.Length; i++)
            countItem[i].text = "" + loadedData.items[i];
        countItemPlay[0].text = "" + loadedData.items[0];
        countItemPlay[1].text = "" + loadedData.items[2];
        countItemPlay[2].text = "" + loadedData.items[3];
        countItemPlay[3].text = "" + loadedData.items[4];
    }    
    void setActiveLevel()
    {
        int k = 0;
        for (int i = 1; i < loadedData.levels.Length - 1; i += 2)
        {
            if (loadedData.levels[i] == 1)
            {
                level[k].setLevel(loadedData.levels[i + 1]);
                level[k].setLock(true);
            }
            k++;
        }
    }
    public void setStar(int k)
    {
        starLevelPlay = k;
        for (int i = 3; i > k; i--)
            if (i - 1 < star.Length)
                star[i - 1].SetActive(false);
    }
    public bool getPlay(int idx)
    {
        if (idx < levelPrefabs.Length && loadedData.levels[idx * 2 + 1] == 1)
        {
            indexLevel = idx;
            start.SetActive(false);
            complete.SetActive(false);
            levelPlay = Instantiate(levelPrefabs[indexLevel], levelPrefabs[indexLevel].transform.position, levelPrefabs[indexLevel].transform.rotation);
            startPoint = GameObject.FindGameObjectWithTag("Start");
            if (startPoint)
                Destroy(startPoint);
            while (!startPoint)
                startPoint = GameObject.FindGameObjectWithTag("Start");
            Debug.Log(startPoint.transform.position);
            player = FindObjectOfType<PlayerControl>();
            player.transform.position = new Vector3(startPoint.transform.position.x + 2f, startPoint.transform.position.y + 2f, player.transform.position.z);
            coin = 0;
            return true;
        }
        return false;
    }
    public void setLevel()
    {
        for (int i = 0; i < 3; i++)
            star[i].SetActive(true);
        Destroy(levelPlay);
    }
    public void levelUp()
    {
        if (indexLevel < levelPrefabs.Length - 1)
        {
            setLevel();
            if (starLevelPlay > loadedData.levels[indexLevel * 2 + 2])
            {
                Debug.Log("getStar");
                loadedData.levels[indexLevel * 2 + 2] = starLevelPlay;
                if (loadedData.levels[indexLevel * 2 + 3] == 0 && indexLevel + 1 < levelPrefabs.Length)
                    loadedData.levels[indexLevel * 2 + 3] = 1;
            }
            indexLevel++;
            coin = starLevelPlay * 50;
            loadedData.playerCoin += coin;
            setCountItem();
            setActiveLevel();
            getPlay(indexLevel);
        }
        else
        {
            setLevel();
            setWin();
        }    
        saveSetting();
    }
    void setWin()
    {
        complete.SetActive(false);
        start.SetActive(false);
        isPlay = false;
        FindObjectOfType<Camera>().transform.position = new Vector3(0, 0, -10);
        end.SetActive(true);
    }   
    public void getEnemy(GameObject _gameObject)
    {
        if (_gameObject)
        {
            enemy = _gameObject.GetComponent<EnemyControl>();
        }
        else
        {
            enemy = null;
        }
    }
    public void getChest(GameObject _gameObject)
    {
        if (_gameObject)
        {
            chest = _gameObject.GetComponent<Chest_>();
            if (chest)
                choice.SetActive(true);
        }
        else
        {
            chest = null;
            choice.SetActive(false);
        }
    }    
    public void setChest()
    {
        if (chest)
        {
            isOpenChest = !isOpenChest;
            chest.getChoice(isOpenChest);
        }    
    }    
    public void increaseBlood()
    {
        if (player)
        {
            if (loadedData.items[0] > 0)
            {
                if (player.blood + 12 < 100)
                    player.blood += 12;
                else
                    player.blood = 100;
                useHp.SetActive(true);
                Invoke("resetUseHp", 0.36f);
                loadedData.items[0]--;
                setCountItem();
                saveSetting();
            }    
        }    
    }    
    public void swapHp()
    {
        if (enemy)
        {
            if(loadedData.items[2]>0)
            {
                float hp = player.blood;
                player.blood = enemy.blood;
                enemy.blood = hp;
                _swapHp.SetActive(true);
                Invoke("resetSwapHp", 0.36f);
                loadedData.items[2]--;
                setCountItem();
                saveSetting();
            }    
        }    
    }    
    void resetUseHp()
    {
        useHp.SetActive(false);
    }    
    void resetSwapHp()
    {
        _swapHp.SetActive(false);
    }    
    public void setBalo()
    {
        isOpenBalo = !isOpenBalo;
        itemInBalo.SetActive(isOpenBalo);
    }    
    public void increaseCoin()
    {
        coin++;
    }
    public void saveSetting()
    {
        SaveDataToJson(loadedData);
    }
    void SaveDataToJson(PlayerData data)
    {
        string jsonData = JsonConvert.SerializeObject(data);
        File.WriteAllText(filePath, jsonData);
    }
    PlayerData LoadDataFromJson()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            return null;
        }
    }
}
