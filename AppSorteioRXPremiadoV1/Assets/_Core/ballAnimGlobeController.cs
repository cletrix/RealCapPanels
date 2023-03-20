using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ballAnimGlobeController : MonoBehaviour
{
    public List<ballAnimGlobe> balls;
    public PhysicMaterial bouncy;
    [SerializeField] private float force = 100;

    [SerializeField] private int currentBall;

    [SerializeField] Transform pos1, pos2, pos3;

    public bool isFinishMovement = false;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].SetNumberBall(i + 1);
        }
        Invoke("ActiveGlobo", 5);
    }
    public void ActiveGlobo()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].canMovement = true;
            balls[i].rigidBody.GetComponent<SphereCollider>().material = bouncy;
            balls[i].rigidBody.mass = 10;
            balls[i].rigidBody.AddExplosionForce(force, transform.position, 100, 10, ForceMode.Impulse);
        }
    }
    public void SetCurrentBall(int _ball)
    {
        currentBall = _ball;
        balls[currentBall - 1].MovementCurrentBall(pos1, pos2, pos3);
    }
    public void ResetBall(int _ball)
    {
        if (balls.Count > 0)
            balls[currentBall - 1].ReturnBallToGlobe(pos1);
    }
}
