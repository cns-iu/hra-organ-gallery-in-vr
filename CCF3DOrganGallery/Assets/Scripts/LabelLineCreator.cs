using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelLineCreator : MonoBehaviour
{
    public enum Sides { Left, Top, Right, Bottom }
    public Sides m_LineAttachedTo = Sides.Bottom;
    public float m_Width = 0.005f;
    public Color m_LineColor;
    private LineRenderer m_Line;
    private GameObject m_LineTarget;
    private void Awake()
    {
        m_LineTarget = transform.GetChild(1).gameObject;
        m_Line = CreateLine();
    }

    void LateUpdate()
    {
        m_Line.SetPositions(
                   positions: new Vector3[]{
                      GetOffsetPosition(),
                        m_LineTarget.transform.position
                        }
               );
    }

    Vector3 GetOffsetPosition()
    {

        RectTransform rt = GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector2 size = new Vector2(
             rt.lossyScale.x * rt.rect.size.x,
             rt.lossyScale.y * rt.rect.size.y);

        Vector3 result = this.transform.position;

        switch (m_LineAttachedTo)
        {
            case Sides.Bottom:
                result.y -= size.y / 2f;
                break;
            case Sides.Left:
                result.x -= size.x / 2f;
                break;
            case Sides.Top:
                result.y += size.y / 2f;
                break;
            case Sides.Right:
                result.x += size.x / 2f;
                break;
            default:
                break;
        }
        return result;
    }


    LineRenderer CreateLine()
    {
        this.gameObject.AddComponent<LineRenderer>();
        LineRenderer renderer = GetComponent<LineRenderer>();
        renderer.startWidth = m_Width;
        renderer.endWidth = m_Width;
        renderer.material.color = m_LineColor;
        return renderer;
    }
}
