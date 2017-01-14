using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves the penguin avatar
public class PenguinMover : MonoBehaviour
{
	public static PenguinMover instance;

	public PenguinMover reflection;

	//The ice cubes the player can move along
	[SerializeField]
	Transform[] iceCubes;
	[SerializeField]
	Vector3 offset;

	//Avatar animator
	[SerializeField]
	Animator anim;

	//The side on view of the penguin
	[SerializeField]
	GameObject sideguin, frontguin;

	//Various sprites that need to be tweaked for front/back walking animations
	[SerializeField]
	SpriteRenderer body, scarf, pole, footL, footR, pompom;
	[SerializeField]
	Sprite[] flipSprites;
	[SerializeField]
	float moveSpeed = 1f;
	[SerializeField]
	int position = 0;
	//max of 17

	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	public IEnumerator MoveToPoint (int diceRoll, NumberRoller callback)
	{
		for (int i = 0; i < diceRoll; i++) {
			yield return StartCoroutine(MoveToTarget(iceCubes [position < 17 ? position + 1 : 0]));
		}
		callback.enabledButton = true;

		frontguin.SetActive(true);
		sideguin.SetActive(false);
		//Do sprites for forward facing penguin
		frontBackSprites(true);
		anim.Play("front_idle");
	}

	IEnumerator MoveToTarget (Transform target)
	{
		float lerpy = 0;
		Vector3 startPos = transform.position;
		while (lerpy < 1) {

			//Decide which animation to play

			//if target is 0 -> 6 we're going left
			//if target is 6 -> 9 we're going down
			//if target is 9 -> 15 we're going right
			//if target is > 15 || 0 going up

			if (position < 6) {
				frontguin.SetActive(false);
				sideguin.SetActive(true);
				sideguin.transform.localScale = Vector3.one;
				anim.Play("left_walk");
			}
			else if (position < 9) {

					frontguin.SetActive(true);
					sideguin.SetActive(false);
					frontBackSprites(true);
					anim.Play("front_walk");
				}
				else if (position < 15) {
						frontguin.SetActive(false);
						sideguin.SetActive(true);
						sideguin.transform.localScale = new Vector3 (-1, 1, 1);
						anim.Play("left_walk");
					}
					else {
						frontguin.SetActive(true);
						sideguin.SetActive(false);
						frontBackSprites(false);
						anim.Play("front_walk");
					}
				
			lerpy += Time.fixedDeltaTime * moveSpeed;
			transform.position = Vector3.Lerp(startPos,target.position + offset,lerpy);
			yield return new WaitForFixedUpdate ();
		}

		//If we've gone round the whole circuit, reset to the beginning 
		position = position < 17 ? position + 1 : 0;
	}

	//Toggle various sprites and sorting orders for back/front wanimations
	void frontBackSprites (bool front)
	{
		scarf.enabled = front;
		pompom.sortingOrder = front ? 101 : 99;
		frontguin.transform.localScale = new Vector3 (front ? 1 : -1, 1, 1);
		body.sprite = flipSprites [front ? 0 : 1];
		footL.sprite = flipSprites [front ? 2 : 3];
		footR.sprite = flipSprites [front ? 2 : 3];
		pole.sortingOrder = front ? 100 : 98;		
	}

	public IEnumerator flashSprites (float duration)
	{
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
		float timeElapsed = 0, interval = 0.1f;
		while (timeElapsed < duration) {
			timeElapsed += interval;
			foreach (SpriteRenderer sr in sprites) {
				sr.enabled = !sr.enabled;
			}
			yield return new WaitForSeconds (interval);
		}

		foreach (SpriteRenderer sr in sprites) {
			sr.enabled = true;
		}
	}
}
