using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlToTexture : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Texture2D tex;
    public float scale = 1f;

    public string url = "";
    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            StartCoroutine(Deez());
        }
    }

    public IEnumerator Deez()
    {
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            if (tex.width < tex.height)
            {
                spriteRenderer.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.width), new Vector2(0.5f, 0.5f), tex.width / scale);
            }
            else
            {
                spriteRenderer.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.height, tex.height), new Vector2(0.5f, 0.5f), tex.height / scale);
            }
        }
    }
}
