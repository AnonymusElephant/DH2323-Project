using UnityEngine;

public class player : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    private string m_TurnAxisName;              // The name of the input axis for turning.
    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private Vector3 m_MouseInputValue;          // The current value of the mouse input
    private int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    private void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Ground");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_MovementAxisName = "Vertical";
        m_TurnAxisName = "Horizontal";
    }

    // Update is called once per frame
    void Update()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        m_MouseInputValue = Input.mousePosition;
    }

    private void FixedUpdate()
    {
        // Move the tank and turn the turret.
        Move();
        Turn();
    }

    private void Move()
    {
        // Move the tank forward or backward based on the input value.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        transform.position += movement;
    }

    private void Turn()
    {
        // Turn the tank left or right based on the input value.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnQuaternion = Quaternion.Euler(0f, turn, 0f);
        transform.Rotate(turnQuaternion.eulerAngles, Space.World);
    }
}
