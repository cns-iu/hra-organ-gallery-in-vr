using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(fileName = "NewAxisHandler")]
public class AxisHandler : InputHandler, ISerializationCallbackReceiver
{
    public enum Axis { None, Trigger, Grip }
    public event Action<XRController, float> OnValueChange;
    public Axis axis = Axis.None;

    private InputFeatureUsage<float> inputFeature;
    private float previousValue = 0f;

    public void OnAfterDeserialize()
    {
        inputFeature = new InputFeatureUsage<float>(axis.ToString());
    }
    public void OnBeforeSerialize(){}

    public override void HandleState(XRController controller)
    {
        float value = GetValue(controller);
        if (value != previousValue)
        {
            previousValue = value;
            OnValueChange?.Invoke(controller, value);
        }
    }

    public float GetValue(XRController controller)
    {
        if (controller.inputDevice.TryGetFeatureValue(inputFeature, out float value))
        {
            return value;
        }
        return 0f;
    }
}
