using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Vector2 velocity;
    public Vector3 _po;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _po = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity;
        if (velocity.x > 0 && transform.position.x > _po.x + 4.2f)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x < _po.x - 4.2f)
            Destroy(gameObject);
    }
}
