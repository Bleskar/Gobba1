using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public static DeathMenu Instance { get; private set; }

    [SerializeField] Image background;
    [SerializeField] Text title;
    [SerializeField] GameObject restart;
    [SerializeField] GameObject quit;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        background.color = Color.clear;
        title.color = Color.clear;
        restart.SetActive(false);
        quit.SetActive(false);
    }

    public void Activate()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);

        float t = 0f;
        while (t <= .5f)
        {
            background.color = new Color(0f, 0f, 0f, t);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(.4f);

        t = 0f;
        while (t <= .5f)
        {
            title.color = new Color(1f, 0f, 0f, t);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(.4f);

        restart.SetActive(true);
        quit.SetActive(true);
    }

    public void Restart()
    {
        GameManager.Instance.ResetLevels();
    }
}
