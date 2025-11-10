using System;
using System.Collections;
using cherrydev;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InvestigationSystem.StepsManager;

namespace InvestigationSystem
{
    public class InvestigationDialogStarter : MonoBehaviour
    {
        [SerializeField] private DialogBehaviour _dialogBehaviour;
        [SerializeField] private List<DialogEntry>  _stepsDialogGraphs = new();

        [Space]
        [SerializeField] private GameObject _black;
        [SerializeField] private DialogNodeGraph _intro;

        public static InvestigationDialogStarter Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
		}

		private void Start()
        {
	        _dialogBehaviour.BindExternalFunction("EndIntro", DebugExternal);
			_dialogBehaviour.BindExternalFunction("EnableSwitch", MakeSwitchAvailable);
            _dialogBehaviour.BindExternalFunction("UnlockedSuitcase", SetClueUnlockedSuitcase);
            _dialogBehaviour.BindExternalFunction("UnlockedAC", SetClueUnlockedAC);
            _dialogBehaviour.BindExternalFunction("UnlockedBackPack", SetClueUnlockedBackPack);
            _dialogBehaviour.BindExternalFunction("GameEnded", SetGameEnded);

	        _dialogBehaviour.StartDialog(_intro);
        }

        private void DebugExternal() => _black.SetActive(false);

		public void StartDialogue(InvestigationStep step, InvestigationTime time)
        {
            if(GetDialogueNode(step, time, out DialogNodeGraph dialogNodeGraph))
            {
	            if (_dialogBehaviour.isDialogStarted)
	            {
		            StartCoroutine(WaitBeforeDialog(dialogNodeGraph));
		            return;
	            }
				_dialogBehaviour.StartDialog(dialogNodeGraph);
            }
            else
            {
                Debug.LogWarning($"No dialogue found for step {step} and time {time}");
			}
		}

        public void StartDialogue(InvestigationClues clue)
        {
	        if (GetDialogueNode(clue, out DialogNodeGraph dialogNodeGraph))
	        {
		        if (_dialogBehaviour.isDialogStarted)
		        {
			        StartCoroutine(WaitBeforeDialog(dialogNodeGraph));
			        return;
		        }
		        _dialogBehaviour.StartDialog(dialogNodeGraph);
	        }
	        else
	        {
		        Debug.LogWarning($"No dialogue found for step {clue}");
	        }
        }

		private bool GetDialogueNode(InvestigationStep step, InvestigationTime time, out DialogNodeGraph dialogNodeGraph)
        {
	        if (_stepsDialogGraphs.Any(entry => (entry.Step == step && entry.Time == time)))
	        {
                dialogNodeGraph = _stepsDialogGraphs.First(entry => (entry.Step == step && entry.Time == time)).DialogGraph;
                return true;
	        }

            dialogNodeGraph = null;
            return false;
		}

        private bool GetDialogueNode(InvestigationClues clue, out DialogNodeGraph dialogNodeGraph)
        {
	        if (_stepsDialogGraphs.Any(entry => entry.Clue == clue))
	        {
		        dialogNodeGraph = _stepsDialogGraphs.First(entry => entry.Clue == clue).DialogGraph;
		        return true;
	        }

	        dialogNodeGraph = null;
	        return false;
        }

        private IEnumerator WaitBeforeDialog(DialogNodeGraph dialogNodeGraph)
        {
	        Debug.Log($"STARTING {dialogNodeGraph.name} WHILE{_dialogBehaviour.isDialogStarted}");
	        while (_dialogBehaviour.isDialogStarted)
	        {
		        Debug.Log($"STARTING {dialogNodeGraph.name} WHILE{_dialogBehaviour.isDialogStarted}");
		        yield return new WaitForSeconds(0.5f);
	        }

	        Debug.Log($"STARTING {dialogNodeGraph.name}");
	        yield return new WaitForSecondsRealtime(1f);
			_dialogBehaviour.StartDialog(dialogNodeGraph);
        }
public void MakeSwitchAvailable()
        {
	        PastSwitchManager.Instance.CanSwitch = true;
            ShowSwitchTooltip();
        }

        public void SetClueUnlockedSuitcase()
        {
            StepsManager.Instance.MarkClueAsFound(InvestigationClues.SuitcaseNotOpened);
        }

        public void SetClueUnlockedBackPack()
        {
	        StepsManager.Instance.MarkClueAsFound(InvestigationClues.BackpackMoved);
        }

		public void SetClueUnlockedAC()
		{
			StepsManager.Instance.MarkClueAsFound(InvestigationClues.ACTurnedOff);
		}

		public void SetGameEnded()
		{
            LoadingSwitchManager.Instance.ShowEndText();
		}

		public void ShowSwitchTooltip()
		{
            LoadingSwitchManager.Instance.ShowSwitchTooltip();
		}

		public void SetVariableForStep(string variableName)
        {
			_dialogBehaviour.SetVariableValue(variableName, true);
		}

		[System.Serializable]
		public class DialogEntry
        {
            public InvestigationStep Step;
            public InvestigationClues Clue;
            public InvestigationTime Time;
            public DialogNodeGraph DialogGraph;

            DialogEntry(InvestigationStep step = InvestigationStep.Other, InvestigationClues clue = InvestigationClues.None, InvestigationTime time = InvestigationTime.Present, DialogNodeGraph dialogGraph = null)
            {
                Clue = clue;
				Step = step;
                Time = time;
                DialogGraph = dialogGraph;
			}

            DialogEntry()
            {
	            Step = InvestigationStep.Other;
                Clue = InvestigationClues.None;
				Time = InvestigationTime.Present;
	            DialogGraph = null;
            }
		}
    }
}