using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEnemy : MonoBehaviour
{
    public EnemyControl Obj;
    public Transform bgr;
    public GameObject bl;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Obj.blood>0)
        {
            float _blood = (1f / Obj.limit) * Obj.blood;
            transform.localScale = new Vector3(_blood, 0.2f, 1);
            _blood = (1f - _blood) / 2f;
            transform.position = new Vector3(bgr.transform.position.x - _blood, bgr.transform.position.y, transform.position.z);
        }   
        else
        {
            float _blood = 0f;
            transform.localScale = new Vector3(_blood, 0.2f, 1);
            _blood = (1f - _blood) / 2f;
            transform.position = new Vector3(bgr.transform.position.x - _blood, bgr.transform.position.y, transform.position.z);
            Invoke("destroy", 1f);
        }
    }
    void destroy()
    {
        Destroy(bl);
    }
}
