using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// Extension methods to simplify working with symbols.
    /// </summary>
    /// <remarks>
    /// Inspiration for some of this came from http://themechanicalbride.blogspot.com/2007/03/symbols-on-steroids-in-c.html
    /// </remarks>
    public static class SymbolExtensions
    {
        /// <summary>
        /// Gets the dotted property path from an expression.
        /// </summary>
        /// <typeparam name="T">The Type of the base object for the expression. Used for Type Inference</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="obj">The object from which to get the properties</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static string GetPropertySymbol<T, TResult>(this T obj, Expression<Func<T, TResult>> expression)
        {
            return GetPropertySymbol(expression);
        }

        /// <summary>
        /// Gets the dotted property path from an expression.
        /// </summary>
        /// <typeparam name="T">The Type of the base object for the expression. Used for Type Inference</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static string GetPropertySymbol<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetPropertySymbol((LambdaExpression)expression);
        }


        /// <summary>
        /// Gets the dotted property path from an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static string GetPropertySymbol(LambdaExpression expression)
        {
            MemberExpression outermostExpression = expression.Body as MemberExpression;

            if (outermostExpression == null)
            {
                throw new ArgumentException("Invalid expression. Expression should consist of Member references only.");
            }

            string memberExpressions = outermostExpression
                .Unfold(memberExpression => memberExpression.Expression as MemberExpression)
                .TakeWhile(x => x != null)
                .Aggregate(string.Empty, (s, memberExpression) => memberExpression.Member.Name + "." + s);

            // remove trailing "."
            return memberExpressions.TrimEnd('.');
        }

        /// <summary>
        /// Gets a PropertyInfo from a LambdaExpression that retrieves a property.
        /// </summary>
        /// <typeparam name="T">The Type of the base object for the expression. Used for Type Inference</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetPropertyInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Gets the PropertyInfo from a LambdaExpression that retrieves a property.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(LambdaExpression expression)
        {
            MemberExpression outermostExpression = expression.Body as MemberExpression;

            if (outermostExpression == null)
            {
                throw new ArgumentException("Invalid expression. Expression should consist of Property reference only.");
            }

            if (!(outermostExpression.Expression is ParameterExpression))
            {
                throw new ArgumentException("Invalid expression. Expression should consist of a single property assessor.");
            }

            PropertyInfo propertyInfo = outermostExpression.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException("Invalid expression. Expression should consist of a single property assessor.");
            }

            return propertyInfo;
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Expression<Action> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetMethodInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            MethodCallExpression outermostExpression = expression.Body as MethodCallExpression;

            if (outermostExpression == null)
            {
                throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
            }

            return outermostExpression.Method;
        }
    }
}
