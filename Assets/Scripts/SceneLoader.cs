using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;

    public void LoadScene(int levelIndex)
    {
        StartCoroutine(LoadSceneAsyncronously(levelIndex));
    }

    // IEnumerator LoadSceneAsyncronously(int levelIndex)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
    //     while (!operation.isDone)
    //     {
    //         loadingScreen.SetActive(true);
    //         loadingBar.value = operation.progress;
    //         yield return null;
    //     }
    // }

    IEnumerator LoadSceneAsyncronously(int levelIndex)
    {
        loadingScreen.SetActive(true);
        loadingBar.value = 0;

        // Fake loading bar that fills over 3 seconds
        float timer = 0f;
        while (timer < 3f)
        {
            timer += Time.deltaTime;
            loadingBar.value = timer / 3f;
            yield return null;
        }

        loadingBar.value = 1f;
        SceneManager.LoadScene(levelIndex);
    }
}
