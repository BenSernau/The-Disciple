using UnityEngine;
using UnityEngine.SceneManagement;

//from Asbjorn Thirslund's 2D Tutorial Series...

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float volumeRand = 0.1f;
    [Range(0f, 0.5f)]
    public float pitchRand = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-volumeRand / 2, volumeRand / 2));
        source.pitch = pitch * (1 + Random.Range(-pitchRand / 2, pitchRand / 2));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
public class AudioMaster : MonoBehaviour
{
    public static AudioMaster instance;

    [SerializeField]
    Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void StopAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            StopSound(sounds[i].name);
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // otherwise, no sound with name...
        Debug.LogWarning("AUDIO MASTER: no sound with name found in list, " + _name);
    }
}

