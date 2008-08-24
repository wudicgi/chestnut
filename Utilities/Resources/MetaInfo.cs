// $Id$

using System;
using System.Collections.Generic;
using System.Text;

namespace WudiStudio.Chestnut.Utilities.Resources
{
    public class MetaInfo
    {
        private string m_version;
        private string m_title;
        private string m_author;
        private string m_creator;
        private int m_contents;
        private int m_count;
        private int m_size;
        private int m_terminal;

        public string Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        public string Creator
        {
            get { return m_creator; }
            set { m_creator = value; }
        }

        public int Contents
        {
            get { return m_contents; }
            set { m_contents = value; }
        }

        public int Count
        {
            get { return m_count; }
            set { m_count = value; }
        }

        public int Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public int Terminal
        {
            get { return m_terminal; }
            set { m_terminal = value; }
        }

        public static MetaInfo Create(string path)
        {
            return new MetaInfo();
        }

        public MetaInfo()
        {
            m_version = "1.0.0";
            m_title = "Undefined";
            m_author = "Unknown";
            m_creator = "Chestnut.Utilities";
            m_contents = 0;
            m_count = 0;
            m_size = 0;
            m_terminal = 1;
        }
    }
}
