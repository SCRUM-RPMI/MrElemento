using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform target; // Referencia al transform del personaje que la cámara seguirá
    public float smoothSpeed = 0.1f; // Velocidad de suavizado del movimiento
    public Vector3 offset = new Vector3(0f, 1.5f, -10f); // Desplazamiento relativo entre la cámara y el personaje

    private Vector3 velocity = Vector3.zero; // Velocidad actual de suavizado

    void FixedUpdate()
    {
        if (target == null)
        {
            return; // No hay objetivo, no seguimos
        }

        // Calcula la posición deseada de la cámara sumando el offset al objetivo
        Vector3 desiredPosition = target.position + offset;

        // Usa SmoothDamp para suavizar el movimiento de la cámara
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Actualiza la posición de la cámara
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}

