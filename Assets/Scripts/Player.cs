using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
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

    private void Awake() {	
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;  
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
        animator = GetComponent<Animator>();
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void Update() {	
        input_velocity.x = Input.GetAxisRaw("Horizontal");
        input_velocity.y = Input.GetAxisRaw("Vertical");
        attacking = Input.GetMouseButtonDown(0);
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (attacktype == 1) {
                //turn all buttons off
                attacktype = 0;
            } else {
                //turn gun button on and the rest off
                attacktype = 1;
            }
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            if (attacktype == 2)
            {
                //turn all buttons off
                attacktype = 0;
            }
            else
            {
                //turn burst button on and the rest off
                attacktype = 2;
            }
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            if (attacktype == 3)
            {
                //turn all buttons off
                attacktype = 0;
            }
            else
            {
                //turn grapple button on and the rest off
                attacktype = 3;
            }
        }
    }

    private void FixedUpdate() {
        ChangeAnimationState(input_velocity == Vector2.zero ? Player_idle : Player_move);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        if (attacking) {
            if (attacktype == 0)
            {
                if (attackCooldown > 0) { attackCooldown -= Time.fixedDeltaTime; }
                else
                {
                    Collider2D hitenemy = Physics2D.OverlapCircle(attackpoint.position,
                    stabradius, 8); //1000 in binary so refers to layer 3
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
                if (currentmana < guncost) Debug.Log("Need 2 mana to shoot");
                else
                {
                    Instantiate(projectile, attackpoint.position, transform.rotation)
                    .GetComponent<Player_Bullet>().dir = direction.normalized;
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
    public void takeDamage(int dmg) {   
        health -= dmg;
        Debug.Log("Player health: " + health);
        if (health <= 0) {  
            Destroy(gameObject);
        }
    }
}
