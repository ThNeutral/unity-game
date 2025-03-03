using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StafProjectile : MonoBehaviour
{
    private bool allowInvoke = true;

    private void Update()
    {
        if (allowInvoke)
        {
            Invoke(nameof(BulletDeath), 2f);
            allowInvoke = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Invoke(nameof(BulletDeath), 0.05f);
    }

    private void BulletDeath()
    {
        Destroy(gameObject);
    }
}
