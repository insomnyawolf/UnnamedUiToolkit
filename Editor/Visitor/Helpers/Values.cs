using System;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public class Values
    {
        private object OldValue;
        private object NewValue;
        public object Value => NewValue;

        public void Update(object UpdateValue)
        {
            OldValue = NewValue;

            NewValue = UpdateValue;
        }

        public bool HasChanged()
        {
            // Maybe use !object.Equals(OldValue, NewValue);

            if (OldValue == NewValue)
            {
                return false;
            }

            if (OldValue is null)
            {
                return true;
            }

            if (OldValue.Equals(NewValue))
            {
                return false;
            }

            return true;
        }
    }
}