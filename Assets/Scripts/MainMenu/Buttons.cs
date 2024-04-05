using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] Animator Transition;
    [SerializeField] float TransitionTime;
    [SerializeField] AudioManager audioManager;

    public void ChangeScene ()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene (int levelIndex)
    {
        audioManager.Play("Tap");
        Transition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void ExitGame()
    {
        audioManager.Play("Tap");
        Application.Quit();
    }
}
