// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using WudiStudio.Chestnut.Utilities.Resources;

namespace WudiStudio.Chestnut.Utilities.Lexicons
{
    public class BinaryLexion : Lexicon
    {
        private const int LEX_SIGNATURE = 0x58454C42; // BLEX
        private const int LEX_THIS_VERSION = 0x00000001;

        private const byte OFS_END = 0;
        private const byte OFS_WORDS_BACKWARD = 1;
        private const byte OFS_WORDS_FORWARD = 2;
        private const byte OFS_FREQS_WORDS = 3;
        private const byte OFS_FREQS_CHARS = 4;
        private const byte OFS_META_INFO = 255;

        private MetaInfo m_meta_info;
        /// <summary>
        /// 词汇列表
        /// </summary>
        private WordList m_word_list;
        /// <summary>
        /// 频率列表
        /// </summary>
        private FreqList m_freq_list;

        //private string[] m_chars;
        //private int[][] m_idxes;

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

        public BinaryLexion()
        {
        }

        public void Build(string path, Contents contents)
        {
            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write);

            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);

            // 数据库信息，占用 8 bytes + 8 bytes = 16 bytes
            writer.Write(LEX_SIGNATURE); // 词典标识符，4 bytes
            writer.Write(LEX_THIS_VERSION); // 词典版本，4 bytes
            writer.Write(new byte[8]); // 保留位置，8 bytes

            // 偏移位置信息，占用 5 * 4 + 1 = 21 bytes
            writer.Write(OFS_WORDS_BACKWARD); // 1 byte
            writer.Write(0); // 逆向词汇起始位置，4 bytes
            writer.Write(OFS_WORDS_FORWARD); // 1 byte
            writer.Write(0); // 正向词汇起始位置，4 bytes
            writer.Write(OFS_FREQS_WORDS); // 1 byte
            writer.Write(0); // 词汇频率起始位置，4 bytes
            writer.Write(OFS_FREQS_CHARS); // 1 byte
            writer.Write(0); // 单字频率起始位置，4 bytes
            writer.Write(OFS_END);

            int ofs_cur = (int)writer.BaseStream.Position;

            List<string> words = m_word_list.Words;

            if ((contents & Contents.WordsBackward) != 0)
            {
                TrieTree tree_b = new TrieTree();
                for (int i = 0; i < words.Count; i++)
                {
                    tree_b.AddString(this.ReverseString(words[i]));
                }

                ArrayList al_b = tree_b.GetDoubleArrays();
                string[] chars_b = (string[])al_b[0];
                int[][] idxes_b = (int[][])al_b[1];
                int[] ofses_b = new int[idxes_b.Length];

                writer.BaseStream.Seek(17, SeekOrigin.Begin);
                writer.Write(ofs_cur);
                writer.BaseStream.Seek(0, SeekOrigin.End);

                for (int i = 0; i < idxes_b.Length; i++)
                {
                    ofses_b[i] = ofs_cur;
                    ofs_cur += Encoding.UTF8.GetByteCount(chars_b[i]) + idxes_b[i].Length * 4;
                }

                for (int i = 0; i < chars_b.Length; i++)
                {
                    byte[] buf = Encoding.UTF8.GetBytes(chars_b[i]);
                    writer.Write(buf.Length - 1);
                    writer.Write(buf);
                    for (int j = 1; j < idxes_b[i].Length; j++)
                    {
                        writer.Write(ofses_b[idxes_b[i][j]]);
                    }
                }
            }

            if ((contents & Contents.WordsForward) != 0)
            {
                TrieTree tree_f = new TrieTree();
                for (int i = 0; i < words.Count; i++)
                {
                    tree_f.AddString(words[i]);
                }

                ArrayList al_f = tree_f.GetDoubleArrays();
                string[] chars_f = (string[])al_f[0];
                int[][] idxes_f = (int[][])al_f[1];
                int[] ofses_f = new int[idxes_f.Length];

                writer.BaseStream.Seek(22, SeekOrigin.Begin);
                writer.Write(ofs_cur);
                writer.BaseStream.Seek(0, SeekOrigin.End);

                for (int i = 0; i < idxes_f.Length; i++)
                {
                    ofses_f[i] = ofs_cur;
                    ofs_cur += Encoding.UTF8.GetByteCount(chars_f[i]) + idxes_f[i].Length * 4;
                }

                for (int i = 0; i < chars_f.Length; i++)
                {
                    byte[] buf = Encoding.UTF8.GetBytes(chars_f[i]);
                    writer.Write(buf.Length - 1);
                    writer.Write(buf);
                    for (int j = 1; j < idxes_f[i].Length; j++)
                    {
                        writer.Write(ofses_f[idxes_f[i][j]]);
                    }
                }
            }

            if ((contents & Contents.FreqsWords) != 0)
            {
//                lex["freqs_words"] = m_freq_list.Words;
            }

            if ((contents & Contents.FreqsChars) != 0)
            {
                writer.BaseStream.Seek(32, SeekOrigin.Begin);
                writer.Write(ofs_cur);
                writer.BaseStream.Seek(0, SeekOrigin.End);

                Dictionary<string, float> chars = m_freq_list.Chars;

                foreach (string key in chars.Keys)
                {
                    byte[] buf = Encoding.UTF8.GetBytes(key);
                    writer.Write(buf.Length);
                    writer.Write(buf);
                    writer.Write(chars[key]);
                }

                writer.Write(0);

                ofs_cur = (int)writer.BaseStream.Position;
            }

            writer.Flush();

            writer.Close();
        }

        public void Read(string path)
        {
        }

        private void ReadFreqs()
        {
        }

        private void ReadWords_Backward()
        {
        }

        private void ReadWords_Backward_Process(int index, string prefix)
        {
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
