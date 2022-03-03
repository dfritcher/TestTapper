using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _player1ScoreDisplay;
    [SerializeField] private TextMeshProUGUI _player2ScoreDisplay;


    private void Start()
    {
        _player1ScoreDisplay.text = "P1 Score: ";
        _player2ScoreDisplay.text = "P2 Score: ";
    }

    public void UpdateScoreDisplay(int player1Score, int player2Score)
    {
        _player1ScoreDisplay.text = $"P1 Score: {player1Score}";
        _player2ScoreDisplay.text = $"P2 Score: {player2Score}";
    }
}
