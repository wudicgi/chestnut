// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using WudiStudio.Chestnut.Utilities.Resources;

namespace WudiStudio.Chestnut.Utilities.Lexicons
{
    public class DotnetLexion : Lexicon
    {
        private MetaInfo m_meta_info;
        /// <summary>
        /// 词汇列表
        /// </summary>
        private WordList m_word_list;
        /// <summary>
        /// 频率列表
        /// </summary>
        private FreqList m_freq_list;

        private string[] m_chars;
        private int[][] m_idxes;

        public MetaInfo MetaInfo
        {
            get { return m_meta_info; }
            set { m_meta_info = value; }
        }

        public WordList WordList
        {
            get { return m_word_list; }
            set { m_word_list = value; }
        }

        public FreqList FreqList
        {
            get { return m_freq_list; }
            set { m_freq_list = value; }
        }

        public DotnetLexion()
        {
        }

        public void Build(string path, Contents contents)
        {
            Hashtable lex = new Hashtable();

            Hashtable metainfo = new Hashtable();
            metainfo.Add("version", m_meta_info.Version);
            metainfo.Add("title", m_meta_info.Title);
            metainfo.Add("author", m_meta_info.Author);
            metainfo.Add("creator", m_meta_info.Creator);
            metainfo.Add("contents", m_meta_info.Contents);
            metainfo.Add("count", m_meta_info.Count);
            metainfo.Add("size", m_meta_info.Size);
            metainfo.Add("terminal", m_meta_info.Terminal);

            lex["metainfo"] = metainfo;

            List<string> words = m_word_list.Words;
            if ((contents & Contents.WordsBackward) != 0)
            {
                TrieTree tree_b = new TrieTree();
                for (int i = 0; i < words.Count; i++)
                {
                    tree_b.AddString(this.ReverseString(words[i]));
                }
                ArrayList al_b = tree_b.GetDoubleArrays();
                lex["chars_b"] = al_b[0];
                lex["idxes_b"] = al_b[1];
            }
            if ((contents & Contents.WordsForward) != 0)
            {
                TrieTree tree_f = new TrieTree();
                for (int i = 0; i < words.Count; i++)
                {
                    tree_f.AddString(words[i]);
                }
                ArrayList al_f = tree_f.GetDoubleArrays();
                lex["chars_f"] = al_f[0];
                lex["idxes_f"] = al_f[1];
            }
            if ((contents & Contents.FreqsWords) != 0)
            {
                lex["freqs_words"] = m_freq_list.Words;
            }
            if ((contents & Contents.FreqsChars) != 0)
            {
                lex["freqs_chars"] = m_freq_list.Chars;
            }
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(fs, lex);
            fs.Close();
        }

        public void Read(string path)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Hashtable lex = (Hashtable)formatter.Deserialize(fs);
            fs.Close();

            Hashtable meta_info = (Hashtable)lex["metainfo"];
            m_meta_info = new MetaInfo();
            m_meta_info.Version = (string)meta_info["version"];
            m_meta_info.Title = (string)meta_info["title"];
            m_meta_info.Author = (string)meta_info["author"];
            m_meta_info.Creator = (string)meta_info["creator"];
            m_meta_info.Contents = (int)meta_info["contents"];
            m_meta_info.Count = (int)meta_info["count"];
            m_meta_info.Size = (int)meta_info["size"];
            m_meta_info.Terminal = (int)meta_info["terminal"];

            Contents tmp = (Contents)m_meta_info.Contents;

            if ((tmp & Contents.WordsBackward) != 0)
            {
                m_chars = (string[])lex["chars_b"];
                m_idxes = (int[][])lex["idxes_b"];
                this.ReadWords_Backward();
            }
            else if ((tmp & Contents.WordsForward) != 0)
            {
            }
            if ((tmp & (Contents.FreqsWords | Contents.FreqsChars)) > 0)
            {
                this.ReadFreqs();
            }
        }

        private void ReadFreqs()
        {
        }

        private void ReadWords_Backward()
        {
            m_word_list = new WordList();

            string chars = m_chars[0];
            int[] idxes = m_idxes[0];

            for (int i = 1; i < chars.Length; i++)
            {
                this.ReadWords_Backward_Process(idxes[i], chars[i].ToString());
            }

            m_word_list.Words.Sort();

            FileStream fs = new FileStream("_ttt.lex.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            for (int i = 0; i < m_word_list.Words.Count; i++)
            {
                sw.WriteLine(m_word_list.Words[i]);
            }
            sw.Flush();
            sw.Close();
        }

        private void ReadWords_Backward_Process(int index, string prefix)
        {
            string chars = m_chars[index];
            int[] idxes = m_idxes[index];
            if (chars[0] == 'K')
            {
                m_word_list.Add(prefix);
            }
            for (int i = 1; i < chars.Length; i++)
            {
                this.ReadWords_Backward_Process(idxes[i], chars[i].ToString() + prefix);
            }
        }

        public void Info()
        {
        }

        private string ReverseString(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
