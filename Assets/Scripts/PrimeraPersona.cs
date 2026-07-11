using UnityEngine;

public class PrimeraPersona : MonoBehaviour
{
    
    public float velocidad = 5f;
    public float sensibilidad = 2f;
    public float gravedad = -9.81f;
    public Transform camara;

    private CharacterController cc;
    private float pitch = 0f;
    private Vector3 velY;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        
        // Asegurarse de que la cámara sea hija del jugador
        if (camara != null && camara.parent != transform)
        {
            camara.SetParent(transform);
        }
        
        // Centrar la cámara en los ejes X y Z para evitar que "orbite" alrededor del jugador
        if (camara != null)
        {
            Vector3 posLocal = camara.localPosition;
            posLocal.x = 0;
            posLocal.z = 0;
            camara.localPosition = posLocal;
        }
    }

    void Update()
    {
        //Mirar con el raton
        float mx = Input.GetAxis("Mouse X") * sensibilidad;
        float my = Input.GetAxis("Mouse Y") * sensibilidad;
        
        // Girar el cuerpo alrededor del eje Y del mundo (para evitar ejes extraños)
        transform.Rotate(0, mx, 0, Space.World); 
        
        pitch = Mathf.Clamp(pitch - my, -80f, 80f); // mirar arriba y abajo
        
        if (camara != null)
        {
            camara.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        // Caminar (WASD o Flechas)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 mov = (transform.right * h + transform.forward * v).normalized * velocidad;

        // Gravedad simple
        if (cc.isGrounded && velY.y < 0) velY.y = -2f;
        velY.y += gravedad * Time.deltaTime;

        cc.Move((mov + velY) * Time.deltaTime);
    }
}
