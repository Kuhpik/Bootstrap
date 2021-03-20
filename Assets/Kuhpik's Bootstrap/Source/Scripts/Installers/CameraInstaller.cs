using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Thx to https://gist.github.com/coastwise/5952119
/// </summary>
namespace Kuhpik
{
    [DefaultExecutionOrder(10)]
    public class CameraInstaller : MonoBehaviour
    {
        [SerializeField] bool scaleFOV;
        [SerializeField] [ShowIf("scaleFOV")] bool allCameras;
        [SerializeField] [ShowIf("scaleFOV")] bool clampAspectRatio;
        [SerializeField] [HideIf("allCameras")] Camera[] cameras;
        [SerializeField] [ShowIf("scaleFOV")] Vector2 targetResolution;
        [SerializeField] [ShowIf("scaleFOV")] [Tooltip("I commonly use this for tablet devices")] Vector2 screenRatioClamp = Vector2.up;

        Dictionary<Camera, float> fovDictionary;

        void Start()
        {
            if (scaleFOV)
            {
                var cams = allCameras ? FindObjectsOfType<Camera>() : cameras;
                fovDictionary = cams.ToDictionary(x => x, x => x.orthographic ? x.orthographicSize : x.fieldOfView);
                Resize();
            }
        }

        /// <summary>
        /// Adding Camera to resizing ones. Most common use case - instantiating cameras via systems.
        /// </summary>
        /// <param name="camera">Camera</param>
        /// <param name="autoResize">Should camera's FOV be resized right after this method call</param>
        public void AddCamera(Camera camera, bool autoResize = true)
        {
            if (scaleFOV)
            {
                if (fovDictionary.ContainsKey(camera))
                {
                    Debug.Log("Camera was already added");
                    return;
                }

                fovDictionary.Add(camera, camera.orthographic ? camera.orthographicSize : camera.fieldOfView);
                if (autoResize) Resize();                
            }
        }

        /// <summary>
        /// Resize FOV method. Use this for specific cases like resize on screenshot.
        /// </summary>
        public void Resize()
        {
            if (!scaleFOV) 
            { 
                Debug.Log("Resize is disabled in Installer's settings"); 
                return; 
            }

            var screenAspect = (float)Screen.width / (float)Screen.height;
            var aspect = targetResolution.x / targetResolution.y;

            if (clampAspectRatio) aspect = Mathf.Clamp(aspect, screenRatioClamp.x, screenRatioClamp.y);
            Debug.Log($"Current screen size is {Screen.width}x{Screen.height}. Aspect ratio is {screenAspect}");

            foreach (var cam in fovDictionary.Keys)
            {
                var fov = fovDictionary[cam];
                var rads = 2f * Mathf.Atan(Mathf.Tan(fov * Mathf.Deg2Rad / 2f) * aspect);
                var screenRads = 2f * Mathf.Atan(Mathf.Tan(rads / 2f) / screenAspect);

                if (cam.orthographic) cam.orthographicSize = screenRads * Mathf.Rad2Deg; 
                else cam.fieldOfView = screenRads * Mathf.Rad2Deg;
            }
        }
    }
}