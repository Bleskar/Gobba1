using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyBase _enemyMov;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3((float)_enemyMov.Health / (float)_enemyMov.maxHealth, transform.localScale.y, 1);
    }
}
