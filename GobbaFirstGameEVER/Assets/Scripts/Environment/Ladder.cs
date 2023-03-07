using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector3(2f, .5f, 1f);
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<PlayerMovement>())
            return;

        GameManager.Instance.NextLevel();
    }
}
