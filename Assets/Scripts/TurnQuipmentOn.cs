using Seagull.Interior_01.Utility;
using UnityEngine;

public class TurnQuipmentOn : MonoBehaviour
{
	private TurnOnAble _turnScript;

	private void Awake()
	{
		_turnScript = GetComponent<TurnOnAble>();
	}

	private void OnEnable()
	{
		if (_turnScript != null)
		{
			_turnScript.turnOn();
		}
	}

	private void OnDisable()
	{
		if (_turnScript != null)
		{
			_turnScript.turnOff();
		}
	}
}
