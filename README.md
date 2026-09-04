To install **MARUS2 Visualization** and its required core dependencies, add them directly to your Unity project's **`Packages/manifest.json`** file under the `"dependencies"` block:

```json
{
  "dependencies": {
    "com.marus2.proto": "https://github.com/MARUSimulator/marus2-proto.git#csharp",
    "com.marus2.core": "https://github.com/MARUSimulator/marus2-core.git",
    "com.marus2.visualization": "https://github.com/MARUSimulator/marus-visualization.git"
  }
}
```

# Visualization usage

This package provides a suite of real-time 3D data visualization and debug drawing tools for Unity. It supports high-density point cloud rendering via compute shaders, dynamic geometric primitives, trajectory tracking, and path recording.

To stream visualization markers or point clouds from external ROS nodes or gRPC clients, use the `marus-visualization-grpc` package.

## Visualizer

A singleton manager for rendering debug primitives such as points, lines, paths, coordinate transforms, and arrows in the scene. Primitives can be tagged with string keys for convenient group filtering, updating, and clearing.

## Point Cloud Manager

Manages GPU-accelerated point cloud rendering using compute shaders and custom point materials. Dynamically allocates mesh buffers to display high-density LiDAR or bathymetric point clouds with high frame rates.

## Live Movement Visualizer

Attaches to any moving GameObject to track and visualize its historical trajectory over time with connected lines and points at a configurable refresh rate. Supports full 3D tracking as well as 2D planar projection.

## Path Recorder

Records the spatial movement of an object into timestamped path segments for trajectory inspection, debugging, and mission replay.

## Primitives

A collection of lightweight visual element classes (`Point`, `Line`, `Path`, `Arrow`, `Transform`, and `PointcloudMesh`) with dedicated visual controllers for customized color, thickness, and coordinate frame representation.