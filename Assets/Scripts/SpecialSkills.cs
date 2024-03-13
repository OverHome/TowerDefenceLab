using System;
using UnityEngine;

public class SpecialSkills : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    
    private float _launchForce = 10f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LaunchFireball();
        }
    }

    public void LaunchFireball()
    {
        var fireball = Instantiate(fireballPrefab, transform.position, new Quaternion());
        Vector3 launchDirection = Camera.main.transform.forward;
        
        Rigidbody projectileRb = fireball.GetComponent<Rigidbody>();
        projectileRb.AddForce(launchDirection * _launchForce, ForceMode.Impulse);
        fireball.transform.LookAt(fireball.transform.position + launchDirection);
    }
    
}
