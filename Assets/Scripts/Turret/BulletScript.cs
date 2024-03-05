using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
           return;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        
        Destroy(gameObject);
    }
}
