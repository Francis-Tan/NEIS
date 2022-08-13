using UnityEngine;
using UnityEngine.Audio;

public enum Sound {
    BGM_MainMenu,
    BGM_MainLevels,
    player_stab,
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

public class AudioManager : MonoBehaviour {
    //current AudioPlayer system is in 1st iteration for new menu
    public static AudioManager instance;
    public VolumeSlider BGMSlider, SFXSlider;
    public AudioMixerGroup BGMMixer;
    public AudioMixerGroup SFXMixer;
    public AudioPlayer[] BGM;
    private AudioPlayer currentBGM;
    public AudioPlayer[] soundEffects;

    [System.Serializable]
    public class AudioPlayer {
        public Sound sound;
        public AudioClip clip;
        [HideInInspector]
        private AudioSource speaker;
        private bool playing;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(.1f, 3f)]
        public float pitch = 1f;

        public void Initialize(AudioMixerGroup AudioMixer) {
            //sound = (Sound) Sound.Parse(typeof(Sound), clip.name);
            speaker = instance.gameObject.AddComponent<AudioSource>();
            speaker.outputAudioMixerGroup = AudioMixer;
            ResetSpeakerSettings();
        }

        public void ResetSpeakerSettings() {
            speaker.clip = clip;
            speaker.volume = volume;
            speaker.pitch = pitch;
        }

        public bool isPlaying() {
            return playing;
        }

        public void PlayOnce() {
            playing = true;
            speaker.PlayOneShot(clip);
        }

        public void PlayOnLoop() {
            speaker.loop = true;
            playing = true;
            speaker.Play();
        }

        public void StopPlaying() {
            speaker.loop = false;
            playing = false;
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
        foreach (AudioPlayer ap in BGM) ap.Initialize(BGMMixer);
        foreach (AudioPlayer ap in soundEffects) ap.Initialize(SFXMixer);
        BGMSlider.Initialize();
        SFXSlider.Initialize();
        currentBGM = BGM[(int)Sound.BGM_MainMenu];
        PlayBGM(Sound.BGM_MainMenu);
    }

    /**
    private void Update() {
        //for adjusting audio during development
        if (Input.GetKeyDown(KeyCode.Z)) {
            ResetAudio();
        }
    }
    

    private void ResetAudio() {
        foreach (AudioPlayer ap in BGM) {
            ap.ResetSpeakerSettings();
        }
        foreach (AudioPlayer ap in soundEffects) {
            ap.ResetSpeakerSettings();
        }
    }
    */

    public void PlayBGM(Sound sound) {
        AudioPlayer bgm = BGM[(int)sound];
        if (!bgm.isPlaying()) {
            currentBGM.StopPlaying();
            currentBGM = bgm;
            bgm.PlayOnLoop();
        }
    }
    public void PlaySound(Sound sound) {
        soundEffects[(int)sound - 2].PlayOnce();
    }
}
