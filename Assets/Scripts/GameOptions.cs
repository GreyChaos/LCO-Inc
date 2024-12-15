using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public static GameOptions instance;

    private float musicVolOption = 1f;
    private float effectVolOption = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void ChangeMusicVol(float musicVol)
    {
        musicVolOption = musicVol;
    }

    public float GetMusicVol()
    {
        return musicVolOption;
    }

    public void ChangeEffectVol(float effectVol)
    {
        effectVolOption = effectVol;
    }

    public float GetEffectVol()
    {
        return effectVolOption;
    }
}
