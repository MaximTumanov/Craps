using UnityEngine;
using System.Collections;

public class NetworkCharacter : MonoBehaviour
{
    [SerializeField] Transform CameraPosition;

    public void ApplyPositionToCamera()
    {
        Camera.main.transform.position = CameraPosition.position;
        Camera.main.transform.rotation = CameraPosition.rotation;
    }
}

