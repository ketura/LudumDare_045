using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void StartButtonClicked()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void ExitButtonClicked()
	{
		SceneManager.LoadScene("Credits");
	}
}
