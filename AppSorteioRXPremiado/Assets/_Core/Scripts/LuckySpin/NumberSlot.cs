using UnityEngine;
using UnityEngine.UI;
public class NumberSlot : MonoBehaviour
{

    public Transform startPos;
    public Transform finalPos;
    public Transform number;
    public int index;
    public Image imgNumbers;
    public float speed = 0;

    public bool isActive = false;
    private void Start()
    {
        imgNumbers = GetComponent<Image>();
        number = GetComponent<Transform>();
    }

    public void NewPosition(float _yPos)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _yPos, transform.localPosition.z);
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            if (transform.localPosition.y <= finalPos.localPosition.y)
            {
                transform.localPosition = startPos.localPosition;
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 70, transform.localPosition.z);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.timeScale != 0.1f)
                Time.timeScale = 0.1f;
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}