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
    public float stablengthfactor = 1;
    public float stabradius = 0.1f;
    private Transform hitArea;
    public float TimeBtwAttacks = 0;
    private float attackCooldown;
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
        attacking = Input.GetMouseButtonDown(0);
    }

    private void FixedUpdate() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
        if (attacking) {
            if (attackCooldown > 0) { attackCooldown -= Time.fixedDeltaTime; }
            else {
                Collider2D hitenemy = Physics2D.OverlapCircle(rb.position + stablengthfactor * direction.normalized,
                stabradius, 8); //1000 in binary so refers to layer 3
                if (hitenemy != null) {
                    currentmana += hitenemy.gameObject.GetComponent<Enemy>().Die();
                    if (currentmana > maxmana) currentmana = maxmana;
                    Debug.Log("mana = " + currentmana);
                }
                attackCooldown = TimeBtwAttacks;
            }
        }
        //burst shd use OverlapCircleAll
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
