using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    public GameObject projectile;
    public int mana = 2;
    private int ammo = 3;
    private bool isdown = false;
    public float downtime;
    private float timetillup;
    private SpriteRenderer sr;
    private Color original_color;

    private void Awake() {
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
        sr = GetComponent<SpriteRenderer>(); 
        original_color = sr.material.color;
        timetillup = downtime;
    }
    private void FixedUpdate() {
        if (isdown) {
            timetillup -= Time.fixedDeltaTime;
            if (timetillup <= 0) {
                isdown = false;
                timetillup = downtime;
                sr.material.color = original_color;
                bc.enabled = false;
            }
        } else if (attackCooldown > 0) {    
            attackCooldown -= Time.fixedDeltaTime;
        } else if (ammo > 0) {
            attack();
            attackCooldown = TimeBtwAttacks;
            ammo--;
        } else {    
            isdown = true;
            attackCooldown = TimeBtwAttacks;
            ammo = 3;
            sr.material.color = Color.gray;
            bc.enabled = true;
        }
        float angle = Vector2.SignedAngle(Vector2.up, player.transform.position - transform.position);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    protected override void attack() { 
        Instantiate(projectile, player.transform.position, Quaternion.identity);
    }

    public override void takeDamage() { 
        if (isdown) base.takeDamage();
    }

    public override int Die() {
        if (isdown) {
            //Instantiate()
            Destroy(gameObject);
            return mana;
        }
        return 0;
    }
}
