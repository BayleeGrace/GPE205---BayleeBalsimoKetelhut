using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider healthSlider;
    //public Pawn pawn;

    void Update()
    {
        OnHealthChange();
    }

    public void OnHealthChange()
    {
        Pawn pawn = GameManager.instance.players[0].pawn;
        Health pawnHealth = pawn.GetComponent<Health>();
        healthSlider.value = pawnHealth.currentHealth;
    }
}
