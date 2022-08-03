using UnityEngine;
public enum Sound {
    BGM_MainMenu,
    BGM_MainLevels,
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
    //current AudioPlayer system is in 1st iteration for new menu
    public static AudioManager instance;
    public AudioPlayer[] BGM;
    public AudioPlayer[] soundEffects;
    private AudioPlayer currentBGM;

    [System.Serializable]
    public class AudioPlayer {
        public Sound sound;
        public AudioClip clip;
        [HideInInspector]
        private AudioSource speaker;
        public bool isPlaying;
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

        public void PlayOnLoop() {
            isPlaying = true;
            speaker.loop = true;
            speaker.Play();
        }

        public void StopPlaying() {
            isPlaying = false;
            speaker.loop = false;
            speaker.Stop();
        }
    }
    
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (AudioPlayer ap in BGM) ap.Initialize();
        foreach (AudioPlayer ap in soundEffects) ap.Initialize();
        currentBGM = BGM[(int)Sound.BGM_MainMenu];
        PlayBGM(Sound.BGM_MainMenu);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            foreach (AudioPlayer ap in BGM) ap.Reset();
            foreach (AudioPlayer ap in soundEffects) ap.Reset();
        }
    }
    public void PlayBGM(Sound sound) {
        AudioPlayer bgm = BGM[(int)sound];
        if (!bgm.isPlaying) {
            currentBGM.StopPlaying();
            bgm.PlayOnLoop();
            currentBGM = bgm;
        }
    }
    public void PlaySound(Sound sound) {
        soundEffects[(int)sound - 2].PlayOnce();
    }
}
