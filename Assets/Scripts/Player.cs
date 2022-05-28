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
    private bool stab, gun, burst, grapple;
    public float stablength = 1;
    public float stabradius = 0.1f;
    public float TimeBtwAttacks = 0;
    private float attackCooldown;
    public int guncost = 2, burstcost = 4, grapplecost = 2;
    public GameObject projectile;
    private Transform hitArea; //forgot what this is for
    private void Awake() {	
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;
        hitArea = GetComponentInChildren<Transform>();
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
    }

    private void Update() {	
        input_velocity.x = Input.GetAxisRaw("Horizontal");
        input_velocity.y = Input.GetAxisRaw("Vertical");
        stab = Input.GetMouseButtonDown(0);
        gun = Input.GetKeyDown("q");
    }

    private void FixedUpdate() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        if (stab) { 
            if (attackCooldown > 0) { attackCooldown -= Time.fixedDeltaTime; }
            else {
                Collider2D hitenemy = Physics2D.OverlapCircle(rb.position + stablength * direction.normalized,
                stabradius, 8); //1000 in binary so refers to layer 3
                if (hitenemy != null) {
                    currentmana += hitenemy.gameObject.GetComponent<Enemy>().Die();
                    if (currentmana > maxmana) currentmana = maxmana;
                    Debug.Log("mana = " + currentmana);
                }
                attackCooldown = TimeBtwAttacks;
            }
        } 
        if (gun) {
            if (currentmana < guncost) Debug.Log("Need 2 mana to shoot");
            else {
                Instantiate(projectile, new Vector2(transform.position.x, transform.position.y) + direction.normalized, 
                transform.rotation)
                .GetComponent<Player_Bullet>().dir = direction.normalized;
                currentmana -= guncost;
                Debug.Log("mana = " + currentmana);
            }
        }
        if (burst) {} //some usage of OverlapCircleAll
        if (grapple) {}
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
