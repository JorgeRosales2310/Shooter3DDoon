using UnityEngine;

public class Botiquin : MonoBehaviour
{
    public int cantidadCura = 1;
    public AudioClip sonidoCurar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<PrimeraPersona>() != null)
        {
            Vida vidaJugador = other.GetComponent<Vida>();
            if (vidaJugador != null)
            {
                // Solo curamos si no tiene la vida al máximo
                if (vidaJugador.VidaActual() < vidaJugador.vidaMax)
                {
                    vidaJugador.Curar(cantidadCura);
                    
                    if (sonidoCurar != null)
                    {
                        AudioSource.PlayClipAtPoint(sonidoCurar, transform.position);
                    }
                    
                    Destroy(gameObject);
                }
            }
        }
    }
}
