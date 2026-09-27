using System.Collections.Generic;
using Game.Audio;
using TMPro;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    [SerializeField] private TMP_Text display;

    private string currentInput = "";
    private readonly List<float> numbers = new();
    private readonly List<char> operations = new();

    private bool justCalculated;

    [SerializeField] private AudioClip clickSound;

    public void NumberPressed(string number)
    {
        PlayClickSound();

        if (justCalculated)
        {
            ClearCalculator();
            justCalculated = false;
        }

        currentInput += number;
        display.text = currentInput;
    }

    public void DecimalPressed()
    {
        PlayClickSound();

        if (justCalculated)
        {
            ClearCalculator();
            justCalculated = false;
        }

        if (!currentInput.Contains("."))
        {
            if (currentInput == "")
                currentInput = "0";

            currentInput += ".";
            display.text = currentInput;
        }
    }

    public void OperationPressed(string op)
    {
        PlayClickSound();

        if (string.IsNullOrEmpty(currentInput)) return;

        if (justCalculated)
            justCalculated = false;

        numbers.Add(float.Parse(currentInput));
        currentInput = "";

        operations.Add(op[0]);

        display.text += $" {op} ";
    }

    public void EqualsPressed()
    {
        PlayClickSound();

        if (string.IsNullOrEmpty(currentInput)) return;

        numbers.Add(float.Parse(currentInput));
        currentInput = "";

        if (numbers.Count == 0) return;

        float result = EvaluateExpression();

        display.text = result.ToString();
        currentInput = result.ToString();

        numbers.Clear();
        operations.Clear();

        justCalculated = true;
    }

    private float EvaluateExpression()
    {
        List<float> values = new(numbers);
        List<char> ops = new(operations);

        for (int i = ops.Count - 1; i >= 0; i--)
        {
            if (ops[i] != '*' && ops[i] != '/') continue;

            float left = values[i];
            float right = values[i + 1];
            float result;

            if (ops[i] == '*')
            {
                result = left * right;
            }
            else
            {
                if (right == 0)
                {
                    display.text = "Error";
                    return 0;
                }

                result = left / right;
            }

            values[i] = result;
            values.RemoveAt(i + 1);

            ops.RemoveAt(i);
        }

        float finalResult = values[0];

        for (int i = 0; i < ops.Count; i++)
        {
            if (ops[i] == '+')
            {
                finalResult += values[i + 1];
            }
            else if (ops[i] == '-')
            {
                finalResult -= values[i + 1];
            }
        }

        return finalResult;
    }

    public void ClearCalculator()
    {
        currentInput = "";
        numbers.Clear();
        operations.Clear();

        justCalculated = false;

        display.text = "";
    }

    private void PlayClickSound() => AudioManager.Instance.PlaySFX(clickSound);
}
