using UnityEngine;

public class Meta : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Esto aparecerá en la Consola de Unity cada vez que ALGO toque la meta
        Debug.Log("Meta tocada por: " + other.gameObject.name + " | Tag: " + other.tag);

        bool esJugador = other.CompareTag("Player") 
                      || other.GetComponent<PrimeraPersona>() != null
                      || other.GetComponentInParent<PrimeraPersona>() != null;

        if (esJugador)
        {
            Debug.Log("¡VICTORIA! El jugador llegó a la meta.");
            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.Victoria();
            }
            else
            {
                Debug.LogWarning("No se encontró el GameManager en la escena.");
            }
        }
    }
}
