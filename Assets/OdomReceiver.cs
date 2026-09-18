using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry; // Importação necessária para a conversão de eixos
using PoseStampedMsg = RosMessageTypes.Geometry.PoseStampedMsg;

public class OdomReceiver : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/mavros/local_position/pose_unity";

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool hasNewData = false;
    private int messageCount = 0;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<PoseStampedMsg>(topicName, ReceivePose);
    }

    void ReceivePose(PoseStampedMsg msg)
    {
        // O .From<FLU>() converte automaticamente a posição e orientação do padrão ROS para o da Unity
        targetPosition = msg.pose.position.From<FLU>();
        targetRotation = msg.pose.orientation.From<FLU>();

        messageCount++;
        hasNewData = true;
    }

    void Update()
    {
        if (!hasNewData) return;

        // Aplica posição local para respeitar a hierarquia da cena
        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;

        Debug.Log($"[Msg #{messageCount}] Pose ROS aplicada nativamente.");

        hasNewData = false;
    }
}