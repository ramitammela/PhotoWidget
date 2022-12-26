using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class CanvasExtensions
{
    public static Vector2 SizeToParent(this RawImage image, float padding = 0)
    {
        float w = 0, h = 0;
        var parent = image.GetComponentInParent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();

        if (image.texture != null)
        {
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;

            padding = 1 - padding;

            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);

            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
            {
                bounds.size = new Vector2(bounds.height, bounds.width); //Invert the bounds if the image is rotated
            }
            
            h = bounds.height * padding;    //Size by height first
            w = h * ratio;

            if (w > bounds.width * padding) //If it doesn't fit, fallback to width;
            { 
                w = bounds.width * padding;
                h = w / ratio;
            }
        }

        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        return imageTransform.sizeDelta;
    }
}