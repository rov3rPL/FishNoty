using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace KoiSoft.VSMine.ViewModels
{
    /// <summary>
    /// Klasa bazowa dla viewmodeli
    /// </summary>
    /// <remarks>Źródło: http://www.galasoft.ch/mvvm/</remarks>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, List<Action>> _callbacks = new Dictionary<string, List<Action>>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Provides access to the PropertyChanged event handler to derived classes.
        /// </summary>
        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return PropertyChanged;
            }
        }

        /// <summary>
        /// Verifies that a property name exists in this ViewModel. This method
        /// can be called before the property is used, for instance before
        /// calling RaisePropertyChanged. It avoids errors when a property name
        /// is changed but some places are missed.
        /// <para>This method is only active in DEBUG mode.</para>
        /// </summary>
        /// <param name="propertyName">Property name</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            var myType = this.GetType();
            if (myType.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <remarks>If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.</remarks>
        /// <param name="propertyName">The name of the property that
        /// changed.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected void RaisePropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that
        /// changed.</typeparam>
        /// <param name="propertyExpression">An expression identifying the property
        /// that changed.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:GenericMethodsShouldProvideTypeParameter",
            Justification = "This syntax is more convenient than other alternatives.")]
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                return;
            }

            var handler = PropertyChanged;

            if (handler != null)
            {
                var body = propertyExpression.Body as MemberExpression;
                var expression = body.Expression as ConstantExpression;
                handler(expression.Value, new PropertyChangedEventArgs(body.Member.Name));
            }
        }

        /// <summary>
        /// When called in a property setter, raises the PropertyChanged event for 
        /// the current property.
        /// </summary>
        /// <exception cref="InvalidOperationException">If this method is called outside
        /// of a property setter.</exception>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged()
        {
            var frames = new StackTrace();

            for (var i = 0; i < frames.FrameCount; i++)
            {
                var frame = frames.GetFrame(i).GetMethod() as MethodInfo;
                if (frame != null)
                    if (frame.IsSpecialName && frame.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
                    {
                        RaisePropertyChanged(frame.Name.Substring(4));
                        return;
                    }
            }

            throw new InvalidOperationException("This method can only by invoked within a property setter.");
        }

        public void RegisterPropertyObserver<T>(Expression<Func<T, object>> propertyExpression, Action callback)
        {
            if (propertyExpression == null)
            {
                return;
            }

            if (_callbacks.Keys.Count == 0)
            {
                this.PropertyChanged += ViewModelBase_PropertyChanged;
            }

            var body = propertyExpression.Body as UnaryExpression;
            string propertyName = ((MemberExpression)body.Operand).Member.Name;

            if (!_callbacks.ContainsKey(propertyName))
            {
                _callbacks[propertyName] = new List<Action>();
            }

            _callbacks[propertyName].Add(callback);
        }

        private void ViewModelBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_callbacks.ContainsKey(e.PropertyName))
            {
                var callbacks = _callbacks[e.PropertyName];
                foreach (var callback in callbacks)
                {
                    callback.Invoke();
                }
            }
        }

        #region IDataErrorInfo

        ///// <summary>
        ///// Instancja walidatora
        ///// </summary>
        //protected internal IValidator Validator { get; set; }

        ///// <summary>
        ///// Treść błędu
        ///// </summary>
        //public string Error
        //{
        //    get { return this.GetError(SelfValidate(String.Empty)); }
        //}

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        var validationResults = SelfValidate(columnName);

        //        if (validationResults == null) return string.Empty;

        //        var columnResults = validationResults.Errors.FirstOrDefault<ValidationFailure>(x => string.Compare(x.PropertyName, columnName, true) == 0);
        //        return columnResults != null ? columnResults.ErrorMessage : string.Empty;
        //    }
        //}

        ///// <summary>
        ///// Wywołanie walidacji
        ///// </summary>
        ///// <returns>Wynik walidacji</returns>
        //protected virtual ValidationResult SelfValidate(string sourcePropertyName)
        //{
        //    if (this.Validator == null)
        //    {
        //        return new ValidationResult();
        //    }
        //    else
        //    {
        //        return Validate(this.Validator);
        //    }
        //}

        ///// <summary>
        ///// Validation status
        ///// </summary>
        //protected bool IsValid
        //{
        //    get { return SelfValidate(String.Empty).IsValid; }
        //}

        //protected ValidationResult Validate(IValidator validatorInstance)
        //{
        //    return validatorInstance.Validate(this);
        //}

        //protected string GetError(ValidationResult result)
        //{
        //    var validationErrors = new StringBuilder();
        //    foreach (var validationFailure in result.Errors.Distinct(new LambdaEqualityComparer<ValidationFailure, string>((f) => f.ErrorMessage)))
        //    {
        //        validationErrors.Append(validationFailure.ErrorMessage);
        //        validationErrors.Append(Environment.NewLine);
        //    }
        //    return validationErrors.ToString();
        //}

        #endregion
    }
}
