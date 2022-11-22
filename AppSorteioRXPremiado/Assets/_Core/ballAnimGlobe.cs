using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ballAnimGlobe : MonoBehaviour
{
    public Rigidbody rigidBody;
    public MeshRenderer meshRenderer;
    [SerializeField] private float force = 50;
    public bool canMovement = false;

    [SerializeField] private int numberBall;

    [SerializeField] private ballAnimGlobeController animGlobeController;

    public Collider objectIgnore;
    public Collider myCollider;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        animGlobeController = FindObjectOfType<ballAnimGlobeController>();
    }

    public void SetNumberBall(int _number)
    {
        numberBall = _number;
    }
    public int GetNumberBall()
    {
        return numberBall;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (canMovement)
        {
            rigidBody.AddExplosionForce(force, transform.position, 100, 10, ForceMode.Impulse);
        }
    }
    public void MovementCurrentBall(Transform pos1, Transform pos2, Transform pos3)
    {
        Physics.IgnoreCollision(this.myCollider, objectIgnore);
        transform.DOMove(pos1.position, 0.1f).OnComplete(() =>
        {   
            transform.DOMove(pos2.position, 0.3f).OnComplete(() =>
            {
                animGlobeController.isFinishMovement = true;
               
                gameObject.SetActive(false);
                transform.DOMove(pos3.position, 0.2f).OnComplete(() =>
                {
                    rigidBody.isKinematic = true;
                    animGlobeController.isFinishMovement = false;

                });
            });
        });
    }

    public void ReturnBallToGlobe(Transform pos)
    {
        gameObject.SetActive(true);
        transform.position = pos.position;
        rigidBody.isKinematic = false;
    }

}
