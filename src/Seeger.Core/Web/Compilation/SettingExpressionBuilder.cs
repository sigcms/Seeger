using Seeger.Config;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;

namespace Seeger.Web.Compilation
{
    public class SettingExpressionBuilder : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(System.Web.UI.BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
        {
            return new CodeSnippetExpression(typeof(GlobalSettingManager).FullName + ".Instance.GetValue(\"" + entry.Expression + "\")");
        }
    }
}
