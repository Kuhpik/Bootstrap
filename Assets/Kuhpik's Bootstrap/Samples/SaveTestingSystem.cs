using Kuhpik;
using Kuhpik.Example;
using UnityEngine;

public class SaveTestingSystem : GameSystemWithScreen<ExampleScreen>, IIniting
{
    void IIniting.OnInit()
    {
        Debug.Log($"PlayerData save int = {player.intToSave}");

        //Generating random int before restarting;
        player.intToSave = Random.Range(0, 100);
        Debug.Log($"RNG int = {player.intToSave}");
        //Will restart the scene and auto-save player data class
        screen.InfoButton.onClick.AddListener(() => Bootstrap.GameRestart(0));
    }
}
