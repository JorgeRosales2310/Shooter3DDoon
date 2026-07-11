using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    [Header("UI - Game Over / Victoria")]
    public GameObject panelGameOver;
    public TextMeshProUGUI textoGameOver; // Para decir "Game Over" o "Victoria"
    
    [Header("UI - Enemigos")]
    public TextMeshProUGUI textoEnemigos;

    private int enemigosRestantes = 0;
    private bool juegoTerminado = false;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
        ActualizarTextoEnemigos();
    }

    public void RegistrarEnemigo()
    {
        enemigosRestantes++;
        ActualizarTextoEnemigos();
    }

    public void EnemigoMuerto()
    {
        enemigosRestantes--;
        ActualizarTextoEnemigos();
    }

    void ActualizarTextoEnemigos()
    {
        if (textoEnemigos != null)
        {
            textoEnemigos.text = "Enemigos: " + enemigosRestantes;
        }
    }

    public void GameOver()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        Cursor.lockState = CursorLockMode.None; // Liberar el cursor
        Cursor.visible = true;

        if (panelGameOver != null) panelGameOver.SetActive(true);
        if (textoGameOver != null) textoGameOver.text = "GAME OVER";

        DesactivarJugador();
    }

    public void Victoria()
    {
        if (juegoTerminado) return;

        if (enemigosRestantes <= 0)
        {
            juegoTerminado = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (panelGameOver != null) panelGameOver.SetActive(true);
            if (textoGameOver != null) textoGameOver.text = "¡VICTORIA!";

            DesactivarJugador();
        }
        else
        {
            Debug.Log("Faltan enemigos por matar.");
        }
    }

    void DesactivarJugador()
    {
        // Busca al jugador para desactivar su movimiento y disparo
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            PrimeraPersona movimiento = jugador.GetComponent<PrimeraPersona>();
            if (movimiento != null) movimiento.enabled = false;

            Disparar[] disparos = jugador.GetComponentsInChildren<Disparar>(true);
            foreach (Disparar disparo in disparos)
            {
                if (disparo != null) disparo.enabled = false;
            }
        }
    }

    // Función que llamará el botón "Reintentar"
    public void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

