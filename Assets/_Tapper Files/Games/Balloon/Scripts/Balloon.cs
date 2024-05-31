using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float _minFallSpeed = 100f;
    [SerializeField] private float _maxFallSpeed = 300f;
    [SerializeField] private float fallSpeed = 100f;
    [SerializeField] private float _minSize = .25f;
    [SerializeField] private float _maxSize = 3f;

    private Image _ballonImage = null;
    private float _width;

    public Vector3 StartPosition;

    private void Awake()
    {
        _ballonImage = GetComponent<Image>();
        _width = ((RectTransform)_ballonImage.transform).rect.width;
        StartPosition = new Vector3(Screen.width / 2, StartPosition.y, StartPosition.z );
    }

    private void Update()
    {
        if (gameObject.activeSelf)
            transform.position = new Vector3(transform.position.x, transform.position.y + fallSpeed * Time.deltaTime, transform.position.z);

        if (transform.position.y > Screen.height)
            Reset();
    }

    public void SetSpawnLocation(Vector3 spawnPosition)
    {
        StartPosition = new Vector3(StartPosition.x, spawnPosition.y, StartPosition.z);
        transform.position = StartPosition;
    }

    public void ActivateBalloon()
    {
        SetRandomFallSpeed();
        SetRandomSize();
        SetRandomHorizontalPosition();
        SetActiveState(true);
    }
    public void Reset()
    {
        SetActiveState(false);
        transform.position = StartPosition;
    }


    private void SetRandomHorizontalPosition()
    {
        var randXPos = Random.Range(0, Screen.width);
        transform.position = new Vector3(randXPos, transform.position.y, transform.position.z);
    }

    private void SetRandomFallSpeed()
    {
        fallSpeed = Random.Range(_minFallSpeed, _maxFallSpeed);
    }


    public void SetRandomSize()
    {
        var randNum = Random.Range(_minSize, _maxSize);
        transform.localScale = new Vector3(randNum, randNum, 1);
    }

    public void SetActiveState(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }
}
