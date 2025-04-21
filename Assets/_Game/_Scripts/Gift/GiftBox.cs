using UnityEngine;

public class GiftBox : MonoBehaviour
{
    /*    private void OnTriggerEnter(Collider other) {
            if (!other.isTrigger && other.CompareTag(ApplicationVariable.PLAYER_TAG)) {
                other.gameObject.GetComponent<PlayerController>().Ultimate();
                gameObject.SetActive(false);
            }
            else if (other.CompareTag(ApplicationVariable.ENEMY_TAG)) {
                other.gameObject.GetComponentInParent<EnemyAI>().Ultimate();
                gameObject.SetActive(false);
            }
        }*/
    private void OnCollisionEnter(Collision other) {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG)) {
            other.gameObject.GetComponent<PlayerController>().Ultimate();
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG)) {
            other.gameObject.GetComponentInParent<EnemyAI>().Ultimate();
            gameObject.SetActive(false);
        }
    }
    /*    private void OnCollisionStay(Collision other) {
            if (other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG)) {
                other.gameObject.GetComponentInParent<EnemyAI>().Ultimate();
                gameObject.SetActive(false);
            }
        }*/
}
