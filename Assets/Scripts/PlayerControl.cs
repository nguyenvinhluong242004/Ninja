using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    GameController gameController;
    public Rigidbody2D rb;
    public BoxCollider2D coll;
    [SerializeField] LayerMask jumpAbleGround;
    SpriteRenderer spr;
    public Animator anm;
    enum MovemenState { idle, run, jump, crouchDash, idleSword, runSword, jumpSword, crouchDashSword, skill1, skill2, skill3, skill1Jump, skill2Jump };
    MovemenState state;


    public float blood, timeUp, limit;
    public GameObject sta, skill, skillSword, firePrefab, upDamage;
    public GameObject[] Status;
    float k;
    public bool isJump, isSword, isTouch, isL, isR, isUp;
    public bool[] isSkill, _Skill;
    public float[] damage;
    public float timeSk1, timeSk2, timeSk3;
    public bool canUseSk1, canUseSk2, canUseSk3;
    public float lastUsedTime1, lastUsedTime2, lastUsedTime3;

    public TextMeshProUGUI text1, text2, text3;
    public GameObject _text1, _text2, _text3;
    //touch
    Camera _camera;
    int leftTouch = 36;
    int rightTouch = 36;
    public Transform attack, attackSw, sk1Sw, sk2Sw, sk3Sw, sk1, sk2, sk3, leftMove, rightMove, change;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Welcom to Ninja Game!");
        gameController = FindObjectOfType<GameController>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponentInChildren<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        anm = GetComponent<Animator>();
        state = MovemenState.idle;
        isSkill = new bool[4];
        _Skill = new bool[3];
        for (int i = 0; i < 3; i++)
        {
            isSkill[i] = false;
            _Skill[i] = false;
        }
        isSkill[3] = false;

        isJump = false;
        isSword = true;
        isTouch = false;
        isL = false;
        isR = false;
        isUp = false;
        if (isSword)
        {
            skill.SetActive(false);
        }
        else
        {
            skillSword.SetActive(false);
        }
        _camera = FindObjectOfType<Camera>();
        timeSk1 = 1f;
        timeSk2 = 2f;
        timeSk3 = 3f;
        canUseSk1 = true;
        canUseSk2 = true;
        canUseSk3 = true;
        _text1 = GameObject.Find("timeSk1");
        _text2 = GameObject.Find("timeSk2");
        _text3 = GameObject.Find("timeSk3");
        text1 = _text1.GetComponent<TextMeshProUGUI>();
        text2 = _text2.GetComponent<TextMeshProUGUI>();
        text3 = _text3.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            Vector2 touchPos = getTouchPosition(t.position); // * -1 for perspective cameras
            if (t.phase == TouchPhase.Began)
            {
                if (t.position.x > Screen.width / 2)
                {
                    rightTouch = t.fingerId;
                    if (isSword)
                    {
                        if (touchPos.x > attackSw.position.x - 1.15f && touchPos.x < attackSw.position.x + 1.15f && touchPos.y > attackSw.position.y - 1.15f && touchPos.y < attackSw.position.y + 1.15f)
                        {
                            if (isGrounded())
                            {
                                isJump = true;
                                isTouch = true;
                                attackSw.localScale = new Vector3(2f, 2f, attackSw.localScale.z);
                                Invoke("resetAttackSw", 0.2f);
                            }
                        }
                        else if (canUseSk1 && touchPos.x > sk1Sw.position.x - 0.8f && touchPos.x < sk1Sw.position.x + 0.8f && touchPos.y > sk1Sw.position.y - 0.8f && touchPos.y < sk1Sw.position.y + 0.8f)
                        {
                            lastUsedTime1 = Time.time;
                            canUseSk1 = false;
                            UpdateCooldownText1(timeSk1);
                            Debug.Log("skill 1");
                            _Skill[0] = true;
                            setSkill(1, true);
                            setSkiller(0, true);
                            Invoke("setSkill1", 0.5f);
                            Invoke("setSkiller1", 1f);
                        }
                        else if (canUseSk2 && touchPos.x > sk2Sw.position.x - 0.8f && touchPos.x < sk2Sw.position.x + 0.8f && touchPos.y > sk2Sw.position.y - 0.8f && touchPos.y < sk2Sw.position.y + 0.8f)
                        {
                            lastUsedTime2 = Time.time;
                            canUseSk2 = false;
                            UpdateCooldownText2(timeSk2);
                            Debug.Log("skill 2");
                            _Skill[1] = true;
                            setSkill(2, true);
                            setSkiller(1, true);
                            Invoke("setSkill2", 0.5f);
                            Invoke("setSkiller2", 1f);
                        }
                        else if (canUseSk3 && touchPos.x > sk3Sw.position.x - 0.8f && touchPos.x < sk3Sw.position.x + 0.8f && touchPos.y > sk3Sw.position.y - 0.8f && touchPos.y < sk3Sw.position.y + 0.8f)
                        {
                            lastUsedTime3 = Time.time;
                            canUseSk3 = false;
                            UpdateCooldownText3(timeSk3);
                            Debug.Log("skill 3");
                            _Skill[2] = true;
                            setSkill(3, true);
                            setSkiller(2, true);
                            Invoke("setSkill3", 0.5f);
                            Invoke("setSkiller3", 1f);
                        }
                    }
                    else
                    {
                        if (touchPos.x > attack.position.x - 1.15f && touchPos.x < attack.position.x + 1.15f && touchPos.y > attack.position.y - 1.15f && touchPos.y < attack.position.y + 1.15f)
                        {
                            if (isGrounded())
                            {
                                isJump = true;
                                isTouch = true;
                                attack.localScale = new Vector3(2f, 2f, attack.localScale.z);
                                Invoke("resetAttack", 0.2f);
                            }
                        }
                        else if (canUseSk1 && touchPos.x > sk1.position.x - 0.8f && touchPos.x < sk1.position.x + 0.8f && touchPos.y > sk1.position.y - 0.8f && touchPos.y < sk1.position.y + 0.8f)
                        {
                            lastUsedTime1 = Time.time;
                            canUseSk1 = false;
                            UpdateCooldownText1(timeSk1);
                            Debug.Log("skill 1");
                            if (spr.flipX == false)
                            {
                                GameObject newFire = Instantiate(firePrefab, transform.position + new Vector3(1f, 0, 0), Quaternion.identity);
                                newFire.GetComponent<Fire>().velocity = new Vector2(5f, 0);
                                newFire.GetComponent<SpriteRenderer>().flipX = false;
                            }
                            else
                            {
                                GameObject newFire = Instantiate(firePrefab, transform.position + new Vector3(-1f, 0, 0), Quaternion.identity);
                                newFire.GetComponent<Fire>().velocity = new Vector2(-5f, 0);
                                newFire.GetComponent<SpriteRenderer>().flipX = true;
                            }
                        }
                        else if (touchPos.x > sk2.position.x - 0.8f && touchPos.x < sk2.position.x + 0.8f && touchPos.y > sk2.position.y - 0.8f && touchPos.y < sk2.position.y + 0.8f)
                        {
                            Debug.Log("skill 2");
                        }
                        else if (touchPos.x > sk3.position.x - 0.8f && touchPos.x < sk3.position.x + 0.8f && touchPos.y > sk3.position.y - 0.8f && touchPos.y < sk3.position.y + 0.8f)
                        {
                            Debug.Log("skill 3");
                        }
                    }
                    if (!isSkill[0] && touchPos.x > change.position.x - 0.68f && touchPos.x < change.position.x + 0.68f && touchPos.y > change.position.y - 0.68f && touchPos.y < change.position.y + 0.68f)
                    {
                        isSword = !isSword;
                        change.localScale = new Vector3(1f, 1f, change.localScale.z);
                        Invoke("setChange", 0.15f);
                        if (isSword)
                        {
                            skill.SetActive(false);
                            skillSword.SetActive(true);
                        }
                        else
                        {
                            skillSword.SetActive(false);
                            skill.SetActive(true);
                        }
                    }
                }
                else
                {
                    leftTouch = t.fingerId;
                    Debug.Log("move");
                    if (touchPos.x > leftMove.position.x - 1f && touchPos.x < leftMove.position.x + 0.8f && touchPos.y > leftMove.position.y - 1f && touchPos.y < leftMove.position.y + 1f)
                    {
                        if (!isSkill[0])
                        {
                            Debug.Log("left");
                            leftMove.localScale = new Vector3(1.5f, 2f, leftMove.localScale.z);
                            isL = true;
                            k = -1f;
                        }
                    }
                    if (touchPos.x > rightMove.position.x - 0.8f && touchPos.x < rightMove.position.x + 1f && touchPos.y > rightMove.position.y - 1f && touchPos.y < rightMove.position.y + 1f)
                    {
                        if (!isSkill[0])
                        {
                            Debug.Log("right");
                            rightMove.localScale = new Vector3(1.5f, 2f, rightMove.localScale.z);
                            isR = true;
                            k = 1f;
                        }
                    }
                }
            }
            else if (t.phase == TouchPhase.Ended && rightTouch == t.fingerId)
            {
                rightTouch = 36;
            }
            else if (t.phase == TouchPhase.Ended && leftTouch == t.fingerId)
            {
                Debug.Log("don't move");
                leftTouch = 36;
                if (isL)
                {
                    leftMove.localScale = new Vector3(1.7f, 2.2f, leftMove.localScale.z);
                    isL = false;
                }
                else
                {
                    rightMove.localScale = new Vector3(1.7f, 2.2f, rightMove.localScale.z);
                    isR = false;
                }
                k = 0;
            }
            rb.velocity = new Vector2(k * 6f, rb.velocity.y);
            if (isTouch)
            {
                rb.velocity = new Vector2(rb.velocity.x, 6.5f);
                isTouch = false;
            }
            ++i;
        }
        if (!canUseSk1)
        {
            float timeRemaining1 = timeSk1 - (Time.time - lastUsedTime1);
            timeRemaining1 = Mathf.Max(0f, timeRemaining1);
            UpdateCooldownText1(timeRemaining1);
        }
        if (!canUseSk2)
        {
            float timeRemaining2 = timeSk2 - (Time.time - lastUsedTime2);
            timeRemaining2 = Mathf.Max(0f, timeRemaining2);
            UpdateCooldownText2(timeRemaining2);
        }
        if (!canUseSk3)
        {
            float timeRemaining3 = timeSk3 - (Time.time - lastUsedTime3);
            timeRemaining3 = Mathf.Max(0f, timeRemaining3);
            UpdateCooldownText3(timeRemaining3);
        }
        //k = Input.GetAxisRaw("Horizontal");
        //if (!isSkill[0])
        //    rb.velocity = new Vector2(k * 6f, rb.velocity.y);
        if (Input.GetKeyDown("space") && isGrounded())
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, 6.5f);
        }
        if (Input.GetKeyDown("1") && isSword)
        {
            //Debug.Log("skill 1");
            _Skill[0] = true;
            setSkill(1, true);
            setSkiller(0, true);
            Invoke("setSkill1", 0.5f);
            Invoke("setSkiller1", 1f);
        }
        if (Input.GetKeyDown("2") && isSword)
        {
            //Debug.Log("skill 2");
            _Skill[1] = true;
            setSkill(2, true);
            setSkiller(1, true);
            Invoke("setSkill2", 0.5f);
            Invoke("setSkiller2", 1f);
        }
        if (Input.GetKeyDown("3") && isSword)
        {
            //Debug.Log("skill 3");
            _Skill[2] = true;
            setSkill(3, true);
            setSkiller(2, true);
            Invoke("setSkill3", 0.5f);
            Invoke("setSkiller3", 1f);
        }
    }
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return _camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
    public void setDamage(bool sta)
    {
        isUp = sta;
        if (sta)
        {
            upDamage.SetActive(true);
            for (int i = 0; i < damage.Length; i++)
                damage[i] += 7;
        }
        else
        {
            upDamage.SetActive(false);
            for (int i = 0; i < damage.Length; i++)
                damage[i] -= 7;
        } 
    }    
    void UpdateAnimation()
    {
        if (!isSword)
        {
            if (blood > 0)
            {
                if (k > 0f)
                {
                    state = MovemenState.run;
                    spr.flipX = false;
                    sta.transform.eulerAngles = new Vector3(0, 0, 0);

                }
                else if (k < 0f)
                {
                    state = MovemenState.run;
                    spr.flipX = true;
                    sta.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (isGrounded() && rb.velocity.y == 0f)
                {
                    isJump = false;
                    state = MovemenState.idle;
                }
                if (rb.velocity.y != 0f)
                {
                    state = MovemenState.jump;
                }
            }
            else
            {
                state = MovemenState.crouchDash;
            }
        }
        else
        {
            if (blood > 0)
            {
                if (k > 0f)
                {
                    state = MovemenState.runSword;
                    spr.flipX = false;
                    sta.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (k < 0f)
                {
                    state = MovemenState.runSword;
                    spr.flipX = true;
                    sta.transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (isGrounded() && rb.velocity.y == 0f)
                {
                    isJump = false;
                    state = MovemenState.idleSword;
                }
                if (rb.velocity.y != 0f)
                {
                    state = MovemenState.jumpSword;
                }
                if (isSkill[1])
                {
                    if (isJump)
                        state = MovemenState.skill1Jump;
                    else
                        state = MovemenState.skill1;
                }
                else if (isSkill[2])
                {
                    if (isJump)
                        state = MovemenState.skill2Jump;
                    else
                        state = MovemenState.skill2;
                }
                else if (isSkill[3])
                {
                    state = MovemenState.skill3;
                }
            }
            else
            {
                state = MovemenState.crouchDashSword;
                Invoke("rePlace", 2.4f);
            }
        }

        anm.SetInteger("state", (int)state);
    }
    void rePlace()
    {
        gameController.setLevel();
        gameController.getPlay(gameController.indexLevel);
    }
    void resetAttackSw()
    {
        attackSw.localScale = new Vector3(2.2f, 2.2f, attackSw.localScale.z);
    }
    void resetAttack()
    {
        attack.localScale = new Vector3(2.2f, 2.2f, attack.localScale.z);
    }
    void setChange()
    {
        change.localScale = new Vector3(1.2f, 1.2f, change.localScale.z);
    }
    bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpAbleGround);
    }
    void setSkill(int idx, bool sta)
    {
        isSkill[0] = sta;
        isSkill[idx] = sta;
        Status[idx - 1].SetActive(sta);
    }
    public void setSkiller(int idx, bool sta)
    {
        _Skill[idx] = sta;
    }
    void setSkill1()
    {
        setSkill(1, false);
    }
    void setSkill2()
    {
        setSkill(2, false);
    }
    void setSkill3()
    {
        setSkill(3, false);
    }
    void setSkiller1()
    {
        setSkiller(0, false);
    }
    void setSkiller2()
    {
        setSkiller(1, false);
    }
    void setSkiller3()
    {
        setSkiller(2, false);
    }
    void UpdateCooldownText1(float timeRemaining)
    {
        int k = Mathf.RoundToInt(timeRemaining);
        text1.text = "" + k;

        if (timeRemaining <= 0.01f)
        {
            canUseSk1 = true;
            text1.text = "";
        }
    }
    void UpdateCooldownText2(float timeRemaining)
    {
        int k = Mathf.RoundToInt(timeRemaining);
        text2.text = "" + k;

        if (timeRemaining <= 0.01f)
        {
            canUseSk2 = true;
            text2.text = "";
        }
    }
    void UpdateCooldownText3(float timeRemaining)
    {
        int k = Mathf.RoundToInt(timeRemaining);
        text3.text = "" + k;

        if (timeRemaining <= 0.01f)
        {
            canUseSk3 = true;
            text3.text = "";
        }
    }
}