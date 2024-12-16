using UnityEngine;
using UnityEngine.Animations;

public class SeasonManager : MonoBehaviour
{
    // Allows attaching audio sources for seasonal background music in the inspector.
    [SerializeField] private AudioClip springMusic;
    [SerializeField] private AudioClip summerMusic;
    [SerializeField] private AudioClip fallMusic;
    [SerializeField] private AudioClip winterMusic;

    // Sets the season at game launch.
    void Start()
    {
        UpdateSeason(TimeManager.getMonth());
    }
    // Called by the TimeManager during month updates if the new month is divisible by 3.
    public void UpdateSeason(int inputMonth)
    {
        if (inputMonth >= 3 && inputMonth < 6)
            ChangeToSpring();

        if (inputMonth >= 6 && inputMonth < 9)
            ChangeToSummer();

        if (inputMonth >= 9 && inputMonth < 12)
            ChangeToFall();

        if (inputMonth == 12 || inputMonth < 3)
            ChangeToWinter();
    }

    // Updates game to Spring.
    private void ChangeToSpring()
    {
        BGMusicManager.Instance.PlayAudioClip(springMusic, .25f);
    }

    // Updates game to Summer.
    private void ChangeToSummer()
    {
        BGMusicManager.Instance.PlayAudioClip(summerMusic, .25f);
    }

    // Updates game to Fall.
    private void ChangeToFall()
    {
        BGMusicManager.Instance.PlayAudioClip(fallMusic, .25f);
    }

    // Updates game to Winter.
    private void ChangeToWinter()
    {
        BGMusicManager.Instance.PlayAudioClip(winterMusic, .25f);
    }
}
