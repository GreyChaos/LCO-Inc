using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundEffectManager : MonoBehaviour
{
    // Makes the script a singleton. Only one instance and can be called by SoundEffectManager.instance.method() in any other script.
    public static SoundEffectManager Instance;

    [SerializeField] private AudioSource SoundEffectObject;

    AudioSource audioSource;

    private float effectInputVolume;

    private void Awake()
    {
        // Something to do with making this a singleton idk I watched a tutorial.
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // I won't lie I got all of this from a Sasquatch B Studios tutorial on youtube but I can't think of a better way to do it.
    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        audioSource = Instantiate(SoundEffectObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume * GameOptions.Instance.GetEffectVol();

        effectInputVolume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void ChangeSoundEffectVolume()
    {
        if (audioSource == null)
            return;
        else
            audioSource.volume = effectInputVolume * GameOptions.Instance.GetEffectVol();
    }
}
