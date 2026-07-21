using UnityEngine;
using System;
using Unity.Robotics.ROSTCPConnector;
using PointCloud2Msg = RosMessageTypes.Sensor.PointCloud2Msg;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PointCloudRenderer : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/run_slam/keyframes"; // Substitua pelo seu tópico de mapa se for diferente

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] indices;

    void Start()
    {
        // Inicializa a malha (Mesh) que conversará direto com a GPU
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Suporta mais de 65 mil pontos
        GetComponent<MeshFilter>().mesh = mesh;

        // Assina o tópico do ROS 2
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<PointCloud2Msg>(topicName, ReceivePointCloud);
    }

    void ReceivePointCloud(PointCloud2Msg msg)
    {
        // Calcula a quantidade total de pontos na mensagem
        int numPoints = (int)(msg.data.Length / msg.point_step);
        
        // Espião: Vai gritar no console toda vez que o mapa atualizar!
        Debug.Log($"[Nuvem de Pontos] Recebido pacote com {numPoints} pontos do SLAM!");

        if (vertices == null || vertices.Length != numPoints)
        {
            vertices = new Vector3[numPoints];
            indices = new int[numPoints];
            
            // Cria um mapa de índices sequenciais para a topologia de pontos
            for (int i = 0; i < numPoints; i++)
            {
                indices[i] = i;
            }
        }

        // Garante que estamos lendo os offsets corretos de X, Y, Z (geralmente 0, 4, 8)
        int xOffset = (int)msg.fields[0].offset;
        int yOffset = (int)msg.fields[1].offset;
        int zOffset = (int)msg.fields[2].offset;

        // Decodificação ultrarrápida do array de bytes da mensagem
        for (int i = 0; i < numPoints; i++)
        {
            int baseIndex = i * (int)msg.point_step;

            float rosX = BitConverter.ToSingle(msg.data, baseIndex + xOffset);
            float rosY = BitConverter.ToSingle(msg.data, baseIndex + yOffset);
            float rosZ = BitConverter.ToSingle(msg.data, baseIndex + zOffset);

            // Inversão de eixos idêntica à do Cubo (ROS para Unity)
            vertices[i] = new Vector3(rosX, -rosY, rosZ);        }

        // Atualiza a GPU de uma vez só com as novas posições otimizadas
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Points, 0); // Define que a GPU deve desenhar PONTOS isolados
    }
}