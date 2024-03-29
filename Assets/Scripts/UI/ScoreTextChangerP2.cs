using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextChangerP2 : MonoBehaviour
{
    //public Pawn player;
    public Text textBox;

    public void Update()
    {
        ChangeScoreText();
    }

    public void ChangeScoreText()
    {
        textBox.text = GameManager.instance.players[1].playerScore.ToString();
    }
}
