using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LivesTextChanger : MonoBehaviour
{
    public Pawn player;
    public Text textBox;

    public void Update()
    {
        ChangeLivesText();
    }

    public void ChangeLivesText()
    {
        textBox.text = GameManager.instance.players[0].pawn.currentLives.ToString();
    }
}
