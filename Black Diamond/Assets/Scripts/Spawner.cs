using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // public GameObject prefab;
    public SkiPipes prefab; 
    public float SpawnRate = 1f; //seconds
    public float MinHeight = -1f;
    public float MaxHeight = 1f;
    public float VerticalGap = 3f;

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
            SkiPipes skiPipes = Instantiate(prefab, transform.position, Quaternion.identity);     
                                        // yo-- every time I invoke spawn, I want you to instantiate my prefab
                                        // at a position, with no rotation (Quaternion.identity)

        skiPipes.transform.position += Vector3.up * Random.Range(MinHeight, MaxHeight);
        skiPipes.gap = VerticalGap;
         }
    }                                   // remember when I invoked spawn? Yeah, I want you to get a random value
                                        // by multiplying our min/max height (-1 to 1 for fractions duh) along the 
                                        // y-axis but let's say Vector3.up -- cool so now I want you to make our 
                                        // position of our new instantiation of the pipes to be that random value 

// --------------------------------------------------------------------------------------------- END BRACKET
}
