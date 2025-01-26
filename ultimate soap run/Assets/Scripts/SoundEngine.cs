using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Classe per la gestione dell'audio.
/// </summary>
public class SoundEngine : MonoBehaviour
{
    const string BackgroudKey = "OST";
    /// <summary>
    /// Lista delle clip del gioco
    /// </summary>
    private IEnumerable<AudioClip> clips { get; set; }
    /// <summary>
    /// Collector delle OST
    /// </summary>
    private List<AudioSource> OSTSources { get; set; } = new List<AudioSource>();

    /// <summary>
    /// Costruttore privato.
    /// </summary>
    private SoundEngine() { }

    void Awake()
    {
        clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (var clip in clips.Where(c => c.name.StartsWith(BackgroudKey)))
        {
            SetupOST(clip);
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (AudioSource source in OSTSources) { source.Play(); }
    }
    // Update is called once per frame
    void Update() { }

    /// <summary>
    /// Metodo per la riproduzione dell'audio di background.
    /// </summary>
    public void PlayOST(string clipName)
    {
        StopOST();
        var clip = OSTSources.FirstOrDefault(c => c.name.Replace("OST_", "").ToLower() == clipName.ToLower());
        if (clip != null) clip.volume = 1f; else Debug.Log("Impossibile riprodurre l'audio di background " + clipName + ". L'audio non esiste.");
    }
    /// <summary>
    /// Metodo per interrompere la riproduzione dell'audio di background.
    /// </summary>
    public void StopOST()
    {
        var playing = OSTSources.FirstOrDefault(c => c.volume == 1f);
        if (playing != null) playing.volume = 0f; else Debug.Log("Nessun audio di background in riproduzione.");
    }
    /// <summary>
    /// Metodo per la gestione dell'audio degli effetti sonori.
    /// </summary>
    /// <param name="_item"></param>
    /// <returns></returns>
    public AudioPlayer SFXPlayer(GameObject _item) => new AudioPlayer(_item.GetComponent<AudioSource>(), clips);

    /// <summary>
    /// Classe di supporto per la gestione dell'audio.

    /// <summary>
    /// Imposta il Game Object come audio source per l'ost
    /// </summary>
    /// <param name="clip"></param>
    private void SetupOST(AudioClip clip)
    {
        var obj = new GameObject();
        obj.name = clip.name.ToUpper();
        obj.transform.parent = transform;
        var source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = 0f;
        OSTSources.Add(source);
    }

    public static SoundEngine Instance
    {
        get
        {
            return FindObjectOfType(typeof(SoundEngine)).GetComponent<SoundEngine>();
        }
    }

    /// </summary>
    public class AudioPlayer
    {
        /// <summary>
        /// Nome dell'audio.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Modalità di riproduzione dell'audio.
        /// </summary>
        private PlayerMode mode { get; set; } = PlayerMode.standard;
        /// <summary>
        /// AudioSource che riproduce l'audio.
        /// </summary>
        private AudioSource _source { get; set; }
        /// <summary>
        /// Lista di AudioClip che possono essere riprodotti.
        /// </summary>
        private IEnumerable<AudioClip> _clips { get; set; }
        /// <summary>
        /// Costruttore con parametri.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="clips"></param>
        public AudioPlayer(AudioSource source, IEnumerable<AudioClip> clips, string name = "")
        {
            this.name = name;
            _source = source;
            _clips = clips ?? new List<AudioClip>();
        }
        /// <summary>
        /// Metodo per riprodurre un audio.
        /// </summary>
        /// <param name="_clipName"></param>
        public void Play(string _clipName = null)
        {
            var _clip = _clipName == null ? _clips.First() : _clips.FirstOrDefault(e => e.name == _clipName);
            if (_clip != null && _source != null)
            {
                _source.clip = _clip;
                switch (mode)
                {
                    case PlayerMode.volume:
                        SetVolume(1f);
                        break;
                    default:
                        _source.Play();
                        break;
                }
            }
            else
            {
                Debug.Log("Impossibile riprodurre l'audio " + _clipName + ". L'audio non esiste.");
            }
        }
        /// <summary>
        /// Metodo per fermare l'audio.
        /// </summary>
        public void Stop()
        {
            switch (mode)
            {
                case PlayerMode.volume:
                    SetVolume(0f);
                    break;
                default:
                    _source.Stop();
                    break;
            }
        }
        /// <summary>
        /// Metodo per mettere in pausa l'audio.
        /// </summary>
        public void Pause()
        {
            _source.Pause();
        }
        /// <summary>
        /// Metodo per riprendere l'audio.
        /// </summary>
        public void Resume()
        {
            _source.UnPause();
        }

        /// <summary>
        /// Metodo per impostare l'audio.
        /// </summary>
        /// <param name="_volume"></param>
        public void SetVolume(float _volume)
        {
            _source.volume = _volume;
        }
        /// <summary>
        /// Metodo per impostare il pitch dell'audio.
        /// </summary>
        /// <param name="_pitch"></param>
        public void SetPitch(float _pitch)
        {
            _source.pitch = _pitch;
        }
        /// <summary>
        /// Metodo per impostare la modalità di riproduzione dell'audio.
        /// </summary>
        /// <param name="mode"></param>
        internal void setMode(PlayerMode mode)
        {
            this.mode = mode;
        }
    }

    public enum PlayerMode
    {
        standard = 0,
        volume = 1,
    }
}
