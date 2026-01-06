using UnityEngine;
public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 50f;

    private float yaw = 0f;
    private float pitch = 0f;

    // 반동 전용 값 (pitch/yaw와 분리)
    private float recoilYaw = 0f;
    private float recoilPitch = 0f;
    void Start()
    {
        Vector3 e = transform.localEulerAngles;
        yaw = e.y;
        pitch = e.x > 180 ? e.x - 360 : e.x;
    }

    private void Update()
    {
        //1. 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //2. 마우스 입력을 누적한다.
        yaw += mouseX * RotationSpeed * Time.deltaTime;

        pitch -= mouseY * RotationSpeed * Time.deltaTime; ;
        BasicRotation();
    }

    private void BasicRotation()
    {
        // 최종 회전 = 마우스 회전 + 반동
        transform.localRotation = Quaternion.Euler(
            Mathf.Clamp(pitch + recoilPitch, -45f, 90),
            yaw + recoilYaw,
            0f
        );
    }

    // 카메라 쉐이킹 추가 회전값
    public void SetRecoil(float rp, float ry)
    {
        recoilPitch = rp;
        recoilYaw = ry;
    }
}
