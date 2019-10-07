using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using Utilities;

[System.Serializable]
public struct LossMessage
{
	public int Count;
	public string Message;
}


public class GameController : Singleton<GameController>
{
	[Range(0.0f, 1.0f)]
	public float Volume;

	public Image Background;
	private CanvasGroup BackgroundPanel;
	public TMP_Text Text;

	public List<LossMessage> LossMessages;

	private bool Displaying = false;
	private float DisplayTime = 0.0f;

	private bool helping = false;

  void Start()
  {
		BackgroundPanel = Background.GetComponent<CanvasGroup>();
		Background.CrossFadeAlpha(0, 0, true);
		Text.CrossFadeAlpha(0, 0, true);

		AudioListener.volume = Volume;
	}

  // Update is called once per frame
  void Update()
  {
		AudioListener.volume = Volume;

		if (!Displaying)
			return;

		DisplayTime -= Time.deltaTime;
		if(DisplayTime <= 0)
		{
			ClearText();
		}
  }

	public void ShowText(string text, float duration)
	{
		helping = false;
		Debug.Log(text);
		Text.SetText(text);

		Background.CrossFadeAlpha(1.0f, 0.3f, true);
		Text.CrossFadeAlpha(1.0f, 0.3f, true);
		Displaying = true;
		DisplayTime = duration;
	}

	public void ClearText()
	{
		Displaying = false;
		Background.CrossFadeAlpha(0.0f, 1.0f, true);
		Text.CrossFadeAlpha(0.0f, 1.0f, true);
	}

	public void ShowExistTutorial()
	{
		ShowText("SPAM SPACE TO EXIST", 30.0f);
	}

	public void ShowControlsTutorial()
	{
		if(helping)
		{
			helping = false;
			ClearText();
			return;
		}
		ShowText("WASD to move. Move into things to grow. Q/E to rotate your cluster. \nHold left-click to create a gravity well.  Right-click to reverse the direction of an existing gravity well.  \nH for this help message.\n\n Backspace to fade back out of reality.  SPAM SPACE TO EXIST.", 30.0f);
		helping = true;
	}

	private int lossCount = 0;
	public void ShowLossTutorial()
	{
		lossCount++;
		int slot = 0;
		//shows the "spam space to exist" text with progressively more hopeless messages the more the player dies
		if(LossMessages.Any(x => x.Count == lossCount))
		{
			slot = lossCount;
		}

		ShowText(LossMessages.Where(x => x.Count == slot).Select(x => x.Message).First(), 10.0f);
	}

	public void ShowBoundaryTutorial()
	{
		ShowText("There feels to be nothing here, and we did not exist to consume nothing!\n\nPerhaps that irritating light holds something to fill the void...", 5.0f);
	}
}
