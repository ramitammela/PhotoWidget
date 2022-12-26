using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChangePostProcessProfile : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private PostProcessProfile _postProcessProfile;

    public void ChangeProfile()
    {
        _postProcessVolume.profile = _postProcessProfile;
    }
}