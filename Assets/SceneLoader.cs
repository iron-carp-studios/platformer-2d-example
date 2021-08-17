using UnityEngine;
using UnityEngine.SceneManagement;


//hacky cleaup and load scene and ui in game mode.
public class SceneLoader : MonoBehaviour
{
    public int ActiveSceneCount;

    public void Start()
    {
        ActiveSceneCount = NumberOfActiveOpenScenes();

        if (NumberOfActiveOpenScenes() > 1)
        {
            CleanUpOpenScenes();
        }

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private int NumberOfActiveOpenScenes()
    {
        var openScenes = SceneManager.sceneCount;
        return openScenes;

    }

    private void CleanUpOpenScenes()
    {

        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < ActiveSceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex != activeSceneIndex)
            {
                SceneManager.UnloadSceneAsync(scene.buildIndex);
                ActiveSceneCount = SceneManager.sceneCount;
            }
        }
    }
}
