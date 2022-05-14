using System.Linq;
using UnityEngine;

namespace Kuhpik.Tools
{
    [DefaultExecutionOrder(600)]
    public class DatasDrawer : MonoBehaviour
    {
        // TODO: Change when data separation feature ready
        // https://answers.unity.com/questions/36628/components-using-generics.html
        private void Start()
        {
            var playerDataType = typeof(PlayerData);
            var playerDataDrawer = new GameObject(playerDataType.Name);
            var playerDataDrawerComponent = playerDataDrawer.AddComponent<PlayerDataDrawer>();

            playerDataDrawer.transform.SetParent(transform);
            playerDataDrawerComponent.SetData(Bootstrap.Instance.itemsToInject.First(x => x.GetType() == playerDataType) as PlayerData);

            //------------------//

            var gameDataType = typeof(GameData);
            var gameDataDrawer = new GameObject(gameDataType.Name);
            var gameDataDrawerComponent = gameDataDrawer.AddComponent<GameDataDrawer>();

            gameDataDrawer.transform.SetParent(transform);
            gameDataDrawerComponent.SetData(Bootstrap.Instance.itemsToInject.First(x => x.GetType() == gameDataType) as GameData);
        }
    }

    public abstract class DataDrawer<T> : MonoBehaviour where T : class
    {
        [SerializeField] T data;

        public void SetData(T data)
        {
            this.data = data;
        }
    }

    public class GameDataDrawer : DataDrawer<GameData> { }
    public class PlayerDataDrawer : DataDrawer<PlayerData> { }
}
