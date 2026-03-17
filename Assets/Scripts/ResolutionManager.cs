using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResolutionManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;

    private Resolution[] sortedResolutions;
    private List<string> resolutionOptions = new List<string>();
    private int currentResolutionIndex = 0;

    void Start()
    {
        
        sortedResolutions = Screen.resolutions
            .OrderBy(r => r.width)
            .ThenBy(r => r.height)
            .ToArray();

        
        ResolutionDropdown.ClearOptions();

        
        for (int i = 0; i < sortedResolutions.Length; i++)
        {
            Resolution res = sortedResolutions[i];
            string option = res.width + "x" + res.height;
            resolutionOptions.Add(option);

           
            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        
        ResolutionDropdown.AddOptions(resolutionOptions);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

     
        if (FullscreenToggle != null)
        {
            FullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    
    public void SetResolution(int resolutionIndex)
    {
       
        Resolution res = sortedResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}