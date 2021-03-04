using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rotator
{
    public enum Direction
    {
        LeftHanded,
        RightHanded,
    }

    public class Player : MonoBehaviour
    {
        public Direction direction;
        float raysCastLenght = 100f;
        /// <summary>
        /// Бот будет держаться левой(0) или правой(1) стороны канала.
        /// </summary>
        [SerializeField]
        [Range(0.1f, 1.9f)]
        public float LeftRightDifference = 1;
        [SerializeField]
        float speed = 0.1f;
        void Start()
        {

        }
        void Moving()
        {
            Vector3 forward = transform.forward * raysCastLenght;
            RaycastHit forwardHit;
            Physics.Raycast(transform.position, forward, out forwardHit);
            if (forwardHit.distance > speed * 30f)
                transform.position += transform.forward * speed;
            else
            {
                transform.position -= transform.forward * speed;
                StartCoroutine(Rotate());
            }
        }

        IEnumerator Rotate()
        {
            for (int n=0; n<10;n++)
            yield return new WaitForSeconds(.02f);
            if (direction == Direction.RightHanded)
                transform.eulerAngles += new Vector3(0, 1f, 0);
            else
                transform.eulerAngles -= new Vector3(0, 1f, 0);
        }

        /// <summary>
        /// Докручиваем объект что бы он повернулся к центру
        /// </summary>
        void RayCast()
        {
            //for (int n=0; n<vectors.Length;n++)
            {
                Vector3 position = transform.position;
                Vector3 right = (transform.forward + transform.right) * raysCastLenght;
                Vector3 left = (transform.forward - transform.right) * raysCastLenght;
                /*if (Physics.Raycast(position, left, out hit))
                {
                    Debug.DrawRay(position, hit.point, Color.blue);
                    if (hit.distance < 1f)
                        transform.eulerAngles -= new Vector3(0, 1, 0);
                }
                if (Physics.Raycast(position, right, out hit))
                {
                    Debug.DrawRay(position, hit.point, Color.red);
                    if (hit.distance < 1f)
                        transform.eulerAngles += new Vector3(0, 1, 0);
                }*/
                RaycastHit leftHit;
                RaycastHit rightHit;
                Physics.Raycast(position, left, out leftHit);
                Physics.Raycast(position, right, out rightHit);
                Debug.DrawRay(position, left.normalized * leftHit.distance, Color.blue);
                Debug.DrawRay(position, right.normalized * rightHit.distance, Color.red);
                if (leftHit.distance / rightHit.distance > LeftRightDifference)
                    transform.eulerAngles -= new Vector3(0, .1f * 100 * speed, 0);
                else
                    transform.eulerAngles += new Vector3(0, .1f * 100 * speed, 0);
                //transform.eulerAngles -= new Vector3(0, rightHit.distance - leftHit.distance, 0);
            }
        }
        void FixedUpdate()
        {
            RayCast();
            Moving();
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.eulerAngles -= new Vector3(0,4f,0);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.eulerAngles += new Vector3(0, 4f, 0);
            }
        }
    }
}