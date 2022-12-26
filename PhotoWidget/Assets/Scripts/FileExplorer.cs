using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Crosstales.FB;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class FileExplorer : MonoBehaviour
{
    public GameObject photoPanel;

    string path;
    public RawImage rawImage;
    
    public RectTransform rectTransform; 

    public List<string> searchPatternList = new List<string>();
   
    private List<string> picturesInFolder = new List<string>();     //  Pictures of the folder (file names)
    private string randomPicturePath;

    [Header("Slideshow")] [Space(5, order = 1)]
    public Button timerButton;
    public GameObject timerPanel;
    public Button speedUpButton;
    public Button speedDownButton;
    private int speedSetting;
    private float speed;
    private bool slideshowOn;

    [Header("Monitor Snap")] [Space(5, order = 1)]
    public DragHandler windowDragScript;
    public Button monitorSnapButton;
    private int monitorSnap;

    [Header("Canvas")] [Space(5, order = 1)]
    public CanvasScaler canvasScaler;
    private int canvasScale;
    public TransparentWindow transparentWindow;
    public Scaler scalerScript;

    // System Message 
    private string messageText;
    private string titleText;


    void Start()
    {
        path = PlayerPrefs.GetString("FolderPath"); //  Load Saved Path

        foreach (string ext in searchPatternList)  //  Add files in path to list
        {
            foreach (string file in Directory.GetFiles(path, ext))
            {
                if (file != "")
                { 
                    picturesInFolder.Add(file);
                }
            }
        }

        SelectRandomPicture();

        // Canvas Scaling by resolution
        int cs = PlayerPrefs.GetInt("CanvasScaleSetting");
        if (cs == 0) { canvasScaler.scaleFactor = 1.5f; }
        if (cs == 1) { canvasScaler.scaleFactor = 2.0f; }
        if (cs == 2) { canvasScaler.scaleFactor = 2.5f; }

        #region Slideshow

        int i = PlayerPrefs.GetInt("SlideshowSetting");
        if (i == 0) { slideshowOn = false; } else { slideshowOn = true; }

        speedSetting = PlayerPrefs.GetInt("SpeedSetting");
        if (speedSetting == 0) { speed = 2.0f; }
        if (speedSetting == 1) { speed = 6.0f; }
        if (speedSetting == 2) { speed = 12.0f; }
        if (speedSetting == 3) { speed = 18.0f; }

        if (slideshowOn)
        {
            InvokeRepeating("SelectRandomPicture", speed, speed);
            timerButton.interactable = true;
            speedUpButton.interactable = true;
            speedDownButton.interactable = true;
        }
        else
        {
            SelectRandomPicture();

            timerButton.gameObject.SetActive(false);

            speedUpButton.interactable = false;
            speedDownButton.interactable = false;
            timerPanel.SetActive(false);
        }

        #endregion

        #region Monitor Snap

        monitorSnap = PlayerPrefs.GetInt("MonitorSnapSetting");
        if (monitorSnap == 0)
        {
            //monitorSnapButton.transform.GetComponent<Image>().color = Color.green;
            windowDragScript.enableMonitorSnap = true;
        }
        else
        {
            //monitorSnapButton.transform.GetComponent<Image>().color = Color.red;
            windowDragScript.enableMonitorSnap = false;
        }

        #endregion

        transparentWindow.SetClickthrough(true);
    }

    public void FileExplorerSelectFolder()
    {
        string path2 = FileBrowser.OpenSingleFolder();

        PlayerPrefs.SetString("FolderPath", path2);     //  Save selected folderpath
        PlayerPrefs.Save();

        path = path2;
        picturesInFolder.Clear();

        foreach (string ext in searchPatternList)
        {
            foreach (string file in Directory.GetFiles(path, ext))
            {
                picturesInFolder.Add(file);
            }
        }

        SelectRandomPicture();
    }

    public void SelectRandomPicture()
    {
        int randomNumber = Random.Range(0, picturesInFolder.Count);
        string randomPicture = picturesInFolder[randomNumber];

        if (picturesInFolder.Count > 0)
        {
            StartCoroutine(GetTexture(randomPicture));
        }
    }

    IEnumerator GetTexture(string random)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + random);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print("error: " + www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            rectTransform.SetLeft(0);
            rectTransform.SetRight(0);
            rectTransform.SetTop(0);
            rectTransform.SetBottom(0);

            rawImage.texture = myTexture;
            rawImage.SizeToParent();
        }
    }

    public void ScaleButton(float i)
    {
        canvasScaler.scaleFactor = i;

        photoPanel.SetActive(false);

        if (i == 1.5f) { transparentWindow.SetClickthrough(true); messageText = "Scale set at 1.0"; titleText = "Scale Setting"; PlayerPrefs.SetInt("CanvasScaleSetting", 0); }
        if (i == 2.0f) { transparentWindow.SetClickthrough(true); messageText = "Scale set at 1.5"; titleText = "Scale Setting"; PlayerPrefs.SetInt("CanvasScaleSetting", 1); }
        if (i == 2.5f) { transparentWindow.SetClickthrough(true); messageText = "Scale set at 2.0"; titleText = "Scale Setting"; PlayerPrefs.SetInt("CanvasScaleSetting", 2); }

        PlayerPrefs.Save();
        Invoke("Message", 0.05f);
        Invoke("ShowPanel", 1.0f);
    }

    public void MonitorSnapButton()
    {
        photoPanel.SetActive(false);

        if (monitorSnap == 0)
        {
            monitorSnap = 1;
            transparentWindow.SetClickthrough(true); messageText = "Monitor Snap disabled"; titleText = "Monitor Snap Setting";
            //monitorSnapButton.transform.GetComponent<Image>().color = Color.red;
            windowDragScript.enableMonitorSnap = false;
            PlayerPrefs.SetInt("MonitorSnapSetting", 1);
            PlayerPrefs.Save();
        }
        else
        {
            monitorSnap = 0;
            transparentWindow.SetClickthrough(true); messageText = "Monitor Snap enabled"; titleText = "Monitor Snap Setting";
            //monitorSnapButton.transform.GetComponent<Image>().color = Color.green;
            windowDragScript.enableMonitorSnap = true;
            PlayerPrefs.SetInt("MonitorSnapSetting", 0);
            PlayerPrefs.Save();
        }

        Invoke("Message", 0.05f);
        Invoke("Reload", 1.0f);
    }

    #region Slideshow

    public void SlideshowButton()
    {
        photoPanel.SetActive(false);

        slideshowOn = !!slideshowOn;
        transparentWindow.windowOpen = true;

        if (slideshowOn)
        {
            timerButton.interactable = false;
            PlayerPrefs.SetInt("SlideshowSetting", 0);
            transparentWindow.SetClickthrough(true); messageText = "Slideshow Disabled"; titleText = "Slideshow Setting";
        }
        else
        {
            timerButton.interactable = true;
            PlayerPrefs.SetInt("SlideshowSetting", 1);
            transparentWindow.SetClickthrough(true); messageText = "Slideshow Enabled"; titleText = "Slideshow Setting";
        }

        PlayerPrefs.Save();
        Invoke("Message", 0.05f);
        Invoke("Reload", 1.0f);
    }

    public void TimerButton() 
    {
        photoPanel.SetActive(false);
        speedSetting++;

        if (speedSetting == 4)
        {
            speedSetting = 0;
        }

        transparentWindow.windowOpen = true;

        if (speedSetting == 0) { speed = 2f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 2s"; titleText = "Speed Setting"; }
        if (speedSetting == 1) { speed = 6f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 6s"; titleText = "Speed Setting"; }
        if (speedSetting == 2) { speed = 12f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 12s"; titleText = "Speed Setting"; }
        if (speedSetting == 3) { speed = 18f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 18s"; titleText = "Speed Setting"; }

        PlayerPrefs.SetInt("SpeedSetting", speedSetting);
        PlayerPrefs.Save();

        Invoke("Message", 0.05f);
        Invoke("Reload", 1.0f);
    }

    public void SpeedUpButton()
    {
        photoPanel.SetActive(false);

        speedSetting++;
        if (speedSetting >= 3) { speedSetting = 3; }

        transparentWindow.windowOpen = true;

        if (speedSetting == 0) { speed = 2f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 2s"; titleText = "Speed Setting"; }
        if (speedSetting == 1) { speed = 6f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 6s"; titleText = "Speed Setting"; }
        if (speedSetting == 2) { speed = 12f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 12s"; titleText = "Speed Setting"; }
        if (speedSetting == 3) { speed = 18f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 18s"; titleText = "Speed Setting"; }

        PlayerPrefs.SetInt("SpeedSetting", speedSetting);
        PlayerPrefs.Save();

        Invoke("Message", 0.05f);
        Invoke("Reload", 1.0f);
    }

    public void SpeedDownButton()
    {
        photoPanel.SetActive(false);

        speedSetting--;
        if (speedSetting <= 0) { speedSetting = 0; }

        transparentWindow.windowOpen = true;

        if (speedSetting == 0) { speed = 2f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 2s"; titleText = "Speed Setting"; }
        if (speedSetting == 1) { speed = 6f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 6s"; titleText = "Speed Setting"; }
        if (speedSetting == 2) { speed = 12f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 12s"; titleText = "Speed Setting"; }
        if (speedSetting == 3) { speed = 18f; transparentWindow.SetClickthrough(true); messageText = "Speed setting changed to 18s"; titleText = "Speed Setting"; }

        PlayerPrefs.SetInt("SpeedSetting", speedSetting);
        PlayerPrefs.Save();

        Invoke("Message", 0.05f);
        Invoke("Reload", 1.0f);
    }

    #endregion

    #region FPS-Settings

    public void FPSButton30()   // 0 = 60fps (default), 1 = 30fps
    {
        photoPanel.SetActive(false);

        PlayerPrefs.SetInt("FrameRateSetting",1);
        transparentWindow.SetClickthrough(true); messageText = "Framerate set at 30"; titleText = "Framerate Setting";

        PlayerPrefs.Save();
        Invoke("Message", 0.05f);
        Invoke("ShowPanel", 1.0f);
    }
    public void FPSButton60()   // 0 = 60fps (default), 1 = 30fps
    {
        photoPanel.SetActive(false);

        PlayerPrefs.SetInt("FrameRateSetting", 0);
        transparentWindow.SetClickthrough(true); messageText = "Framerate set at 60"; titleText = "Framerate Setting";

        PlayerPrefs.Save();
        Invoke("Message", 0.05f);
        Invoke("ShowPanel", 1.0f);
    }

    #endregion


    public void Message() { transparentWindow.Message(messageText, titleText); }

    public void ShowPanel() 
    {   
        photoPanel.gameObject.SetActive(true); photoPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -116.6465f);
        scalerScript.ResetSize();
    }

    public void Reload() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

    public void CloseButton()
    {
        photoPanel.SetActive(false);
        Screen.SetResolution(350, 150, false);
        Application.Quit();
    }
}