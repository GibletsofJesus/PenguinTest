using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinMover : MonoBehaviour
{
	public static PenguinMover instance;

	[SerializeField]
	Transform[] iceCubes;
	[SerializeField]
	Vector3 offset;
	[SerializeField]
	Animator anim;

	int position = 0;
	//max of 17

	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	[SerializeField]
	bool moving = true;

	// Update is called once per frame
	void Update ()
	{
	}

	public IEnumerator MoveToPoint (int diceRoll, NumberRoller callback)
	{
		for (int i = 0; i < diceRoll; i++) {
			yield return StartCoroutine(MoveToTarget(iceCubes [position < 17 ? position + 1 : 0]));
		}
		callback.enabledButton = true;
		anim.Play("front_idle");
	}

	IEnumerator MoveToTarget (Transform target)
	{
		float lerpy = 0;
		while (lerpy < 1) {

			//Decide which animation to play

			//if target is 0 -> 

			anim.Play("front_walk_2");

			lerpy += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position,target.position + offset,lerpy);
			yield return new WaitForEndOfFrame ();
		}
		position = position < 17 ? position + 1 : 0;
		moving = false;
	}
}
