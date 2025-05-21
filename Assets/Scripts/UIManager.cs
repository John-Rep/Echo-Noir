using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool paused = false;
    GameObject gameUI, pauseMenu, mainMenu;
    InputAction cancel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu = transform.GetChild(0).gameObject;
        pauseMenu = transform.GetChild(1).gameObject;
        cancel = InputSystem.actions.FindAction("Cancel");
        pauseMenu.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cancel.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }
}
