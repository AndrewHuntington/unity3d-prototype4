using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float speed = 5.0f;
  public bool hasPowerup;
  private Rigidbody playerRb;
  private GameObject focalPoint;

  // Start is called before the first frame update
  void Start()
  {
    playerRb = GetComponent<Rigidbody>();
    focalPoint = GameObject.Find("Focal Point");
  }

  // Update is called once per frame
  void Update()
  {
    float forwardInput = Input.GetAxis("Vertical");
    playerRb.AddForce(forwardInput * speed * focalPoint.transform.forward);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Powerup"))
    {
      hasPowerup = true;
      Destroy(other.gameObject);
    }
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Enemy") && hasPowerup)
    {
      Debug.Log("Collied with " + other.gameObject.name + " with powerup set to " + hasPowerup);
    }
  }
}
