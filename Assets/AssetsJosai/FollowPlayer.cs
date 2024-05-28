using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Referencia al transform del jugador
    public Vector3 offset = new Vector3(0f, 0f, 0f); // Desplazamiento de la cámara con respecto al jugador

    void Update()
    {
        if (playerTransform != null)
        {
            // Actualiza la posición de la cámara para seguir al jugador con el desplazamiento dado
            transform.position = playerTransform.position + offset;
        }
    }
}

