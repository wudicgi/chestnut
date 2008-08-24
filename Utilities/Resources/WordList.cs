// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WudiStudio.Chestnut.Utilities.Resources
{
    public class WordList
    {
        /// <summary>
        /// �ʻ��б�
        /// </summary>
        private List<string> m_words;

        private int m_count;

        public List<string> Words
        {
            get { return m_words; }
            //set { m_words = value; }
        }

        public WordList()
        {
            // ��յ�ǰ�ʻ��б�
            m_words = new List<string>();
            m_count = 0;
        }

        public void Clear()
        {
            m_words.Clear();
            m_count = 0;
        }

        public void Add(string word)
        {
            m_words.Add(word);
            m_count++;
        }

        /// <summary>
        /// ���ļ���ȡ�ʻ�
        /// </summary>
        /// <remarks>
        /// �ļ���ʽ: 
        /// 1. �ļ�����Ϊ UTF-8
        /// 2. ÿ��һ����
        /// </remarks>
        /// <param name="path">�ļ�·��</param>
        public void ReadFromFile(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string[] parts;
            Dictionary<string, int> temp = new Dictionary<string, int>();
            string line;
            // ���ж�ȡ
            while ((line = reader.ReadLine()) != null)
            {
                // ����ǿ���
                if (line.Length == 0)
                {
                    // ����
                    continue;
                }
                // ���Ʊ���ָ��ʺ�Ƶ��
                parts = line.Split('\t');
                // ������ӵ���ǰ�ʻ��б���
                m_words.Add(parts[0]);
                m_count++;
            }
            reader.Close();
        }

        public void UpdateMetainfo(MetaInfo meta_info)
        {
            meta_info.Count = m_count;
        }
    }
}
