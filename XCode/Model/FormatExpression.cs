﻿using System;
using System.Collections.Generic;
using XCode.Configuration;

namespace XCode
{
    /// <summary>格式化表达式。通过字段、格式化字符串和右值去构建表达式</summary>
    /// <remarks>右值可能为空，比如{0} Is Null</remarks>
    public class FormatExpression : Expression
    {
        #region 属性
        /// <summary>字段</summary>
        public FieldItem Field { get; set; }

        /// <summary>格式化字符串</summary>
        public String Format { get; set; }
        #endregion

        #region 构造
        /// <summary>构造格式化表达式</summary>
        /// <param name="field"></param>
        /// <param name="format"></param>
        /// <param name="value"></param>
        public FormatExpression(FieldItem field, String format, String value)
        {
            Field = field;
            Format = format;
            Text = value;
        }
        #endregion

        #region 输出
        /// <summary>已重载。输出字段表达式的字符串形式</summary>
        /// <param name="needBracket">外部是否需要括号。如果外部要求括号，而内部又有Or，则加上括号</param>
        /// <param name="ps">参数字典</param>
        /// <returns></returns>
        public override String GetString(Boolean needBracket, IDictionary<String, Object> ps)
        {
            if (Field == null || Format.IsNullOrWhiteSpace()) return null;

            if (ps == null) return String.Format(Format, Field.FormatedName, Text);

            // 参数化处理
            var name = Field.Name;
            var i = 2;
            while (ps.ContainsKey(name)) name = Field.Name + i++;

            // 数值留给字典
            ps[name] = Text;

            var op = Field.Factory;
            return String.Format(Format, Field.FormatedName, op.Session.FormatParameterName(name));
        }
        #endregion
    }
}