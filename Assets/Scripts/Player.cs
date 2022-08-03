using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour {
    // GetInstance must be called on start, otherwise the caller may awake before player
    public bool canDie;
    public int skillLevel; // 0: dagger, 1: += gunIcon, 2: += stun
    private static GameObject instance;
    private static SpriteRenderer sr;
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
    public Skill_Icon gunIcon, stunIcon;
    private int gunCost, stunCost;
    public GameObject projectile;
    private static Dagger dagger;
    private static GunVisual gunVisual;
    private static StunVisual stunVisual;

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

    private void Start() { 
        //start to ensure it retrieves icons from playerinfo, though could set in inspector
        if (instance != null) {
            Destroy(gameObject); return;
        }
        instance = gameObject; 
        CheckPointManager.UpdateCheckpoint(1, 50, 0);

        DontDestroyOnLoad(instance);
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        input_velocity = Vector2.zero;
        attackCooldown = TimeBtwAttacks;
        currentmana = 0;
        animator = GetComponent<Animator>();

        gunIcon = PlayerInfo.instance.gunIcon;
        stunIcon = PlayerInfo.instance.stunIcon;
        gunCost = gunIcon.skillcost;
        stunCost = stunIcon.skillcost;
        dagger = GetComponentInChildren<Dagger>();
        gunVisual = GetComponentInChildren<GunVisual>();
        Color c = gunVisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        gunVisual.GetComponent<SpriteRenderer>().material.color = c;
        stunVisual = GetComponentInChildren<StunVisual>();
        c = stunVisual.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        stunVisual.GetComponent<SpriteRenderer>().material.color = c;
        gameObject.SetActive(false);
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
        mousepos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane; //to make stunVisual be in the right z-pos
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
            if (skillLevel >= 1) gunIcon.pressed(false);
            if (skillLevel >= 2) stunIcon.pressed(false);
            attacktype = 0;
        } else if (skillLevel >= 1 && Input.GetKeyDown(KeyCode.Q)) {
            if (attacktype == 1) {
                gunIcon.pressed(false);
                attacktype = 0;
            } else {
                gunIcon.pressed(true);
                if (skillLevel >= 2) stunIcon.pressed(false);
                attacktype = 1;
            }
        } else if (skillLevel >= 2 && Input.GetKeyDown(KeyCode.E)) {
            if (attacktype == 2) {
                stunIcon.pressed(false);
                attacktype = 0;
            } else {
                gunIcon.pressed(false);
                stunIcon.pressed(true);
                attacktype = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) increaseMana(maxmana - currentmana);
        if (Input.GetKeyDown(KeyCode.G)) Die();
    }
    private void UpdateVisuals(Vector3 mousepos) {
        sr.flipX = mousepos.x < transform.position.x;
        dagger.GetComponent<SpriteRenderer>().flipY = sr.flipX;
        gunVisual.GetComponent<SpriteRenderer>().flipY = sr.flipX;
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

        c = gunVisual.GetComponent<SpriteRenderer>().material.color;
        c.a = attacktype == 1 ? 1 : 0;
        gunVisual.GetComponent<SpriteRenderer>().material.color = c;

        c = stunVisual.GetComponent<SpriteRenderer>().material.color;
        c.a = attacktype == 2 ? 1 : 0;
        stunVisual.GetComponent<SpriteRenderer>().material.color = c;
        stunVisual.transform.rotation = Quaternion.identity;
        stunVisual.transform.position = mousepos;
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
                   Collider2D enemyCollider = Physics2D.OverlapCircle(attackpoint.position,
                   stabradius, 8); //8 in binary so only sees layer 3 colliders
                   if (enemyCollider != null) {
                       dagger.PlayHitAnimation();
                       Enemy enemy = enemyCollider.GetComponent<Enemy>();
                       increaseMana(enemy.getMana());
                       enemy.Death();
                   }
                   attackCooldown = TimeBtwAttacks;
               }
           } else if (attacktype == 1) {
               if (currentmana >= gunCost && gunIcon.isready()) {
                   gunVisual.PlayShootAnimation();
                   AudioManager.instance.PlaySound(Sound.player_gunfire);
                   Instantiate(projectile, attackpoint.position, Quaternion.identity);
                   increaseMana(-gunCost);
                   gunIcon.reset();
               } else {
                   AudioManager.instance.PlaySound(Sound.player_noammo);
               }
           } else {
               if (currentmana >= stunCost && stunIcon.isready()) {
                   increaseMana(-stunCost);
                   stunVisual.PlayAttackAnimation();
                   AudioManager.instance.PlaySound(Sound.player_burst);
                   Camera.main.GetComponent<CameraEffects>().Shake();
                   Collider2D[] enemies = Physics2D.OverlapCircleAll(stunVisual.transform.position, 4.35f, 8);
                   for (int i = 0; i < enemies.Length; ++i) {
                       enemies[i].GetComponent<Enemy>().getStunned();
                   }
                   stunIcon.reset();
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
        stunVisual.updateSprite(currentmana);
    }

    private void Die() {
        AudioManager.instance.PlaySound(Sound.player_die);
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        StartCoroutine(DeathScene());
        IEnumerator DeathScene() {
            ChangeAnimationState(Player_die);
            yield return new WaitForSeconds(0.6f);
            SceneMethods.GoToLevelSelect();
        }
    }

    public void Spawn(Vector2 pos, int health, int mana, int skillLevel, bool canDie = true) {
        SetVisible(true);
        GetComponent<Collider2D>().enabled = true;
        rb.velocity = Vector2.zero;
        transform.position = pos;
        HealthBar.sethealth(this.health = health);
        increaseMana(mana - currentmana);
        this.skillLevel = skillLevel;
        this.canDie = canDie;

        //equip dagger
        gunIcon.Initialize(); stunIcon.Initialize();
        gunIcon.pressed(false);
        stunIcon.pressed(false);
        attacktype = 0; 

        enabled = true;
    }

    public static void SetVisible(bool visible) {
        instance.SetActive(visible);
    }
}
