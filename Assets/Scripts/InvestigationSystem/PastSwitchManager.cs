using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace InvestigationSystem
{
	public class PastSwitchManager : MonoBehaviour
	{
		public InputActionReference input;
		[Space] 
		public Volume globalVolume;
		[Space]
		public GameObject PresentObjects;
		public GameObject PastObjects;

		public Material presentFloorMaterial;
		public Material presentWallMaterial;
		public Material pastMaterial;

		public List<MeshRenderer> floorRenderers;
		public List<MeshRenderer> wallRenderers;

		[Space] 
		public Material maskMaterial;
		public Texture2D normalMaskTexture;
		public Texture2D scanningMaskTexture;

		[Space]
		public AudioSource sound;

		private List<int> _floorMaterialIndices = new (){3,4};
		private List<int> _wallMaterialIndices = new (){0};

		private VolumeProfile _volumeProfile;
		private FilmGrain _volumeGrain;
		private ChromaticAberration _volumeAberration;

		public StepsManager.InvestigationTime CurrentTime { get; private set; } = StepsManager.InvestigationTime.Present;

		public bool CanSwitch { get; set; } = false;

		public static PastSwitchManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;

			_volumeProfile = globalVolume.sharedProfile;
			
			_volumeProfile.TryGet<FilmGrain>(out _volumeGrain);
			_volumeProfile.TryGet<ChromaticAberration>(out _volumeAberration);

			input.action.performed += OnButtonPressed;
		}

		private void OnButtonPressed(InputAction.CallbackContext _)
		{
			if (CanSwitch)
			{
				bool toPast = CurrentTime == StepsManager.InvestigationTime.Present;
				SwitchTimeline(toPast);
			}
		}

		public void SwitchTimeline(bool isPast)
		{
			sound.Play();

			if (_volumeAberration != null)
			{
				_volumeAberration.active = isPast;
			}
			if (_volumeGrain != null)
			{
				_volumeGrain.active = isPast;
			}

			maskMaterial.SetTexture("_EmissionMap", isPast ? scanningMaskTexture : normalMaskTexture);

			foreach (var floorRenderer in floorRenderers)
			{
				var mats = floorRenderer.sharedMaterials;
				foreach (var floorMaterialIndex in _floorMaterialIndices)
				{
					mats[floorMaterialIndex] = isPast ? pastMaterial : presentFloorMaterial;
				}
				floorRenderer.sharedMaterials = mats;
			}
			foreach (var wallRenderer in wallRenderers)
			{
				var mats = wallRenderer.sharedMaterials;
				foreach (var wallMaterialIndex in _wallMaterialIndices)
				{
					mats[wallMaterialIndex] = isPast ? pastMaterial : presentWallMaterial; ;
				}
				wallRenderer.sharedMaterials = mats;
			}

			PresentObjects.SetActive(!isPast);
			PastObjects.SetActive(isPast);

			CurrentTime = isPast ? StepsManager.InvestigationTime.Past : StepsManager.InvestigationTime.Present;
		}

		private void OnApplicationQuit()
		{
			maskMaterial.SetTexture("_EmissionMap", normalMaskTexture);
		}
	}
}
