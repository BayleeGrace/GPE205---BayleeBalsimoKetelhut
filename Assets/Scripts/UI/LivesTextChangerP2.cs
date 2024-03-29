using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LivesTextChangerP2 : MonoBehaviour
{
    public GameObject player;
    public Text textBox;

    public void Update()
    {
        ChangeLivesText();
    }

    public void ChangeLivesText()
    {
        textBox.text = GameManager.instance.players[1].pawn.currentLives.ToString();
    }
}
