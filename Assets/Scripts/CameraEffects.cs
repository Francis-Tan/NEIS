using UnityEngine;
using System.Collections;
public class CameraEffects : MonoBehaviour {
    public AnimationCurve curve;
    public float shakeDuration;
    /**public float lookAheadFactor = 1f;
    private float camZ;

    private void Awake() {
        camZ = transform.position.z;
    }
    private void Update() {
        Vector3 player_pos = Player.GetInstance().transform.position;
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 new_pos = player_pos + (mouse_pos - player_pos).normalized * lookAheadFactor;
        new_pos.z = camZ;
        transform.position = new_pos;
    }*/
    public void Shake() {
        StartCoroutine(Shake());
        IEnumerator Shake() {
            Vector3 startPos = transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < shakeDuration) {
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / shakeDuration);
                Vector3 delta = Random.insideUnitCircle * strength;
                transform.position = startPos + delta;
                yield return null;
            }
            transform.position = startPos;
        }
    }
}
