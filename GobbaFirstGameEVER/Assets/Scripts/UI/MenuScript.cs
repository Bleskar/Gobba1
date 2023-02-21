using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public int scene;
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
