using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] star;
    public GameObject _lock;

    // Start is called before the first frame update
    void Start()
    {
        star = new GameObject[3];
        Transform[] childObjects = transform.GetComponentsInChildren<Transform>(true); // tìm object con
        int i = 0;
        foreach (Transform child in childObjects)
        {
            if (child.CompareTag("Star"))
            {
                star[i] = child.gameObject;
                i++;
            }
            else if (child.name == "lock")
            {
                _lock = child.gameObject;
            }
        }
    }
    public void setLevel(int k)
    {
        for (int i = 0; i < 3; i++)
            star[i].SetActive(true);
        for (int i = 3; i > k; i--)
            if (i - 1 < star.Length)
                star[i - 1].SetActive(false);
    }
    public void setLock(bool sta)
    {
        if (sta)
            _lock.SetActive(false);
    }
}
