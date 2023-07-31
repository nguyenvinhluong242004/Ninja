using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    GameController gameController;
    PlayerControl player;
    Rigidbody2D rb;
    SpriteRenderer spr;
    public Animator anm;
    enum MovemenState { idle, run, hit, skill1, skill2 };
    MovemenState state;
    public float blood, limit;
    public GameObject sta;
    public GameObject[] Status;
    public bool[] skill, _skill;
    public bool flx, isOn, isDie, isFar;
    bool timeSkill;
    public bool isImpact;
    public Transform leftPoint, rightPoint;
    float xleft, xright;
    Vector3 _po;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        player = FindObjectOfType<PlayerControl>();
        spr = GetComponent<SpriteRenderer>();
        anm = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        state = MovemenState.idle;
        xleft = leftPoint.position.x;
        xright = rightPoint.position.x;
        skill = new bool[2];
        skill[0] = false;
        skill[1] = false;
        _skill = new bool[2];
        _skill[0] = false;
        _skill[1] = false;
        flx = false;
        isOn = false;
        isFar = false;
        isDie = false;
        _po = transform.position;
        isImpact = false;
        timeSkill = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        if (isOn && !isDie)
        {
            if (player.transform.position.x > transform.position.x - 3f && player.transform.position.x < transform.position.x + 3f)
            {
                isImpact = true;
                rb.velocity = new Vector2(0, 0);
            }
            else if (!isFar)
            {
                if (player.transform.position.x < transform.position.x)
                {
                    isImpact = false;
                    rb.velocity = new Vector2(-2f, 0);
                    spr.flipX = true;
                    sta.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    isImpact = false;
                    rb.velocity = new Vector2(2f, 0);
                    spr.flipX = false;
                    sta.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }    
        }
        else if (player.transform.position.x - 10f < _po.x && player.transform.position.x + 10f > _po.x)
        {
            isOn = true;
        }
        if (transform.position.x < xleft ) 
        {
            isFar = true;
            if (player.transform.position.x > transform.position.x)
            {
                isFar = false;
                rb.velocity = new Vector2(2f, 0);
            }
            else
                rb.velocity = new Vector2(0, 0);
        }
        else if(transform.position.x > xright)
        {
            isFar = true;
            if (player.transform.position.x < transform.position.x)
            {
                isFar = false;
                rb.velocity = new Vector2(-2f, 0);
            }
            else
                rb.velocity = new Vector2(0, 0);
        }    
        if (isImpact && timeSkill)
        {
            float sk = Random.Range(1f, 3f);
            int _sk = Mathf.RoundToInt(sk);
            if (_sk == 1)
            {
                timeSkill = false;
                _skill[0] = true;
                setSkill(0, true);
                setSkiller(0, true);
                Invoke("setSkill1", 0.5f);
                Invoke("setSkiller1", 1f);
                Invoke("upDate", 3f);
            }
            else
            {
                timeSkill = false;
                _skill[1] = true;
                setSkill(1, true);
                setSkiller(1, true);
                Invoke("setSkill2", 0.5f);
                Invoke("setSkiller2", 1f);
                Invoke("upDate", 3f);
            }
        }
    }
    void upDate()
    {
        timeSkill = true;
    }    
    void UpdateAnimation()
    {
        if (blood > 0)
        {
            if (isOn)
            {
                state = MovemenState.run;
            }
            if (isImpact || isFar)
            {
                state = MovemenState.idle;
            }
            if (skill[0])
            {
                state = MovemenState.skill1;
            }
            else if (skill[1])
            {
                state = MovemenState.skill2;
            }

            anm.SetInteger("state", (int)state);
        }
        else
        {
            isDie = true;
            sta.SetActive(false);
            rb.velocity = new Vector2(0, 0);
            anm.SetTrigger("trigger");
            Invoke("destroy", 2f);
        }
    }
    void destroy()
    {
        Destroy(gameObject);
    }
    void setSkill(int idx, bool sta)
    {
        skill[idx] = sta;
        Status[idx].SetActive(sta);
    }
    public void setSkiller(int idx, bool sta)
    {
        _skill[idx] = sta;
    }
    void setSkill1()
    {
        setSkill(0, false);
    }
    void setSkill2()
    {
        setSkill(1, false);
    }
    void setSkiller1()
    {
        setSkiller(0, false);
    }
    void setSkiller2()
    {
        setSkiller(1, false);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (player._Skill[0])
        {

            Debug.Log("1");
            player.setSkiller(0, false);
            blood -= player.damage[0];
        }
        else if (player._Skill[1])
        {

            Debug.Log("2");
            player.setSkiller(1, false);
            blood -= player.damage[1];
        }
        else if (player._Skill[2])
        {
            Debug.Log("3");
            player.setSkiller(2, false);
            blood -= player.damage[2];
        } 
        else if(collision.gameObject.CompareTag("Bomb"))
        {
            Debug.Log("bommm");
            blood -= player.damage[1];
            Invoke("setBomb", 0.7f);
        }    
        else if (!player.isJump)
        {
            Debug.Log("Chạm");
            player.rb.velocity = new Vector2(rb.velocity.x, -1f);
        }
    }
    void setBomb()
    {
        if (spr.flipX == false)
        {
            if (isImpact)
                transform.position -= new Vector3(0.09f, 0, 0);
            else
                transform.position += new Vector3(0.09f, 0, 0);
        }
        else
        {
            if (isImpact)
                transform.position += new Vector3(0.12f, 0, 0);
            else
                transform.position -= new Vector3(0.12f, 0, 0);
        }
    }    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            Debug.Log("11");
            blood -= player.damage[0];
            Destroy(collision.gameObject);
        }
    }
}
