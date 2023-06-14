using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TurretAI : NetworkBehaviour
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
        //LUCA ADDITON
        audioSource = GetComponent<AudioSource>();
        if (ConnectionManager.Instance == null)
        {
            audioSource = GetComponent<AudioSource>();
            PlayPowerOn();
        }

        PlayerNetwork.LocalIstance.interactionCollider.GetComponent<PickAndPlace>().OnPlaceObject += TurretAI_OnPlaceObject; 
       
    }

    private void TurretAI_OnPlaceObject(object sender, System.EventArgs e)
    {
        Debug.Log("FIRE EVENT SOUND");
        PlayerNetwork.LocalIstance.interactionCollider.GetComponent<PickAndPlace>().OnPlaceObject -= TurretAI_OnPlaceObject;
        PlayPowerOnClientRpc();
       
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

            //LUCA ADDITION NETWORK 
            if (ConnectionManager.Instance != null)
            {
                if (IsHost && gameObject.GetComponent<FollowTransform>().GetTargetTransform().gameObject.layer== 7)
                {
                   
                    audioSource = GetComponent<AudioSource>();
                    AlarmPlayClientRpc();
                    status = TurretStatus.Active;
                    StartCoroutine(DieLater(10));
                }
            }
            else
            {
                PlayAlarm();
                status = TurretStatus.Active;
                StartCoroutine(DieLater(10));
            }
        }
    }

    IEnumerator DieLater(float time)
    {
        yield return new WaitForSeconds(time);
        if (ConnectionManager.Instance != null)
        {
            if (IsHost)
            {
                AlarmOffClientRpc();
                StartCoroutine(SoundAndDie());
            }
        }
        else
        {
            PlayPowerOff();
            StartCoroutine(SoundAndDie());
        }
       
    }

    IEnumerator SoundAndDie() {
        yield return new WaitForSeconds(powerOff.length);
        if (ConnectionManager.Instance != null)
        {
            if (IsHost)
            {
                DestoryAlarmClientRpc();
            }
        }
        else
        {
            Destroy(gameObject);
        }
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

    //synch Play sOund for all players
    [ClientRpc]
    private void AlarmPlayClientRpc()
    {
        PlayAlarm();
    }
    [ClientRpc]
    private void AlarmOffClientRpc()
    {
        PlayPowerOff();
    }
    [ClientRpc]
    private void DestoryAlarmClientRpc()
    {
       
        Destroy(gameObject);
    }
    [ClientRpc]
    private void PlayPowerOnClientRpc()
    {
        PlayPowerOn();
    }
}
