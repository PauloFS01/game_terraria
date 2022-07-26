using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] float secondsToLoad = 2f;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) {
        GetComponent<Animator>().SetTrigger("Open");
    }

    public void StartLoadingNextLevel(){
        GetComponent<Animator>().SetTrigger("Close");

        StartCoroutine(LoadingNextLevel());
    }

    IEnumerator LoadingNextLevel(){
        yield return new WaitForSeconds(secondsToLoad);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
