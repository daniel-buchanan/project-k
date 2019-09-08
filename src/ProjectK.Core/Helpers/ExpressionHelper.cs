using System;
using System.Linq.Expressions;

namespace ProjectK.Core.Helpers
{
    public class ExpressionHelper
    {
        private readonly ConstantAccess _constantAccess;
        private readonly MemberAccess _memberAccess;
        private readonly ConvertAccess _convertAccess;
        private readonly ReflectionHelper _reflectionHelper;

        public ExpressionHelper()
        {
            // setup helpers
            _constantAccess = new ConstantAccess();
            _memberAccess = new MemberAccess();
            _convertAccess = new ConvertAccess();
            _reflectionHelper = new ReflectionHelper();
        }

        /// <summary>
        /// Get the field name from the expression. i.e. p => p.ID (ID would be the field name). Note that this *should* be a SIMPLE expression, as in the previous example.
        /// </summary>
        /// <param name="expression">The expression to get the name from</param>
        /// <returns>The name of the field</returns>
        public string GetName(Expression expression)
        {
            // switch on node type
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return _convertAccess.GetName(expression, this);
                case ExpressionType.MemberAccess:
                    return _memberAccess.GetName(expression);
                case ExpressionType.Constant:
                    return _memberAccess.GetName(expression);
                case ExpressionType.Call:
                    {
                        var call = (MethodCallExpression)expression;
                        return GetName(call.Object);
                    }
                case ExpressionType.Lambda:
                    var body = ((LambdaExpression)expression).Body;
                    return this.GetName(body);
            }

            // if not found, return null
            return null;
        }

        public string GetMethodName(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda) expression = ((LambdaExpression)expression).Body;

            if (!(expression is MethodCallExpression)) return null;

            var call = (MethodCallExpression)expression;
            return call.Method.Name;
        }

        /// <summary>
        /// Get the name of the parameter used in an expression. i.e. p => p.ID (p would be the parameter name). Note that this *should* be a SIMPLE expression, as in the previous example.
        /// </summary>
        /// <param name="expr">The expression to get the parameter name from</param>
        /// <returns>The paramter name</returns>
        public string GetParameterName(Expression expr)
        {
            // check the expression for null
            if (expr == null)
                throw new ArgumentNullException("propertyRefExpr", "propertyRefExpr is null.");

            // check for member expressions
            if (expr is MemberExpression)
            {
                // get member expression
                var memberExpression = (MemberExpression)expr;

                // check if we have a parameter expression
                if (memberExpression.Expression is ParameterExpression)
                {
                    // if so return name
                    return ((ParameterExpression)memberExpression.Expression).Name;
                }
            }

            if (expr is MethodCallExpression)
            {
                var call = (MethodCallExpression)expr;
                return GetParameterName(call.Object);
            }

            // if the expression is not a lambda, return nothing
            if (!(expr is LambdaExpression)) return null;

            // get lambda and parameter
            var lambdaExpr = (LambdaExpression)expr;
            var param = (ParameterExpression)lambdaExpr.Parameters[0];

            // if no parameter, return null
            if (param == null)
                return null;

            // return parameter name
            return param.Name;
        }

        /// <summary>
        /// Get the table/type name
        /// </summary>
        /// <typeparam name="TObject">The type to get the name for</typeparam>
        /// <returns>The name of the type or table</returns>
        public string GetTypeName<TObject>()
        {
            // use the reflection helper to get the table name
            // NOTE: this will use the [TableName] attribute if present
            return typeof(TObject).Name;
        }

        public object GetValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                try
                {
                    expression = _convertAccess.GetExpression(expression);
                }
                catch
                {
                    return null;
                }
            }


            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return _memberAccess.GetValue(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return _constantAccess.GetValue(expression);
            }
            else if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetValue(body);
            }
            else if (expression.NodeType == ExpressionType.Call)
            {
                object result = null;
                try
                {
                    result = Expression.Lambda(expression).Compile().DynamicInvoke();
                }
                catch { }

                return result;
            }

            return null;
        }

        public Type GetType(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                return _convertAccess.GetType(expression, this);
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return _memberAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                return _constantAccess.GetType(expression);
            }
            else if (expression.NodeType == ExpressionType.Lambda)
            {
                var body = ((LambdaExpression)expression).Body;
                return GetType(body);
            }

            return null;
        }

        private class ConstantAccess
        {
            public object GetValue(Expression expression)
            {
                return ((ConstantExpression)expression).Value;
            }

            public Type GetType(Expression expression)
            {
                return ((ConstantExpression)expression).Type;
            }
        }

        private class MemberAccess
        {
            private ReflectionHelper _reflectionHelper = new ReflectionHelper();

            public object GetValue(Expression expression)
            {
                var objectMember = Expression.Convert(expression, expression.Type);
                var getterLambda = Expression.Lambda(objectMember);

                try
                {
                    var getter = getterLambda.Compile();

                    return getter.DynamicInvoke();
                }
                catch
                {
                    return null;
                }
            }

            public Type GetType(Expression expression)
            {
                return ((MemberExpression)expression).Type;
            }

            public string GetName(Expression expression)
            {
                var memberExpr = (MemberExpression)expression;
                return memberExpr.Member.Name;
            }
        }

        private class ConvertAccess
        {
            private Expression GetOperand(Expression expression)
            {
                var unaryExpression = (UnaryExpression)expression;
                return unaryExpression.Operand;
            }

            public Expression GetExpression(Expression expression)
            {
                return GetOperand(expression);
            }

            public object GetValue(Expression expression)
            {
                return GetValue(GetOperand(expression));
            }

            public Type GetType(Expression expression, ExpressionHelper helper)
            {
                return helper.GetType(GetOperand(expression));
            }

            public string GetName(Expression expression, ExpressionHelper helper)
            {
                return helper.GetName(GetOperand(expression));
            }
        }
    }
}
