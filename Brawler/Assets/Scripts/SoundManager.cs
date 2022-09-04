using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource, _backgroundSource;
    [SerializeField] private AudioClip[] _audioClips;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        EventManager.Instance.OnStartGame += PlaySound;
        EventManager.Instance.OnRestartGame += PlayBackgroundMusic;
        EventManager.Instance.OnBoxFell += PlaySound;
        EventManager.Instance.OnLoseGame += PlaySound;
        EventManager.Instance.OnWinGame += PlaySound;
        EventManager.Instance.OnStopGame += StopBackgroundSound;
    }

    public void PlaySound(EventManager.OnStartEventArgs args)
    {
        _backgroundSource.Play();
    }

    public void PlaySound(EventManager.OnRestartGameEventArgs args)
    {
        if (!args.Clip) return;
        _musicSource.PlayOneShot(args.Clip);
    }

    public void PlaySound(EventManager.OnBoxFellEventArgs args)
    {
        if (!args.Clip) return;
        _musicSource.PlayOneShot(args.Clip);
    }

    public void PlaySound(EventManager.OnLoseGameEventArgs args)
    {
        if (!args.Clip) return;
        _musicSource.PlayOneShot(args.Clip);
    }

    public void PlaySound(EventManager.OnWinGameEventArgs args)
    {
        if (!args.Clip) return;
        _musicSource.PlayOneShot(args.Clip);
    }

    public void PlayBackgroundMusic(EventManager.OnRestartGameEventArgs args)
    {
        _backgroundSource.Play();
    }

    public void StopBackgroundSound()
    {
        _backgroundSource.Stop();
    }

    public AudioClip FindClip(string title)
    {
        foreach (AudioClip clip in _audioClips)
        {
            if (clip.name == title)
            {
                return clip;
            }
        }

        return null;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnStartGame -= PlaySound;
        EventManager.Instance.OnRestartGame -= PlaySound;
        EventManager.Instance.OnBoxFell -= PlaySound;
        EventManager.Instance.OnLoseGame -= PlaySound;
        EventManager.Instance.OnWinGame -= PlaySound;
        EventManager.Instance.OnStopGame -= StopBackgroundSound;
    }
}
