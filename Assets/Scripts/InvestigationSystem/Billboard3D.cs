using UnityEngine;

namespace InvestigationSystem
{
	public class Billboard3D : MonoBehaviour
	{
		public float rotateSpeed = 0.1f;
		Camera cam;
		LTDescr currentTween;

		void Start()
		{
			cam = Camera.main;
		}

		void LateUpdate()
		{
			if (cam == null) return;

			Vector3 targetDir = transform.position - cam.transform.position;
			Quaternion targetRot = Quaternion.LookRotation(targetDir);

			if (currentTween != null)
			{
				LeanTween.cancel(gameObject);
			}

			// Tweener smooth
			currentTween = LeanTween.rotate(gameObject, targetRot.eulerAngles, rotateSpeed)
				.setEaseOutSine();
		}
	}
}
