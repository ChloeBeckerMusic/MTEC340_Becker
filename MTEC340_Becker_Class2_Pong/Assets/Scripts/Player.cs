using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
      // backing variable
    private int _score;

    public int Score
    {
        get => _score;
        
        set
        {
                _score = value;
                _scoreText.text = Score.ToString();
        }

    }

    [SerializeField] private TMP_Text _scoreText;
}
