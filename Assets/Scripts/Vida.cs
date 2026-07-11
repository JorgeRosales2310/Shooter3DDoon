using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    public int vidaMax = 3;
    public bool esJugador = false;
    
    [Header("UI Jugador")]
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoEscudo;

    [Header("Escudo")]
    public int escudoMax = 5;
    private int escudoActual = 0;

    [Header("Feedback de Daño")]
    public Image pantallaRoja; // Asignar en el inspector
    public float velocidadFade = 2f;

    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMax;
        escudoActual = 0; // Empieza con 0 de escudo
        if (pantallaRoja != null)
        {
            Color c = pantallaRoja.color;
            c.a = 0f;
            pantallaRoja.color = c;
        }
        ActualizarTextoVida();
    }

    public void RecibirDano(int cantidad)
    {
        // Primero quitar escudo si hay
        if (escudoActual > 0)
        {
            if (cantidad >= escudoActual)
            {
                cantidad -= escudoActual;
                escudoActual = 0;
            }
            else
            {
                escudoActual -= cantidad;
                cantidad = 0;
            }
            ActualizarTextoVida();
        }

        // Si todavía sobra daño, quitar vida
        if (cantidad > 0)
        {
            vidaActual -= cantidad;
            ActualizarTextoVida();
            
            if (esJugador && pantallaRoja != null)
            {
                StopCoroutine(EfectoDano());
                StartCoroutine(EfectoDano());
            }

            if (vidaActual <= 0) Morir();
        }
    }

    public void Curar(int cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMax)
        {
            vidaActual = vidaMax;
        }
        ActualizarTextoVida();
    }

    void ActualizarTextoVida()
    {
        if (esJugador && textoVida != null)
        {
            textoVida.text = "Vida: " + vidaActual;
        }
        if (esJugador && textoEscudo != null)
        {
            textoEscudo.text = "Escudo: " + escudoActual;
        }
    }

    public void AnadirEscudo(int cantidad)
    {
        escudoActual += cantidad;
        if (escudoActual > escudoMax)
        {
            escudoActual = escudoMax;
        }
        ActualizarTextoVida();
    }

    void Morir()
    {
        if (esJugador)
        {
            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.GameOver();
            }
        }
        else
        {
            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.EnemigoMuerto();
            }
            Destroy(gameObject);
        }
    }

    public int VidaActual()
    {
        return vidaActual;
    }

    IEnumerator EfectoDano()
    {
        Color c = pantallaRoja.color;
        c.a = 0.5f; // Opacidad al recibir daño
        pantallaRoja.color = c;

        while (pantallaRoja.color.a > 0f)
        {
            c.a -= Time.deltaTime * velocidadFade;
            pantallaRoja.color = c;
            yield return null;
        }
    }
}
