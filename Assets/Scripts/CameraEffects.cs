using UnityEngine;
using System.Collections;
public class CameraEffects : MonoBehaviour {
    public AnimationCurve curve;
    public float shakeDuration;
    public IEnumerator Shake() {
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
