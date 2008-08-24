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
        /// 词汇列表
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
            // 清空当前词汇列表
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
        /// 从文件读取词汇
        /// </summary>
        /// <remarks>
        /// 文件格式: 
        /// 1. 文件编码为 UTF-8
        /// 2. 每行一个词
        /// </remarks>
        /// <param name="path">文件路径</param>
        public void ReadFromFile(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string[] parts;
            Dictionary<string, int> temp = new Dictionary<string, int>();
            string line;
            // 逐行读取
            while ((line = reader.ReadLine()) != null)
            {
                // 如果是空行
                if (line.Length == 0)
                {
                    // 跳过
                    continue;
                }
                // 按制表符分隔词和频率
                parts = line.Split('\t');
                // 将词添加到当前词汇列表中
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
