using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderP2 : MonoBehaviour
{
    public Slider healthSlider;
    //public Pawn pawn;

    void Update()
    {
        OnHealthChange();
    }

    public void OnHealthChange()
    {
        Pawn pawn = GameManager.instance.players[1].pawn;
        Health pawnHealth = pawn.GetComponent<Health>();
        healthSlider.value = pawnHealth.currentHealth;
    }
}
