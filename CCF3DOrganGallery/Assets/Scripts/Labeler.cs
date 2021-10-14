using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Labeler : MonoBehaviour
{
    public GameObject pre_Label;
    public GameObject pre_LabelLine;
    public GameObject m_RightHand;
    public Camera m_Camera;
    public Canvas m_Canvas;
    public float m_MaxProximity = .2f;
    private string m_Name;
    private GameObject m_Label;
    private GameObject m_LabelLine;
    private Vector3 DISTANCE;
    // Start is called before the first frame update

    private void Awake()
    {
        UserInputModule.OnUpdateJoystickValueEvent += SetLabelAndLabelLineOpacity;

        // assign global properties
        m_Name = gameObject.name;
        m_Camera = GameObject.FindObjectOfType<Camera>();
        m_Canvas = GameObject.FindObjectOfType<Canvas>();
        m_RightHand = GameObject.FindGameObjectWithTag("GameController");

        pre_Label = Resources.Load("Prefabs/Label") as GameObject;
        pre_LabelLine = Resources.Load("Prefabs/LabelLine") as GameObject;
    }

    void OnDestroy()
    {
        UserInputModule.OnUpdateJoystickValueEvent -= SetLabelAndLabelLineOpacity;
    }


    void Start()
    {
        CreateAndSetLabelAndLabelLine();
    }

    private void Update()
    {
        AdjustLabelPosition();
        AdjustLabelDirection();
        DrawLabelLines();
    }

    void SetLabelAndLabelLineOpacity(float joystickValue)
    {
        Color zm = m_Label.transform.GetChild(0).GetComponent<Text>().color;
        zm.a = joystickValue;
        m_Label.transform.GetChild(0).GetComponent<Text>().color = zm;

        // Color m = m_LabelLine.GetComponent<LineRenderer>().material.color;
        // m.a = joystickValue;
        // m_LabelLine.GetComponent<LineRenderer>().material.color = m;

    }

    void CreateAndSetLabelAndLabelLine()
    {
        m_Label = Instantiate(pre_Label, this.transform.position, Quaternion.identity, m_Canvas.transform);
        m_LabelLine = Instantiate(pre_LabelLine, this.transform.position, Quaternion.identity);
        m_Label.transform.position = Vector3.MoveTowards(
            this.transform.position,
            m_Camera.gameObject.transform.position,
            Vector3.Distance(this.transform.position, m_Camera.gameObject.transform.position) / 8f);

        m_Label.transform.position = new Vector3(
            m_Label.transform.position.x,
            this.transform.position.y,
            m_Label.transform.position.z
        );

        DISTANCE = this.transform.position - m_Label.transform.position;

        m_Label.GetComponentInChildren<Text>().text = m_Name;
    }


    void AdjustLabelDirection()
    {
        // Quaternion previousRotation = m_Label.transform.rotation;
        m_Label.transform.LookAt(m_Camera.transform);
        m_Label.transform.eulerAngles = new Vector3(
            m_Label.transform.eulerAngles.x,
            m_Label.transform.eulerAngles.y,
            m_Label.transform.eulerAngles.z
        );
    }

    void AdjustLabelPosition()
    {
        m_Label.transform.position = this.transform.position - DISTANCE;
    }

    void DrawLabelLines()
    {
        LineRenderer renderer = m_LabelLine.GetComponent<LineRenderer>();
        renderer.useWorldSpace = true;
        renderer.SetPositions(
            new Vector3[] {
           m_Label.transform.position,
           this.transform.position
        }
        );
    }

}
