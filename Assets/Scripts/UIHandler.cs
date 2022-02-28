using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Score Overlay")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    private int score;
    public int Score { get { return score; } }

}
