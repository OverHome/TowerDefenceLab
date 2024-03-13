using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireballSkill : MonoBehaviour
{
    [SerializeField] private SpecialSkills specialSkills;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Image fireballButtonImage;
    [SerializeField] private int trajectoryPoints = 100;
    [SerializeField] private float minLaunchForce = 5f;
    [SerializeField] private float maxLaunchForce = 20;
    [SerializeField] private float spacing = 0.1f;
    [SerializeField] private float timeWait = 40;

    private float _launchForce = 10f;
    private Camera _camera;
    private LineRenderer _trajectoryLine;
    private Vector3[] _trajectoryPointsArray;
    private Vector3 _startOffset = new Vector3(0, 1, 0);
    private Coroutine _calculateCoroutine;
    private Rigidbody _projectileRb;
    private Button _fireballButton;

    private void Start()
    {
        _camera = Camera.main;
        _trajectoryLine = gameObject.AddComponent<LineRenderer>();
        _trajectoryLine.positionCount = trajectoryPoints;
        _trajectoryLine.startWidth = 0.1f;
        _trajectoryLine.endWidth = 0.1f;
        _trajectoryLine.enabled = false;
        _trajectoryPointsArray = new Vector3[trajectoryPoints];
        _fireballButton = fireballButtonImage.GetComponent<Button>();
    }

    public void PrepareFireball()
    {
        _calculateCoroutine = StartCoroutine(CalculateTrajectoryPoints());
        specialSkills.FireEvent.AddListener(LaunchFireball);
        specialSkills.CanselEvent.AddListener(CancelFireball);
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
        StopCalculateCoroutine();
        ClearTrajectory();
        var fireball = Instantiate(fireballPrefab, _camera.transform.position + _startOffset, Quaternion.identity);
        Vector3 launchDirection = _camera.transform.forward;

        fireball.GetComponent<Rigidbody>().AddForce(launchDirection * _launchForce, ForceMode.Impulse);
        fireball.transform.LookAt(fireball.transform.position + launchDirection);
        RemoveEventListeners();
        StartCoroutine(TimeOut());
    }

    private void CancelFireball()
    {
        StopCalculateCoroutine();
        ClearTrajectory();
        RemoveEventListeners();
    }

    private void EditFireball(float value)
    {
        _launchForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, value);
    }

    private void StopCalculateCoroutine()
    {
        if (_calculateCoroutine != null)
        {
            StopCoroutine(_calculateCoroutine);
        }
    }

    private void RemoveEventListeners()
    {
        specialSkills.FireEvent.RemoveListener(LaunchFireball);
        specialSkills.CanselEvent.RemoveListener(CancelFireball);
        specialSkills.EditEvent.RemoveListener(EditFireball);
    }

    private IEnumerator TimeOut()
    {
        _fireballButton.enabled = false;
        float timer = 0f;

        while (timer <= timeWait)
        {
            fireballButtonImage.fillAmount = Mathf.Lerp(0, 1, timer / timeWait);
            timer += Time.deltaTime;
            yield return null;
        }

        fireballButtonImage.fillAmount = 1f;
        _fireballButton.enabled = true;
    }
}
