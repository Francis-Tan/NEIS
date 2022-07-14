using UnityEngine;
public enum Sound {
    gunner_shoot,
    player_gunfire,
    bullet_hitwall,
    player_bullet_hitenemy,
    assassin_stab,
    drone_die,
    enter_checkpoint
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public SFX[] soundEffects;

    [System.Serializable]
    public class SFX {
        public Sound sound;
        public AudioClip clip;
        [HideInInspector]
        public AudioSource speaker;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(.1f, 3f)]
        public float pitch = 1f;

        public void initialize() {
            speaker = instance.gameObject.AddComponent<AudioSource>();
            resetSpeaker();
        }

        public void resetSpeaker() { 
            speaker.volume = volume;
            speaker.pitch = pitch;
        }

        public void PlayOnce() {
            speaker.PlayOneShot(clip);
        }
    }
    
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (SFX s in soundEffects) s.initialize();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            foreach (SFX s in soundEffects) s.resetSpeaker();
        }
    }

    public void PlaySound(Sound sound) {
        soundEffects[(int)sound].PlayOnce();
    }
}
