using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private bool hasScored = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (hasScored) return;

        if (collision.CompareTag("Player"))
        {
           
            hasScored = true;
            GameManager.Instance.AddScore();
        }
    }
    public void ResetZone()
    {
        hasScored = false;
    }
}
