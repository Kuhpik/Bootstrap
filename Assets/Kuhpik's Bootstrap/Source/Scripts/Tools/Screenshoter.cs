#if UNITY_EDITOR

using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Kuhpik
{
    // Thx to https://gist.github.com/yasirkula/fba5c7b5280aa90cdb66a68c4005b52d
    // Unity's github https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GameView/GameView.cs
    // And lovely Google https://docs.unity3d.com/ScriptReference/ScreenCapture.html

    [DefaultExecutionOrder(10)]
    public class Screenshoter : MonoBehaviour
    {
        [SerializeField] [BoxGroup("Settings")] KeyCode screenshotKey;
        [SerializeField] [BoxGroup("Settings")] Vector2Int[] targetResolutions;
        [SerializeField] [BoxGroup("Path")] [ReadOnly] string path;

        [Button] void SelectFolder() { path = EditorUtility.OpenFolderPanel("Choose output directory", "", ""); }
        [Button] void OpenFolder() { if (Directory.Exists(path)) System.Diagnostics.Process.Start("explorer.exe", path.Replace("/", "\\")); }
        [Button] void Capture() { if (!isBusy) StartCoroutine(ScreenshoterRoutine()); }

        CameraInstaller cameraInstaller;
        List<Vector2Int> indexesList;
        EditorWindow gameView;
        object sizeHolder;
        bool isBusy;
        int index;

        Dictionary<Vector2Int, int> indexes;

        void Awake()
        {
            sizeHolder = GetType("GameViewSizes").FetchProperty("instance").FetchProperty("currentGroup");
            gameView = GetWindow(GetType("GameView"));
            index = (int)gameView.FetchProperty("selectedSizeIndex");
            isBusy = false;

            indexes = new Dictionary<Vector2Int, int>();
            indexesList = new List<Vector2Int>();

            foreach (var resolution in targetResolutions)
            {
                var customSize = GetFixedResolution(resolution.x, resolution.y);
                sizeHolder.CallMethod("AddCustomSize", customSize);

                var customIndex = (int)sizeHolder.CallMethod("IndexOf", customSize) + (int)sizeHolder.CallMethod("GetBuiltinCount"); 
                indexes.Add(resolution, customIndex);

                indexesList.Add(resolution);
            }
        }

        void OnDestroy()
        {
            for (int i = indexesList.Count - 1; i >= 0 ; i--)
            {
                sizeHolder.CallMethod("RemoveCustomSize", indexes[indexesList[i]]);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(screenshotKey) && !isBusy)
            {
                StartCoroutine(ScreenshoterRoutine());
            }
        }

        IEnumerator ScreenshoterRoutine()
        {
            isBusy = true;
            Time.timeScale = 0;

            foreach (var resolution in targetResolutions)
            {
                gameView.CallMethod("SizeSelectionCallback", indexes[resolution], null);
                gameView.Repaint();
                yield return new WaitForSecondsRealtime(0.1f);

                cameraInstaller.FindOrUse().Resize();
                yield return new WaitForEndOfFrame();

                CaptureScreenshot(resolution);
                yield return null;
            }

            gameView.CallMethod("SizeSelectionCallback", index, null);
            gameView.Repaint();
            yield return null;
            cameraInstaller.FindOrUse().Resize();

            Time.timeScale = 1;
            isBusy = false;
        }

        void CaptureScreenshot(Vector2Int resolution)
        {
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            var shotPath = path + $"/{resolution.x}x{resolution.y}";

            if (!Directory.Exists(shotPath)) Directory.CreateDirectory(shotPath);
            File.WriteAllBytes(Path.Combine(shotPath, $"Screenshot {Directory.GetFiles(shotPath).Length + 1}.png"), screenshot.EncodeToJPG());
            Destroy(screenshot);
        }

        #region Helpers

        Type GetType(string type)
        {
            return typeof(EditorWindow).Assembly.GetType("UnityEditor." + type);
        }

        EditorWindow GetWindow(Type type)
        {
            return EditorWindow.GetWindow(type);
        }

        object GetFixedResolution(int width, int height)
        {
            object sizeType = Enum.Parse(GetType("GameViewSizeType"), "FixedResolution");
            return GetType("GameViewSize").CreateInstance(sizeType, width, height, "temporary");
        }

        #endregion
    }
}

#endif
