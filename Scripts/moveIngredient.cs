using UnityEngine;

public class moveIngredient : MonoBehaviour
{
    [Header("Layer")]
    public LayerMask raycastLayer;

    [Header("Object Move Speed")]
    [Range(0f, 100f)] public float baseMoveSpeed = 5f;

    private Camera mainCamera;
    private Rigidbody2D rb;
    public bool isDragging = false;
    private Vector3 lastMousePosition;

    public GameObject lastMovedObject;

    private void Start()
    {
        mainCamera = Camera.main;
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, raycastLayer);

            if (hit.collider != null)
            {
                lastMovedObject = hit.collider.gameObject;
                rb = hit.collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                    isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            MoveObjectWithRigidbody();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void MoveObjectWithRigidbody()
    {
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        targetPosition.z = 0;

        Vector2 targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
        float mouseSpeed = (Input.mousePosition - lastMousePosition).magnitude * Time.deltaTime;

        float moveSpeed = baseMoveSpeed + mouseSpeed;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition2D, moveSpeed * Time.deltaTime);

        rb.MovePosition(newPosition);
        lastMousePosition = Input.mousePosition;
    }
}
