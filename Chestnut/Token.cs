// $Id$

using System;
using System.Collections.Generic;
using System.Text;

namespace WudiStudio.Chestnut
{
    /// <summary>
    /// Token 类
    /// </summary>
    public class Token
    {
        private string m_text;
        private int m_start;
        private int m_end;

        /// <summary>
        /// 词
        /// </summary>
        public string Text
        {
            get { return m_text; }
        }

        /// <summary>
        /// 起始位置
        /// </summary>
        public int Start
        {
            get { return m_start; }
        }

        /// <summary>
        /// 结尾位置
        /// </summary>
        public int End
        {
            get { return m_end; }
        }

        /// <summary>
        /// 构造一个 Token
        /// </summary>
        /// <param name="text">词</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结尾位置</param>
        public Token(string text, int start, int end)
        {
            m_text = text;
            m_start = start;
            m_end = end;
        }
    }
}
