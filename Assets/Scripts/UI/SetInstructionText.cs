using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetInstructionText : MonoBehaviour
{
    private TMP_Text instruction;

    private void Start()
    {
        instruction = GetComponent<TMP_Text>();
    }

    public void SetAngleInstruction()
    {
        instruction.text = "Move the pointer to select three points then rotate right controller to set angle";
    }
    
    public void SetPointHeightInstruction()
    {
        instruction.text = "Move the pointer to select a point then move right controller up and down to set point height";
    }
    
    public void DeletePointInstruction()
    {
        instruction.text = "Move the pointer to select a point then press button to delete";
    }
    
    public void ShowAngleInstruction()
    {
        instruction.text = "Move the pointer to select three points to show angle value";
    }
    
    public void SetPointSurfaceInstruction()
    {
        instruction.text = "Move the pointer to select a point then move right controller left, right, backward, forward to set point position";
    }

    public void SeLengthInstruction()
    {
        instruction.text = "Move the pointer to select two point then move right controller to set length";
    }
    
    public void PointInSpaceInstruction()
    {
        instruction.text = "Move to desired position and press to create point";
    }

    public void PointOnSurfaceInstruction()
    {
        instruction.text = "Move the pointer to the desired position on a surface and press to create point";
    }

    public void MiddlePointInstruction()
    {
        instruction.text = "Move the pointer to select a line or two points to create middle point";
    }
    
    public void SectionPointInstruction()
    {
        instruction.text = "Move the pointer to select a line or two points, then enter a ratio to create a section point";
    }

    public void LineInstruction()
    {
        instruction.text = "Move the pointer and select two points to create a line";
    }
    
    public void PerpendicularLineInstruction()
    {
        instruction.text = "Move the pointer and select a point and a line or a polygon to create a perpendicular line from that point";
    }

    public void IrregularPolygonInstruction()
    {
        instruction.text = "Move the pointer and select at least three points to create a polygon";
    }

    public void RegularPolygonInstruction()
    {
        instruction.text = "Move the pointer and select two points or one line, then enter the number of vertices";
    }

    public void TetrahedronInstruction()
    {
        instruction.text = "Move the pointer and select a base triangle, then a top point";
    }

    public void PyramidInstruction()
    {
        instruction.text = "Move the pointer and select a base polygon, then a top point";
    }

    public void PrismInstruction()
    {
        instruction.text = "Move the pointer and select a base polygon, then a top point";
    }
    
    public void CubeInstruction()
    {
        instruction.text = "Move the pointer and select a base square";
    }
}