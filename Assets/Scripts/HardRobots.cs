using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HardRobots : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
   
    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;

    private RubyController rubyController;

    Animator animator;


    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        GameObject RubyController = GameObject.FindWithTag("RubyController");

        if (RubyController != null)

        {
            rubyController = RubyController.GetComponent<RubyController>();
        }
    }

    void Update()
    {
        
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
       
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //RubyController player = other.gameObject.GetComponent<RubyController>();

        //if (rubyController != null)

        if(collision.gameObject.tag == "RubyController")
        {
            rubyController.ChangeHealth(-2);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        rubyController.ChangeScore(1);
    }
}