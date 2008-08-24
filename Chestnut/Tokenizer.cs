// $Id$

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace WudiStudio.Chestnut
{
    /// <summary>
    /// ƥ��ģʽ
    /// </summary>
    public enum MatchMode
    {
        /// <summary>
        /// ��׼ƥ�� (ʹ��˫��ƥ�䣬���ݵ��ֳɴ�Ƶ�ʴ�������)
        /// </summary>
        Normal,
        /// <summary>
        /// ����ƥ�� (��ʹ���������ƥ��)
        /// </summary>
        Fast
    }

    /// <summary>
    /// ���������ֵĴ���ģʽ
    /// </summary>
    public enum SingleMode
    {
        /// <summary>
        /// �����ֿ��ɴ�
        /// </summary>
        Normal,
        /// <summary>
        /// �������ĵ������ӳ�һ����
        /// </summary>
        /// <remarks>
        /// C1 C2 C3 ��������Ϊ C1C2C3
        /// </remarks>
        Concat,
        /// <summary>
        /// �������ĵ���ʹ�ö�Ԫ���з�
        /// </summary>
        /// <remarks>
        /// C1 C2 C3 ��������Ϊ C1C2 C2C3
        /// </remarks>
        Dualize,
        /// <summary>
        /// �������е���
        /// </summary>
        ThrowAway
    }

    /// <summary>
    /// ��Ӣ�����Ĵ���ģʽ
    /// </summary>
    public enum EnglishMode
    {
        /// <summary>
        /// ��׼
        /// </summary>
        Normal,
        /// <summary>
        /// ����
        /// </summary>
        ThrowAway
    }

    /// <summary>
    /// Tokenizer
    /// </summary>
    abstract public class Tokenizer
    {
        /// <summary>
        /// �������ı�
        /// </summary>
        private string m_text;
        /// <summary>
        /// ��ǰ����λ��
        /// </summary>
        private int m_pos;

        /// <summary>
        /// ƥ��ģʽ
        /// </summary>
        private MatchMode m_match_mode = MatchMode.Normal;
        /// <summary>
        /// ���ִ���ģʽ
        /// </summary>
        private SingleMode m_single_mode = SingleMode.Dualize;
        /// <summary>
        /// Ӣ����䴦��ģʽ
        /// </summary>
        private EnglishMode m_english_mode = EnglishMode.ThrowAway;

        /// <summary>
        /// �����ʱ� (�ھ�̬���캯���г�ʼ��)
        /// </summary>
        static Hashtable m_stoptable;
        /// <summary>
        /// ����������
        /// </summary>
        static string[] m_stopwords = new string[] {
            "and", "are", "as", "at", "be", "but", "by",
            "for", "if", "in", "into", "is", "it", "no",
            "not", "of", "on", "or", "such", "that",
            "the", "their", "then", "there", "these",
            "they", "this", "to", "was", "will", "with",
            /* "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "��", */ "��", "��", "��", "ʱ",
            "��", "��", "��", "��", "��", "��", "��",
            "��", "��", "Ϊ", "��", "��", "��", "ȥ"
        };

        /// <summary>
        /// ��仺��
        /// </summary>
        protected char[] m_buf_char;
        /// <summary>
        /// ��仺���С
        /// </summary>
        protected int m_buf_size = 128; // org: 64
        /// <summary>
        /// ��ǰ��䳤��
        /// </summary>
        protected int m_buf_len = 0;

        /// <summary>
        /// ����ִʽ���б�
        /// </summary>
        protected List<string> m_list_backward;
        /// <summary>
        /// ����ִʽ���б�
        /// </summary>
        protected List<string> m_list_forward;
        /// <summary>
        /// ���ս���б�
        /// </summary>
        private List<string> m_list_final;

        /// <summary>
        /// ���� Tokens �Ķ���
        /// </summary>
        private List<Token> m_tokens;

        /// <summary>
        /// ƥ��ģʽ
        /// </summary>
        public MatchMode MatchMode
        {
            get { return m_match_mode; }
            set { m_match_mode = value; }
        }

        /// <summary>
        /// ���ִ���ģʽ
        /// </summary>
        public SingleMode SingleMode
        {
            get { return m_single_mode; }
            set { m_single_mode = value; }
        }

        /// <summary>
        /// Ӣ����䴦��ģʽ
        /// </summary>
        public EnglishMode EnglishMode
        {
            get { return m_english_mode; }
            set { m_english_mode = value; }
        }

        /// <summary>
        /// ��̬���캯��
        /// </summary>
        static Tokenizer()
        {
            // ��ʼ�������ʱ�
            m_stoptable = new Hashtable();
            for (int i = 0; i < m_stopwords.Length; i++)
            {
                m_stoptable[m_stopwords[i]] = 1;
            }
        }

        /// <summary>
        /// ���ݸ����Ĵʵ乹����Ӧ�� Tokenizer ʵ��
        /// </summary>
        /// <param name="path">�ʵ��ļ�·��</param>
        /// <returns></returns>
        public static Tokenizer factory(string path) {
            //byte[] sign_blex = new byte[] { 0x42, 0x4C, 0x45, 0x58 };
            //byte[] sign_dnet = new byte[] { 0x00, 0x01, 0x00, 0x00 };
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] buf = new byte[4];
            stream.Read(buf, 0, buf.Length);
            stream.Close();
            Tokenizer tokenizer;
            switch (buf[0]) {
                case (byte)0x42:
                    tokenizer = new LazyTokenizer();
                    tokenizer.LoadLexicon(path);
                    return tokenizer;
                case (byte)0x00:
                    tokenizer = new NormalTokenizer();
                    tokenizer.LoadLexicon(path);
                    return tokenizer;
                default:
                    throw new Exception("Invalid sign.");
            }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public Tokenizer()
        {
        }

        /// <summary>
        /// ���شʵ�
        /// </summary>
        /// <param name="path">�ʵ��ļ�·��</param>
        abstract public void LoadLexicon(string path);

        /// <summary>
        /// ���ı����зִ�
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <returns>Tokens</returns>
        public Token[] Tokenize(string text)
        {
            m_text = text;
            this.Process();
            Token[] tokens = m_tokens.ToArray();
            return tokens;
        }

        /// <summary>
        /// �����ı�
        /// </summary>
        private void Process()
        {
            // ��� Tokens ����
            m_tokens = new List<Token>();
            // �������ִʽ���б�
            m_list_backward = new List<string>();
            // �������ִʽ���б�
            m_list_forward = new List<string>();
            // ������ս���б�
            m_list_final = new List<string>();
            // �����仺��
            m_buf_char = new char[m_buf_size];
            // ���õ�ǰ����λ��Ϊ��ʼλ��
            m_pos = 0;
            // ��ȡ�ı�����
            int len = m_text.Length;
            // ���õ�ǰ����Ӣ������е�״̬Ϊ true
            bool in_english = true;
            // ���嵱ǰ�����ַ��������ı���
            char chr;
            UnicodeCategory cat;
            UnicodeCategory cat_last = UnicodeCategory.SpaceSeparator;
            // ���ַ������ı�
            while (m_pos < len)
            {
                // ��ȡ��ǰ�ַ�
                chr = m_text[m_pos];
                // ��ȡ��ǰ�ַ��� Unicode ����
                cat = Char.GetUnicodeCategory(chr);
                Debug("m_pos = {0}, chr = {1} ({2})", m_pos, chr, cat);
                // �����ַ����Ĳ�ͬ���д���
                switch (cat)
                {
                    /************ ���պ��ַ� ************/
                    case UnicodeCategory.OtherLetter: // ��ĸ�������Ǵ�д��ĸ��Сд��ĸ��������ĸ��д�����η���ĸ
                        if (in_english)
                        {
                            // �����ǰ����Ӣ������У��ȴ���ǰ���
                            this.ProcessEnglish();
                            in_english = false;
                        }
                        // ����ǰ�ַ���ӵ���仺����
                        m_buf_char[m_buf_len++] = chr;
                        // �����仺������
                        if (m_buf_len == m_buf_size)
                        {
                            // ����ǰ���
                            this.ProcessChinese();
                        }
                        break;
                    /*********** ��������ַ� ***********/
                    case UnicodeCategory.LowercaseLetter: // Сд��ĸ
                    case UnicodeCategory.UppercaseLetter: // ��д��ĸ
                    case UnicodeCategory.TitlecaseLetter: // ������ĸ��д��ĸ
                    case UnicodeCategory.DecimalDigitNumber: // ʮ��������
                    case UnicodeCategory.LetterNumber: // ����ĸ��ʾ������
                    /*
                    case UnicodeCategory.ConnectorPunctuation: // ���������ַ������ӷ����
                    */
                        if (!in_english)
                        {
                            // �����ǰ������������У��ȴ���ǰ���
                            this.ProcessChinese();
                            in_english = true;
                        }
                        // ����ǰ�ַ���ӵ���仺����
                        m_buf_char[m_buf_len++] = chr;
                        // �����仺������
                        if (m_buf_len == m_buf_size)
                        {
                            // ����ǰ���
                            this.ProcessEnglish();
                        }
                        break;
                    /*********** ��������ַ� ***********/
                    default:
                        if (in_english)
                        {
                            this.ProcessEnglish();
                        }
                        else
                        {
                            this.ProcessChinese();
                        }
                        break;
                }
                // ���䵱ǰ�ַ������
                cat_last = cat;
                // ǰ��һ���ַ�
                m_pos++;
            }
            // �����仺������ʣ�����ݣ����д���
            if (in_english)
            {
                this.ProcessEnglish();
            }
            else
            {
                this.ProcessChinese();
            }
#if DEBUG
            Console.Write("Result: ");
            Token[] arr = m_tokens.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i].Text);
                Console.Write('/');
            }
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// ������������
        /// </summary>
        private void ProcessEnglish()
        {
            // �������Ҫ��������
            if (m_buf_len == 0)
            {
                // �����д���
                return;
            }
            // �����Ӣ�����Ĵ���ģʽΪ����
            if (m_english_mode == EnglishMode.ThrowAway)
            {
                // ��ջ���
                m_buf_len = 0;
                // �����д���
                return;
            }
            // ���㵱ǰ��俪ʼ����λ��
            int pos_base = m_pos - m_buf_len;

            /*
            if (m_single_mode == SingleMode.Dualize)
            {
            */
            // ��ȡ��ǰ��Сд��ʽ���ַ���
            string word = (new string(m_buf_char, 0, m_buf_len)).ToLower();
            // �����ǰ�ʲ���������
            if (m_stoptable[word] == null)
            {
                // ����ǰ����ӵ����� Tokens ������
                Debug("Enqueue {0}, {1}, {2}", new string(m_buf_char, 0, m_buf_len), pos_base, pos_base + m_buf_len);
                m_tokens.Add(new Token(word, pos_base, pos_base + m_buf_len));
            }
            /*
            }
            */
            /*
            // �����ַ���д������
            char[] buf = new char[m_buf_size * 2];
            buf[0] = m_buf_char[0];
            int pos = 1;

            UnicodeCategory cat;
            UnicodeCategory cat_last = Char.GetUnicodeCategory(m_buf_char[0]);

            for (int i = 1; i < m_buf_len; i++)
            {
                cat = Char.GetUnicodeCategory(m_buf_char[i]);
                if (cat != cat_last)
                {
                    if (cat == UnicodeCategory.LowercaseLetter
                        && cat_last == UnicodeCategory.UppercaseLetter)
                    {
                        buf[pos] = buf[pos - 1];
                        buf[pos - 1] = ' ';
                        pos++;
                    }
                    else if (cat != UnicodeCategory.ConnectorPunctuation
                        && cat_last != UnicodeCategory.ConnectorPunctuation)
                    {
                        buf[pos++] = ' ';
                    }
                }
                buf[pos++] = m_buf_char[i];
                cat_last = cat;
            }

            Debug("OUT: {0}", new string(buf, 0, pos));
            Console.WriteLine("OUT: {0}", new string(buf, 0, pos));

            buf[pos++] = ' ';

            // ��ȡ��ǰ��Сд��ʽ���ַ���
            string temp = (new string(buf, 0, pos)).ToLower();
            string[] words = temp.Split(new char[] { '\uF001' }, StringSplitOptions.RemoveEmptyEntries);

            int ofs = 0;

            for (int i = 0; i < words.Length; i++)
            {
                // �����ǰ�ʲ���������
                if (m_stoptable[words[i]] == null)
                {
                    // ����ǰ����ӵ����� Tokens ������
                    Debug("Enqueue {0}, {1}, {2}", words[i], pos_base + ofs, pos_base + ofs + words[i].Length);
                    m_tokens.Add(new Token(words[i], pos_base + ofs, pos_base + ofs + words[i].Length));
                }
                ofs += words[i].Length;
            }
            */
            // �����仺��
            m_buf_len = 0;
        }

        /// <summary>
        /// �����������
        /// </summary>
        private void ProcessChinese()
        {
            // �������Ҫ��������
            if (m_buf_len == 0)
            {
                // �����д���
                return;
            }
            // ���㵱ǰ��俪ʼ����λ��
            int pos_base = m_pos - m_buf_len;
            Debug("CN: {0}", new string(m_buf_char, 0, m_buf_len));
            Debug("==============================");
            // �������ִʽ���б�
            m_list_backward.Clear();
            // �Ե�ǰ����������ִ�
            this.ProcessChinese_Backward();
            if (m_match_mode == MatchMode.Fast)
            {
                // ��ʹ�ÿ���ƥ��ģʽ��ֱ�Ӳ�������ִʽ��
                m_list_final = m_list_backward;
            }
            else
            {
                // ��ʹ�ñ�׼ƥ��ģʽ
                // �������ִʽ���б�
                m_list_forward.Clear();
                // �Ե�ǰ����������ִ�
                this.ProcessChinese_Forward();
                // ������ս���б�
                m_list_final.Clear();
                // �������岿��
                this.ProcessChinese_Ambiguities();
            }
            // ���� Tokens ����
            switch (m_single_mode)
            {
                case SingleMode.Dualize:
                    // ����������ʹ�ö�Ԫ���з�
                    this.ProcessChinese_Tokens_Dualize(pos_base);
                    break;
                case SingleMode.Concat:
                    // �������ĵ������ӳ�һ����
                    this.ProcessChinese_Tokens_Concat(pos_base);
                    break;
                case SingleMode.ThrowAway:
                    // �������е���
                    this.ProcessChinese_Tokens_Throwaway(pos_base);
                    break;
                default:
                    // �����ֿ��ɴ�
                    this.ProcessChinese_Tokens_Normal(pos_base);
                    break;
            }
            // �����仺��
            m_buf_len = 0;
        }

        /// <summary>
        /// ����ִ�
        /// </summary>
        abstract protected void ProcessChinese_Backward();

        /// <summary>
        /// ����ִ�
        /// </summary>
        abstract protected void ProcessChinese_Forward();

        /// <summary>
        /// �������첿��
        /// </summary>
        private void ProcessChinese_Ambiguities()
        {
#if DEBUG
            string[] arr_f = m_list_forward.ToArray();
            string[] arr_b = m_list_backward.ToArray();
            Console.WriteLine("F: {0}", String.Join("/", arr_f));
            Console.WriteLine("B: {0}", String.Join("/", arr_b));
            Console.WriteLine();
#endif
            // ��ǰ����������ִʽ���ı��е�λ��
            int ofs_f = 0, ofs_b = 0;
            // ��������ִʽ���е�ǰλ�õĴ�
            string str_f, str_b;
            float exp_f = 0f, exp_b = 0f;
            // ��ǰ����������ִʽ���б��е�λ��
            int idx_f = 0, idx_b = 0;
            // ��������ʱ��������������ִʽ���б��е�λ��
            int idx_f_2, idx_b_2;
            string str_f_2, str_b_2;
            // ���������ִʽ���ı��е�λ��û�е���β������ʣ�ಿ��
            while (ofs_f < m_buf_len)
            {
                // ��ȡ����ִʽ���е�ǰλ�õĴʣ���ǰ�� 1 ��λ��
                str_f = m_list_forward[idx_f++];
                // ��ȡ����ִʽ���е�ǰλ�õĴʣ���ǰ�� 1 ��λ��
                str_b = m_list_backward[idx_b++];
                if (str_f.Length == str_b.Length)
                {
                    // �����ǰλ����������ִʽ�������죬��ֱ�ӽ�����ӵ����ս���б���
                    //Debug("Enqueue {0}, {1}, {2}", str_b, t_start, t_start + str_b.Length);
                    m_list_final.Add(str_b);
                    ofs_f += str_f.Length;
                    ofs_b += str_b.Length;
                }
                else
                {
                    // ������������
                    // ��ʼ��**
                    exp_f = 1f;
                    exp_b = 1f;
                    // ��ʼ��**������ǰ���� idx_f++ �������ʴ˴� -1
                    idx_f_2 = idx_f - 1;
                    // ��ʼ��**������ǰ���� idx_b++ �������ʴ˴� -1
                    idx_b_2 = idx_b - 1;
                    do
                    {
                        if (ofs_f > ofs_b)
                        {
                            if (idx_b_2 == m_list_backward.Count)
                            {
                                // �������ִʽ���Ѵ�����ϣ���ֹѭ��
                                break;
                            }
                            str_b_2 = m_list_backward[idx_b_2++];
                            ofs_b += str_b_2.Length;
                            this.ProcessChinese_Frequency(str_b_2, ref exp_b);
                        }
                        else
                        {
                            if (idx_f_2 == m_list_forward.Count)
                            {
                                // �������ִʽ���Ѵ�����ϣ���ֹѭ��
                                break;
                            }
                            str_f_2 = m_list_forward[idx_f_2++];
                            ofs_f += str_f_2.Length;
                            this.ProcessChinese_Frequency(str_f_2, ref exp_f);
                        }
                    } while (ofs_f != ofs_b);
                    Debug("ofs_f = {0}, idx_f_2 = {1}, exp_f = {2}", ofs_f, idx_f_2, exp_f);
                    Debug("ofs_b = {0}, idx_b_2 = {1}, exp_b = {2}", ofs_b, idx_b_2, exp_b);
                    if (exp_f > exp_b)
                    {
                        for (int i = idx_f - 1; i < idx_f_2; i++)
                        {
                            //Debug("[F]Enqueue {0}, {1}, {2}", tmp_f[i], t_start, t_start + tmp_f[i].Length);
                            m_list_final.Add(m_list_forward[i]);
                        }
                    }
                    else
                    {
                        for (int i = idx_b - 1; i < idx_b_2; i++)
                        {
                            //Debug("pos_base = {0}, ofs_b = {1}, idx_b = {2}, idx_b_2 = {3}, i = {4}", pos_base, ofs_b, idx_b, idx_b_2, i);
                            //Debug("[B]Enqueue {0}, {1}, {2}", tmp_b[i], t_start, t_start + tmp_b[i].Length);
                            m_list_final.Add(m_list_backward[i]);
                        }
                    }
                    idx_f = idx_f_2;
                    idx_b = idx_b_2;
                }
            }
        }

        /// <summary>
        /// �������� Tokens ����
        /// (�����ֿ��ɴ�)
        /// </summary>
        /// <param name="start">��ǰ�����ʼλ��</param>
        private void ProcessChinese_Tokens_Normal(int start)
        {
            for (int i = 0; i < m_list_final.Count; i++)
            {
                if (m_stoptable[m_list_final[i]] == null)
                {
                    m_tokens.Add(new Token(m_list_final[i], start, start + m_list_final[i].Length));
                }
                start += m_list_final[i].Length;
            }
        }

        /// <summary>
        /// �������� Tokens ����
        /// (�������ĵ������ӳ�һ����)
        /// </summary>
        /// <param name="start">��ǰ�����ʼλ��</param>
        private void ProcessChinese_Tokens_Concat(int start)
        {
            int single_start = -1;
            StringBuilder single_builder = new StringBuilder();
            bool last_is_single = false;
            for (int i = 0; i < m_list_final.Count; i++)
            {
                if (m_list_final[i].Length == 1)
                {
                    if (last_is_single)
                    {
                        if (m_stoptable[m_list_final[i]] == null)
                        {
                            single_builder.Append(m_list_final[i]);
                        }
                        else
                        {
                            string str = single_builder.ToString();
                            m_tokens.Add(new Token(str, single_start, single_start + str.Length));
                            single_builder.Length = 0;
                            single_start = -1;
                            last_is_single = false;
                        }
                    }
                    else
                    {
                        if (m_stoptable[m_list_final[i]] == null)
                        {
                            single_builder.Append(m_list_final[i]);
                            single_start = start;
                            last_is_single = true;
                        }
                    }
                }
                else
                {
                    if (last_is_single)
                    {
                        string str = single_builder.ToString();
                        m_tokens.Add(new Token(str, single_start, single_start + str.Length));
                        single_builder.Length = 0;
                        single_start = -1;
                        last_is_single = false;
                    }
                    if (m_stoptable[m_list_final[i]] == null)
                    {
                        m_tokens.Add(new Token(m_list_final[i], start, start + m_list_final[i].Length));
                    }
                }
                start += m_list_final[i].Length;
            }
            // ��β������refer to php version
        }

        /// <summary>
        /// �������� Tokens ����
        /// (����������ʹ�ö�Ԫ���з�)
        /// </summary>
        /// <param name="start">��ǰ�����ʼλ��</param>
        private void ProcessChinese_Tokens_Dualize(int start)
        {
            bool last_is_single = false;
            for (int i = 0; i < m_list_final.Count; i++)
            {
                if (m_list_final[i].Length == 1)
                {
                    if (last_is_single)
                    {
                        m_tokens.Add(new Token(m_list_final[i - 1].ToString() + m_list_final[i].ToString(), start - 1, start + 1));
                    }
                    else
                    {
                        if (i == m_list_final.Count - 1 || m_list_final[i + 1].Length != 1)
                        {
                            if (m_stoptable[m_list_final[i]] == null)
                            {
                                m_tokens.Add(new Token(m_list_final[i].ToString(), start, start + 1));
                            }
                        }
                        last_is_single = (m_stoptable[m_list_final[i]] == null);
                    }
                }
                else
                {
                    if (m_stoptable[m_list_final[i]] == null)
                    {
                        m_tokens.Add(new Token(m_list_final[i], start, start + m_list_final[i].Length));
                    }
                    last_is_single = false;
                }
                start += m_list_final[i].Length;
            }
#if DEBUG
            string[] arr_l = m_list_final.ToArray();
            Console.WriteLine("L: {0}", String.Join("/", arr_l));
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// �������� Tokens ����
        /// (�������е���)
        /// </summary>
        /// <param name="start">��ǰ�����ʼλ��</param>
        private void ProcessChinese_Tokens_Throwaway(int start)
        {
            for (int i = 0; i < m_list_final.Count; i++)
            {
                if (m_list_final[i].Length > 1
                    && m_stoptable[m_list_final[i]] == null)
                {
                    m_tokens.Add(new Token(m_list_final[i], start, start + m_list_final[i].Length));
                }
                start += m_list_final[i].Length;
            }
        }

        /// <summary>
        /// ���㵥�ֳɴ�Ƶ��
        /// </summary>
        /// <param name="str">����</param>
        /// <param name="exp">����Ƶ��</param>
        abstract protected void ProcessChinese_Frequency(string str, ref float exp);

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="value">��Ϣ</param>
        [System.Diagnostics.Conditional("DEBUG")]
        protected void Debug(string value)
        {
            Console.WriteLine(value);
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="format">��ʽ�ַ���</param>
        /// <param name="arg">����</param>
        [System.Diagnostics.Conditional("DEBUG")]
        protected void Debug(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
