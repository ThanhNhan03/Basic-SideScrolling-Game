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
    private IEnumerator LoadNextLevel()
    {
       
        yield return new WaitForSecondsRealtime(levelExitDelay);
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }
        SceneManager.LoadScene(nextLevelIndex);
        //Load the next level
        Debug.Log("Loading next level");
    }
}
