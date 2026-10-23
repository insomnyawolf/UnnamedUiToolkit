using System;
using System.Runtime.CompilerServices;
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

            if(RuntimeHelpers.ReferenceEquals(OldValue, NewValue))
            {
                // If same reference they are exactly the same always
                return false;
            }
            
            if(RuntimeHelpers.Equals(OldValue, NewValue))
            {
                // Default C# comparer
                return false;
            }

            // == for UnityEngine.Object
            if (OldValue == NewValue)
            {
                return false;
            }


            // if (OldValue is null)
            // {
            //     return true;
            // }

            return true;
        }
    }
}