using UnityEngine;

public class Player_Bullet : MonoBehaviour {
    private Rigidbody2D rb;
    private Vector2 dir;
    public float moveSpeed = 20;

    private Animator animator;
    private string currentState;
    const string
        pb_start = "pb_start",
        pb_fly = "pb_fly",
        pb_explode = "pb_explode";

    private void Start() {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 tpos = transform.position;
        Vector2 playerpos = Player.GetInstance().transform.position;
        dir = (mousepos - playerpos).magnitude > 1.69 ? moveSpeed * (mousepos - tpos).normalized : moveSpeed * (tpos - mousepos).normalized;
        transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, dir));
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeAnimationState(pb_start);
        ChangeAnimationState(pb_fly);
    }
    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void FixedUpdate() {
        rb.position += Time.fixedDeltaTime * dir;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (enabled) {
            GameObject runinto = other.gameObject;
            if (runinto.CompareTag("Enemy")) {
                enabled = false;
                runinto.GetComponent<Enemy>().takeDamage();
                AudioManager.instance.PlaySound(Sound.player_bullet_hitenemy);
                ChangeAnimationState(pb_explode);
            }
            else if (runinto.CompareTag("Blocking")) {
                enabled = false;
                AudioManager.instance.PlaySound(Sound.bullet_hitwall);
                ChangeAnimationState(pb_explode);
            }
        }
    }

    //called by pb_explode
    private void explode() {
        Destroy(gameObject);
    }
}