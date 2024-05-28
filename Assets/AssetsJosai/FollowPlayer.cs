using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Referencia al transform del jugador
    public Vector3 offset = new Vector3(0f, 0f, 0f); // Desplazamiento de la c�mara con respecto al jugador

    void Update()
    {
        if (playerTransform != null)
        {
            // Actualiza la posici�n de la c�mara para seguir al jugador con el desplazamiento dado
            transform.position = playerTransform.position + offset;
        }
    }
}

