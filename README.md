# VORIS: VR Point Cloud Viewer (ROS2 to Meta Quest 3)

## 📌 Overview
**VORIS** is a Native Unity-based application designed to bridge the gap between robotic perception and immersive Virtual Reality. It enables the real-time visualization of 3D Point Clouds and Odometry data streamed from a ROS2 environment directly into a Meta Quest 3 headset. 

This project aims to provide researchers and developers working with Simultaneous Localization and Mapping (SLAM) and Computer Vision an intuitive, spatial way to inspect sensor data, tracking trajectories, and map generation in a fully untethered (standalone) VR workspace.

## 🚀 Key Features
*   **ROS2 Integration:** Seamlessly receives point cloud arrays and spatial odometry data.
*   **Immersive Visualization:** Renders dense point clouds natively within the Unity Universal Render Pipeline (URP).
*   **Standalone VR Execution:** Built targeting the Android platform for Meta Quest 3 via OpenXR, removing the need for a PC tether.
*   **Real-time Trajectory Tracking:** Maps robotic or camera odometry directly to the XR space.

## 🛠️ Development Environment
*   **Game Engine:** Unity 2023.2.22f1
*   **Graphics Pipeline:** Universal Render Pipeline (URP)
*   **VR Framework:** OpenXR Plugin
*   **Target Platform:** Android (Meta Quest 3 APK)
*   **Middleware:** ROS2 (TCP/IP Endpoint connection)

## ⚙️ Prerequisites
To build and run this project, you will need:
1.  Unity Hub with **Unity 2023.2.22f1** installed (including Android Build Support).
2.  A Meta Quest 3 headset with **Developer Mode** enabled(Didnt test in other model).
3.  A working ROS2 environment capable of publishing `sensor_msgs/PointCloud2` and `nav_msgs/Odometry`.

## 🚧 Current Development Status
The project is currently in active development. We have successfully validated the PC-tethered visualization and are transitioning to a native standalone APK. 
*Current known issues being addressed:*
*   Troubleshooting Android GPU buffer/shader incompatibilities (`qdgralloc` / `GraphicBufferAllocator` rendering errors).
*   Correctly binding the `XR Origin` to the headset's pose for standalone 6DOF head tracking.

## 📥 Installation & Setup
1. Clone the repository:
   ```bash
   git clone [https://github.com/Projeto-Voris/VORIS-VR-PointCloud-Viewer-ROS2-To-METAQUEST3.git](https://github.com/Projeto-Voris/VORIS-VR-PointCloud-Viewer-ROS2-To-METAQUEST3.git)