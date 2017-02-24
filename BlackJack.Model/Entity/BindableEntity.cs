#region

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BlackJack.Model.Annotations;

#endregion

namespace BlackJack.Model.Entity
{
    [Serializable]
    public abstract class BindableEntity : INotifyPropertyChanged,IDisposable
    {
        #region Public Event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ProtectedMethods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers that
        ///     support CallerMemberName.
        /// </param>
        /// <returns>
        ///     True if the value was changed, false if the existing value matched the
        ///     desired value.
        /// </returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.OnPropertyChanged();
            return true;
        }

        #endregion

        #region DisposeMethods
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    //--- Release Managed Resource ---//
                }

                //--- Release Unmanaged Resource ---//
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion
    }
}