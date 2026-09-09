using UnityEngine;
using Unity.Collections;
using Marus.Core; // Rely on the Core interface, not the Sensors module

namespace Marus.Visualization
{
    public class PointCloudVisualizerHandler : MonoBehaviour
    {
        private IPointCloudSensor _sensor;
        private PointCloudManager _pointCloudManager;

        void Awake()
        {
            _sensor = GetComponent<IPointCloudSensor>();

            if (_sensor != null)
            {
                _sensor.OnPointCloudInitialized += HandleInitialization;
                _sensor.OnPointCloudUpdated += HandleUpdate;
            }
            else
            {
                Debug.LogWarning("PointCloudVisualizerHandler requires a component implementing IPointCloudSensor on the same GameObject.");
            }
        }

        private void HandleInitialization(GameObject parent, string name, int totalRays, Material mat, ComputeShader shader)
        {
            if (mat == null) mat = PointCloudManager.FindMaterial("PointMaterial");
            if (shader == null) shader = PointCloudManager.FindComputeShader("PointCloudCS");

            _pointCloudManager = PointCloudManager.CreatePointCloud(parent, name, totalRays, mat, shader);
        }

        private void HandleUpdate(NativeArray<Vector3> points)
        {
            if (_pointCloudManager != null)
            {
                _pointCloudManager.UpdatePointCloud(points);
            }
        }

        void OnDestroy()
        {
            if (_sensor != null)
            {
                _sensor.OnPointCloudInitialized -= HandleInitialization;
                _sensor.OnPointCloudUpdated -= HandleUpdate;
            }
        }
    }
}