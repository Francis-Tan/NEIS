using System;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour {
    public event EventHandler OnPlayerEnterTrigger;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
