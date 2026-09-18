using UnityEngine;
using UnityEngine.XR;

public class VRDroneManager : MonoBehaviour
{
    [Header("Referências do VR e Drone")]
    public GameObject xrOrigin;          // O jogador VR (XR Origin)
    public Camera droneCamera;           // A Câmera anexada ao objeto do Drone na cena
    public Transform droneTransform;     // O Transform do objeto Drone na cena

    [Header("Configurações do Drone")]
    public float droneSpeed = 5.0f;
    public float verticalSpeed = 3.0f;
    public float rotationSpeed = 60.0f;

    [Header("Estado Atual")]
    public bool modoDroneAtivo = false;

    private Camera vrCameraMain;
    private bool yButtonPressedLastFrame = false;

    void Start()
    {
        // Pega a câmera VR principal dentro do XR Origin
        if (Camera.main != null)
        {
            vrCameraMain = Camera.main;
        }

        // Garante que a câmera do drone comece desativada
        if (droneCamera != null)
        {
            droneCamera.enabled = false;
        }
    }

    void Update()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // 1. Alternar Modo Drone ao pressionar o Botão Y (Controle Esquerdo)
        bool buttonYPressed = false;
        leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonYPressed);

        // Detecção de clique (edge detection) para evitar alternar repetidamente se segurar o botão
        if (buttonYPressed && !yButtonPressedLastFrame)
        {
            AlternarModoDrone();
        }
        yButtonPressedLastFrame = buttonYPressed;

        // 2. Se o modo drone estiver ativo, processa a movimentação 3D do Drone
        if (modoDroneAtivo)
        {
            ProcessarMovimentoDrone(leftHand, rightHand);
        }
    }

    void AlternarModoDrone()
    {
    modoDroneAtivo = !modoDroneAtivo;

    if (modoDroneAtivo)
    {
        // Ativa o POV do Drone/BLUEROV e desativa a câmera principal
        if (vrCameraMain != null) vrCameraMain.enabled = false;
        if (droneCamera != null) droneCamera.enabled = true;
        Debug.Log("Modo Drone (BLUEROV) Ativado!");
    }
    else
    {
        // Retorna ao POV Principal
        if (droneCamera != null) droneCamera.enabled = false;
        if (vrCameraMain != null) vrCameraMain.enabled = true;
        Debug.Log("Modo Visão Normal Ativado!");
    }
}

    void ProcessarMovimentoDrone(InputDevice leftHand, InputDevice rightHand)
    {
        if (droneTransform == null) return;

        // --- A. Entrada de Movimento Horizontal (Joystick Esquerdo) ---
        Vector2 leftJoystick = Vector2.zero;
        leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftJoystick);

        // --- B. Entrada de Movimento Vertical (Cima / Baixo) ---
        float subidaDescida = 0f;

        // Usando o Gatilho do Indicador Esquerdo para Subir e Gatilho do Indicador Direito para Descer
        float leftTrigger = 0f;
        float rightTrigger = 0f;
        leftHand.TryGetFeatureValue(CommonUsages.trigger, out leftTrigger);
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out rightTrigger);

        // Subir com gatilho direito, descer com gatilho esquerdo
        subidaDescida = rightTrigger - leftTrigger;

        // Alternativa: Ler botões A (Subir) e B (Descer)
        bool buttonA = false;
        bool buttonB = false;
        rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out buttonA);
        rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonB);

        if (buttonA) subidaDescida = 1.0f;  // Botão A sobe
        if (buttonB) subidaDescida = -1.0f; // Botão B desce

        // --- C. Rotação / Giro do Drone (Joystick Direito) ---
        Vector2 rightJoystick = Vector2.zero;
        rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightJoystick);

        // --- APLICAÇÃO DOS MOVIMENTOS NO DRONE ---

        // Direção horizontal relativa à orientação atual do Drone
        Vector3 moveDirection = (droneTransform.forward * leftJoystick.y) + (droneTransform.right * leftJoystick.x);

        // Adiciona a componente vertical (Subir / Descer no Eixo Y global)
        Vector3 verticalDirection = Vector3.up * subidaDescida;

        // Aplica translação (Horizontal + Vertical)
        Vector3 finalVelocity = (moveDirection * droneSpeed) + (verticalDirection * verticalSpeed);
        droneTransform.Translate(finalVelocity * Time.deltaTime, Space.World);

        // Aplica rotação (Girar no eixo Y)
        float rotationAmount = rightJoystick.x * rotationSpeed * Time.deltaTime;
        droneTransform.Rotate(0, rotationAmount, 0, Space.World);
    }
}