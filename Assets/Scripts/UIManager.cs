using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool paused = false, endLevel = false;
    GameObject gameUI, pauseMenu, mainMenu, loseScreen, winScreen, introScreen;
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
        loseScreen = transform.GetChild(3).gameObject;
        winScreen = transform.GetChild(4).gameObject;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            introScreen = transform.GetChild(5).gameObject;
        }
        rockText = gameUI.GetComponentInChildren<TextMeshProUGUI>();
        cancel = InputSystem.actions.FindAction("Cancel");
        pauseMenu.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            mainMenu.SetActive(true);
            gameUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
            Time.timeScale = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            mainMenu.SetActive(false);
            gameUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
            Time.timeScale = 0;
            introScreen.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
            gameUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            paused = false;
            Time.timeScale = 1;
        }
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cancel.WasPressedThisFrame() && introScreen.activeSelf)
        {
            introScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            paused = false;
        } else if (cancel.WasPressedThisFrame() && !endLevel)
        {
            TogglePause();
        }
        if (player != null)
        {
            if (player.dead)
            {
                loseScreen.SetActive(true);
                EndLevel();
            }
            else if (player.win)
            {
                winScreen.SetActive(true);
                EndLevel();
            }
        }
    }

    void EndLevel()
    {
        endLevel = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
