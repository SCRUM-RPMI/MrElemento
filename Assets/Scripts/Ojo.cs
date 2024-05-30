using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ojo : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del objeto
    public float tiempoMinParada = 1f; // Tiempo mínimo de parada
    public float tiempoMaxParada = 3f; // Tiempo máximo de parada
    private Vector3 direccion = Vector3.right; // Dirección inicial de movimiento
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoverAleatoriamente());
    }

    void Update()
    {
        // Mover el objeto en la dirección actual
        rb.MovePosition(rb.position + new Vector2(direccion.x, direccion.y) * velocidad * Time.deltaTime);
    }

    IEnumerator MoverAleatoriamente()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio
            float tiempoParada = Random.Range(tiempoMinParada, tiempoMaxParada);
            yield return new WaitForSeconds(tiempoParada);

            // Decidir una nueva dirección aleatoriamente
            if (Random.value > 0.5f)
            {
                direccion = Vector3.right;
            }
            else
            {
                direccion = Vector3.left;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Cambiar la dirección al chocar con una pared
        if (col.gameObject.CompareTag("Pared"))
        {
            direccion = -direccion;
        }
    }
}
