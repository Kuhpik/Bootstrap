using NaughtyAttributes;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour, IUIScreen
{
    [SerializeField] [BoxGroup("Base Settings")] private EGamestate type;
    [SerializeField] [BoxGroup("Base Settings")] private GameObject screen;
    [SerializeField] [BoxGroup("Background")] private bool useBackground;
    [SerializeField] [BoxGroup("Background")] [ShowIf("useBackground")] Color backgroundColor;

    public EGamestate Type => type;
    public bool UseBackground => useBackground;
    public Color BackgroundColor => backgroundColor;

    public virtual void Open()
    {
        screen.SetActive(true);
    }

    public virtual void Close()
    {
        screen.SetActive(false);
    }

    /// <summary>
    /// Use it for special cases.
    /// </summary>
    public virtual void Refresh()
    {
    }

    /// <summary>
    /// Subscribe is called on Awake()
    /// </summary>
    public virtual void Subscribe()
    {     
    }
}