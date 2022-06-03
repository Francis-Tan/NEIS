using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour 
{
    // GetInstance must be called on start, otherwise that object may awake before player
    private static GameObject instance;
    private Rigidbody2D rb; //rigidbody movement better for collision
    public float moveSpeed = 13; //toggle rate of movement
    private Vector2 input_velocity;
    public int health = 10;
    private int currentmana;
    public int maxmana = 10;
    private bool attacking;
    private int attacktype;
    public Transform attackpoint;
    public float stabradius = 0.4f;
    public float TimeBtwAttacks = 0;
    private float attackCooldown;
    public int guncost = 2, burstcost = 4, grapplecost = 2;
    public GameObject projectile;

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
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;  
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
        animator = GetComponent<Animator>();
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
        attacking = Input.GetMouseButtonDown(0);
        //on pressing a skill's button, deactivate skill if we're using skill, else go to skill
        if (Input.GetKeyDown(KeyCode.Q))
        {
            attacktype = attacktype == 1 ? 0 : 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            attacktype = attacktype == 2 ? 0 : 2;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            attacktype = attacktype == 3 ? 0 : 3;
        }
    }

    private void FixedUpdate() 
    {
        ChangeAnimationState(input_velocity == Vector2.zero ? Player_idle : Player_move);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        if (attacking) 
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
                        currentmana += hitenemy.gameObject.GetComponent<Enemy>().Die();
                        if (currentmana > maxmana) currentmana = maxmana;
                        Debug.Log("mana = " + currentmana);
                    }
                    attackCooldown = TimeBtwAttacks;
                }
            }
            else if (attacktype == 1)
            {
                if (currentmana < guncost)
                {
                    Debug.Log("Need " + guncost + " mana to shoot");
                }
                else
                {
                    Instantiate(projectile, attackpoint.position, transform.rotation);
                    currentmana -= guncost;
                    Debug.Log("mana = " + currentmana);
                }
            }
            else if (attacktype == 2) 
            {
                //some usage of OverlapCircleAll
            }
            else 
            {
                
            }
        } 
        
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
        rb.MovePosition(rb.position + input_velocity * moveSpeed * Time.fixedDeltaTime);
    }
    public void takeDamage(int dmg) 
    {   
        health -= dmg;
        Debug.Log("Player health: " + health);
        if (health <= 0) 
        {
            ChangeAnimationState(Player_die);
            Destroy(gameObject);
        }
    }
}
