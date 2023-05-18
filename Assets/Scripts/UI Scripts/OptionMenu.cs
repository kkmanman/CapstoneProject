using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    //Volume variables
    [Header("Volume variables")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;

    //Fullscreen variables
    [Header("Fullscreen variables")]
    [SerializeField] private Toggle fullscreenToggle;

    //Resolution variables for UI scale
    [Header("Resolution variables")]
    [SerializeField] private Transform mainMenuTransform;
    [SerializeField] private Transform optionMenuTransform;
    private const float minResolutionWidth = 1152f;
    private const float minResolutionHeight = 864f;    
    private const float uiScaleSlopeValue = 1280f;
    private UIFlexibleTransformer optionMenuFlexibleTransformer = new UIFlexibleTransformer(240f, 150f);
    private UIFlexibleTransformer mainMenuFlexibleTransformer = new UIFlexibleTransformer(110f, 70f);

    //Resolution variables for UI dropdown
    [SerializeField] private Dropdown resolutionDropdown;    
    private Resolution[] resolutions;
    private int currentResolutionIndex;    

    //Graphics variables
    [Header("Graphic Quality variables")]
    [SerializeField] private Dropdown graphicsDropdown;

    private void Start()
    {
        ResolutionDropdownInit();
        FullscreenInit();
        GraphicsDropdownInit();
        MasterVolumeSliderInit();
    }

    #region initializationMethods
    //Initialize the fullscreen toggle
    private void FullscreenInit()
    {
        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetString("fullscreen", "true");
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }
        else
        {
            SetFullscreenFromInit();
        }
    }

    //Set the preferred display mode from PlayerPrefs
    private void SetFullscreenFromInit()
    {
        if (PlayerPrefs.GetString("fullscreen") == "true")
        {
            //Fullscreen Mode
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }
        else
        {
            //Windowed Mode
            Screen.fullScreen = false;
            fullscreenToggle.isOn = false;
        }
    }

    //Initialize the graphics dropdown options
    private void GraphicsDropdownInit()
    {
        if (!PlayerPrefs.HasKey("graphicQuality"))
        {
            PlayerPrefs.SetInt("graphicQuality", QualitySettings.GetQualityLevel());
            graphicsDropdown.value = QualitySettings.GetQualityLevel();
            graphicsDropdown.RefreshShownValue();
        }
        else
        {
            graphicsDropdown.value = PlayerPrefs.GetInt("graphicQuality");
            graphicsDropdown.RefreshShownValue();
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("graphicQuality"));
        }        
    }

    //Initialize the resolution dropdown options
    private void ResolutionDropdownInit()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> resolutionsOptions = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " (" + resolutions[i].refreshRate + " Hz)";
            resolutionsOptions.Add(option);
            CheckScreenResolution(i);
        }
        resolutionDropdown.AddOptions(resolutionsOptions);

        if (PlayerPrefs.HasKey("resolution"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("resolution");
            resolutionDropdown.RefreshShownValue();

            Screen.SetResolution(resolutions[PlayerPrefs.GetInt("resolution")].width,
                resolutions[PlayerPrefs.GetInt("resolution")].height, Screen.fullScreen);
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            PlayerPrefs.SetInt("resolution", currentResolutionIndex);
        }

        CheckCurrentResolutionForUIScale(resolutions[PlayerPrefs.GetInt("resolution")]);
    }

    //Check if the resolution is too small for displaying all UI elements
    private void CheckCurrentResolutionForUIScale(Resolution resolution)
    {        
        if (resolution.width < minResolutionWidth && resolution.height < minResolutionHeight)
        {
            float finalizedScale = resolution.width / uiScaleSlopeValue;

            mainMenuTransform.localScale = new Vector2(finalizedScale, finalizedScale);
            optionMenuTransform.localScale = new Vector2(finalizedScale, finalizedScale);

            mainMenuTransform.localPosition = new Vector2(mainMenuTransform.localPosition.x, mainMenuFlexibleTransformer.UiPosYChangedValue);
            optionMenuTransform.localPosition = new Vector2(optionMenuTransform.localPosition.x, optionMenuFlexibleTransformer.UiPosYChangedValue);
        }
        else
        {
            mainMenuTransform.localScale = new Vector2(1f, 1f);
            optionMenuTransform.localScale = new Vector2(1f, 1f);

            mainMenuTransform.localPosition = new Vector2(mainMenuTransform.localPosition.x, mainMenuFlexibleTransformer.UiPosYOriginalValue);
            optionMenuTransform.localPosition = new Vector2(optionMenuTransform.localPosition.x, optionMenuFlexibleTransformer.UiPosYOriginalValue);
        }
    }

    //Check whether the current resolution matches the target resolution in the current index of the array list
    //when PlayerPrefs have not saved the resolution preference
    private void CheckScreenResolution(int i)
    {
        if (!PlayerPrefs.HasKey("resolution"))
        {
            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
    }

    //Initialize the master volume slider
    private void MasterVolumeSliderInit()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", -25);
            audioMixer.SetFloat("volume", Mathf.Log10(PlayerPrefs.GetFloat("volume") * 20));
            masterVolumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            audioMixer.SetFloat("volume", Mathf.Log10(PlayerPrefs.GetFloat("volume")) * 20);
            masterVolumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }
    #endregion

    #region settingMethods
    //Set the master volume of the game
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    //Set the overall graphics quality of the game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("graphicQuality", qualityIndex);
    }

    //Set the display mode between windowed mode and fullscreen mode
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if (isFullscreen)
        {
            PlayerPrefs.SetString("fullscreen", "true");
        }
        else
        {
            PlayerPrefs.SetString("fullscreen", "false");
        }
    }

    //Set the resolution of the game screen
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);

        CheckCurrentResolutionForUIScale(resolutions[resolutionIndex]);
    }
    #endregion
}
