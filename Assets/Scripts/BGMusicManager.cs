using Unity.VisualScripting;
using UnityEngine;

public class BGMusicManager : MonoBehaviour
{
    // Makes a background music manager singleton. More or less a copy of the sound effect script, changed slightly so it continues looping.
    public static BGMusicManager instance;

    [SerializeField] private AudioSource BGMusicObject;

    private void Start()
    {
        // Something to do with making this a singleton idk I watched a tutorial.
        if (instance == null)
        {
            instance = this;
        }
    }

    // Essentially copies sound effect script, except removes positioning based off input object and doesn't destroy the audioSource.
    public void PlayAudioClip(AudioClip audioClip, float volume)
    {
        // If Background music object currently exists, destroy it.
        if (BGMusicObject != null)
            GameObject.Destroy(BGMusicObject);
        
        // Instantiate new background music object of input audo clip.
        AudioSource audioSource = Instantiate(BGMusicObject, Vector3.zero, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume * GameOptions.instance.GetMusicVol();

        audioSource.Play();
    }
}
