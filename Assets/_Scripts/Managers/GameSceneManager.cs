using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO use this
//https://forum.unity.com/threads/c-coroutines-in-static-functions.134546/#post-3417063

public static class GameSceneManager
{

    public enum Scenes { Loading, Menu, Intro, Arena, Shop };

    /// <summary>
    /// Loads the specified scene immediately.
    /// </summary>
    /// <param name="_scene"></param>
    public static void LoadScene(Scenes _scene)
    {

            SceneManager.LoadScene(_scene.ToString());

    }


    /// <summary>
    /// Restart the currently loaded scene.
    /// </summary>
    public static void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }



    public static void LoadSceneASync(Scenes _scene)
    {


        //MonoBehaviour.StartCoroutine(LoadAsyncScene(_scene.ToString()));

    }


    private static IEnumerator LoadAsyncScene(string _scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene2");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }


}
