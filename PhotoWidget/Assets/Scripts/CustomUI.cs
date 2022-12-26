using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CustomUI : MonoBehaviour
{

    void Start()
    {

        #region folder

        if (!System.IO.Directory.Exists(Application.dataPath + "/../" + "CustomUI/"))   // PhotoWidget/CustomUI/
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/../CustomUI");
        }

        #endregion

        #region load images

        //Main buttons
        LoadImage("UI_Close.png", "Button_Close");
        LoadImage("UI_Folder.png", "Button_Folder");
        LoadImage("UI_Minimalize.png", "Button_Mini");
        LoadImage("UI_Effect.png", "Button_Effect");
        LoadImage("UI_Next.png", "Button_Next");
        LoadImage("UI_AutoPlay.png", "Button_AutoPlay");
        LoadImage("UI_Timer.png", "Button_Timer");
        LoadImage("UI_Monitor.png", "Button_Monitor");

        //Frames
        LoadImage("UI_Frame.png", "Frame_Timer");
        LoadImage("UI_Frame.png", "Frame_Effect");
        LoadImage("UI_Frame.png", "Frame_Monitor");

        //Buttons inside the frames
        LoadImage("UI_Timer.png", "Button_Timer_On-Frame");
        LoadImage("UI_Effect.png", "Button_Effect_On-Frame");
        LoadImage("UI_Monitor.png", "Button_Monitor_On-Frame");

        //Monitor change
        LoadImage("UI_MonitorChangeLeft.png", "Button_Left_Monitor");
        LoadImage("UI_MonitorChangeRight.png", "Button_Right_Monitor");

        //30 & 60
        LoadImage("UI_Minus.png", "Button_Speed_Down");
        LoadImage("UI_Plus.png", "Button_Speed_Up");

        //Scale 1-3
        LoadImage("UI_Scale1.png", "Button_Scale_1");
        LoadImage("UI_Scale2.png", "Button_Scale_2");
        LoadImage("UI_Scale3.png", "Button_Scale_3");

        //30 & 60
        LoadImage("UI_30.png", "Button_30");
        LoadImage("UI_60.png", "Button_60");

        //Filters
        LoadImage("UI_Filter_Normal.png", "Button_Effect_1");
        LoadImage("UI_Filter_Sepia.png", "Button_Effect_2");
        LoadImage("UI_Filter_BW.png", "Button_Effect_3");
        LoadImage("UI_Filter_Rainbow.png", "Button_Effect_4");
        LoadImage("UI_Filter_RedBW.png", "Button_Effect_5");
        LoadImage("UI_Filter_Warm.png", "Button_Effect_6");
        LoadImage("UI_Filter_Pink.png", "Button_Effect_7");
        LoadImage("UI_Filter_Blue.png", "Button_Effect_8");

        #endregion

    }

    public void LoadImage(string fileName, string name)
    {
        var texPath = Path.GetFullPath(Application.dataPath + "/../" + "CustomUI/" + fileName);

        if (System.IO.File.Exists(texPath))
        {
            var bytes = System.IO.File.ReadAllBytes(texPath);
            var tex = new Texture2D(1, 1);
            tex.LoadImage(bytes);
            Sprite newSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.0f, 0.0f), 100.0f);

            GameObject target = GameObject.Find(name);

            if (target) 
            { 
                target.GetComponent<Image>().sprite = newSprite;
            }
        }
    }

}