using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }

    public abstract class BaseViewModel<TClass> : BaseViewModel where TClass : BaseViewModel<TClass>
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the property
        /// in the given expression.
        /// </summary>
        /// <param name="expression">
        /// A member expression used to refer to the changed property in a type-safe way.
        /// For example, to raise an event for the "Foo" property you could pass the expression
        /// "x => x.Foo".
        /// </param>
        /// <exception cref="ArgumentException">
        /// The given expression is not a member expression for a property.
        /// </exception>
        protected void RaisePropertyChanged(Expression<Func<TClass, object>> expression)
        {
            if (expression == null)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug, LogViewModel.LogMassage.AnotherMassage, "ArgumentNullException(\"expression\")");
                return;
            }

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                RaisePropertyChanged(memberExpression.Member.Name);
            }
        }

        #endregion
    }

 
}
