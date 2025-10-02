using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    Transform transform;

    void Start()
    {
        transform = GetComponent<Transform>();

        transform.rotation = Quaternion.Euler(0, 0, 90);
        transform.position = transform.position + new Vector3(0, 0.25f, 0);
    }

    void Update()
    {
        transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
    }
}
