using UnityEngine;
using UnityEngine.SceneManagement;

public class MonitorChange : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(350, 150, false);

        if (Screen.currentResolution.width <= 2559)   // Less than 2K monitor/resolution
        {
            PlayerPrefs.SetInt("CanvasScaleSetting", 0);
        }
        if (Screen.currentResolution.width >= 2560)  // Equal or Greater than 2K monitor/resolution
        {
            PlayerPrefs.SetInt("CanvasScaleSetting", 1);
        }
        if (Screen.currentResolution.width >= 3840)  // Equal or Greater than 4K monitor/resolution
        {
            PlayerPrefs.SetInt("CanvasScaleSetting", 2);
        }
        PlayerPrefs.Save();

        StartApp();
    }

    public void DefaultResolution()
    {
        Screen.SetResolution(350, 150, false);
    }

    public void StartApp ()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        SceneManager.LoadScene("Scene");
    }
}