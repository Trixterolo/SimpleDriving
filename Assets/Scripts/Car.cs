using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float speedGainPerSecond = 1.2f;
    [SerializeField] float turnSpeed;

    public int steerValue; //-1 = left, 0 = forward, 1 = right

    // Update is called once per frame
    void Update()
    {
        if(speed < 50f)
        {
            speed += speedGainPerSecond * Time.deltaTime;

        }
        else
        {
            speed = 50f;
        }

        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
 
    public void Steer(int value)
    {
        steerValue = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) 
        {

            SceneManager.LoadScene(0);
            print("crashed");
        }
    }
}
