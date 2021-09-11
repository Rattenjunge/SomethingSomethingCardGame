using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDetailPopup : MonoBehaviour
{
    public void OpenPopup()
    {
        gameObject.SetActive(true);
    }

    public void UpdatePopup()
    {
        //Update card information
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
