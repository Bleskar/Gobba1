using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    //comon dude give me the camera shaker, shake your camera

    public float duration = 1f;
    public float scale = 1f;
    //public AnimationCurve curve;

    private void Update()
    {
        //if (Input.GetKeyDown("h"))
        //{
        //    Shake();
        //}
    }
    public void Shake()
    {
        StartCoroutine(Shaking());
    }


    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            //float strength = curve.Evaluate(elapsedTime/ duration);
            transform.position = startPosition + Random.insideUnitSphere * scale;
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            yield return null;
        }
        transform.position = startPosition;
    }
}
