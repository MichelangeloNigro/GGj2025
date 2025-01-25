using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Classe per la gestione dell'audio.
/// </summary>
public class SoundEngine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }
    // Update is called once per frame
    void Update() { }
    private static readonly Lazy<SoundEngine> _instance = new Lazy<SoundEngine>(() => new SoundEngine());
    /// <summary>
    /// Proprietà per l'istanza della classe.
    /// </summary>
    public static SoundEngine Instance => _instance.Value;
    private SoundEngine()
    {
        backgroundSource = gameObject.AddComponent<AudioSource>();
        clips = Resources.LoadAll<AudioClip>("Audio");
        BackgroundPlayer = new AudioPlayer(backgroundSource, clips, true);
    }
    private AudioSource backgroundSource { get; set; }
    private IEnumerable<AudioClip> clips { get; set; }
    /// <summary>
    /// Proprietà per la gestione dell'audio di background.
    /// </summary>
    public AudioPlayer BackgroundPlayer { get; set; }
    /// <summary>
    /// Metodo per la gestione dell'audio degli effetti sonori.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_loop"></param>
    /// <returns></returns>
    public AudioPlayer SFXPlayer(GameObject _item, bool _loop = false) => new AudioPlayer(_item.GetComponent<AudioSource>(), clips, _loop);

    /// <summary>
    /// Classe di supporto per la gestione dell'audio.
    /// </summary>
    public class AudioPlayer
    {
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
        public AudioPlayer(AudioSource source, IEnumerable<AudioClip> clips, bool loop = false)
        {
            _source = source;
            _source.loop = loop;
            _clips = clips ?? new List<AudioClip>();
        }
        /// <summary>
        /// Metodo per riprodurre un audio.
        /// </summary>
        /// <param name="_clipName"></param>
        public void Play(string _clipName)
        {
            var _clip = _clips.FirstOrDefault(e => e.name == _clipName);
            if (_clip != null && _source != null)
            {
                _source.clip = _clip;
                _source.Play();
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
            _source.Stop();
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
        /// Metodo per impostare il loop dell'audio.
        /// </summary>
        /// <param name="_loop"></param>
        public void SetLoop(bool _loop)
        {
            _source.loop = _loop;
        }
    }
}
