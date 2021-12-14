using Kuhpik;
using Kuhpik.Extensions;
using UnityEngine;

public class ScreenInjectTestSystem : GameSystemWithScreen<MenuUIScreen>
{
    public override void OnInit()
    {
        if (screen == null) LogExtensions.Log("Screen Injection test <color=red>failed</color>");
        else LogExtensions.Log($"Screen Injection test <color=green>succeed</color>. Screen data: { screen.GetType().FullName }");
    }
}
