using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    GameController gameController;
    public int key;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    // 0..n: level  -1: play  -2:setting  -3: back   -4: store   -5: onmusic  -6: offmusic   -7: next  -8: replace -9: quit
    // -10: close  -11: buy  -12: choice  -13: Balo  -20: useItem1  -> -29: useItem10 
    public void _onClick()
    {
        if (key >= 0)
        {
            if (gameController.getPlay(key))
            {
                gameController.listLevel.SetActive(false);
                gameController.isPlay = true;
                gameController.ui_play.SetActive(true);
            }
        }
        else if(key == -1)
        {
            gameController.start.SetActive(false);
            gameController.listLevel.SetActive(true);
        }
        else if (key == -2)
        {
            gameController.start.SetActive(false);
            gameController.setting.SetActive(true);
        }

        else if (key == -3)
        {
            if (gameController.isPlay)
            {
                gameController.setLevel();
                gameController.listLevel.SetActive(true);
                gameController.isPlay = false;
                gameController.ui_play.SetActive(false);
                for (int i = 0; i < 3; i++)
                    gameController.star[i].SetActive(true);
                gameController.complete.SetActive(false);
                FindObjectOfType<Camera>().transform.position = new Vector3(0, 0, -10);
            }
            else
            {
                gameController.infor.SetActive(false);
                gameController.shop.SetActive(false);
                gameController.listLevel.SetActive(false);
                gameController.setting.SetActive(false);
                gameController.start.SetActive(true);
            }
        }
        else if (key == -4)
        {
            gameController.start.SetActive(false);
            gameController.shop.SetActive(true);
        }
        else if (key == -5)
        {
            gameController.setMusic(true);
            gameController.saveSetting();
        }
        else if (key == -6)
        {
            gameController.setMusic(false);
            gameController.saveSetting();
        }
        else if (key == -7)
        {
            gameController.levelUp();
        }
        else if (key == -8)
        {
            gameController.setLevel();
            gameController.getPlay(gameController.indexLevel);
        }
        else if (key == -9)
        {
            Application.Quit();
        }
        if (key == -10)
        {
            gameController.infor.SetActive(false);
        }    
        else if (key == -11)
        {
            if (gameController.isEnough)
            {
                gameController.loadedData.playerCoin -= gameController._price;
                gameController.loadedData.items[gameController.idxItem]++;
                if (gameController.loadedData.playerCoin<gameController._price)
                {
                    gameController.isEnough = false;
                    gameController.onBuy.SetActive(false);
                    gameController.offBuy.SetActive(true);
                }
                gameController.setCountItem();
                gameController.saveSetting();
            }    
        }  
        else if (key==-12)
        {
            gameController.setChest();
        }
        else if (key == -13)
        {
            gameController.setBalo();
        }    
        else if (key == -20)
        {
            gameController.increaseBlood();
        }
        else if (key == -22)
        {
            gameController.swapHp();
        }
        else if (key == -23)
        {
            gameController.getBomb();
        }
        else if (key == -24)
        {
            gameController.getUpDamage();
        }
        else if (key<=-30)
        {
            gameController.infor.SetActive(true);
            int idx = -30 - key;
            int _price = int.Parse(gameController.priceItem[idx].text);
            if (gameController.loadedData.playerCoin>=_price)
            {
                gameController.isEnough = true;
                gameController.offBuy.SetActive(false);
                gameController.onBuy.SetActive(true);
                gameController.idxItem = idx;
                gameController._price = _price;
            }  
            else
            {
                gameController.isEnough = false;
                gameController.onBuy.SetActive(false);
                gameController.offBuy.SetActive(true);
            }
        }    
    }
}
