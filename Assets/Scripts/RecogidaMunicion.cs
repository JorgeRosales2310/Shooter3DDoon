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
            // Busca TODOS los scripts Disparar en el jugador y sus hijos (incluso las armas inactivas)
            Disparar[] armas = other.GetComponentsInChildren<Disparar>(true);
            if (armas.Length > 0)
            {
                foreach (Disparar disparar in armas)
                {
                    disparar.AnadirMunicion(cantidad);
                }
                
                if (sonidoRecoger != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}
