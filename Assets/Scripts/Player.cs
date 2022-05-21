using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb; //rigidbody movement better for collision
    public float moveSpeed; //toggle rate of movement
    private Vector2 input_velocity;
    public int health;
    private void Awake() {	
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;
    }

    private void Update() {	
        input_velocity.x = Input.GetAxisRaw("Horizontal");
        input_velocity.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + input_velocity * moveSpeed * Time.fixedDeltaTime);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
    public void takeDamage(int dmg) {   
        health -= dmg;
        Debug.Log("Player health: " + health);
        if (health <= 0) {  
            Destroy(gameObject);
        }
    }
}
