using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    #region Animation
    [Header("1 move with cam, 0 dont move")]
    private float length, startPosX, startPosY;
    [SerializeField] private Transform player; // Tham chiếu đến vị trí người chơi
    [SerializeField] private float parallaxEffectX = 0.5f; // Điều chỉnh mức độ Parallax theo trục X
    // [SerializeField] private float parallaxEffectY = 0.1f; // Điều chỉnh mức độ Parallax theo trục Y (nếu cần)
    #endregion

    private Vector3 lastPlayerPosition;

    void Start()
    {
        // Lấy vị trí ban đầu của nền và độ dài của nó
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Lưu trữ vị trí ban đầu của người chơi
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        // Tính toán sự thay đổi vị trí của người chơi
        Vector3 playerDeltaMovement = player.position - lastPlayerPosition;

        // Di chuyển nền dựa trên sự thay đổi vị trí của người chơi
        float distX = playerDeltaMovement.x * parallaxEffectX;
        // float distY = playerDeltaMovement.y * parallaxEffectY;

        // Cập nhật vị trí nền
        transform.position = new Vector2(transform.position.x + distX, transform.position.y);

        // Lưu lại vị trí mới của người chơi để tính toán cho khung hình tiếp theo
        lastPlayerPosition = player.position;

        // Tạo hiệu ứng lặp nền khi người chơi di chuyển xa
        if (player.position.x - startPosX > length) startPosX += length;
        else if (player.position.x - startPosX < -length) startPosX -= length;
    }
}
