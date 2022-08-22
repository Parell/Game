using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VolumeUpdater : MonoBehaviour
{
    UnityEngine.Camera _camera;
    UniversalAdditionalCameraData _cameraData;
    bool _volumeStackDirty;

    void Awake()
    {
        _camera = Camera.main;
        _cameraData = _camera.GetUniversalAdditionalCameraData();
    }

    void LateUpdate()
    {
        if (this._volumeStackDirty)
        {
            this._volumeStackDirty = false;
            this._camera.UpdateVolumeStack(this._cameraData);
        }
    }
}
