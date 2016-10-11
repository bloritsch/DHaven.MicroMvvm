#region Copyright 2016 D-Haven.org

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace DHaven.MicroMvvm
{
    public class ObservableObject : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> propertyValues = new Dictionary<string, object>();
        private bool isReadonly;

        /// <summary>
        /// Makes all properties backed with GetValue/SetValue read only.
        /// </summary>
        protected void MakeReadonly()
        {
            isReadonly = true;
        }

        protected TValue GetValue<TValue>(string propertyName)
        {
            Debug.Assert(propertyName != null, $"{nameof(propertyName)} is null");

            object value;
            if (!propertyValues.TryGetValue(propertyName, out value))
            {
                value = default(TValue);
            }

            return (TValue) value;
        }

        protected void SetValue<TValue>(string propertyName, TValue value)
        {
            if (isReadonly)
            {
                throw new ArgumentException($"{GetType().Name} is read-only.");
            }

            Debug.Assert(propertyName != null);

            if (propertyValues.ContainsKey(propertyName))
            {
                if (Equals(propertyValues[propertyName], value))
                {
                    // If it's the same value, return immediately.
                    return;
                }

                propertyValues[propertyName] = value;
            }
            else
            {
                propertyValues.Add(propertyName, value);
            }

            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
        }

        #region Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}