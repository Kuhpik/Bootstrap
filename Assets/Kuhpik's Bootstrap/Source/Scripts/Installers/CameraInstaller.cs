using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Thx to https://gist.github.com/coastwise/5952119
/// </summary>
namespace Kuhpik
{
    public class CameraInstaller : MonoBehaviour
    {
        [SerializeField] private bool scaleFOV;
        [SerializeField] [ShowIf("scaleFOV")] private bool portrait;
        [SerializeField] [ShowIf("scaleFOV")] private bool allCameras;
        [SerializeField] [HideIf("allCameras")] private Camera[] cameras;

        public void Process()
        {
            if (scaleFOV)
            {
                var screenSize = portrait ? new Vector2(1080, 1920) : new Vector2(1920, 1080);
                var collection = allCameras ? FindObjectsOfType<Camera>() : cameras;
                var screenAspect = (float)Screen.width / (float)Screen.height;
                var aspect = screenSize.x / screenSize.y;

                Debug.Log($"Current screen size is {Screen.width}x{Screen.height}. Aspect ratio is {screenAspect}");

                foreach (var cam in collection)
                {
                    var camRads = (cam.orthographic ? cam.orthographicSize : cam.fieldOfView) * Mathf.Deg2Rad;
                    camRads = 2f * Mathf.Atan(Mathf.Tan(camRads / 2f) * aspect);
                    var screenRads = 2f * Mathf.Atan(Mathf.Tan(camRads / 2f) / screenAspect);

                    if (!cam.orthographic)
                    {
                        cam.fieldOfView = screenRads * Mathf.Rad2Deg;
                    }

                    else
                    {
                        cam.orthographicSize = screenRads * Mathf.Rad2Deg;
                    }
                }
            }
        }
    }
}