using System.Linq;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    [SerializeField] private Balloon[] _balloons = null;
    [SerializeField] private TimerUtility _timerUtility = null;
    [SerializeField] private GameObject _spawnLocation;
    private bool spawnBalloon = true;
    
    private void Start()
    {
        _timerUtility.OneSecond += SpawnBalloon;
        for (var i = 0; i < _balloons.Length; i++)
        {
            _balloons[i].SetSpawnLocation(_spawnLocation.transform.localPosition);
            if (i % 2 == 0)
                SpawnBalloon();
        }
    }

    private void SpawnBalloon()
    {
        if (!spawnBalloon)
            return;
        else
        {
            spawnBalloon = !spawnBalloon;
        }
        spawnBalloon = !spawnBalloon;

        if (_balloons.Any(b => !b.gameObject.activeSelf))
        {
            _balloons.First(b => !b.gameObject.activeSelf).ActivateBalloon();
        }
    }
}
