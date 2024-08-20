# Flow-Unity
Easily create key bind managers with ease.

Original made for roblox & now rewritten in C# for Unity.

# Example
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Packages;
using System.Diagnostics;

[System.Serializable]
public class MovementKeyBindsClass
{
    public KeyBind Forward = new KeyBind(KeyCode.W, false, Axis.Y);
    public KeyBind Backwards = new KeyBind(KeyCode.S, true, Axis.Y);
    public KeyBind Left = new KeyBind(KeyCode.A, false, Axis.X);
    public KeyBind Right = new KeyBind(KeyCode.D, true, Axis.X);
}
[System.Serializable]
public class LoadoutKeyBindsClass
{
    public KeyBind Item1 = new KeyBind(KeyCode.Alpha1);
    public KeyBind Item2 = new KeyBind(KeyCode.Alpha2);
}

public class InputController : MonoBehaviour
{
    public MovementKeyBindsClass MovementKeybinds = new MovementKeyBindsClass();
    public LoadoutKeyBindsClass LoadoutKeyBinds = new LoadoutKeyBindsClass();

    private Flow MovementInputController;
    private Flow LoadoutInputController;

    protected void Pressing(KeyCode KeyBind)
    {
        UnityEngine.Debug.Log("Holding KeyBind: " + KeyBind);
    }
    protected void Pressed()
    {
        UnityEngine.Debug.Log("Pressed KeyBind");
    }


    public void StartInputController()
    {
        MovementInputController = new Flow(
            new List<KeyBind>() { MovementKeybinds.Forward, MovementKeybinds.Backwards, MovementKeybinds.Left, MovementKeybinds.Right }, 
            Pressing
        );

        LoadoutKeyBinds.Item1.Connect(Pressed);

        LoadoutInputController = new Flow(
            new List<KeyBind>() { LoadoutKeyBinds.Item1, LoadoutKeyBinds.Item2 },
            null
        );
    }

    public void UpdateInputController()
    {
        MovementInputController.Update();
        LoadoutInputController.Update();
        UnityEngine.Debug.Log(MovementInputController.GetVector3Value());
    }
}
```
