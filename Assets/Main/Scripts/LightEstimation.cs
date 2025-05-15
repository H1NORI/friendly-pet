using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// A Unity component for using ARFoundation's Light Estimation feature to modify 
/// the main light in our scene
/// </summary>
public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the ARCameraManager")]
    private ARCameraManager aRCameraManager;

    [SerializeField]
    [Tooltip("The light that we want to modify based on light estimation data")]
    private Light mainLight;

    private void OnEnable()
    {
        aRCameraManager.frameReceived += OnFrameReceived;
    }

    private void OnDisable()
    {
        aRCameraManager.frameReceived -= OnFrameReceived;
    }

    private void OnFrameReceived(ARCameraFrameEventArgs args)
    {
        var lightData = args.lightEstimation;
        if (lightData.averageMainLightBrightness.HasValue)
        {
            mainLight.intensity = lightData.averageMainLightBrightness.Value * 1.3f;
        }
        if (lightData.mainLightColor.HasValue)
        {
            mainLight.color = lightData.mainLightColor.Value;
        }
        if (lightData.mainLightDirection.HasValue)
        {
            mainLight.transform.rotation = Quaternion.LookRotation(lightData.mainLightDirection.Value);
        }
    }
}