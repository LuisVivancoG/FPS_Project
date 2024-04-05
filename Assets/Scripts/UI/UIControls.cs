using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Canvas DungeonCanvas;
    [SerializeField] Canvas GalleryCanvas;
    [SerializeField] Canvas PlayerCanvas;
    [SerializeField] Canvas VictoryCanvas;
    [SerializeField] AudioManager audioManager;
    public bool inGallery { get; set; }
    public bool inDungeon {get; set;}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void ResumeGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseCanvas.GetComponent<Canvas>().enabled = false;
        if (inGallery)
        {
            PlayerCanvas.GetComponent<Canvas>().enabled = true;
            GalleryCanvas.GetComponent<Canvas>().enabled = true;
        }
        if (inDungeon)
        {
            PlayerCanvas.GetComponent<Canvas>().enabled = true;
            DungeonCanvas.GetComponent<Canvas>().enabled = true;
        }
        audioManager.Play("Tap");
        Time.timeScale = 1;
    }

    public void WinUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        VictoryCanvas.GetComponent<Canvas>().enabled = true;
        GalleryCanvas.GetComponent<Canvas>().enabled = false;
        PlayerCanvas.GetComponent<Canvas>().enabled = false;
        DungeonCanvas.GetComponent<Canvas>().enabled = false;
        pauseCanvas.GetComponent <Canvas>().enabled = false;
    }

    public void GalleryUI()
    {
        GalleryCanvas.GetComponent<Canvas>().enabled = true;
        PlayerCanvas.GetComponent<Canvas>().enabled = true;
        DungeonCanvas.GetComponent<Canvas>().enabled = false;
        inGallery = true;
        inDungeon = false;
    }

    public void DungeonUI()
    {
        DungeonCanvas.GetComponent<Canvas>().enabled = true;
        PlayerCanvas.GetComponent <Canvas>().enabled = true;
        GalleryCanvas.GetComponent<Canvas>().enabled = false;
        inDungeon = true;
        inGallery = false;
    }
    private void Pause()
    {
        if (inGallery == true)
        {
            GalleryCanvas.GetComponent<Canvas>().enabled = false;
            PlayerCanvas.GetComponent<Canvas>().enabled = false;
        }
        if (inDungeon == true)
        {
            DungeonCanvas.GetComponent<Canvas>().enabled = false;
            PlayerCanvas.GetComponent<Canvas>().enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseCanvas.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0f;
    }

    public void GoToScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        audioManager.Play("Tap");
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        audioManager.Play("Tap");
        Application.Quit();
    }
}
