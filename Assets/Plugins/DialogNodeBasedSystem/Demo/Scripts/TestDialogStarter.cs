using UnityEngine;
using cherrydev;
using UnityEngine.Playables;

namespace DialogNodeBasedSystem.Demo.Scripts
{
    public class TestDialogStarter : MonoBehaviour
    {
        [SerializeField] private DialogBehaviour _dialogBehaviour;
        [SerializeField] private DialogNodeGraph _dialogGraph;
        [SerializeField] private GameObject _black;

        private void Start()
        {
            _dialogBehaviour.BindExternalFunction("EndIntro", DebugExternal);
            _dialogBehaviour.StartDialog(_dialogGraph);
        }

        private void DebugExternal() => _black.SetActive(false);
    }
}