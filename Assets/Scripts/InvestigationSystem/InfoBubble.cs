using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InvestigationSystem
{
	public class InfoBubble : MonoBehaviour
	{
		[Header("References")]
		public Transform bubble;
		public IconAtlasSwitch interactionIcon;      

		[Header("Interaction")]
		public InputActionReference inputActions;
		[Space]
		public UnityEvent onInteract;

		public bool canInteract;

		[Header("Animation")]
		public float tweenDuration = 0.4f;
		public float enterOvershoot = 1.15f;

		public Vector3 baseScale;

		private bool playerInside = false;
		private InputAction interactAction;

		[Space]
		public StepsManager.InvestigationStep step;

		public void StartDialogue()
		{
			InvestigationDialogStarter.Instance.StartDialogue(step, PastSwitchManager.Instance.CurrentTime);
			StepsManager.Instance.MarkStepAsCompleted(step, PastSwitchManager.Instance.CurrentTime);
		}

		void Start()
		{
			// Récupère l’action depuis l’asset
			interactAction = inputActions.action;

			if (interactAction == null)
			{
				Debug.LogError("Action introuvable : " + inputActions.name);
			}

			interactAction?.Enable();

			if (bubble != null)
				bubble.localScale = Vector3.zero;

			if (interactionIcon != null)
				interactionIcon.ToggleIcon(canInteract);
		}

		void Update()
		{
			if (playerInside && canInteract)
			{
				if (interactAction.WasReleasedThisFrame())
				{
					canInteract = false;
					StartDialogue();
					onInteract?.Invoke();
				}
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player")) return;

			playerInside = true;

			LeanTween.cancel(bubble.gameObject);

			// Tween scale 0 -> overshoot -> 1
			bubble.localScale = Vector3.zero;
			LeanTween.scale(bubble.gameObject, baseScale * enterOvershoot, tweenDuration * 0.7f)
				.setEaseOutBack()
				.setOnComplete(() =>
				{
					LeanTween.scale(bubble.gameObject, baseScale, tweenDuration * 0.3f)
						.setEaseOutSine();
				});
		}

		void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Player")) return;

			playerInside = false;

			// Tween scale 1 -> 0
			LeanTween.cancel(bubble.gameObject);
			LeanTween.scale(bubble.gameObject, Vector3.zero, tweenDuration)
				.setEaseInBack();
		}
	}
}
