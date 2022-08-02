using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {
    // GetInstance must be called on start, otherwise the caller may awake before player
    public bool canDie;
    public int skillLevel; // 0: dagger, 1: += gun, 2: += stun
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
    public Skill_Icon gun, burst;
    private int guncost, burstcost;
    public GameObject projectile;
    private Dagger dagger;
    private GunVisual gunvisual;
    private BurstVisual burstvisual;

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
        if (skillLevel > 0) DontDestroyOnLoad(instance);
        sr = GetComponent<SpriteRenderer>();
        renderers = GetComponentsInChildren<Renderer>();
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
        animator = GetComponent<Animator>();
        guncost = gun.skillcost;
        burstcost = burst.skillcost;
        dagger = GetComponentInChildren<Dagger>();
        gunvisual = GetComponentInChildren<GunVisual>();
        Color c = gunvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        gunvisual.GetComponent<SpriteRenderer>().material.color = c;
        burstvisual = GetComponentInChildren<BurstVisual>();
        c = burstvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        burstvisual.GetComponent<SpriteRenderer>().material.color = c;
        if (skillLevel == 0) {
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
        UpdateSkills();
        UpdateVisuals(mousepos);
        if (Input.GetMouseButtonDown(0)) Attack();
    }

    private void UpdateSkills() {
        //if we're not using that skill and we press its button, equip it
        //if we press that button again, go back to dagger
        if (Input.GetMouseButtonDown(1)) {
            if (skillLevel >= 1) gun.pressed(false);
            if (skillLevel >= 2) burst.pressed(false);
            attacktype = 0;
        } else if (skillLevel >= 1 && Input.GetKeyDown(KeyCode.Q)) {
            if (attacktype == 1) {
                gun.pressed(false);
                attacktype = 0;
            } else {
                gun.pressed(true);
                if (skillLevel >= 2) burst.pressed(false);
                attacktype = 1;
            }
        } else if (skillLevel >= 2 && Input.GetKeyDown(KeyCode.E)) {
            if (attacktype == 2) {
                burst.pressed(false);
                attacktype = 0;
            } else {
                gun.pressed(false);
                burst.pressed(true);
                attacktype = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) increaseMana(maxmana - currentmana);
        if (Input.GetKeyDown(KeyCode.G)) Die();
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
                //the death animation play under player_idle state
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

        c = burstvisual.GetComponent<SpriteRenderer>().material.color;
        c.a = attacktype == 2 ? 1 : 0;
        burstvisual.GetComponent<SpriteRenderer>().material.color = c;
        burstvisual.transform.rotation = Quaternion.identity;
        if (attacktype == 2) burstvisual.transform.position = mousepos;
    }
    private void Attack() {
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
           } else if (attacktype == 1) {
               if (currentmana >= guncost && gun.isready()) {
                   gunvisual.PlayShootAnimation();
                   AudioManager.instance.PlaySound(Sound.player_gunfire);
                   Instantiate(projectile, attackpoint.position, Quaternion.identity);
                   increaseMana(-guncost);
                   gun.reset();
               } else {
                   AudioManager.instance.PlaySound(Sound.player_noammo);
               }
           } else {
               if (currentmana >= burstcost && burst.isready()) {
                   increaseMana(-burstcost);
                   burstvisual.PlayAttackAnimation();
                   AudioManager.instance.PlaySound(Sound.player_burst);
                   StartCoroutine(Camera.main.GetComponent<CameraEffects>().Shake());
                   Collider2D[] enemies = Physics2D.OverlapCircleAll(burstvisual.transform.position, 4.35f, 8);
                   for (int i = 0; i < enemies.Length; ++i) {
                       enemies[i].gameObject.GetComponent<Enemy>().becomestunned();
                   }
                   burst.reset();
               }
           }
    }

    public void takeDamage(int dmg) {
        if (health > 0) { //prevent assassins from playing death sound multiple times
            health = canDie ? health - dmg : Mathf.Max(health - dmg, 1);
            if (health > 50) health = 50;
            HealthBar.sethealth(health);
            if (health <= 0) Die();
        }
    }

    public void increaseMana(int amt) {
        ManaBar.instance.updateBars(currentmana = Mathf.Min(currentmana + amt, maxmana));
        burstvisual.updateSprite(currentmana);
    }

    /**public void switchToDagger() {
        gun.pressed(false);
        burst.pressed(false);
        attacktype = 0;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        UpdateVisuals(mousepos);
    }*/

    private void Die() {
        AudioManager.instance.PlaySound(Sound.player_die);
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        StartCoroutine(DeathScene());
        IEnumerator DeathScene() {
            ChangeAnimationState(Player_die);
            yield return new WaitForSeconds(0.6f);
            LoadLevelSelect();
        }
    }

    public void LoadLevelSelect() {
        EnableAllPlayerVisuals(false);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void Spawn(Vector2 pos, int health, int mana) {
        GetComponent<Collider2D>().enabled = true;
        rb.velocity = Vector2.zero;
        transform.position = pos;
        HealthBar.sethealth(this.health = health);
        ManaBar.instance.updateBars(currentmana = mana);
        gun.Initialize(); burst.Initialize();
        EnableAllPlayerVisuals(true);
        enabled = true;
    }

    private void EnableAllPlayerVisuals(bool enable) {
        sr.enabled = enable;
        dagger.GetComponent<SpriteRenderer>().enabled = enable;
        gunvisual.GetComponent<SpriteRenderer>().enabled = enable;
        burstvisual.GetComponent<SpriteRenderer>().enabled = enable;
        PlayerInfoCanvas.instance.GetComponent<Canvas>().enabled = enable;
    }
}
