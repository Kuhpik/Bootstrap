using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Kuhpik.Example
{
    public class ExampleScreen : UIScreen
    {
        [SerializeField] [BoxGroup()] private Button infoButton;

        public override void Subscribe()
        {
            base.Subscribe();
            infoButton.onClick.AddListener(() => Debug.Log($"{infoButton.name} button clicked"));
        }

        public override void Open()
        {
            base.Open();
            Debug.Log("Example screen opened");
        }

        public override void Refresh()
        {
            base.Refresh();
            Debug.Log("Example screen refreshed");
        }

        public override void Close()
        {
            base.Close();
            Debug.Log("Example screen closed");
        }
    }
}