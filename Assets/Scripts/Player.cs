using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour 
{
    // GetInstance must be called on start, otherwise that object may awake before player
    private static GameObject instance;
    private SpriteRenderer sr;
    private Rigidbody2D rb; //rigidbody movement better for collision
    public float moveSpeed = 13;
    private Vector2 input_velocity;
    public int health = 10;
    private int currentmana;
    public int maxmana = 10;
    private int attacktype;
    public Transform attackpoint;
    public float stabradius = 0.4f;
    public float TimeBtwAttacks = 0;
    private float attackCooldown;
    public Skill_Icon gun, burst, grapple;
    private int guncost, burstcost, grapplecost;
    public GameObject projectile;
    public Transform burstvisual;

    private Animator animator;
    private string currentState;
    const string
        Player_idle = "Player_idle",
        Player_move = "Player_move",
        Player_stab = "Player_stab",
        Player_hit = "Player_hit",
        Player_die = "Player_die",
        Player_gun = "Player_gun",
        Player_burst = "Player_burst",
        Player_grapple = "Player_grapple";

    private void Awake() 
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(instance);
        }
        else if (instance != gameObject)
        {
            Destroy(instance);
        }
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;  
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
        animator = GetComponent<Animator>();
        guncost = gun.skillcost;
        burstcost = burst.skillcost; 
        grapplecost = grapple.skillcost;
        Color c = burstvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        burstvisual.GetComponent<SpriteRenderer>().material.color = c;
    }

    public static GameObject GetInstance()
    {
        return instance;
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void Update() 
    {
        input_velocity.x = Input.GetAxisRaw("Horizontal");
        input_velocity.y = Input.GetAxisRaw("Vertical");
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        //on pressing a skill's button, deactivate skill if we're using skill, else go to skill
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (attacktype == 1)
            {
                gun.pressed(false);
                attacktype = 0;
            } 
            else
            {
                gun.pressed(true);
                burst.pressed(false);
                grapple.pressed(false);
                attacktype = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (attacktype == 2)
            {
                burst.pressed(false);
                attacktype = 0;
            }
            else
            {
                gun.pressed(false);
                burst.pressed(true);
                grapple.pressed(false);
                attacktype = 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (attacktype == 3)
            {
                grapple.pressed(false);
                attacktype = 0;
            }
            else
            {
                gun.pressed(false);
                burst.pressed(false);
                grapple.pressed(true);
                attacktype = 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            updateMana(maxmana - currentmana);
        }
        Color c = burstvisual.GetComponent<SpriteRenderer>().material.color;
        if (attacktype == 2)
        {
            c.a = 1;
            burstvisual.position = mousepos;
        }
        else
        {
            c.a = 0;
        }
        burstvisual.GetComponent<SpriteRenderer>().material.color = c;


        ChangeAnimationState(input_velocity == Vector2.zero ? Player_idle : Player_move);
        attackpoint.RotateAround(transform.position, Vector3.forward,
            Vector2.SignedAngle(attackpoint.position - transform.position, mousepos - transform.position));
        if (Input.GetMouseButtonDown(0))
        {
            if (attacktype == 0)
            {
                if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;
                else
                {
                    Collider2D hitenemy = Physics2D.OverlapCircle(attackpoint.position,
                    stabradius, 8); //1000 in binary so only layer 3 colliders are seen
                    if (hitenemy != null)
                    {
                        Enemy enemy = hitenemy.gameObject.GetComponent<Enemy>();
                        updateMana(enemy.getmana());
                        enemy.Die();

                    }
                    attackCooldown = TimeBtwAttacks;
                }
            }
            else if (attacktype == 1)
            {
                if (currentmana >= guncost && gun.isready())
                {
                    Instantiate(projectile, attackpoint.position, transform.rotation);
                    updateMana(-guncost);
                    gun.reset();
                }
            }
            else if (attacktype == 2)
            {
                if (currentmana >= burstcost && burst.isready())
                {
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(burstvisual.position, 4.35f, 8);
                    for (int i = 0; i < enemies.Length; ++i)
                    {
                        enemies[i].gameObject.gameObject.GetComponent<Enemy>().getstunned();
                    }
                    updateMana(-burstcost);
                    burst.reset();
                }
            }
            else
            {
                if (currentmana >= grapplecost && grapple.isready())
                {
                    updateMana(-grapplecost);
                    grapple.reset();
                }
            }
        }
        rb.MovePosition(rb.position + input_velocity * moveSpeed * Time.fixedDeltaTime);
        sr.flipX = mousepos.x < transform.position.x;
    }

    public void takeDamage(int dmg) 
    {   
        health -= dmg;
        HealthBar.sethealth(health);
        if (health <= 0) 
        {
            Color c = burstvisual.GetComponent<SpriteRenderer>().material.color;
            c.a = 0;
            burstvisual.GetComponent<SpriteRenderer>().material.color = c;
            c = attackpoint.GetComponent<SpriteRenderer>().material.color;
            c.a = 0;
            attackpoint.GetComponent<SpriteRenderer>().material.color = c;
            enabled = false;
            ChangeAnimationState(Player_die);
        }
    }

    public void gameover()
    {
        SceneManager.LoadScene(2);
        Destroy(gameObject);
    }

    private void updateMana(int amt)
    {
        ManaBar.setmana(currentmana = Mathf.Min(currentmana + amt, maxmana));
        gun.updatesprite(currentmana);
        burst.updatesprite(currentmana);
        grapple.updatesprite(currentmana);
    }
}
