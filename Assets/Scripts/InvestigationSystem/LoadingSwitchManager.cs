using TMPro;
using UnityEngine;

namespace InvestigationSystem
{
	public class LoadingSwitchManager : MonoBehaviour
	{
		public Sprite[] frames;
		public SpriteRenderer spriteRenderer;
		public TextMeshProUGUI text;

		[Space] public TextMeshProUGUI switchTooltipText;
		public TextMeshProUGUI endedText;

		int index = 0;

		public static LoadingSwitchManager Instance
		{
			get; private set;
		}

		private void Awake()
		{
			Instance = this;
		}

		public void NextFrame()
		{
			index = (index + 1) % frames.Length;
			spriteRenderer.sprite = frames[index];
			Debug.Log($"INDEX IS {index}");
			if (index == frames.Length - 1)
			{
				text.text = "CCTV Data available_";
				InvestigationDialogStarter.Instance.StartDialogue(StepsManager.InvestigationClues.CCTV);
			}
		}

		public void ShowSwitchTooltip()
		{
			switchTooltipText.gameObject.SetActive(true);
		}

		public void ShowEndText()
		{
			endedText.gameObject.SetActive(true);
		}
	}
}
