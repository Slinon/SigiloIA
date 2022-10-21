using UnityEngine;
using Cinemachine;

public class PanAndZoom : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    private float panSpeed = 2f;
    [SerializeField]
    private float zoomSpeed = 3f;
    [SerializeField]
    private float zoomInMax = 40f;
    [SerializeField]
    private float zoomOutMax = 90f;
    [SerializeField]
    private float viewRange = 5f;

    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    private void Awake()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);

        if (x != 0 || y != 0)
        {
            PanScreen(x, y);
        }
        
        if (z != 0)
        {
            ZoomScreen(z);
        }
        
    }
    public void ZoomScreen(float increment)
    {
        float fov = virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, zoomInMax, zoomOutMax);
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime);
    }
    

    public Vector3 PanDirection(float x, float y)
    {
        Vector3 direction = Vector2.zero;

        if (y >= Screen.height * .95f & cameraTransform.position.z <= 40f & cameraTransform.position.z < playerTransform.position.z + viewRange) { direction.z += 1; }
        else if (y <= Screen.height * .05f & cameraTransform.position.z >= -40f & cameraTransform.position.z > playerTransform.position.z - viewRange) { direction.z -= 1; }

        if (x >= Screen.width * .95f & cameraTransform.position.x <= 40f & cameraTransform.position.x < playerTransform.position.x + viewRange) { direction.x += 1; }
        else if (x <= Screen.width * .05f & cameraTransform.position.x >= -40f & cameraTransform.position.x > playerTransform.position.x - viewRange) { direction.x -= 1; }

        return direction;
    }
    public void PanScreen(float x, float y)
    {
        Vector3 direction = PanDirection(x, y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
                                                cameraTransform.position + direction,
                                                panSpeed * Time.deltaTime);
    }
}
