using System.Collections;
using TMPro;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    public Camera camara;
    public int dano = 2;
    public float alcance = 100f;
    public float cadencia = 0.5f;
    public AudioClip sonidoDisparo;
    public GameObject muzzle;
    public GameObject muzzle2;

    [Header("Sistema de Municion")]
    public int tamanoCargador = 10;
    public int municionReserva = 0;
    public float tiempoRecarga = 2f;
    public TextMeshProUGUI textoMunicion;
    private int municionActual;
    private bool recargando = false;

    private AudioSource fuente;
    private float proximo = 0f;

    void Start()
    {
        fuente = GetComponent<AudioSource>();
        if (muzzle != null) muzzle.SetActive(false);
        if (muzzle2 != null) muzzle2.SetActive(false);

        municionActual = tamanoCargador;
        ActualizarTextoMunicion();
    }

    void Update()
    {
        if (recargando) return;

        if (Input.GetKeyDown(KeyCode.R) && municionActual < tamanoCargador && municionReserva > 0)
        {
            StartCoroutine(Recargar());
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= proximo)
        {
            if (municionActual > 0)
            {
                proximo = Time.time + cadencia;
                Disparo();
            }
            else
            {
                // Podrias anadir un sonido de "click" de arma vacia aqui
            }
        }    
    }

    IEnumerator Recargar()
    {
        recargando = true;
        if (textoMunicion != null) textoMunicion.text = "Recargando...";
        
        yield return new WaitForSeconds(tiempoRecarga);
        
        int balasFaltantes = tamanoCargador - municionActual;
        int balasARecargar = Mathf.Min(balasFaltantes, municionReserva);

        municionActual += balasARecargar;
        municionReserva -= balasARecargar;

        recargando = false;
        ActualizarTextoMunicion();
    }

    public void AnadirMunicion(int cantidad)
    {
        municionReserva += cantidad;
        ActualizarTextoMunicion();
    }

    void Disparo()
    {
        municionActual--;
        ActualizarTextoMunicion();

        if (sonidoDisparo != null) fuente.PlayOneShot(sonidoDisparo);
        
        bool hayMuzzle = false;

        if (muzzle != null) 
        { 
            muzzle.SetActive(true);
            hayMuzzle = true;
        }

        if (muzzle2 != null) 
        { 
            muzzle2.SetActive(true);
            hayMuzzle = true;
        }

        if (hayMuzzle)
        {
            Invoke("ApagarMuzzle", 0.05f);
        }

        Ray ray = camara.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, alcance))
        {
            Vida v = hit.collider.GetComponentInParent<Vida>();
            if (v != null) v.RecibirDano(dano);
        }
    }

    void ActualizarTextoMunicion()
    {
        if (textoMunicion != null)
        {
            textoMunicion.text = municionActual + " / " + municionReserva;
        }
    }

    void ApagarMuzzle()
    {
        if (muzzle != null) muzzle.SetActive(false);
        if (muzzle2 != null) muzzle2.SetActive(false);
    }
}
