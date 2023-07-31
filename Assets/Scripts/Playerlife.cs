using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerlife : MonoBehaviour
{
    GameController gameController;
    PlayerControl player;
    public GameObject[] item;
    public int count, point;
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        player = GetComponent<PlayerControl>();
        Invoke("getItem", 0.1f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameController.getEnemy(collision.gameObject);
        }    
        else if (collision.gameObject.CompareTag("item"))
        {
            point++;
            gameController.scoreText.text = "" + point + " / " + count;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            if (point == count)
            {
                gameController.setStar(3);
            }
            else if (point > count * 2 / 3)
            {
                gameController.setStar(2);
            }
            else
            {
                gameController.setStar(1);
            }
            gameController.complete.SetActive(true);
            resetItems();
        }
        else if (collision.gameObject.CompareTag("dealth"))
        {
            gameController.setLevel();
            gameController.getPlay(gameController.indexLevel);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameController.getEnemy(null);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyControl ene = collision.gameObject.GetComponent<EnemyControl>();
        if (ene)
        {
            if (ene._skill[0])
            {
                Debug.Log("1");
                ene.setSkiller(0, false);
                player.blood -= 3;
            }
            else if (ene._skill[1])
            {
                Debug.Log("2");
                ene.setSkiller(1, false);
                player.blood -= 7;
            }
        }
    }
    public void resetItems()
    {
       for (int i = 0; i < item.Length; i++)
            if (item[i])
                Destroy(item[i]);
        item = new GameObject[0];
    }    
    void getItem()
    {
        item = GameObject.FindGameObjectsWithTag("item");
        count = item.Length;
        point = 0;
        gameController.scoreText.text = "" + point + " / " + count;
    }    
}
