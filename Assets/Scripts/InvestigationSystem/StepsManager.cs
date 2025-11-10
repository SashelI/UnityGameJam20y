using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace InvestigationSystem
{
	public class StepsManager : MonoBehaviour
	{
		public LoadingSwitchManager loading;
		public SpriteRenderer loadingSprites;

		public List<string> dialogVariables = new()
		{
			"PresentSuitcase",
			"PastSuitcase",
			"PresentAirConditionner",
			"PastAirConditionner",
			"PresentTeddybear",
			"PastTeddybear",
			"PresentRemote",
			"PastRemote",
			"PresentLasers",
			"PastBackpack",
			"BackpackMoved",
			"SuitcaseNotOpened",
			"KeycardInAC"
		};

		public static StepsManager Instance
		{
			get
			{
				return _instance;
			}
		}

		private static StepsManager _instance;

		private void Awake()
		{
			_instance = this;
		}

		public enum InvestigationStep
		{
			Other,
			Suitcase,
			AirConditionner,
			Teddybear,
			Remote,
			Lasers,
			Backpack
		}

		public enum InvestigationTime
		{
			Past,
			Present
		}

		public enum InvestigationClues
		{
			None,
			CCTV,
			BackpackMoved,
			SuitcaseNotOpened,
			KeycardInAC,
			ACTurnedOff
		}

		private Dictionary<InvestigationStep, bool> _presentSteps = new()
		{
			{ InvestigationStep.Suitcase, false }, { InvestigationStep.Teddybear, false },
			{ InvestigationStep.Remote, false }
		};

		private Dictionary<InvestigationStep, bool> _pastSteps = new()
		{
			{ InvestigationStep.Suitcase, false },
			{ InvestigationStep.AirConditionner, false }, { InvestigationStep.Teddybear, false },
			{ InvestigationStep.Remote, false }, { InvestigationStep.Lasers, false },
			{ InvestigationStep.Backpack, false }
		};

		private Dictionary<InvestigationClues, bool> _clues = new()
		{
			{ InvestigationClues.CCTV , false}, { InvestigationClues.BackpackMoved, false },
			{ InvestigationClues.SuitcaseNotOpened, false }, { InvestigationClues.ACTurnedOff, false }
		};

		private bool _firstStepCompleted;

		public void MarkStepAsCompleted(InvestigationStep step, InvestigationTime time)
		{
			if (time == InvestigationTime.Present)
			{
				_presentSteps[step] = true;
				if (!_firstStepCompleted)
				{
					_firstStepCompleted = true;
					loadingSprites.enabled = true;
				}
				else
				{
					loading.NextFrame();
				}

				if (_presentSteps.Count(s => s.Value) == _presentSteps.Count) //Loading completed
				{
					StartCoroutine(CCTVCoroutine());
				}
			}
			else
			{
				_pastSteps[step] = true;
			}

			var dialogVariableString = $"{time}{step.ToString()}";
			InvestigationDialogStarter.Instance.SetVariableForStep(dialogVariableString);
		}

		public void MarkClueAsFound(InvestigationClues clue)
		{
			_clues[clue] = true;

			var dialogVariableStringClue = string.Empty;

			switch (clue)
			{
				case InvestigationClues.ACTurnedOff:
					dialogVariableStringClue = "KeycardInAC";
					InvestigationDialogStarter.Instance.SetVariableForStep(dialogVariableStringClue);
					break;
				case InvestigationClues.SuitcaseNotOpened:
					dialogVariableStringClue = "SuitcaseNotOpened";
					InvestigationDialogStarter.Instance.SetVariableForStep(dialogVariableStringClue);
					break;
				case InvestigationClues.BackpackMoved:
					dialogVariableStringClue = "BackpackMoved";
					InvestigationDialogStarter.Instance.SetVariableForStep(dialogVariableStringClue);
					break;
				default: break;
			}

			if (_clues.Count(c => c.Value) == _clues.Count)
			{
				InvestigationDialogStarter.Instance.StartDialogue(InvestigationClues.KeycardInAC);
			}
		}

		public bool IsStepCompleted(InvestigationStep step, InvestigationTime time)
		{
			if (time == InvestigationTime.Present)
			{
				return _presentSteps[step];
			}
			else
			{
				return _pastSteps[step];
			}
		}

		public bool IsClueFound(InvestigationClues clue)
		{
			return _clues[clue];
		}

		private IEnumerator CCTVCoroutine()
		{
			yield return new WaitForSecondsRealtime(1f);
			loading.NextFrame();
			yield return new WaitForSecondsRealtime(1f);
			loading.NextFrame();
			yield return new WaitForSecondsRealtime(1f);
			_clues[InvestigationClues.CCTV] = true;
		}
	}
}
