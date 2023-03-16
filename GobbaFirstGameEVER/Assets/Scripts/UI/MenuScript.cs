using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public int scene;
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void Play()
    {
        SceneManager.LoadScene(scene);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
