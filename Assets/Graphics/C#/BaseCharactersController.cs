using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharactersController : MonoBehaviour {

	[SerializeField] private BaseCharacterController[] Characters;

	[ContextMenu ("ShowWin")]
	public void ShowWin () 
	{
		foreach (var item in Characters)
		{
			item.WinEmotion ();
		}
	}
}
