using UnityEngine;
public enum Sound {
    //player_stab,
    player_gunfire,
    player_noammo,
    bullet_hitwall,
    player_bullet_hitenemy,
    player_burst,
    player_die,
    gunner_shoot,
    //gunner_bullet_hitplayer,
    //gunner_reload ?
    gunner_die,
    assassin_disappear,
    assassin_appear,
    assassin_stab,
    assassin_die,
    //drone_aura,
    //explosion_hitplayer,
    drone_deactivate,
    drone_activate,
    drone_die,
    enter_checkpoint
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioPlayer[] soundEffects;
    public bool inTutorial;

    [System.Serializable]
    public class AudioPlayer {
        public Sound sound;
        public AudioClip clip;
        [HideInInspector]
        public AudioSource speaker;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(.1f, 3f)]
        public float pitch = 1f;

        public void Initialize() {
            //sound = (Sound) Sound.Parse(typeof(Sound), clip.name);
            speaker = instance.gameObject.AddComponent<AudioSource>();
            Reset();
        }

        public void Reset() {
            speaker.clip = clip;
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
        if (!inTutorial) DontDestroyOnLoad(gameObject);
        foreach (AudioPlayer ap in soundEffects) ap.Initialize();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            foreach (AudioPlayer ap in soundEffects) ap.Reset();
        }
    }

    public void PlaySound(Sound sound) {
        soundEffects[(int)sound].PlayOnce();
    }
}
