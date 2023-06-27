using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float time = 1f;

    private void Start() {
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(time);

        RemoveSmokeServerRpc();
       // Destroy(gameObject);
    }
    [ServerRpc(RequireOwnership = false)]
    private void RemoveSmokeServerRpc()
    {
        Destroy(gameObject);
    }
}
