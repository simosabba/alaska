using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class QueryOperand : QueryNode
    {
        public QueryOperand(ExpressionType operand)
        {
            Operand = operand;
        }

        public ExpressionType Operand { get; private set; }

        public override string Representation => Operand.ToString();
    }
}
