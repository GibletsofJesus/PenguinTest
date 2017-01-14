using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Actiavtes visual effects for collecting fish
public class Collectable : MonoBehaviour
{
	[SerializeField]
	Animator collectionEffects;
	[SerializeField]
	ParticleSystem particles;
	[SerializeField]
	AudioClip collectionSound;
	public bool badFish;

	bool triggered;

	void OnTriggerEnter2D (Collider2D col)
	{
		if (!triggered) {
			triggered = true;
			StartCoroutine(coolStuff());
		}
	}

	IEnumerator coolStuff ()
	{
		if (badFish)
			PenguinMover.instance.StartCoroutine(PenguinMover.instance.flashSprites(1.25f));

		//Spin + scale fish
		collectionEffects.Play("collection");
		SoundManager.instance.playSound(collectionSound,1,badFish ? 0.3f : 1);
		yield return new WaitForSeconds (0.7f);
		//Rad particles
		particles.Play();
		yield return new WaitForSeconds (0.25f);

		//To make sure this can't happen twice, disable the game object
		gameObject.SetActive(false);
	}
}
