using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ojo : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del objeto
    public float tiempoMinParada = 1f; // Tiempo m�nimo de parada
    public float tiempoMaxParada = 3f; // Tiempo m�ximo de parada
    private Vector3 direccion = Vector3.right; // Direcci�n inicial de movimiento
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoverAleatoriamente());
    }

    void Update()
    {
        // Mover el objeto en la direcci�n actual
        rb.MovePosition(rb.position + new Vector2(direccion.x, direccion.y) * velocidad * Time.deltaTime);
    }

    IEnumerator MoverAleatoriamente()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio
            float tiempoParada = Random.Range(tiempoMinParada, tiempoMaxParada);
            yield return new WaitForSeconds(tiempoParada);

            // Decidir una nueva direcci�n aleatoriamente
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
        // Cambiar la direcci�n al chocar con una pared
        if (col.gameObject.CompareTag("Pared"))
        {
            direccion = -direccion;
        }
    }
}
