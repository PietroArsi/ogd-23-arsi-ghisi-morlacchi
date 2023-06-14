using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float radius = 5f;
    public LayerMask detectionLayer;
    public int alarmDuration = 5;
    //public Transform player;
    private Transform detectedEnemy;

    public AudioClip alarm;
    public AudioClip powerOn;
    public AudioClip powerOff;

    private AudioSource audioSource;

    private TurretStatus status;
    public enum TurretStatus {
        Idle, Active
    }

    void Start()
    {
        //GameObject.Find("PlayerCC");
        status = TurretStatus.Idle;
        audioSource = GetComponent<AudioSource>();
        PlayPowerOn();
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
            if (c.GetComponent<PawliceMovement>()) {
                detectedEnemy = c.transform;
                foundEnemy = true;
            }
        }

        if (status == TurretStatus.Idle && foundEnemy){
            //if (player != null){
            //    //player.GetComponent<PlayerSoundManager>().PlaySound();
            //}
            //Debug.Log("Enemy detected");
            PlayAlarm();
            status = TurretStatus.Active;
            StartCoroutine(DieLater(10));
        }
    }

    IEnumerator DieLater(float time)
    {
        yield return new WaitForSeconds(time);

        PlayPowerOff();
        StartCoroutine(SoundAndDie());
    }

    IEnumerator SoundAndDie() {
        yield return new WaitForSeconds(powerOff.length);

        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        if (detectedEnemy) {
            Gizmos.DrawLine(transform.position, detectedEnemy.position);
        }
    }

    public void PlayAlarm() {
        audioSource.clip = alarm;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayPowerOn() {
        audioSource.clip = powerOn;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayPowerOff() {
        audioSource.clip = powerOff;
        audioSource.loop = false;
        audioSource.Play();
    }
}
