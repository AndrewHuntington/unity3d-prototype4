using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float speed = 5.0f;
  public bool hasPowerup;
  public GameObject powerupIndicator;

  private float powerupStrength = 15f;
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
    powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Powerup"))
    {
      hasPowerup = true;
      powerupIndicator.gameObject.SetActive(true);
      Destroy(other.gameObject);
      StartCoroutine(PowerupCountdownRoutine());
    }
  }

  IEnumerator PowerupCountdownRoutine()
  {
    yield return new WaitForSeconds(7);
    hasPowerup = false;
    powerupIndicator.gameObject.SetActive(false);
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Enemy") && hasPowerup)
    {
      Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
      // formula: enemy position - player position
      // ex. if (E)emy position = 3 and (P)layer = 1 then P - E = 2 (which is going away from the player) 
      Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

      enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
      Debug.Log("Collied with " + other.gameObject.name + " with powerup set to " + hasPowerup);
    }
  }
}
