using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VRTK;

public class PointData : MonoBehaviour
{
    public bool isSelectable;
    public bool isSelected;
    public string name;
    public Vector3 position;
    private MeshRenderer _mesh;
    public VRTK_ObjectTooltip tooltip;
    public bool isUpdating;

    private void OnEnable()
    {
        isUpdating = false;
        position = transform.position;
    }

    private void Start()
    {
        isUpdating = false;
        position = transform.position;
        _mesh = GetComponent<MeshRenderer>();
        isSelected = false;
        UpdateColor();
    }

    private void Update()
    {
        if (!isUpdating) return;
        position = transform.position;
    }

    public void PointSelectRequestHandler()
    {
        //if (!isSelectable) return;
        isSelected = !isSelected;
        UpdateColor();
    }
    
    public void SetName(string n)
    {
        name = n;
        tooltip.UpdateText(name);
    }
    
    private void UpdateColor()
    {
        _mesh.material.color = isSelected ? Color.red : isSelectable ? Color.blue : Color.gray;
    }  
}