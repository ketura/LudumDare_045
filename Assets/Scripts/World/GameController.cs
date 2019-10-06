using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

public class GameController : Singleton<GameController>
{
  // Start is called before the first frame update
  void Start()
  {
		
  }

  // Update is called once per frame
  void Update()
  {
	
  }

	public void ShowText(string text)
	{
		//display text box with appropriate text
	}

	public void ShowExistTutorial()
	{
		//shows on game start, "spam space to exist" with animated space bar until existence happens the first time
	}

	public void ShowControlsTutorial()
	{
		//shows brief controls overlay after existing the first time, also summoned when the player presses H for help
	}

	public void ShowLossTutorial()
	{
		//shows the "spam space to exist" text with progressively more hopeless messages the more the player dies
	}
}
