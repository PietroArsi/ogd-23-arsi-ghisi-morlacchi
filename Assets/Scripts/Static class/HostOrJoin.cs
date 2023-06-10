using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HostOrJoin
{
    public static bool pressHost;
    public static void pressHostBtn()
    {
         pressHost=true;
    }
    public static void  pressJoinBtn()
    {
         pressHost = false;
    }

    public static bool returnValueHost()
    {
        return pressHost;
    }
}
