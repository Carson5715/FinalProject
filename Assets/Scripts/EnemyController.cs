using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;
   
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;

    Animator animator;
    private RubyController rubyController;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();


        GameObject RubyController = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby

        if (RubyController != null)

        {

            rubyController = RubyController.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print("Found the RubyConroller Script!");



            if (rubyController == null)

            {
                print("Cannot find GameController Script!");
            }
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

        if (collision.gameObject.tag == "RubyController")
        {
            rubyController.ChangeHealth(-1);

        }
    }


    public void Fix()
    {
        //RubyController player = other.gameObject.GetComponent<RubyController>();

        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        
        rubyController.ChangeScore(1);

    }
}