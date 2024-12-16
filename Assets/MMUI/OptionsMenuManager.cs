using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [SerializeField] private Button lowQualityButton;
    [SerializeField] private Button mediumQualityButton;
    [SerializeField] private Button highQualityButton;

    [SerializeField] private Color selectedColor = Color.white; // Highlighted color
    [SerializeField] private Color unselectedColor = Color.gray; // Dark color for unselected buttons


    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int defaultQuality = 2;
        QualitySettings.SetQualityLevel(defaultQuality, true);
        UpdateButtonColors(defaultQuality);

        lowQualityButton.onClick.AddListener(() => SetGraphicsQuality(0));
        mediumQualityButton.onClick.AddListener(() => SetGraphicsQuality(1));
        highQualityButton.onClick.AddListener(() => SetGraphicsQuality(2));

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);

        UpdateButtonColors(qualityIndex);
    }

    private void UpdateButtonColors(int selectedQuality)
    {
        lowQualityButton.GetComponent<Image>().color = selectedQuality == 0 ? selectedColor : unselectedColor;
        mediumQualityButton.GetComponent<Image>().color = selectedQuality == 1 ? selectedColor : unselectedColor;
        highQualityButton.GetComponent<Image>().color = selectedQuality == 2 ? selectedColor : unselectedColor;
    }


    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetFulllscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
