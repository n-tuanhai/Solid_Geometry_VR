using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisableAllInputOptions : MonoBehaviour
{
    private void Start()
    {
        DisableInputOptions();
    }

    public void DisableInputOptions()
    {
        var optionList = GetComponentsInChildren<Transform>().ToList();
        optionList.RemoveAt(0);
        foreach (var op in optionList)
        {
            op.gameObject.SetActive(false);
        }
    }
}
