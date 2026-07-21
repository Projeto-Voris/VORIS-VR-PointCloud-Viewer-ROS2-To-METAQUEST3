using UnityEngine;

public class ViewerCamera : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float lookSpeed = 2.0f;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 10.0f;
    public float flySpeed = 5.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        // Salva a rotação inicial para a câmera não dar um "pulo" quando você clicar a primeira vez
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationX = rot.y;
        rotationY = rot.x;
    }

    void Update()
    {
        // 1. ROTACIONAR / OLHAR (Segurar Botão Direito do Mouse)
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
            
            // Destrava a câmera para olhar livremente
            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        // 2. PAN / DESLIZAR (Segurar Botão do Meio/Bolinha do Mouse)
        if (Input.GetMouseButton(2))
        {
            float panX = -Input.GetAxis("Mouse X") * panSpeed;
            float panY = -Input.GetAxis("Mouse Y") * panSpeed;
            transform.Translate(panX, panY, 0, Space.Self);
        }

        // 3. ZOOM (Rolar a bolinha do mouse)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            transform.Translate(0, 0, scroll * zoomSpeed, Space.Self);
        }

        // 4. VOO ESTILO DRONE (Teclado - Opcional)
        float moveForward = Input.GetAxis("Vertical") * flySpeed * Time.deltaTime;    // W / S
        float moveRight = Input.GetAxis("Horizontal") * flySpeed * Time.deltaTime;    // A / D
        
        float moveUp = 0;
        if (Input.GetKey(KeyCode.E)) moveUp = flySpeed * Time.deltaTime;              // Subir
        if (Input.GetKey(KeyCode.Q)) moveUp = -flySpeed * Time.deltaTime;             // Descer

        transform.Translate(moveRight, moveUp, moveForward, Space.Self);
    }
}