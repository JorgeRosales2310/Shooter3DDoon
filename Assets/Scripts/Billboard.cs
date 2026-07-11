using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera camaraPrincipal;

    void Start()
    {
        camaraPrincipal = Camera.main;
    }

    void LateUpdate()
    {
        if (camaraPrincipal != null)
        {
            // Hace que la imagen siempre mire hacia la cámara (Estilo DOOM)
            transform.forward = camaraPrincipal.transform.forward;
        }
    }
}
