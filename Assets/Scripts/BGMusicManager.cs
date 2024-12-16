using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BGMusicManager : MonoBehaviour
{
    // Makes a background music manager singleton. More or less a copy of the sound effect script, changed slightly so it continues looping.
    public static BGMusicManager Instance;

    [SerializeField] private AudioSource BGMusicObject;

    AudioSource audioSource;
    private float musicInputVolume;

    private void Awake()
    {
        // Something to do with making this a singleton idk I watched a tutorial.
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Essentially copies sound effect script, except removes positioning based off input object and doesn't destroy the audioSource.
    public void PlayAudioClip(AudioClip audioClip, float volume)
    {
        // If Background music object currently exists, destroy it.
        if (GameObject.Find("BGMusicObject"))
            Destroy(audioSource.gameObject);
        
        // Instantiate new background music object of input audo clip.
        audioSource = Instantiate(BGMusicObject, Vector3.zero, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume * GameOptions.Instance.GetMusicVol();

        musicInputVolume = volume;

        audioSource.Play();
    }
    public void ChangeBGMusicVolume()
    {
        audioSource.volume = musicInputVolume * GameOptions.Instance.GetMusicVol();
    }
}
