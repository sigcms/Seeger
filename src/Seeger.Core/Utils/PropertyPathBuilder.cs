using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Seeger.Utils
{
    public static class PropertyPathBuilder
    {
        public static string BuildPropertyPath(Expression propertyAccessExpression)
        {
            var visitor = new Visitor();
            visitor.Visit(propertyAccessExpression);
            return visitor.Path.ToString();
        }

        class Visitor
        {
            public StringBuilder Path = new StringBuilder();

            public Action<Expression, StringBuilder> OnVisitedExpression = null;

            public void Visit(Expression expression)
            {
                if (expression.NodeType == ExpressionType.Lambda)
                {
                    VisitLambda((LambdaExpression)expression);
                }
                else if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    VisitMemberExpression((MemberExpression)expression);
                }
                else if (expression.NodeType == ExpressionType.ArrayIndex)
                {
                    VisitArrayIndex((BinaryExpression)expression);
                }
                else if (expression.NodeType == ExpressionType.ArrayLength)
                {
                    VisitArrayLength((UnaryExpression)expression);
                }
                else if (expression.NodeType == ExpressionType.Call)
                {
                    VisitMethodCall((MethodCallExpression)expression);
                }

                if (OnVisitedExpression != null)
                {
                    OnVisitedExpression(expression, Path);
                }
            }

            private void VisitLambda(LambdaExpression expression)
            {
                var body = expression.Body;

                if (body is UnaryExpression)
                {
                    Visit(((UnaryExpression)body).Operand);
                }
                else
                {
                    Visit(body);
                }
            }

            private void VisitMemberExpression(MemberExpression expression)
            {
                if (expression.Expression != null)
                {
                    Visit(expression.Expression);
                }

                AppendIfPathIsNotEmpty(".");
                Path.Append(expression.Member.Name);
            }

            private void VisitArrayIndex(BinaryExpression expression)
            {
                Visit(expression.Left);
                Path.Append("[" + expression.Right + "]");
            }

            private void VisitArrayLength(UnaryExpression expression)
            {
                Visit(expression.Operand);
                Path.Append(".Length");
            }

            private void VisitMethodCall(MethodCallExpression expression)
            {
                // DictionaryProperty["key"] or ListProperty[index]
                if (expression.Method.Name == "get_Item")
                {
                    Visit(expression.Object);

                    var arg = (ConstantExpression)expression.Arguments[0];
                    Path.Append("[" + arg.Value + "]");
                }
            }

            private void AppendIfPathIsNotEmpty(string stringToAppend)
            {
                if (Path.Length > 0)
                {
                    Path.Append(stringToAppend);
                }
            }
        }
    }
}
