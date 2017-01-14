using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles the roll button and associated UI

public class NumberRoller : MonoBehaviour
{
	public static NumberRoller instance;

	[SerializeField]
	Text uiNumber;
	[SerializeField]
	SpriteRenderer rollButtonSprite;
	[SerializeField]
	AudioClip tickSound;
	[SerializeField]
	AnimationCurve curve;
	[SerializeField]
	AudioClip SoundEffect;
	public bool enabledButton = true;

	void Start ()
	{
		//Set up singleton for referencing
		instance = this;
	}

	void Update ()
	{
		//Visually indicate to the user if they can press the roll button
		rollButtonSprite.color = enabledButton ? Color.white : Color.Lerp(Color.white,Color.black,0.5f);
	}

	//Public function called from UI button
	public void rollNumber ()
	{
		if (enabledButton) {
			enabledButton = false;
			StartCoroutine(roll());
		}
	}

	IEnumerator roll ()
	{
		//How long this process should last for
		float maxSpinTime = 4, minSpinTime = 2;
		float spinTime = Random.Range(minSpinTime,maxSpinTime);

		float waitTime = 0, timeElapsed = 0;
		int result = int.Parse(uiNumber.text);

		//Scroll through dice numbers, slowly increasing the time between numbers changing until a value is settled on
		while (timeElapsed + waitTime < spinTime) {
			timeElapsed += waitTime + Time.deltaTime;

			//Animation curve used to slowly increase the interval between each number displayed
			waitTime += curve.Evaluate(timeElapsed / spinTime);

			//Make sure we don't go over 6
			result = (result < 6) ? result + 1 : 1;

			//Display result
			uiNumber.text = "" + result;

			//Play a sound for each changing number
			SoundManager.instance.playSound(tickSound,1,Random.Range(.95f,1.05f));
			//if (timeElapsed + waitTime < spinTime)
			yield return new WaitForSeconds (waitTime);
		}
		StartCoroutine(flashNumber(0.5f));
		SoundManager.instance.playSound(SoundEffect);
		//Do a special sound & make the UI number flash
		//yield return new WaitForSeconds (.5f);


		//Move that penguin.
		StartCoroutine(PenguinMover.instance.MoveToPoint(result,this));
		if (PenguinMover.instance.reflection != null)
			StartCoroutine(PenguinMover.instance.reflection.MoveToPoint(result,this));
	}

	IEnumerator flashNumber (float duration)
	{
		float timeElapsed = 0, interval = 0.1f;
		while (timeElapsed < duration) {
			timeElapsed += interval;
			uiNumber.enabled = !uiNumber.enabled;
			yield return new WaitForSeconds (interval);
		}

		uiNumber.enabled = true;
	}
}