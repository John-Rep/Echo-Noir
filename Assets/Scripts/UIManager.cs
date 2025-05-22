using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool paused = false;
    GameObject gameUI, pauseMenu, mainMenu;
    TextMeshProUGUI rockText;
    InputAction cancel;
    PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.OnRockInteraction += UpdateRockCount;
        }
        mainMenu = transform.GetChild(0).gameObject;
        pauseMenu = transform.GetChild(1).gameObject;
        gameUI = transform.GetChild(2).gameObject;
        rockText = gameUI.GetComponentInChildren<TextMeshProUGUI>();
        cancel = InputSystem.actions.FindAction("Cancel");
        pauseMenu.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            mainMenu.SetActive(true);
            gameUI.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            gameUI.SetActive(true);
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

    void UpdateRockCount(int rockCount)
    {
        rockText.text = "X " + rockCount;
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
