using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Canvas GameplayCanvas;
    [SerializeField] Canvas GalleryCanvas;
    public bool InGallery { get ; set; }
    public bool InGameplay { get; set; }
    public void ResumeGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseCanvas.GetComponent<Canvas>().enabled = false;
        if (InGallery)
        {
            GalleryCanvas.GetComponent<Canvas>().enabled = true;
        }
        if (InGameplay)
        {
            GameplayCanvas.GetComponent<Canvas>().enabled = true;
        }
        Time.timeScale = 1;
    }

    public void GalleryUI()
    {
        GalleryCanvas.GetComponent<Canvas>().enabled = true;
        GameplayCanvas.GetComponent<Canvas>().enabled = false;
        InGallery = true;
        InGameplay = false;
    }

    public void GameplayUI()
    {
        GameplayCanvas.GetComponent<Canvas>().enabled = true;
        GalleryCanvas.GetComponent<Canvas>().enabled = false;
        InGameplay = true;
        InGallery = false;
    }

    public void GoToScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
