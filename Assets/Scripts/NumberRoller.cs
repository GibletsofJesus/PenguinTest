using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public bool enabledButton = true;

	void Start ()
	{
		instance = this;
	}

	void Update ()
	{
		rollButtonSprite.color = enabledButton ? Color.white : Color.Lerp(Color.white,Color.black,0.5f);
	}

	public void rollNumber ()
	{
		if (enabledButton) {
			enabledButton = false;
			StartCoroutine(roll());
		}
	}

	public float timel;

	IEnumerator roll ()
	{  
		//For a random amount of time, scroll through the available numbers

		float maxSpinTime = 4, minSpinTime = 2;
		float spinTime = Random.Range(minSpinTime,maxSpinTime);
		float waitTime = 0, timeElapsed = 0;
		int result = int.Parse(uiNumber.text);
		while (timeElapsed + waitTime < spinTime) {
			timeElapsed += waitTime + Time.deltaTime;
			timel = timeElapsed;
			waitTime += curve.Evaluate(timeElapsed / spinTime);
			result = (result < 6) ? result + 1 : 1; 
			uiNumber.text = "" + result;
			//Play a sound for each changing number
			SoundManager.instance.playSound(tickSound,1,Random.Range(.95f,1.05f));
			yield return new WaitForSeconds (waitTime);
		}
		//Do a special sound when it finishes
		yield return new WaitForSeconds (.5f);


		//Move that penguin.
		StartCoroutine(PenguinMover.instance.MoveToPoint(result,this));
	}
}