using System;
using System.Collections;
using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    [SerializeField] private SpecialSkills specialSkills;
    [SerializeField] private GameObject fireballPrefab;
    public int trajectoryPoints = 100;
    public float minLaunchForce = 5f;
    public float maxLaunchForce = 20;
    private float _launchForce = 10f;
    public float spacing = 0.1f;

    private Camera _camera;
    private LineRenderer _trajectoryLine;
    private Vector3[] _trajectoryPointsArray;
    private Vector3 _startOffset = new Vector3(0, 1, 0);
    private Coroutine _calculateCoroutine;
    
    void Start()
    {
        _camera = Camera.main;
        _trajectoryLine = gameObject.AddComponent<LineRenderer>();
        _trajectoryLine.positionCount = trajectoryPoints;
        _trajectoryLine.startWidth = 0.1f;
        _trajectoryLine.endWidth = 0.1f;
        _trajectoryLine.enabled = false;
        _trajectoryPointsArray = new Vector3[trajectoryPoints];
    }

    public void PrepareFireball()
    {
        _calculateCoroutine = StartCoroutine(CalculateTrajectoryPoints());
        specialSkills.FireEvent.AddListener(LaunchFireball);
        specialSkills.CanselEvent.AddListener(CanselFireball);
        specialSkills.EditEvent.AddListener(EditFireball);
    }
    private IEnumerator CalculateTrajectoryPoints()
    {
        _trajectoryLine.enabled = true;
        while (true)
        {
            Vector3 launchDirection = _camera.transform.forward;
        
            for (int i = 0; i < trajectoryPoints; i++)
            {
                float time = i * spacing;
                Vector3 point = _camera.transform.position + _startOffset + launchDirection * _launchForce * time + 0.5f * Physics.gravity * time * time;
                _trajectoryPointsArray[i] = point;
            }
            _trajectoryLine.SetPositions(_trajectoryPointsArray);
            
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    private void ClearTrajectory()
    {
        for (int i = 0; i < trajectoryPoints; i++)
        {
            _trajectoryPointsArray[i] = Vector3.zero;
        }
        
        _trajectoryLine.enabled = false;
    }

    private void LaunchFireball()
    {
        StopCoroutine(_calculateCoroutine);
        ClearTrajectory();
        var fireball = Instantiate(fireballPrefab, _camera.transform.position + _startOffset, new Quaternion());
        Vector3 launchDirection = _camera.transform.forward;
        
        Rigidbody projectileRb = fireball.GetComponent<Rigidbody>();
        projectileRb.AddForce(launchDirection * _launchForce, ForceMode.Impulse);
        fireball.transform.LookAt(fireball.transform.position + launchDirection);
        specialSkills.FireEvent.RemoveListener(LaunchFireball);
        specialSkills.CanselEvent.RemoveListener(CanselFireball);
        specialSkills.EditEvent.RemoveListener(EditFireball);
    }

    private void CanselFireball()
    {
        StopCoroutine(_calculateCoroutine);
        ClearTrajectory();
        specialSkills.FireEvent.RemoveListener(LaunchFireball);
        specialSkills.CanselEvent.RemoveListener(CanselFireball);
        specialSkills.EditEvent.RemoveListener(EditFireball);
    }
    
    private void EditFireball(float value)
    {
        _launchForce =  Mathf.Lerp(minLaunchForce, maxLaunchForce, value);
    }
}
