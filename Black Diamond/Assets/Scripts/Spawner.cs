using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // public GameObject prefab;
    [SerializeField] private SkiPipes[] _prefabs;
    private int _lastPrefabIndex = -1;
    public float SpawnRate = 1f; //seconds
    public float MinHeight = -1f;
    public float MaxHeight = 1f;
    public float VerticalGap = 3f;

    private int _nextWaffleGate = 21 + 7;
    private bool _waffleExists = false;
    [SerializeField] private WaffleBehavior _wafflePrefab;

//0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000//
// --------------------------------------------------------------------------------------------- ON ENABLE

    private void OnEnable()
    {

        InvokeRepeating(nameof(Spawn), SpawnRate, SpawnRate);
                                        // how long until you invoke spawn, how many seconds between each spawn
        
    }
// --------------------------------------------------------------------------------------------- ON DISABLE

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }


// --------------------------------------------------------------------------------------------- SPAWN


    private void Spawn()
    {
        if (GameBehavior.Instance.State == Utilities.GameState.Play)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, _prefabs.Length);
            }
            while (randomIndex == _lastPrefabIndex);     // while recognizes that if there's a duplicate,
                                                         // it won't go ahead with that one

            _lastPrefabIndex = randomIndex;

            SkiPipes skiPipes = Instantiate(
                _prefabs[randomIndex],
                transform.position,
                Quaternion.identity);
                                      

        skiPipes.transform.position += Vector3.up * Random.Range(MinHeight, MaxHeight);
        skiPipes.gap = VerticalGap;

        if (!_waffleExists)
        {
            SpawnWaffle(skiPipes);
        }

        }                                 

        }

    private void SpawnWaffle(SkiPipes skiPipes)
    {
        WaffleBehavior waffle = Instantiate(
            _wafflePrefab,
            skiPipes.transform.position,
            Quaternion.identity);

        waffle.transform.position += Vector3.left * 2.5f;

        _waffleExists = true;
    }

// --------------------------------------------------------------------------------------------- END BRACKET
}
