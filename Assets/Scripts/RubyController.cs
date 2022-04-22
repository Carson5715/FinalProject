using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public ParticleSystem Hiteffect;

    public int score = 0;
    public Text RobotTotalText;
    public int maxHealth = 5;
    public bool WinCheck = false;
    public bool LoseCheck = false;
    public bool DisplayWin = true;
    public GameObject projectilePrefab;
     
    public AudioSource HitNoise;
    public AudioSource ThrowCog;
    public AudioSource BackgroundMusic;
    public AudioSource LoseMusic;
    public AudioSource WinMusic;

    public Text TextCogammount;
    public GameObject LoseImage;
    public GameObject WinImage;

    public int health { get { return currentHealth; } }
    int currentHealth;
    public Text LoseScreen;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    public int CogAmmount = 5;
    public ParticleSystem Healtheffect;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Hiteffect.Pause();
        currentHealth = maxHealth;
        Healtheffect.Pause();
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;

        LoseImage.SetActive(false);
        
            WinImage.SetActive(false);
     

    }


    // Update is called once per frame
    void Update()    
    {
        if (score == 6)
        {
            HitNoise.Stop();
        }

        if (score == 6 && WinCheck == false)
        {
            
            BackgroundMusic.Stop();
           
            Debug.Log("LoseMusicPlaying");
            WinMusic.Play();
            
            
            WinCheck = true; 
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        TextCogammount.text = CogAmmount.ToString();

        if (currentHealth == 0)
        {
            HitNoise.Stop();
        }
        if (currentHealth == 0 && LoseCheck == false)
        {
            
            LoseImage.SetActive(true);
            LoseScreen.enabled = true;
            BackgroundMusic.Stop();
            
            LoseMusic.Play();
            

            LoseCheck = true;
        }

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && CogAmmount != 0)
        {
            Launch();
            ThrowCog.Play();

        }
       
        if (Input.GetKeyDown(KeyCode.X)&& score == 6)
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPCSpeaking character = hit.collider.GetComponent<NPCSpeaking>();
                if (character != null)
                {
                    
                    
                    SceneManager.LoadScene(1);
                }
            }
        }
        if (WinCheck == true || LoseCheck == true)
        {
            if (Input.GetKey("r"))
            {
                SceneManager.LoadScene(0);
            }
        }
        if (score == 6)
        {
           if (DisplayWin == true)
            {
            DisplayDialogWin();
                DisplayWin = false;
            }
            
             
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
     
    }

    public void DisplayDialogWin()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }

    void FixedUpdate()
    {

        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        

        if (!LoseImage.activeSelf)
        {
            rigidbody2d.MovePosition(position);
        }
        
        
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {

            animator.SetTrigger("Hit");

            if (isInvincible)
                return;
            HitNoise.Play();
            Hiteffect.Play();
            isInvincible = true;
            invincibleTimer = timeInvincible;
            horizontal = 0;
            vertical = 0;

        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void ChangeScore(int Scoreamount)
    {
        score  += Scoreamount;
        RobotTotalText.text = score.ToString();

    }


    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        CogAmmount -= 1;


    }
}