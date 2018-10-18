using UnityEngine;
using UnityEditor;

public class PID
{
    public float proportionalValue;
    public float integralValue;
    public float derivativeValue;

    private float integral;
    private float currentValue;
    private float targetValue;
    private float lastError;

    private float maxIntegral;
    private float minIntegral;

    public PID(float pValue, float iValue, float dValue, float minI, float maxI)
    {
        proportionalValue = pValue;
        integralValue = iValue;
        derivativeValue = dValue;

        integral = 0;
        minIntegral = minI;
        maxIntegral = maxI;
    }

    public float Control(float timeSinceLastUpdate, float target, float current)
    {
        float error = target - current;

        integral += error * timeSinceLastUpdate * integralValue;
        float integralTerm = integral;

        float dInput = error - lastError;
        float derivativeTerm = derivativeValue * (dInput / timeSinceLastUpdate);

        float proportionalTerm = proportionalValue * error;

        float output = proportionalTerm + integralTerm + derivativeTerm;

        lastError = error;
        return output;
    }

    public float Control360(float timeSinceLastUpdate, float target, float current)
    {
        target = (target + 180) % 360;
        current = (current + 180) % 360;

        float error = target - current;

        integral += error * timeSinceLastUpdate;
        float integralTerm = Mathf.Clamp(integral * integralValue, minIntegral, maxIntegral);

        float dInput = error - lastError;
        float derivativeTerm = derivativeValue * (dInput / timeSinceLastUpdate);

        float proportionalTerm = proportionalValue * error;

        float output = proportionalTerm + integralTerm + derivativeTerm;

        KU.Log("D : " + derivativeTerm+"\n", 0f, Color.magenta, true, false);
        KU.Log("I : " + integralTerm, 0f, Color.yellow, true, false);
        KU.Log("P : " + proportionalTerm, 0f, Color.cyan, true, false);

        lastError = error;
        return output;
    }

    public float Clamp(float value, float min, float max)
    {
        if (value <= min)
            return min;
        else if (value >= max)
            return max;
        else return value;
    }
}