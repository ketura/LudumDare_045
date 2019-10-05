using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Utilities;

public class ExitHandler : Singleton<ExitHandler>
{
    public void InstantExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void GracefulExit(float time)
    {
        StartCoroutine(TimedExit(time));
    }

    bool exiting = false;
    IEnumerator TimedExit(float time)
    {
        //if (!exiting)
        //{
            exiting = true;
            yield return new WaitForSeconds(time);
            InstantExit();
            //handle graceful exiting here
            //SceneManager.LoadScene("credits");
        //}
    }
}
