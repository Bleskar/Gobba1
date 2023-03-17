using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, IKillable
{
    SpriteRenderer sr;
    bool dying;
    Collider2D cd;

    [Header("Death")]
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] DropTable<GameObject> dropPrefabs = new DropTable<GameObject>();

    [SerializeField] int health = 1;   

    // Start is called before the first frame update
    void Start()
    {
        dropPrefabs.Initialize();

        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    public void Damage(int dmg, Vector2 knock, IKillable attacker)
    {
        health -= dmg;
        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        if (dying)
            return;

        AudioManager.Play("Break Box");
        dying = true;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        sr.enabled = false;
        cd.enabled = false;
        deathParticles.Emit(10);

        GameObject drop = dropPrefabs.Drop();
        if (drop)
        {
            GameObject obj = Instantiate(drop, transform.position + new Vector3(Random.value - .5f, Random.value - .5f, 0f).normalized, Quaternion.identity);
            obj.transform.parent = transform.parent;
        }

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
