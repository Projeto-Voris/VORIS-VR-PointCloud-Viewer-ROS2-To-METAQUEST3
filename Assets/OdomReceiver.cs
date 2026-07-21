
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using OdometryMsg = RosMessageTypes.Nav.OdometryMsg;

public class OdomReceiver : MonoBehaviour
{
    ROSConnection ros;
    
    // Deixamos público para você poder trocar o nome do tópico direto na Unity
    public string topicName = "/run_slam/camera_pose"; 

    void Start()
    {
        // Pega a conexão com a Jetson e assina o tópico
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<OdometryMsg>(topicName, ReceiveOdometry);
    }

    void ReceiveOdometry(OdometryMsg msg)
    {
        // --- 1. POSIÇÃO (X, Y, Z) ---
        float posX = (float)msg.pose.pose.position.x;
        float posY = (float)msg.pose.pose.position.y;
        float posZ = (float)msg.pose.pose.position.z;
        
        Vector3 unityPosition = new Vector3(-posY, posZ, posX);
        transform.position = unityPosition;

        // --- 2. ORIENTAÇÃO / ROTAÇÃO (QUATERNION) ---
        float rotX = (float)msg.pose.pose.orientation.x;
        float rotY = (float)msg.pose.pose.orientation.y;
        float rotZ = (float)msg.pose.pose.orientation.z;
        float rotW = (float)msg.pose.pose.orientation.w;
        
        // Espião: Imprime no console os valores brutos de rotação chegando do ROS
        Debug.Log($"Rotação ROS -> X: {rotX:F3} | Y: {rotY:F3} | Z: {rotZ:F3} | W: {rotW:F3}");

        // Conversão Padrão de eixos de rotação: ROS para Unity
        Quaternion unityRotation = new Quaternion(rotY, -rotZ, -rotX, rotW);
        transform.rotation = unityRotation;
    }
}