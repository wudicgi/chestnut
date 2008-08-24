// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WudiStudio.Chestnut
{
    /// <summary>
    /// Lazy 模式的 Tokenizer
    /// </summary>
    public class LazyTokenizer : Tokenizer
    {
        private const byte OFS_END = 0;
        private const byte OFS_WORDS_BACKWARD = 1;
        private const byte OFS_WORDS_FORWARD = 2;
        private const byte OFS_FREQS_WORDS = 3;
        private const byte OFS_FREQS_CHARS = 4;
        private const byte OFS_META_INFO = 255;

        private FileStream m_stream;
        private BinaryReader m_reader;

        private string m_b_root;
        private int m_b_root_idx;
        private string m_f_root;
        private int m_f_root_idx;

        /// <summary>
        /// 单字成词频率表
        /// </summary>
        static Dictionary<string, float> m_s_freqs;

        /// <summary>
        /// 加载词典
        /// </summary>
        /// <param name="path">词典文件路径</param>
        public override void LoadLexicon(string path)
        {
            int ofs_b = 0, ofs_f = 0, ofs_c = 0;
            m_stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            m_reader = new BinaryReader(m_stream, Encoding.UTF8);
            // 跳过文件头，to be noticed
            m_stream.Seek(16, SeekOrigin.Begin);
            byte sign_ofs;
            while ((sign_ofs = m_reader.ReadByte()) != OFS_END)
            {
                switch (sign_ofs)
                {
                    case OFS_WORDS_BACKWARD:
                        ofs_b = m_reader.ReadInt32();
                        break;
                    case OFS_WORDS_FORWARD:
                        ofs_f = m_reader.ReadInt32();
                        break;
                    case OFS_FREQS_WORDS:
                        m_stream.Seek(4, SeekOrigin.Current);
                        break;
                    case OFS_FREQS_CHARS:
                        ofs_c = m_reader.ReadInt32();
                        break;
                    case OFS_META_INFO:
                        m_stream.Seek(4, SeekOrigin.Current);
                        break;
                    default:
                        throw new Exception("Invalid offset type.");
                        //break;
                }
            }

            if (ofs_b > 0)
            {
                m_stream.Seek(ofs_b, SeekOrigin.Begin);
                int len = m_reader.ReadInt32();
                m_stream.Seek(1, SeekOrigin.Current);
                m_b_root = Encoding.UTF8.GetString(m_reader.ReadBytes(len));
                m_b_root_idx = (int)m_stream.Position;
            }

            if (ofs_f > 0)
            {
                m_stream.Seek(ofs_f, SeekOrigin.Begin);
                int len = m_reader.ReadInt32();
                m_stream.Seek(1, SeekOrigin.Current);
                m_f_root = Encoding.UTF8.GetString(m_reader.ReadBytes(len));
                m_f_root_idx = (int)m_stream.Position;
            }

            if (ofs_c > 0)
            {
                m_stream.Seek(ofs_c, SeekOrigin.Begin);
                string key;
                int len = m_reader.ReadInt32();
                m_s_freqs = new Dictionary<string, float>();
                while (len > 0)
                {
                    key = Encoding.UTF8.GetString(m_reader.ReadBytes(len));
                    m_s_freqs[key] = m_reader.ReadSingle();
                    len = m_reader.ReadInt32();
                }
            }
        }

        /// <summary>
        /// 逆向分词
        /// </summary>
        protected override void ProcessChinese_Backward()
        {
            int pos;
            int matched, idx, ofs, p;
            Debug("Backward:");
            Debug("===============");
            pos = m_buf_len - 1;
            int len = 0;
            string chars;
            while (pos >= 0)
            {
                Debug("pos = {0}", pos);
                matched = 0;
                idx = pos;
                ofs = 0;
                while (idx >= 0)
                {
                    Debug("Find {0} ... ", m_buf_char[idx]);
                    if (idx == pos)
                    {
                        chars = m_b_root;
                        m_stream.Seek(m_b_root_idx, SeekOrigin.Begin);
                    }
                    else
                    {
                        if (len == 0)
                        {
                            Debug("Not Found.");
                            break;
                        }
                        chars = Encoding.UTF8.GetString(m_reader.ReadBytes(len));
                    }
                    if ((p = chars.IndexOf(m_buf_char[idx], 1)) == -1)
                    {
                        Debug("Not Found.");
                        break;
                    }
                    else
                    {
                        Debug("Found at {0}.", p);
                        m_stream.Seek(p * 4, SeekOrigin.Current);
                        ofs = m_reader.ReadInt32();
                        m_stream.Seek(ofs, SeekOrigin.Begin);
                        len = m_reader.ReadInt32();
                        if (m_reader.ReadByte() == (byte)0x4B)
                        {
                            matched = pos - idx + 1;
                            Debug("Is K, matched = {0}", matched);
                        }
                    }
                    idx--;
                }
                if (matched > 0)
                {
                    Debug("Add: {0}", new string(m_buf_char, pos - matched + 1, matched));
                    m_list_backward.Add(new string(m_buf_char, pos - matched + 1, matched));
                    pos -= matched;
                }
                else
                {
                    Debug("Add: {0}", m_buf_char[pos]);
                    m_list_backward.Add(m_buf_char[pos].ToString());
                    pos--;
                }
            }
            m_list_backward.Reverse();
        }

        /// <summary>
        /// 正向分词
        /// </summary>
        protected override void ProcessChinese_Forward()
        {
            int pos;
            int matched, idx, ofs, p;
            Debug("Forward:");
            Debug("===============");
            pos = 0;
            int len = 0;
            string chars;
            while (pos < m_buf_len)
            {
                Debug("pos = {0}", pos);
                matched = 0;
                idx = pos;
                ofs = 0;
                while (idx < m_buf_len)
                {
                    Debug("Find {0} ... ", m_buf_char[idx]);
                    if (idx == pos)
                    {
                        chars = m_f_root;
                        m_stream.Seek(m_f_root_idx, SeekOrigin.Begin);
                    }
                    else
                    {
                        if (len == 0)
                        {
                            Debug("Not Found.");
                            break;
                        }
                        chars = Encoding.UTF8.GetString(m_reader.ReadBytes(len));
                    }
                    if ((p = chars.IndexOf(m_buf_char[idx], 1)) == -1)
                    {
                        Debug("Not Found.");
                        break;
                    }
                    else
                    {
                        Debug("Found at {0}.", p);
                        m_stream.Seek(p * 4, SeekOrigin.Current);
                        ofs = m_reader.ReadInt32();
                        m_stream.Seek(ofs, SeekOrigin.Begin);
                        len = m_reader.ReadInt32();
                        if (m_reader.ReadByte() == (byte)0x4B)
                        {
                            matched = idx - pos + 1;
                            Debug("Is K, matched = {0}", matched);
                        }
                    }
                    idx++;
                }
                if (matched > 0)
                {
                    Debug("Add: {0}", new string(m_buf_char, pos, matched));
                    m_list_forward.Add(new string(m_buf_char, pos, matched));
                    pos += matched;
                }
                else
                {
                    Debug("Add: {0}", m_buf_char[pos]);
                    m_list_forward.Add(m_buf_char[pos].ToString());
                    pos++;
                }
            }
        }

        /// <summary>
        /// 计算单字成词频率
        /// </summary>
        /// <param name="str">单字</param>
        /// <param name="exp">最终频率</param>
        protected override void ProcessChinese_Frequency(string str, ref float exp)
        {
            Debug("Calc: {0}", str);
            if (str.Length == 1)
            {
                if (m_s_freqs.ContainsKey(str))
                {
                    exp *= m_s_freqs[str];
                }
                else
                {
                    exp = 0;
                }
            }
        }
    }
}
