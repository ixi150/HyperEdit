using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Xunity.Behaviours;

public class SplashStarting : GameBehaviour
{
	[SerializeField] float splashDuration = 3;
	[SerializeField] float fadeSpeed = 1;

	CanvasGroup canvasGroup;
	
	void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		Invoke(Fade, splashDuration);
	}

	void Fade()
	{
		StartCoroutine(Fading());
	}

	IEnumerator Fading()
	{
		while (canvasGroup.alpha > 0)
		{
			canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
			yield return null;
		}
		Destroy(transform.parent.gameObject);
	}
}
