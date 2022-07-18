using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class Tag : PropertyAttribute
{
    int selected;
    string name;
    public int Selected { get { return selected; } set { selected = value; } }
    public string Name { get { return name; } set { name = value; } }
}
