using System.Collections;
using UnityEngine;

public class StatusTile : MonoBehaviour {
    public int incrHP;
    public int incrMana;
    public Color activatedColor;
    public Color deactivatedColor;

    public void enable() {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player != null) {
            player.takeDamage(-incrHP);
            player.increaseMana(incrMana);
            GetComponent<SpriteRenderer>().color = deactivatedColor;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(deactivate());
            IEnumerator deactivate() {
                yield return new WaitForSeconds(1f);
                GetComponent<SpriteRenderer>().color = activatedColor;
                GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
