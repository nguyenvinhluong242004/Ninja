using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    GameController gameController;
    public PlayerControl Obj;
    public bool isUpDamage;
    public Transform bgr;
    public GameObject bl;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Obj.blood>0 && !isUpDamage)
        {
            float _blood = (1f / Obj.limit) * Obj.blood;
            transform.localScale = new Vector3(_blood, 0.2f, 1);
            _blood = (1f - _blood) / 2f;
            transform.position = new Vector3(bgr.transform.position.x - _blood, bgr.transform.position.y, transform.position.z);
        } 
        else if (Obj.timeUp> 0 && isUpDamage)
        {
            float _timeUp = (1f / Obj.limit) * Obj.timeUp;
            transform.localScale = new Vector3(_timeUp, 0.2f, 1);
            _timeUp = (1f - _timeUp) / 2f;
            transform.position = new Vector3(bgr.transform.position.x - _timeUp, bgr.transform.position.y, transform.position.z);
        }    
        else if (!isUpDamage)
        {
            float _blood = 0f;
            transform.localScale = new Vector3(_blood, 0.2f, 1);
            _blood = (1f - _blood) / 2f;
            transform.position = new Vector3(bgr.transform.position.x - _blood, bgr.transform.position.y, transform.position.z);
            Invoke("destroy", 1f);
        }
        if (isUpDamage)
        {
            if (Obj.isUp)
            {
                if (Obj.timeUp >= 1.4f)
                    Obj.timeUp -= 1.4f;
                else
                {
                    Obj.timeUp = 100;
                    gameController.resetDamage();
                }
            }
        }
    }
    void destroy()
    {
        Destroy(bl);
    }
}
