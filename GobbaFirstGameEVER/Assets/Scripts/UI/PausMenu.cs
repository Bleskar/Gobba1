using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PausMenu : MonoBehaviour
{
    bool paused;

    public GameObject pausmenu;

    private void Start()
    {
        Time.timeScale = 1;
        pausmenu.SetActive(false);
        paused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                pausmenu.SetActive(true);
            }
            else
            {
                paused = false;
                Time.timeScale = 1;
                pausmenu.SetActive(false);
            }
        }
    }
    public void Resume()
    {
        Debug.Log("dasd");
        paused = false;
        Time.timeScale = 1;
        pausmenu.SetActive(false);
    }
    public void Restart()
    {
        GameManager.Instance.ResetLevels();
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
