using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void Damage(int dmg, Vector2 knockback);
    void Kill();
}
