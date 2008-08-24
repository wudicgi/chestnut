// $Id$

using System;
using System.Collections.Generic;
using System.Text;

namespace WudiStudio.Chestnut
{
    /// <summary>
    /// Token ��
    /// </summary>
    public class Token
    {
        private string m_text;
        private int m_start;
        private int m_end;

        /// <summary>
        /// ��
        /// </summary>
        public string Text
        {
            get { return m_text; }
        }

        /// <summary>
        /// ��ʼλ��
        /// </summary>
        public int Start
        {
            get { return m_start; }
        }

        /// <summary>
        /// ��βλ��
        /// </summary>
        public int End
        {
            get { return m_end; }
        }

        /// <summary>
        /// ����һ�� Token
        /// </summary>
        /// <param name="text">��</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">��βλ��</param>
        public Token(string text, int start, int end)
        {
            m_text = text;
            m_start = start;
            m_end = end;
        }
    }
}
