using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float xMovement = 0;
    [SerializeField] float yMovement = 25.0f;
    [SerializeField] float zMovement = 0;
    Vector3 movementVector;
    Vector3 startingPosition;
    [SerializeField] bool positiveMovement = true;
    float movementFactor;
    float oscillationPeriod = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        movementVector = new Vector3(xMovement, yMovement, zMovement);
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveObstacle();
    }

    private void MoveObstacle()
    {
            movementFactor = SinFactor();
            Vector3 movementOffset = movementVector * movementFactor;
        if (!positiveMovement)
        {
            movementOffset = movementOffset * -1;
        }
            transform.position = startingPosition + movementOffset;
    }

    private float SinFactor()
    {
        if(oscillationPeriod <= Mathf.Epsilon) { return 0f; }

        const float tau = Mathf.PI * 2;
        float cycles = Time.time / oscillationPeriod;

        float rawSinWave = Mathf.Sin(cycles * tau) * 0.5f + 0.5f;
        return rawSinWave;
    }
}
