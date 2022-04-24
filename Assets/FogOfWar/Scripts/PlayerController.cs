using UnityEngine;
using System.Collections;

namespace UnityLibrary.CharacterController
{
    public enum Facing
    {
        Up,
        Right,
        Down,
        Left
    }

    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 1;
        public bool blockDiagonals = false;
        public int playerID = 0;

        public Facing facingDir;
        public Vector3 facingVector;

        Animator anim;
        Vector2 movementVelocity;
        Rigidbody2D rb;

        void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // lock diagonals
            if (blockDiagonals)
            {
                if (Mathf.Abs(v) >= Mathf.Abs(h) && v != 0)
                {
                    h = 0;
                }
            }

            // if we are moving
            if (Mathf.Abs(h) + Mathf.Abs(v) != 0)
            {
                anim.SetBool("Walk", true);
                anim.SetFloat("SpeedHorizontal", h);
                anim.SetFloat("SpeedVertical", v);

                // check facing direction
                if (Mathf.Abs(v) >= Mathf.Abs(h))
                {
                    if (v > 0)
                    {
                        facingDir = Facing.Up;
                        facingVector = -Vector2.right;
                    }
                    else
                    {
                        facingDir = Facing.Down;
                        facingVector = Vector2.right;
                    }
                }
                else
                {
                    if (h > 0)
                    {
                        facingDir = Facing.Right;
                        facingVector = Vector2.up;

                    }
                    else
                    {
                        facingDir = Facing.Left;
                        facingVector = -Vector2.up;
                    }
                }


            }
            else
            {
                anim.SetBool("Walk", false);
            }

            movementVelocity = new Vector2(h, v) * walkSpeed;
        }

        void FixedUpdate()
        {
            rb.velocity = movementVelocity;
        }

    } // class
}
