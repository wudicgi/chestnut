// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace WudiStudio.Chestnut
{
    /// <summary>
    /// ��׼ģʽ�� Tokenizer
    /// </summary>
    public class NormalTokenizer : Tokenizer
    {
        /// <summary>
        /// ����ִ��ַ�����
        /// </summary>
        static string[] m_b_chars;
        /// <summary>
        /// ����ִ���������
        /// </summary>
        static int[][] m_b_idxes;
        /// <summary>
        /// ����ִ��ַ�����
        /// </summary>
        static string[] m_f_chars;
        /// <summary>
        /// ����ִ���������
        /// </summary>
        static int[][] m_f_idxes;
        /// <summary>
        /// ���ֳɴ�Ƶ�ʱ�
        /// </summary>
        static Dictionary<string, float> m_s_freqs;

        /// <summary>
        /// ���شʵ�
        /// </summary>
        /// <param name="path">�ʵ��ļ�·��</param>
        public override void LoadLexicon(string path)
        {
            // �����л����ݵõ�����
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Hashtable lex = (Hashtable)formatter.Deserialize(fs);
            fs.Close();
            m_b_chars = (string[])lex["chars_b"];
            m_b_idxes = (int[][])lex["idxes_b"];
            m_f_chars = (string[])lex["chars_f"];
            m_f_idxes = (int[][])lex["idxes_f"];
            m_s_freqs = (Dictionary<string, float>)lex["freqs_chars"];
        }

        /// <summary>
        /// ����ִ�
        /// </summary>
        protected override void ProcessChinese_Backward()
        {
            int pos;
            int matched, idx, ofs, p;
            Debug("Backward:");
            Debug("===============");
            pos = m_buf_len - 1;
            while (pos >= 0)
            {
                Debug("pos = {0}", pos);
                matched = 0;
                idx = pos;
                ofs = 0;
                while (idx >= 0)
                {
                    Debug("Find {0} ... ", m_buf_char[idx]);
                    if ((p = m_b_chars[ofs].IndexOf(m_buf_char[idx], 1)) == -1)
                    {
                        Debug("Not Found.");
                        break;
                    }
                    else
                    {
                        Debug("Found at {0}.", p);
                        //ofs = int.Parse(m_b_idxes[ofs].Substring((p - 1) * 6, 6));
                        ofs = m_b_idxes[ofs][p];
                        if (m_b_chars[ofs][0] == 'K')
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
        /// ����ִ�
        /// </summary>
        protected override void ProcessChinese_Forward()
        {
            int pos;
            int matched, idx, ofs, p;
            Debug("Forward:");
            Debug("===============");
            pos = 0;
            while (pos < m_buf_len)
            {
                Debug("pos = {0}", pos);
                matched = 0;
                idx = pos;
                ofs = 0;
                while (idx < m_buf_len)
                {
                    Debug("Find {0} ... ", m_buf_char[idx]);
                    if ((p = m_f_chars[ofs].IndexOf(m_buf_char[idx], 1)) == -1)
                    {
                        Debug("Not Found.");
                        break;
                    }
                    else
                    {
                        Debug("Found at {0}.", p);
                        //ofs = int.Parse(m_f_idxes[ofs].Substring((p - 1) * 6, 6));
                        ofs = m_f_idxes[ofs][p];
                        if (m_f_chars[ofs][0] == 'K')
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
        /// ���㵥�ֳɴ�Ƶ��
        /// </summary>
        /// <param name="str">����</param>
        /// <param name="exp">����Ƶ��</param>
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
