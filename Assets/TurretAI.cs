using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float radius = 5f;
    public LayerMask detectionLayer;
    public int alarmDuration = 5;
    public Transform player;

    public enum TurretStatus {
        Idle, Active
    }

    void Start()
    {
        //GameObject.Find("PlayerCC");
        
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, detectionLayer);
        if (hitColliders.Length < 0){
            return;
        }

        bool foundEnemy = false;
        foreach(Collider c in hitColliders){
            //applicare sovraimpressione?
            if (c.GetComponent<PawliceMovement>()){
                foundEnemy = true;
            }
        }

        if (foundEnemy){
            if (player != null){
                //player.GetComponent<PlayerSoundManager>().PlaySound();
            }
            Debug.Log("Enemy detected");
            StartCoroutine(DieLater(10));
        }
    }

    IEnumerator DieLater(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
