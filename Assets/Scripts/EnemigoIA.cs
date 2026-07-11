using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemigoIA : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform jugador;
    private Vida vidaJugador;

    [Header("Movimiento")]
    public float distanciaDeteccion = 30f; // Distancia a la que el enemigo te "ve"
    public float distanciaParar = 8f; // Distancia a la que se detiene y dispara

    [Header("Ataque a distancia")]
    public int dano = 1;
    public float alcance = 20f;
    public float cadenciaDisparo = 1.5f;
    public AudioClip sonidoDisparo;
    public GameObject muzzleFlash;

    private float proximoDisparo = 0f;
    private AudioSource audioSource;
    private bool estaMuerto = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
            vidaJugador = objJugador.GetComponent<Vida>();
            if (vidaJugador == null) vidaJugador = objJugador.GetComponentInParent<Vida>();
        }
        else
        {
            Debug.LogWarning("EnemigoIA: No se encontró un objeto con Tag 'Player'.");
        }

        if (GameManager.Instancia != null)
            GameManager.Instancia.RegistrarEnemigo();

        if (muzzleFlash != null) muzzleFlash.SetActive(false);
    }

    void Update()
    {
        if (jugador == null || estaMuerto) return;

        if (vidaJugador != null && vidaJugador.VidaActual() <= 0)
        {
            agent.isStopped = true;
            return; // Detener toda acción si el jugador está muerto
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia > distanciaDeteccion)
        {
            // El jugador está muy lejos, el enemigo se queda quieto
            agent.isStopped = true;
            return;
        }

        if (distancia > distanciaParar)
        {
            // Perseguir al jugador
            agent.isStopped = false;
            agent.SetDestination(jugador.position);
        }
        else
        {
            // Detenerse, mirar al jugador y disparar
            agent.isStopped = true;
            MirarAlJugador();

            if (Time.time >= proximoDisparo)
            {
                proximoDisparo = Time.time + cadenciaDisparo;
                Disparar();
            }
        }
    }

    void MirarAlJugador()
    {
        Vector3 dir = (jugador.position - transform.position);
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), Time.deltaTime * 8f);
    }

    void Disparar()
    {
        if (sonidoDisparo != null) audioSource.PlayOneShot(sonidoDisparo);
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            Invoke("ApagarMuzzle", 0.15f);
        }

        // Origen del disparo: posición del enemigo, un poco hacia arriba 
        Vector3 origen = transform.position + Vector3.up * 1.2f;
        // Dirección hacia el pecho del jugador
        Vector3 destino = jugador.position + Vector3.up * 0.8f;
        Vector3 direccion = (destino - origen).normalized;

        Debug.DrawRay(origen, direccion * alcance, Color.red, 0.5f);

        // El Raycast ignora el propio enemigo usando su layer
        int maskEnemigo = ~(1 << gameObject.layer);

        if (Physics.Raycast(origen, direccion, out RaycastHit hit, alcance, maskEnemigo))
        {
            Debug.Log("Enemigo disparo y golpeó: " + hit.collider.gameObject.name);
            Vida v = hit.collider.GetComponent<Vida>();
            if (v == null) v = hit.collider.GetComponentInParent<Vida>();
            
            if (v != null && v.esJugador)
            {
                v.RecibirDano(dano);
            }
        }
    }

    void ApagarMuzzle()
    {
        if (muzzleFlash != null) muzzleFlash.SetActive(false);
    }

    private void OnDestroy()
    {
        estaMuerto = true;
    }
}
