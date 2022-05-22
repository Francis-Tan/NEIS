using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy
{
    private Color suitcolor; //to change visibility
    public float moveSpeed = 10, appearSpeed = 0.7f; //toggle rate of movement and visibility
    public int dmg = 3;
    public int mana = 2;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        suitcolor = GetComponent<SpriteRenderer>().material.color;
        suitcolor.a = 0;
        GetComponent<SpriteRenderer>().material.color = suitcolor;
    }

    private void FixedUpdate() { //consider lateupdate if it could be useful
        suitcolor.a += appearSpeed * Time.fixedDeltaTime;
        GetComponent<SpriteRenderer>().material.color = suitcolor;
        MoveAndFacePlayer(moveSpeed);
    }

    protected override void attack() { player.GetComponent<Player>().takeDamage(dmg); }

    public override int Die() { 
        //Instantiate()
        Destroy(gameObject);
        return mana;
    }
}
