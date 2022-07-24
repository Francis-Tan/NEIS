using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {
    // GetInstance must be called on start, otherwise the caller may awake before player
    public bool inTutorial;
    public int skillLevel; //0 means only dagger, 1 means gun, 2 means all
    private static GameObject instance;
    private SpriteRenderer sr;
    public static Renderer[] renderers;
    private Rigidbody2D rb; //rigidbody movement better for collision
    public float moveSpeed = 23;
    private Vector2 input_velocity;
    public int health = 10;
    public int currentmana;
    public int maxmana = 10;
    private int attacktype;
    public Transform attackpoint;
    public float stabradius = 0.4f;
    public float TimeBtwAttacks = 0;
    private float attackCooldown;
    public Skill_Icon gun, burst;// grapple;
    private int guncost, burstcost;// grapplecost;
    public GameObject projectile;
    private Dagger dagger;
    private GunVisual gunvisual;
    public Transform burstvisual;

    private Animator animator;
    private string currentState;
    const string
        Player_idle = "Player_idle",
        Player_moveLR = "Player_moveLR",
        Player_moveU = "Player_moveU",
        Player_moveD = "Player_moveD",
        Player_stab = "Player_stab",
        Player_hit = "Player_hit",
        Player_die = "Player_die",
        Player_gun = "Player_gun",
        Player_burst = "Player_burst",
        Player_grapple = "Player_grapple";

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject); return;
        }
        instance = gameObject;
        if (!inTutorial) DontDestroyOnLoad(instance);
        sr = GetComponent<SpriteRenderer>();
        renderers = GetComponentsInChildren<Renderer>();
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
        animator = GetComponent<Animator>();
        guncost = gun.skillcost;
        burstcost = burst.skillcost;
        //grapplecost = grapple.skillcost;
        dagger = GetComponentInChildren<Dagger>();
        gunvisual = GetComponentInChildren<GunVisual>();
        Color c = gunvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        gunvisual.GetComponent<SpriteRenderer>().material.color = c;
        c = burstvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        burstvisual.GetComponent<SpriteRenderer>().material.color = c;
        if (inTutorial) {
            gun = null;
            burst = null;
        }
    }

    public static GameObject GetInstance() {
        return instance;
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        //Debug.Log(newState);
        animator.Play(newState);
        currentState = newState;
    }
    private void Update() {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane; //to make burstvisual be in the right z-pos
        input_velocity.x = Input.GetAxisRaw("Horizontal");
        input_velocity.y = Input.GetAxisRaw("Vertical");
        attackpoint.RotateAround(transform.position, Vector3.forward,
            Vector2.SignedAngle(attackpoint.position - transform.position, mousepos - transform.position));
        rb.MovePosition(rb.position + input_velocity * (moveSpeed * Time.fixedDeltaTime));
        UpdateSkillsandAttacktype();
        UpdateVisuals(mousepos);
        Attack();
    }

    private void UpdateSkillsandAttacktype() {
        //on pressing a skill's button, deactivate skill if we're using skill, else go to skill
        if (Input.GetMouseButtonDown(1)) {
            if (skillLevel >= 1) gun.pressed(false);
            if (skillLevel >= 2) burst.pressed(false);
            //grapple.pressed(false);
            attacktype = 0;
        }
        else if (skillLevel >= 1 && Input.GetKeyDown(KeyCode.Q)) {
            if (attacktype == 1) {
                gun.pressed(false);
                attacktype = 0;
            }
            else {
                gun.pressed(true);
                if (skillLevel == 2) burst.pressed(false);
                //grapple.pressed(false);
                attacktype = 1;
            }
        }
        else if (skillLevel >= 2 && Input.GetKeyDown(KeyCode.E)) {
            if (attacktype == 2) {
                burst.pressed(false);
                attacktype = 0;
            }
            else {
                gun.pressed(false);
                burst.pressed(true);
                //grapple.pressed(false);
                attacktype = 2;
            }
        }
        /**else if (Input.GetKeyDown(KeyCode.R)) {
            if (attacktype == 3) {
                grapple.pressed(false);
                attacktype = 0;
            }
            else {
                gun.pressed(false);
                burst.pressed(false);
                grapple.pressed(true);
                attacktype = 3;
            }
        }*/
        if (Input.GetKeyDown(KeyCode.F)) increaseMana(maxmana - currentmana);
        if (Input.GetKeyDown(KeyCode.G)) takeDamage(health);
    }
    private void UpdateVisuals(Vector3 mousepos) {
        sr.flipX = mousepos.x < transform.position.x;
        dagger.GetComponent<SpriteRenderer>().flipY = sr.flipX;
        gunvisual.GetComponent<SpriteRenderer>().flipY = sr.flipX;
        if (input_velocity.x == 0) {
            if (input_velocity.y > 0) {
                ChangeAnimationState(Player_moveU);
            }
            else if (input_velocity.y < 0) {
                ChangeAnimationState(Player_moveD);
            }
            else if (enabled) {
                //check is to prevent overloading animator during death and making
                //the death animation fall under player_idle
                ChangeAnimationState(Player_idle);
            }
        }
        else {
            ChangeAnimationState(Player_moveLR);
        }

        Color c = dagger.GetComponent<SpriteRenderer>().material.color;
        c.a = attacktype == 0 ? 1 : 0;
        dagger.GetComponent<SpriteRenderer>().material.color = c;

        c = gunvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = attacktype == 1 ? 1 : 0;
        gunvisual.GetComponent<SpriteRenderer>().material.color = c;

        c = currentmana < burstcost ? Color.blue : Color.white;
        if (attacktype == 2) {
            c.a = 1;
            burstvisual.position = mousepos;
        }
        else {
            c.a = 0;
        }
        burstvisual.GetComponent<SpriteRenderer>().material.color = c;
    }
    private void Attack() {
        if (Input.GetMouseButtonDown(0)) {
            if (attacktype == 0) {
                if (attackCooldown > 0) attackCooldown -= Time.fixedDeltaTime;
                else {
                    StartCoroutine(attack());
                    IEnumerator attack() {
                        //could use dagger.transform.localPosition which should be marginally faster
                        //but local scaling shortens stab length
                        attackpoint.transform.position += (attackpoint.transform.position - transform.position).normalized;
                        yield return new WaitForSeconds(0.1f);
                        attackpoint.transform.position -= (attackpoint.transform.position - transform.position).normalized;
                    }
                    Collider2D hitenemy = Physics2D.OverlapCircle(attackpoint.position,
                    stabradius, 8); //8 in binary so only sees layer 3 colliders
                    if (hitenemy != null) {
                        dagger.PlayHitAnimation();
                        Enemy enemy = hitenemy.gameObject.GetComponent<Enemy>();
                        increaseMana(enemy.getmana());
                        enemy.Death();
                    }
                    attackCooldown = TimeBtwAttacks;
                }
            }
            else if (attacktype == 1) {
                if (currentmana >= guncost && gun.isready()) {
                    gunvisual.PlayShootAnimation();
                    AudioManager.instance.PlaySound(Sound.player_gunfire);
                    Instantiate(projectile, attackpoint.position, Quaternion.identity);
                    increaseMana(-guncost);
                    gun.reset();
                } else {
                    AudioManager.instance.PlaySound(Sound.player_noammo);
                }
            }
            else if (attacktype == 2) {
                if (currentmana >= burstcost && burst.isready()) {
                    AudioManager.instance.PlaySound(Sound.player_burst);
                    StartCoroutine(Camera.main.GetComponent<CameraEffects>().Shake());
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(burstvisual.position, 4.35f, 8);
                    for (int i = 0; i < enemies.Length; ++i) {
                        enemies[i].gameObject.GetComponent<Enemy>().becomestunned();
                    }
                    increaseMana(-burstcost);
                    burst.reset();
                }
            }
            /**else {
                if (currentmana >= grapplecost && grapple.isready()) {
                    updateMana(-grapplecost);
                    grapple.reset();
                }
            }*/
        }
    }

    public void takeDamage(int dmg) {
        if (health > 0) { //assassins can cause death sound to play multiple times
            health = inTutorial ? Mathf.Max(health - dmg, 1) : health - dmg;
            if (health > 50) health = 50;
            HealthBar.sethealth(health);
            if (health <= 0) {
                AudioManager.instance.PlaySound(Sound.player_die);
                enabled = false;
                GetComponent<Collider2D>().enabled = false;
                rb.velocity = Vector2.zero;
                ChangeAnimationState(Player_die);
                StartCoroutine(waitForGameOver());
                IEnumerator waitForGameOver() {
                    yield return new WaitForSeconds(0.575f);
                    gameover();
                }
            }
        }
    }

    public void increaseMana(int amt) {
        ManaBar.instance.updateBars(currentmana = Mathf.Min(currentmana + amt, maxmana));
    }

    /**public void switchToDagger() {
        gun.pressed(false);
        burst.pressed(false);
        attacktype = 0;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        UpdateVisuals(mousepos);
    }*/

    public void gameover() {
        sr.enabled = false;
        dagger.GetComponent<SpriteRenderer>().enabled = false;
        gunvisual.GetComponent<SpriteRenderer>().enabled = false;
        burstvisual.GetComponent<SpriteRenderer>().enabled = false;
        PlayerInfoCanvas.instance.GetComponent<Canvas>().enabled = false;
        SceneManager.LoadScene("GameOver");
    }

    public void spawn(Vector2 pos, int health, int mana) {
        GetComponent<Collider2D>().enabled = true;
        rb.velocity = Vector2.zero;
        transform.position = pos;
        HealthBar.sethealth(this.health = health);
        ManaBar.instance.updateBars(currentmana = mana);
        gun.initialize(); burst.initialize();
        sr.enabled = true;
        dagger.GetComponent<SpriteRenderer>().enabled = true;
        gunvisual.GetComponent<SpriteRenderer>().enabled = true;
        burstvisual.GetComponent<SpriteRenderer>().enabled = true;
        enabled = true;
    }
}
