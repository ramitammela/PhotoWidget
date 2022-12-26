using UnityEngine;
using UnityEngine.UI;

public class Scaler : MonoBehaviour
{
    public RectTransform rectTransform;
    //public FileExplorer fe;

    public RectTransform rawImage_rt;
    public RawImage rawImage_img;

    public float scaleAmount;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f) // Scale Up
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + 10, rectTransform.sizeDelta.y + scaleAmount);
            rawImage_rt.SetLeft(0);
            rawImage_rt.SetRight(0);
            rawImage_rt.SetTop(0);
            rawImage_rt.SetBottom(0);
            rawImage_img.SizeToParent();
        }

        else if (Input.GetAxis("Mouse ScrollWheel") <= -0.1f && rectTransform.sizeDelta.x > 200) // Scale Down
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - 10, rectTransform.sizeDelta.y - scaleAmount);
            rawImage_rt.SetLeft(0);
            rawImage_rt.SetRight(0);
            rawImage_rt.SetTop(0);
            rawImage_rt.SetBottom(0);
            rawImage_img.SizeToParent();
        }
    }

    public void ResetSize ()
    {
        rectTransform.sizeDelta = new Vector2(200f, 200f); // 200x200
        rawImage_rt.SetLeft(0);
        rawImage_rt.SetRight(0);
        rawImage_rt.SetTop(0);
        rawImage_rt.SetBottom(0);
        rawImage_img.SizeToParent();
    }
}