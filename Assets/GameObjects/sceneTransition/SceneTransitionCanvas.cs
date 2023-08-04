using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCanvas : MonoBehaviour
{

    private Animator anim;

    public float transitionTime = 1f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void ResetLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));

    }
    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadPreviousScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        anim.SetTrigger("StartTransition");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

    }
}
