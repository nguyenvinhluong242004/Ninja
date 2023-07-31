using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator anm;
    public BoxCollider2D on;
    // Start is called before the first frame update
    void Start()
    {
        anm = GetComponent<Animator>();
        on.enabled = true;
        Invoke("setAnm", 0.5f);
        Invoke("_desTroy", 0.8f);
    }
    void setAnm()
    {
        anm.SetBool("isOn", true);
    }    
    void _desTroy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }    
}
