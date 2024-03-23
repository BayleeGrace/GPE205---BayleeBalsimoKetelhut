using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    [HideInInspector] public bool isMoving = false;
    // Start is called before the first frame update
    public abstract void Start();

    // When the pawn moves, they move in the direction times x speed
    public abstract void Move(Vector3 direction, float speed);

    public abstract void Rotate(float turnSpeed);

    public abstract void PlayRotateSound();

}
