using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject menu;
    public bool isMenuActive;

    void Awake()
    {
        // int numGameControllers = FindObjectsOfType<GameController>().Length;
        // if (numGameControllers > 1)
        // {
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     DontDestroyOnLoad(gameObject);
        // }
    }

    void Start()
    {
        menu.SetActive(false);
        isMenuActive = false;
    }


    public void ActivateMenu()
    {
        menu.SetActive(true);
        isMenuActive = true;
    }

    public void DisableMenu()
    {
        menu.SetActive(false);
        isMenuActive = false;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
