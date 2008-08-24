// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WudiStudio.Chestnut.Utilities.Resources
{
    public class FreqList
    {
        private Dictionary<string, int> m_words;
        private Dictionary<string, int> m_chars;
        private int m_size;

        public Dictionary<string, float> Words
        {
            get
            {
                float size_f = (float)m_size;
                Dictionary<string, float> dict = new Dictionary<string, float>();
                foreach (string key in m_words.Keys)
                {
                    dict[key] = m_words[key] / size_f;
                }
                return dict;
            }
        }

        public Dictionary<string, float> Chars
        {
            get
            {
                float size_f = (float)m_size;
                Dictionary<string, float> dict = new Dictionary<string, float>();
                foreach (string key in m_chars.Keys)
                {
                    dict[key] = m_chars[key] / size_f;
                }
                return dict;
            }
        }

        public FreqList()
        {
            m_words = new Dictionary<string, int>();
            m_chars = new Dictionary<string, int>();
            m_size = 0;
        }

        public void Clear()
        {
            m_words.Clear();
            m_chars.Clear();
            m_size = 0;
        }

        public void Add(string word, int freq)
        {
            m_words[word] = freq;
            // �����ǰ��Ϊ���ִ�
            if (word.Length == 1)
            {
                m_chars[word] = freq;
            }
            m_size += freq;
        }

        public void ReadFromFile(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string[] parts;
            int freq;
            Dictionary<string, int> temp = new Dictionary<string, int>();
            string line;
            // ���ж�ȡ
            while ((line = reader.ReadLine()) != null)
            {
                // ������ǿ���
                if (line.Length > 0)
                {
                    // ���Ʊ���ָ��ʺͳ��ִ���
                    parts = line.Split('\t');
                    // ����д�����Ϣ
                    if (parts.Length > 1)
                    {
                        // ������������ (����)
                        freq = int.Parse(parts[1]);
                        m_words[parts[0]] = freq;
                        // �����ǰ��Ϊ���ִ�
                        if (parts[0].Length == 1)
                        {
                            m_chars[parts[0]] = freq;
                        }
                        m_size += freq;
                    }
                }
            }
            reader.Close();
        }

        public void UpdateMetainfo(MetaInfo meta_info)
        {
            meta_info.Size = m_size;
        }
    }
}
