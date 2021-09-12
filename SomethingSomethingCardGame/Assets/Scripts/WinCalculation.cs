using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class WinCalculation : NetworkBehaviour
{
  [Command]
  public void CmdCalculateWinner()
    {
        DropZone[] dropZones = FindObjectsOfType<DropZone>();

    }

}
