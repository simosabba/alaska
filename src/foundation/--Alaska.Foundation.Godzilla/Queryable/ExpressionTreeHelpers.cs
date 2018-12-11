using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    internal static class ExpressionTreeHelpers
    {
        #region MethodCallExpression

        public static Type GetMethodExpressionGenericType(this MethodCallExpression m)
        {
            var expression = m.Method.GetParameters().FirstOrDefault();
            if (expression == null)
                throw new InvalidOperationException("Generic type not found for expression type");

            var func = expression.ParameterType.GenericTypeArguments.FirstOrDefault();
            if (func == null)
                throw new InvalidOperationException("Generic type not found for func type");

            var entityType = func.GenericTypeArguments.FirstOrDefault();
            if (entityType == null)
                throw new InvalidOperationException("Generic type not found for entity type");

            return entityType;
        }

        public static Type GetGenericMethodArgument(this MethodCallExpression m)
        {
            return m.Method.GetGenericArguments().First();
        }

        public static bool IsGenericMethod<TOutput>(this MethodCallExpression m, Type declaringType, string methodName)
        {
            return IsGenericMethod(m, declaringType, methodName) && m.Method.ReturnType == typeof(TOutput);
        }

        public static bool IsGenericMethod(this MethodCallExpression m, Type declaringType, string methodName)
        {
            return IsMethod(m, declaringType, methodName) && m.Method.IsGenericMethod;
        }

        public static bool IsNonGenericMethod<TOutput>(this MethodCallExpression m, Type declaringType, string methodName)
        {
            return IsNonGenericMethod(m, declaringType, methodName) && m.Method.ReturnType == typeof(TOutput);
        }

        public static bool IsNonGenericMethod(this MethodCallExpression m, Type declaringType, string methodName)
        {
            return IsMethod(m, declaringType, methodName) && !m.Method.IsGenericMethod;
        }

        public static bool IsMethod<TOutput>(this MethodCallExpression m, Type declaringType, string methodName)
        {
            return IsMethod(m, declaringType, methodName) && m.Method.ReturnType == typeof(TOutput);
        }

        public static bool IsMethod(this MethodCallExpression m, Type declaringType, string methodName)
        {
            return IsMatchingDeclaringType(declaringType, m.Method.DeclaringType) && m.Method.Name == methodName;
        }

        public static Expression GetUnaryExpressionOperand(this MethodCallExpression m)
        {
            return ((UnaryExpression)m.GetMemberExpression()).Operand;
        }

        public static string GetValueFromMemberExpression(this MethodCallExpression m)
        {
            return GetValueFromExpression(m.GetMemberExpression());
        }

        public static Expression GetMemberExpression(this MethodCallExpression m)
        {
            return m.Arguments.First();
        }

        private static bool IsMatchingDeclaringType(Type declaringType, Type other)
        {
            if (declaringType.FullName.EndsWith("`1"))
                return other.FullName.StartsWith(declaringType.FullName);

            return declaringType == other;
        }

        #endregion

        #region UnaryExpression

        public static bool IsNotExpression(this UnaryExpression exp)
        {
            return exp.NodeType == ExpressionType.Not;
        }

        #endregion

        #region BinaryExpression

        public static string GetValueFromEqualsExpression(this BinaryExpression be, Type memberDeclaringType, string memberName)
        {
            if (be.NodeType != ExpressionType.Equal)
                throw new Exception("There is a bug in this program.");

            if (be.Left.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression me = (MemberExpression)be.Left;

                if (me.Member.DeclaringType == memberDeclaringType && me.Member.Name == memberName)
                {
                    return GetValueFromExpression(be.Right);
                }
            }
            else if (be.Right.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression me = (MemberExpression)be.Right;

                if (me.Member.DeclaringType == memberDeclaringType && me.Member.Name == memberName)
                {
                    return GetValueFromExpression(be.Left);
                }
            }

            // We should have returned by now. 
            throw new Exception("There is a bug in this program.");
        }

        #endregion

        #region Expression

        public static bool IsMemberEqualsValueExpression(this Expression exp, Type declaringType, string memberName)
        {
            if (exp.NodeType != ExpressionType.Equal)
                return false;

            BinaryExpression be = (BinaryExpression)exp;

            // Assert. 
            if (IsSpecificMemberExpression(be.Left, declaringType, memberName) &&
                IsSpecificMemberExpression(be.Right, declaringType, memberName))
                throw new Exception("Cannot have 'member' == 'member' in an expression!");

            return (IsSpecificMemberExpression(be.Left, declaringType, memberName) ||
                IsSpecificMemberExpression(be.Right, declaringType, memberName));
        }

        public static bool IsSpecificMemberExpression(this Expression exp, Type declaringType, string memberName)
        {
            return ((exp is MemberExpression) &&
                (((MemberExpression)exp).Member.DeclaringType == declaringType) &&
                (((MemberExpression)exp).Member.Name == memberName));
        }

        public static string GetValueFromExpression(this Expression expression)
        {
            if (expression.NodeType != ExpressionType.Constant)
                throw new InvalidQueryException(
                    string.Format("The expression type {0} is not supported to obtain a value.", expression.NodeType));

            return (string)(((ConstantExpression)expression).Value);
        }

        #endregion
    }
}
