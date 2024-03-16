using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class FireballScript : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float damageRadius = 2.5f;
    [SerializeField] private float minDamageScale = 0.5f;
    [SerializeField] private float maxDamageScale = 1.2f;

    private float _rotationSpeed = 5f;
    private Rigidbody _rb;
    private ParticleSystem _explosionParticleSystem;
    private MeshRenderer _meshRenderer;
    private bool _damageDealt = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _explosionParticleSystem = explosion.GetComponent<ParticleSystem>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        RotateTowardsMovementDirection();
    }

    private void RotateTowardsMovementDirection()
    {
        if (_rb.velocity.magnitude > 0.1f)
        {
            Vector3 direction = _rb.velocity.normalized;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_damageDealt && other.gameObject.CompareTag("Flore"))
        {
            print(other.gameObject);
            _meshRenderer.enabled = false;
            _rb.useGravity = false;
            _rb.isKinematic = true;
            fire.gameObject.SetActive(false);
            explosion.gameObject.SetActive(true);
            _explosionParticleSystem.Play();
            _damageDealt = true;
            DealDamageInRadius();
            Invoke(nameof(DestroyObject), _explosionParticleSystem.main.duration);
        }
    }

    private void DealDamageInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            EnemyScript healthScript = collider.GetComponent<EnemyScript>();

            if (healthScript != null)
            {
                healthScript.TakeDamage(healthScript.StartHealth*Random.Range(minDamageScale, maxDamageScale));
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
