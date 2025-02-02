using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelExitDelay = 2f;
    //Exit the level when the player touches the exit collider use coroutine to delay the level loading
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadNextLevel());
        }
    }
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelExitDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().DestroyScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
