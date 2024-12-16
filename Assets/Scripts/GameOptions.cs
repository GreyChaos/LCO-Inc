using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public static GameOptions Instance { get; private set; }

    private float musicVolOption = 1f;
    private float effectVolOption = 1f;

    public Slider mainMusicVolSlider;
    public Slider mainEffectVolSlider;

    // Creates singelton instance and adds listener to detect audio volume slider changes.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Creates an event listener that, upon detecting a change in the slider value updates the music volume.
        mainMusicVolSlider.onValueChanged.AddListener(value => {ChangeMusicVol(value); }); 
        mainEffectVolSlider.onValueChanged.AddListener(value => {ChangeEffectVol(value); });
    }

    public void ChangeMusicVol(float musicVol)
    {
        musicVolOption = musicVol;
        BGMusicManager.Instance.ChangeBGMusicVolume();
    }

    public float GetMusicVol()
    {
        return musicVolOption;
    }

    public void ChangeEffectVol(float effectVol)
    {
        effectVolOption = effectVol;
        SoundEffectManager.Instance.ChangeSoundEffectVolume();
    }

    public float GetEffectVol()
    {
        return effectVolOption;
    }
}
