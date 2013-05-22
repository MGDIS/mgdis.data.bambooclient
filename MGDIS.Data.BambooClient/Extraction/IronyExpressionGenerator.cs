#region License
// MGDIS.Data.BambooClient, an ADO.NET provider for Bamboo object prevalence engine
// Copyright (C) 2013 MGDIS
// For details see https://github.com/MGDIS/mgdis.data.bambooclient.

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

// This is free software, and you are welcome to redistribute it
// under certain conditions; for details see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Irony.Parsing;
using Irony.Samples.SQL;
using MGDIS.Data.BambooClient.Mapping;

namespace MGDIS.Data.BambooClient.Extraction
{
    public class IronyExpressionGenerator : IExpressionGenerator
    {
        private IMapper _Mapping;

        public List<string> SelectClause { get; private set; }
        public string FromTable { get; private set; }
        public Func<object, object> SelectFunction { get; private set; }
        public Func<object, bool> WhereFunction { get; private set; }

        private ParameterExpression p = Expression.Parameter(typeof(object), "p");

        public IronyExpressionGenerator(string CommandText, IMapper Mapping)
        {
            this._Mapping = Mapping;
            ParseTreeNode statementNode = ParseSQLRequest(CommandText);
            ExtractFromClause(statementNode);
            ExtractSelectClause(statementNode);
            ExtractWhereClause(statementNode);
        }

        private ParseTreeNode ParseSQLRequest(string CommandText)
        {
            Parser parser = new Parser(new LanguageData(new SQLGrammar()));
            ParseTree tree = parser.Parse(CommandText);
            ParseTreeNode node = tree.Root;
            if (node.ChildNodes.Count != 1)
                throw new NotImplementedException("Only a single statement is supported");
            ParseTreeNode statementNode = node.ChildNodes[0];
            if (statementNode.ToString() != "selectStmt")
                throw new NotImplementedException("Only SELECT statements are supported");
            return statementNode;
        }

        private void ExtractFromClause(ParseTreeNode statementNode)
        {
            ParseTreeNode fromClauseNode = statementNode.ChildNodes[4];
            ParseTreeNode fromListNode = fromClauseNode.ChildNodes[1];
            if (fromListNode.ChildNodes.Count != 1)
                throw new NotImplementedException("Only one table in the FROM statement is supported");
            ParseTreeNode fromTableNode = fromListNode.ChildNodes[0];
            FromTable = fromTableNode.ChildNodes[0].Token.Text;
        }

        private void ExtractSelectClause(ParseTreeNode statementNode)
        {
            ParseTreeNode selectListNode = statementNode.ChildNodes[2];
            ParseTreeNode columnItemListNode = selectListNode.ChildNodes[0];
            SelectClause = new List<string>(); // TODO : Improve SelectClause encapsulation (since this is a List, the content can be modified even if the set part of the property is private)
            if (columnItemListNode.Term.ToString() == "*")
                SelectClause.AddRange(_Mapping.GetFields(FromTable));
            else
                foreach (ParseTreeNode columnItemNode in columnItemListNode.ChildNodes)
                {
                    ParseTreeNode columnSourceNode = columnItemNode.ChildNodes[0];
                    ParseTreeNode columnIdNode = columnSourceNode.ChildNodes[0];
                    SelectClause.Add(columnIdNode.ChildNodes[0].Token.Text);
                }
            Expression ExprSelect = ExtractSelectExpression(p);
            SelectFunction = Expression.Lambda<Func<object, object>>(ExprSelect, p).Compile();
        }

        private void ExtractWhereClause(ParseTreeNode statementNode)
        {
            ParseTreeNode whereClauseNode = statementNode.ChildNodes[5];
            if (whereClauseNode.ChildNodes.Count > 0)
            {
                ParseTreeNode binaryNode = whereClauseNode.ChildNodes[1];
                Expression ExpWhere = RecursivelyExtractWhereExpression(binaryNode, p);
                WhereFunction = Expression.Lambda<Func<object, bool>>(ExpWhere, p).Compile();
            }
        }

        private Expression ExtractSelectExpression(Expression p)
        {

            List<Expression> exprList = new List<Expression>();
            foreach (string selectAtom in SelectClause)
            {
                PropertyInfo fieldProperty = _Mapping.GetPropertyInfo(FromTable, selectAtom);
                exprList.Add(Expression.Call(
                    Expression.Constant(fieldProperty),
                    typeof(System.Reflection.PropertyInfo).GetMethod("GetValue", new Type[] { typeof(object), typeof(object[]) }),
                    p,
                    Expression.Constant(new object[0])));
            }

            return Expression.NewArrayInit(typeof(object), exprList);
        }

        private Expression RecursivelyExtractWhereExpression(ParseTreeNode node, Expression p)
        {
            if (node.Term.Name == "Id")
            {
                string fieldName = node.ChildNodes[0].Token.Text;
                int indexField = SelectClause.IndexOf(fieldName);
                if (indexField == -1)
                    throw new NotImplementedException("Use of a field in the WHERE clause without it being present in the SELECT clause is not supported at this time");

                return Expression.Convert(
                    Expression.ArrayAccess(
                        Expression.Convert(p, typeof(object[])),
                        Expression.Constant(indexField)),
                    _Mapping.GetPropertyInfo(FromTable, fieldName).PropertyType);
            }
            // TODO : all other types should be supported
            else if (node.Term.Name == "string")
            {
                return Expression.Constant(node.Token.Value.ToString());
            }
            else if (node.ChildNodes.Count() == 3)
            {
                Expression Variable = RecursivelyExtractWhereExpression(node.ChildNodes[0], p);
                string Operator = node.ChildNodes[1].ChildNodes[0].Term.Name;
                Expression Operand = RecursivelyExtractWhereExpression(node.ChildNodes[2], p);

                switch (Operator)
                {
                    case "<": return Expression.LessThan(Variable, Operand);
                    case "<=": return Expression.LessThanOrEqual(Variable, Operand);
                    case ">": return Expression.GreaterThan(Variable, Operand);
                    case ">=": return Expression.GreaterThanOrEqual(Variable, Operand);
                    case "=": return Expression.Equal(Variable, Operand);
                    case "AND": return Expression.And(Variable, Operand);
                    case "OR": return Expression.Or(Variable, Operand);
                    default: throw new NotImplementedException(string.Format("{0} is not a supported operator", Operator));
                }
            }
            else
                throw new BambooException("WHERE clause could not be successfully extracted");
        }
    }
}
