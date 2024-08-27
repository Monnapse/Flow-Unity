using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Packages
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [Serializable]
    public class KeyBind
    {
        public KeyCode KeyCode;
        public bool Inverted;
        public Axis ValueAxis = Axis.X;
        private bool HasBeenCalledValue = false;
        private bool ReleasedHasBeenCalledValue = true;
        private Action KeyPress;
        private Action KeyReleased;

        public KeyBind(KeyCode KeyCode)
        {
            this.KeyCode = KeyCode;
        }
        public KeyBind(KeyCode KeyCode, bool Inverted)
        {
            this.KeyCode = KeyCode;
            this.Inverted = Inverted;
        }
        public KeyBind(KeyCode KeyCode, bool Inverted, Axis ValueAxis)
        {
            this.KeyCode = KeyCode;
            this.Inverted = Inverted;
            this.ValueAxis = ValueAxis;
        }
        public void PressConnect(Action KeyPress) { this.KeyPress = KeyPress; }
        public void ReleasedConnect(Action KeyReleased) { this.KeyReleased = KeyReleased; }
        public bool HasBeenCalled() { return HasBeenCalledValue; }
        public void Uncall() { HasBeenCalledValue = false; }
        public void CallReleased()
        {
            if (!ReleasedHasBeenCalledValue && this.KeyReleased != null)
            {
                ReleasedHasBeenCalledValue = true;
                KeyReleased.Invoke();
            }
        }
        public void CallPressed()
        {
            if (!HasBeenCalledValue)
            {
                HasBeenCalledValue = true;
                ReleasedHasBeenCalledValue = false;
                if (KeyPress != null)
                {
                    KeyPress.Invoke();
                }
            }
        }
        
    }

    public class ValueClass
    {
        public float Value1 = 0;
        public float Value2 = 0;
        public float Value3 = 0;

        public ValueClass(float Value1, float Value2, float Value3)
        {
            this.Value1 = Value1;
            this.Value2 = Value2;
            this.Value3 = Value3;
        }
    }

    public class ValueHolderClass
    {
        public ValueClass Value = new ValueClass(0, 0, 0);

        public ValueClass GetValue()
        {
            return Value;
        }

        public void UpdateValue(float FloatValue, Axis ValueAxis)
        {
            if (ValueAxis == Axis.X)
            {
                Value.Value1 = FloatValue;
            } else if (ValueAxis == Axis.Y)
            {
                Value.Value2 = FloatValue;
            } else if (ValueAxis == Axis.Z)
            {
                Value.Value3 = FloatValue;
            }
        }
    }

    public class Flow
    {
        private Action<KeyCode> KeyPressDown;
        private Action<KeyCode> KeyPressUp;
        private List<KeyBind> Keybinds = new List<KeyBind>();
        private ValueHolderClass ValueHolder = new ValueHolderClass();

        public Flow(List<KeyBind> Keybinds)
        {
            this.Keybinds = Keybinds;
        }
        public Flow(List<KeyBind> Keybinds, Action<KeyCode> KeyPressDown)
        {
            this.Keybinds = Keybinds;
            this.KeyPressDown = KeyPressDown;
        }
        public Flow(List<KeyBind> Keybinds, Action<KeyCode> KeyPressDown, Action<KeyCode> KeyReleasedDown)
        {
            this.Keybinds = Keybinds;
            this.KeyPressDown = KeyPressDown;
            this.KeyPressUp = KeyReleasedDown;
        }

        public void CheckKeybind(KeyBind Keybind)
        {
            bool IsDown = Input.GetKey(Keybind.KeyCode);
            //bool Released = Input.GetKeyUp(Keybind.KeyCode);

            //if (Released)
            //{
                //    KeyPressUp.Invoke(Keybind.KeyCode);
            //    Keybind.CallReleased();
            //}
            if (IsDown)
            {
                //if (KeyPressDown != null)
                //{
                //    KeyPressDown.Invoke(Keybind.KeyCode);
                //}
                
                Keybind.CallPressed();

                int KeybindValue = 1;
                if (Keybind.Inverted)
                {
                    KeybindValue = -1;
                }
                ValueHolder.UpdateValue(KeybindValue, Keybind.ValueAxis);
            } else
            {
                if (Keybind.HasBeenCalled())
                {
                    ValueHolder.UpdateValue(0, Keybind.ValueAxis);
                }
                Keybind.Uncall();
                Keybind.CallReleased();
            }
        }

        public void Update()
        {
            foreach(KeyBind Key in Keybinds)
            {
                CheckKeybind(Key);
            }
        }

        public float GetFloatValue()
        {
            ValueClass Value = ValueHolder.GetValue();
            return Value.Value1;
        }
        public Vector2 GetVector2Value()
        {
            ValueClass Value = ValueHolder.GetValue();
            return new Vector2(Value.Value1, Value.Value2);
        }
        public Vector3 GetVector3Value()
        {
            ValueClass Value = ValueHolder.GetValue();
            return new Vector3(Value.Value1, Value.Value2, Value.Value3);
        }
    }
}
