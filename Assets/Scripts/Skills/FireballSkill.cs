using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FireballSkill : MonoBehaviour
{
    [SerializeField] private SpecialSkills specialSkills;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Image fireballButtonImage;
    [SerializeField] private float timeWait = 40;
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private int crystalPrice = 10;
    
    private Camera _camera;
    private LineRenderer _trajectoryLine;
    private Vector3[] _trajectoryPointsArray;
    private Vector3 _startOffset = new Vector3(0, 1, 0);
    private int _trajectoryPoints = 100;
    private float _spacing = 0.1f;
    private Coroutine _calculateCoroutine;
    private Rigidbody _projectileRb;
    private Button _fireballButton;
    private TextMeshProUGUI _priceText;

    private void Start()
    {
        _camera = Camera.main;
        _trajectoryLine = gameObject.AddComponent<LineRenderer>();
        _trajectoryLine.positionCount = _trajectoryPoints;
        _trajectoryLine.startWidth = 0.1f;
        _trajectoryLine.endWidth = 0.1f;
        _trajectoryLine.enabled = false;
        _trajectoryPointsArray = new Vector3[_trajectoryPoints];
        _fireballButton = fireballButtonImage.GetComponent<Button>();
        _priceText = fireballButtonImage.GetComponentInChildren<TextMeshProUGUI>();
        GameManager.Instance.OnCrystalCountEdit.AddListener(ButtonVisibility);
        _priceText.text = crystalPrice.ToString();
        ButtonVisibility();
    }

    private void ButtonVisibility()
    {
        _fireballButton.interactable = GameManager.Instance.TempСrystalCount >= crystalPrice;
    }

    public void PrepareFireball()
    {
        _calculateCoroutine = StartCoroutine(CalculateTrajectoryPoints());
        specialSkills.FireEvent.AddListener(LaunchFireball);
        specialSkills.CanselEvent.AddListener(CancelFireball);
        GameManager.Instance.SpendСrystal(crystalPrice);
    }

    private IEnumerator CalculateTrajectoryPoints()
    {
        _trajectoryLine.enabled = true;
        while (true)
        {
            Vector3 launchDirection = _camera.transform.forward;

            for (int i = 0; i < _trajectoryPoints; i++)
            {
                float time = i * _spacing;
                Vector3 point = _camera.transform.position + _startOffset + launchDirection * launchForce * time + 0.5f * Physics.gravity * time * time;
                _trajectoryPointsArray[i] = point;
            }
            _trajectoryLine.SetPositions(_trajectoryPointsArray);

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void ClearTrajectory()
    {
        for (int i = 0; i < _trajectoryPoints; i++)
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

        fireball.GetComponent<Rigidbody>().AddForce(launchDirection * launchForce, ForceMode.Impulse);
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
        _fireballButton.enabled = true;
        fireballButtonImage.fillAmount = 1f;
        ButtonVisibility();
    }
}
