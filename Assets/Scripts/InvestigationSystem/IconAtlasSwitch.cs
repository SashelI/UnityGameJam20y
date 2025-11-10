using UnityEngine;

namespace InvestigationSystem
{
	public class IconAtlasSwitch : MonoBehaviour
	{
		public MeshRenderer mRenderer;

		public Material canInteractMaterial;
		public Material cannotInteractMaterial;

		public void ToggleIcon(bool enable)
		{
			mRenderer.enabled = enable;
		}

		public void SetCanInteractIcon()
		{
			mRenderer.material = canInteractMaterial;
		}

		public void SetCannotInteractIcon()
		{
			mRenderer.material = cannotInteractMaterial;
		}
	}
}
