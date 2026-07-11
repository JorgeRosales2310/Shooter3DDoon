using UnityEngine;

public class RecogidaMunicion : MonoBehaviour
{
    public int cantidad = 10;
    public AudioClip sonidoRecoger;

    private void OnTriggerEnter(Collider other)
    {
        // Asegúrate de que tu jugador tenga el Tag "Player"
        if (other.CompareTag("Player"))
        {
            // Busca el script Disparar en el jugador o en sus hijos (como la cámara)
            Disparar disparar = other.GetComponentInChildren<Disparar>();
            if (disparar != null)
            {
                disparar.AnadirMunicion(cantidad);
                
                if (sonidoRecoger != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}
