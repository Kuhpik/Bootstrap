using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour, IUIScreen
{
    [SerializeField] [BoxGroup("Base Settings")] private UIScreenType type;
    [SerializeField] [BoxGroup("Base Settings")] private GameObject screen;
    [SerializeField] [BoxGroup("Background")] private bool useBackground;
    [SerializeField] [BoxGroup("Background")] [ShowIf("useBackground")] Color backgroundColor;

    public UIScreenType Type => type;
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

    public virtual void Refresh()
    {
    }
}

