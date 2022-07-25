using UnityEngine;

public class Gunner_Bullet : MonoBehaviour {
    private Rigidbody2D rb; //rigidbody movement is to avoid moving in a rotated transform after rotation
    private Vector2 dir = Vector2.down, newdir;
    public float moveSpeed = 16;
    public int dmg = 1;
    public bool destroy = false;

    private Animator animator;
    private string currentState;
    const string
        EB_move = "EB_move",
        EB_explode = "EB_explode";

    private void Awake() {
        resetDirection();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeAnimationState(EB_move);
    }
    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    public void resetDirection() {
        newdir = moveSpeed * (Player.GetInstance().transform.position - transform.position).normalized;
        transform.RotateAround(transform.position, Vector3.forward,
            Vector2.SignedAngle(dir, newdir));
        dir = newdir;
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //would be more efficient to check layers
        GameObject runinto = other.gameObject;
        if (runinto.CompareTag("Player")) {
            ChangeAnimationState(EB_explode);
            runinto.GetComponent<Player>().takeDamage(dmg);
        }
        else if (runinto.CompareTag("Blocking")) {
            ChangeAnimationState(EB_explode);
            AudioManager.instance.PlaySound(Sound.bullet_hitwall);
        }
    }

    //called by EB_explode
    private void deactivate() {
        ChangeAnimationState(EB_move); //events don't stop animator
        if (destroy) {
            Destroy(gameObject);
        } else {
            gameObject.SetActive(false);
        }
    }
}
