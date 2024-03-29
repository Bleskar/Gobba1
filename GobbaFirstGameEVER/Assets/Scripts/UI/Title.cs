using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public static Title Instance { get; private set; }

    [Header("Refrences")]
    [SerializeField] Text title;
    [SerializeField] Text subTitle;
    [SerializeField] Image background;
    [SerializeField] Text singleTitle;
    [SerializeField] Image singleBackground;

    [Header("Fade options")]
    [SerializeField] float stayTime = .5f;
    [SerializeField] float fadeTime = .5f;

    void Awake()
    {
        Instance = this;
        ResetTitle();
    }

    public static void Activate(string title)
    {
        Instance.StartSingleFade(title);
    }

    public static void Activate(string title, string subTitle)
    {
        Instance.StartFade(title, subTitle);
    }


    public void StartFade(string title, string subTitle)
    {
        this.title.text = title;
        this.subTitle.text = subTitle;

        StartCoroutine(TitleFade());
    }

    IEnumerator TitleFade()
    {
        title.color = Color.white;
        subTitle.color = Color.white;

        background.color = new Color(0f, 0f, 0f, .5f);

        yield return new WaitForSeconds(stayTime);

        float timer = fadeTime;
        while (timer > 0f)
        {
            Color c = Color.Lerp(Color.clear, Color.white, timer / fadeTime);

            title.color = c;
            subTitle.color = c;

            background.color = new Color(0f, 0f, 0f, .5f * c.a);

            timer -= Time.deltaTime;
            yield return null;
        }

        ResetTitle();
    }

    public void StartSingleFade(string title)
    {
        singleTitle.text = title;

        StartCoroutine(SingleTitleFade());
    }

    IEnumerator SingleTitleFade()
    {
        singleTitle.color = Color.white;

        singleBackground.color = new Color(0f, 0f, 0f, .5f);

        yield return new WaitForSeconds(stayTime);

        float timer = fadeTime;
        while (timer > 0f)
        {
            Color c = Color.Lerp(Color.clear, Color.white, timer / fadeTime);

            singleTitle.color = c;
            singleBackground.color = new Color(0f, 0f, 0f, .5f * c.a);

            timer -= Time.deltaTime;
            yield return null;
        }

        ResetTitle();
    }

    void ResetTitle()
    {
        background.color = Color.clear;
        title.color = Color.clear;
        subTitle.color = Color.clear;

        singleTitle.color = Color.clear;
        singleBackground.color = Color.clear;
    }
}
