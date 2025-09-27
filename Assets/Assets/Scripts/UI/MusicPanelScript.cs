using UnityEngine;
using UnityEngine.UI;

public class MusicPanelScript : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void OnEnable()
    {
        if (GlobalVariables.Instance == null)
        {
            Debug.Log("GlobalVariables Instance is null in MusicPanelScript");
            masterSlider.value = 0.5f;
            musicSlider.value = 0.5f;
            sfxSlider.value = 0.5f;
        }
        else
        {
            masterSlider.value = GlobalVariables.Instance.masterVolume;
            musicSlider.value = GlobalVariables.Instance.musicVolume;
            sfxSlider.value = GlobalVariables.Instance.SFXVolume;
        }
    }

    public void SetMasterMusic()
    {
        GlobalVariables.Instance.masterVolume = masterSlider.value;
        AudioManager.Instance.SetMusicVolume();
    }
    public void SetMusicVolume()
    {
        GlobalVariables.Instance.musicVolume = musicSlider.value;
        AudioManager.Instance.SetMusicVolume();
    }
    public void SetSFXVolume()
    {
        GlobalVariables.Instance.SFXVolume = sfxSlider.value;
    }

}
