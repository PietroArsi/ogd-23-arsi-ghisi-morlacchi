using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CatnipStatus : NetworkBehaviour
{
    public enum status
    {
        Unprocessed,
        Cooodked,
        Cut,
    }

    public status currentStatus;
}
