using UnityEngine;

public class PlaySoundScript : StateMachineBehaviour {

	[Header("Audo Clips")]
	public AudioClip soundClip;

	override public void OnStateEnter
		(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//Play sound clip at camera
		SoundMonoMechanic.Instance.PlayClipAtPoint(soundClip, 
		                            Camera.main.transform.position);
	}
}