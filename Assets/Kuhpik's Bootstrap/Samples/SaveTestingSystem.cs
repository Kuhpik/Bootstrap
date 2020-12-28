using Kuhpik;
using Kuhpik.Example;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SaveTestingSystem : GameSystemWithScreen<ExampleScreen>, IIniting
{
    void IIniting.OnInit()
    {
        Debug.Log($"PlayerData save int = {player.intToSave}");
        //Generating random int before restarting;
        player.intToSave = Random.Range(0, 100);
        Debug.Log($"RNG int = {player.intToSave}");
        //Will restart the scene and auto-save player data class
        screen.InfoButton.onClick.AddListener(() => SwitchToGame());
    }

    async void SwitchToGame()
    {
        Bootstrap.ChangeGameState(EGamestate.Game);
        await Task.Delay(TimeSpan.FromSeconds(3f));
        Bootstrap.GameRestart(0);
    }
}
