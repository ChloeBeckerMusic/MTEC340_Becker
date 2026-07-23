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

    private int _nextWaffleGate = 21;
    private bool _waffleExists = false;
    [SerializeField] private WaffleBehavior _wafflePrefab;

// --------------------------------------------------------------------------------------------- 

    private void OnEnable()
    {

        InvokeRepeating(nameof(Spawn), SpawnRate, SpawnRate);
                                        // how long until you invoke spawn, how many seconds between each spawn
        
    }
// --------------------------------------------------------------------------------------------- 

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }


// --------------------------------------------------------------------------------------------- 


    private void Spawn()
    {
        if (GameBehavior.Instance.State == Utilities.GameState.Play)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, _prefabs.Length);
            } while (randomIndex == _lastPrefabIndex);              // while recognizes that if there's a duplicate,
                                                                    // it won't go ahead with that one

            _lastPrefabIndex = randomIndex;

            SkiPipes skiPipes = Instantiate(
                _prefabs[randomIndex],
                transform.position,
                Quaternion.identity);


            skiPipes.transform.position += Vector3.up * Random.Range(MinHeight, MaxHeight);
            skiPipes.gap = VerticalGap;

            if (!_waffleExists &&                                                      // also waffle if it is "in play" 
                !GameBehavior.Instance.HasWaffle &&
                GameBehavior.Instance.Score >= _nextWaffleGate)
            {
                // Move the next opportunity 7 gates ahead
                _nextWaffleGate += 7;

                // 50% chance to actually spawn
                if (Random.value < 0.5f)    // 50% chance the waffle spawns every 7 gates 
                {
                    SpawnWaffle(skiPipes);
                }
            }
        }
    }

// --------------------------------------------------------------------------------------------- 

    private void SpawnWaffle(SkiPipes skiPipes)                                     // waffle between ski pipes
    {
        WaffleBehavior waffle = Instantiate(
            _wafflePrefab,
            skiPipes.transform.position,
            Quaternion.identity);

        waffle.transform.position += Vector3.left * 2.5f;

        _waffleExists = true;
    }

// --------------------------------------------------------------------------------------------- 

    public void WaffleCollected()
    {
        _waffleExists = false;

        // Wait 20 gates before another waffle can spawn.
        _nextWaffleGate = GameBehavior.Instance.Score + 20;
    }

// --------------------------------------------------------------------------------------------- 

    public void WaffleMissed()
    {
        _waffleExists = false;

        // Do NOT touch _nextWaffleGate.
        // It already advanced by 7 when this waffle spawned.
    }

// --------------------------------------------------------------------------------------------- 

    public void ResetSpawner()                                                // these are random vals I chose btw
                                                                              // (21 and 7)
    {
        _nextWaffleGate = 21;
        _waffleExists = false;
    }

// --------------------------------------------------------------------------------------------- END BRACKET
}
