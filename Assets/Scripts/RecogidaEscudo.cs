using UnityEngine;

public class RecogidaEscudo : MonoBehaviour
{
    public int cantidadEscudo = 2;
    public AudioClip sonidoRecoger;

    private void OnTriggerEnter(Collider other)
    {
        // Asegurarse de que sea el jugador
        if (other.CompareTag("Player"))
        {
            Vida vidaJugador = other.GetComponent<Vida>();
            if (vidaJugador == null) vidaJugador = other.GetComponentInParent<Vida>();

            if (vidaJugador != null)
            {
                vidaJugador.AnadirEscudo(cantidadEscudo);
                
                if (sonidoRecoger != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
                }

                // Destruye el objeto del escudo en el mapa tras agarrarlo
                Destroy(gameObject);
            }
        }
    }
}
