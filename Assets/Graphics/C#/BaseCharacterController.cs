using UnityEngine;
using System.Collections;

public class BaseCharacterController : MonoBehaviour 
{
	[SerializeField] private Animator AnimCntrl;
	[SerializeField] private string WinTrigger;

	private void Start ()
	{
		if (!AnimCntrl)
			AnimCntrl = gameObject.GetComponent <Animator> ();
	}

	public void WinEmotion (string winTrigger)
	{
		if (AnimCntrl)
			AnimCntrl.SetTrigger (winTrigger);
	}

	[ContextMenu ("WinEmotion")]
	public void WinEmotion () 
	{
		WinEmotion (WinTrigger);
	}

}
