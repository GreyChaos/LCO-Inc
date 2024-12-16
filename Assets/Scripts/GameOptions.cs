using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public static GameOptions Instance { get; private set; }

    private float musicVolOption = 1f;
    private float effectVolOption = 1f;

    public Slider musicVolSlider;
    public Slider effectVolSlider;

    // Creates singelton instance and adds listener to detect audio volume slider changes.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadOptions();
        
        // Creates an event listener that, upon detecting a change in the slider value updates the music volume.
        musicVolSlider.onValueChanged.AddListener(value => {ChangeMusicVol(value); }); 
        effectVolSlider.onValueChanged.AddListener(value => {ChangeEffectVol(value); });
    }

    public void ChangeMusicVol(float musicVol)
    {
        musicVolOption = musicVol;
        BGMusicManager.Instance.ChangeBGMusicVolume();

        SaveOptions();
    }

    public void ChangeEffectVol(float effectVol)
    {
        effectVolOption = effectVol;
        SoundEffectManager.Instance.ChangeSoundEffectVolume();

        SaveOptions();
    }

    public void UpdateSliderPosition()
    {
        musicVolSlider.value = musicVolOption;
        effectVolSlider.value = effectVolOption;
    }

    public float GetMusicVol()
    {
        return musicVolOption;
    }

    public float GetEffectVol()
    {
        return effectVolOption;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolOption);
        PlayerPrefs.SetFloat("EffectsVolume", effectVolOption);
    }

    public void LoadOptions()
    {
        musicVolOption = PlayerPrefs.GetFloat("MusicVolume");
        effectVolOption = PlayerPrefs.GetFloat("effectVolOption");
    }
}
