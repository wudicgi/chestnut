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
    /// 匹配模式
    /// </summary>
    public enum MatchMode
    {
        /// <summary>
        /// 标准匹配 (使用双向匹配，依据单字成词频率处理歧义)
        /// </summary>
        Normal,
        /// <summary>
        /// 快速匹配 (仅使用逆向最大匹配)
        /// </summary>
        Fast
    }

    /// <summary>
    /// 对连续单字的处理模式
    /// </summary>
    public enum SingleMode
    {
        /// <summary>
        /// 将单字看成词
        /// </summary>
        Normal,
        /// <summary>
        /// 将连续的单字连接成一个词
        /// </summary>
        /// <remarks>
        /// C1 C2 C3 将被处理为 C1C2C3
        /// </remarks>
        Concat,
        /// <summary>
        /// 对连续的单字使用二元法切分
        /// </summary>
        /// <remarks>
        /// C1 C2 C3 将被处理为 C1C2 C2C3
        /// </remarks>
        Dualize,
        /// <summary>
        /// 丢弃所有单字
        /// </summary>
        ThrowAway
    }

    /// <summary>
    /// 对英文语句的处理模式
    /// </summary>
    public enum EnglishMode
    {
        /// <summary>
        /// 标准
        /// </summary>
        Normal,
        /// <summary>
        /// 丢弃
        /// </summary>
        ThrowAway
    }

    /// <summary>
    /// Tokenizer
    /// </summary>
    abstract public class Tokenizer
    {
        /// <summary>
        /// 待处理文本
        /// </summary>
        private string m_text;
        /// <summary>
        /// 当前处理位置
        /// </summary>
        private int m_pos;

        /// <summary>
        /// 匹配模式
        /// </summary>
        private MatchMode m_match_mode = MatchMode.Normal;
        /// <summary>
        /// 单字处理模式
        /// </summary>
        private SingleMode m_single_mode = SingleMode.Dualize;
        /// <summary>
        /// 英文语句处理模式
        /// </summary>
        private EnglishMode m_english_mode = EnglishMode.ThrowAway;

        /// <summary>
        /// 噪音词表 (在静态构造函数中初始化)
        /// </summary>
        static Hashtable m_stoptable;
        /// <summary>
        /// 噪音词数组
        /// </summary>
        static string[] m_stopwords = new string[] {
            "and", "are", "as", "at", "be", "but", "by",
            "for", "if", "in", "into", "is", "it", "no",
            "not", "of", "on", "or", "such", "that",
            "the", "their", "then", "there", "these",
            "they", "this", "to", "was", "will", "with",
            /* "。", "，", "、", "；", "：", "？", "！",
            "…", "—", "ˉ", "ˇ", "‘", "’", "“",
            "”", "々", "～", "‖", "∶", "＂", "＇",
            "｀", "｜", "〃", "〔", "〕", "〈", "〉",
            "《", "》", "「", "」", "『", "』", "．",
            "〖", "〗", "【", "】", "（", "）", "［",
            "］", "｛", "｝", */ "的", "和", "与", "时",
            "在", "是", "被", "所", "那", "这", "有",
            "将", "会", "为", "对", "了", "过", "去"
        };

        /// <summary>
        /// 语句缓冲
        /// </summary>
        protected char[] m_buf_char;
        /// <summary>
        /// 语句缓冲大小
        /// </summary>
        protected int m_buf_size = 128; // org: 64
        /// <summary>
        /// 当前语句长度
        /// </summary>
        protected int m_buf_len = 0;

        /// <summary>
        /// 逆向分词结果列表
        /// </summary>
        protected List<string> m_list_backward;
        /// <summary>
        /// 正向分词结果列表
        /// </summary>
        protected List<string> m_list_forward;
        /// <summary>
        /// 最终结果列表
        /// </summary>
        private List<string> m_list_final;

        /// <summary>
        /// 最终 Tokens 的队列
        /// </summary>
        private List<Token> m_tokens;

        /// <summary>
        /// 匹配模式
        /// </summary>
        public MatchMode MatchMode
        {
            get { return m_match_mode; }
            set { m_match_mode = value; }
        }

        /// <summary>
        /// 单字处理模式
        /// </summary>
        public SingleMode SingleMode
        {
            get { return m_single_mode; }
            set { m_single_mode = value; }
        }

        /// <summary>
        /// 英文语句处理模式
        /// </summary>
        public EnglishMode EnglishMode
        {
            get { return m_english_mode; }
            set { m_english_mode = value; }
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Tokenizer()
        {
            // 初始化噪音词表
            m_stoptable = new Hashtable();
            for (int i = 0; i < m_stopwords.Length; i++)
            {
                m_stoptable[m_stopwords[i]] = 1;
            }
        }

        /// <summary>
        /// 根据给定的词典构造相应的 Tokenizer 实例
        /// </summary>
        /// <param name="path">词典文件路径</param>
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
        /// 构造函数
        /// </summary>
        public Tokenizer()
        {
        }

        /// <summary>
        /// 加载词典
        /// </summary>
        /// <param name="path">词典文件路径</param>
        abstract public void LoadLexicon(string path);

        /// <summary>
        /// 对文本进行分词
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>Tokens</returns>
        public Token[] Tokenize(string text)
        {
            m_text = text;
            this.Process();
            Token[] tokens = m_tokens.ToArray();
            return tokens;
        }

        /// <summary>
        /// 处理文本
        /// </summary>
        private void Process()
        {
            // 清空 Tokens 序列
            m_tokens = new List<Token>();
            // 清空逆向分词结果列表
            m_list_backward = new List<string>();
            // 清空正向分词结果列表
            m_list_forward = new List<string>();
            // 清空最终结果列表
            m_list_final = new List<string>();
            // 清空语句缓冲
            m_buf_char = new char[m_buf_size];
            // 设置当前处理位置为开始位置
            m_pos = 0;
            // 获取文本长度
            int len = m_text.Length;
            // 设置当前处于英文语句中的状态为 true
            bool in_english = true;
            // 定义当前处理字符及其分类的变量
            char chr;
            UnicodeCategory cat;
            UnicodeCategory cat_last = UnicodeCategory.SpaceSeparator;
            // 逐字符处理文本
            while (m_pos < len)
            {
                // 获取当前字符
                chr = m_text[m_pos];
                // 获取当前字符的 Unicode 分类
                cat = Char.GetUnicodeCategory(chr);
                Debug("m_pos = {0}, chr = {1} ({2})", m_pos, chr, cat);
                // 根据字符类别的不同进行处理
                switch (cat)
                {
                    /************ 中日韩字符 ************/
                    case UnicodeCategory.OtherLetter: // 字母，但不是大写字母、小写字母、词首字母大写或修饰符字母
                        if (in_english)
                        {
                            // 如果当前处于英文语句中，先处理当前语句
                            this.ProcessEnglish();
                            in_english = false;
                        }
                        // 将当前字符添加到语句缓冲中
                        m_buf_char[m_buf_len++] = chr;
                        // 如果语句缓冲已满
                        if (m_buf_len == m_buf_size)
                        {
                            // 处理当前语句
                            this.ProcessChinese();
                        }
                        break;
                    /*********** 有意义的字符 ***********/
                    case UnicodeCategory.LowercaseLetter: // 小写字母
                    case UnicodeCategory.UppercaseLetter: // 大写字母
                    case UnicodeCategory.TitlecaseLetter: // 词首字母大写字母
                    case UnicodeCategory.DecimalDigitNumber: // 十进制数字
                    case UnicodeCategory.LetterNumber: // 由字母表示的数字
                    /*
                    case UnicodeCategory.ConnectorPunctuation: // 连接两个字符的连接符标点
                    */
                        if (!in_english)
                        {
                            // 如果当前处于中文语句中，先处理当前语句
                            this.ProcessChinese();
                            in_english = true;
                        }
                        // 将当前字符添加到语句缓冲中
                        m_buf_char[m_buf_len++] = chr;
                        // 如果语句缓冲已满
                        if (m_buf_len == m_buf_size)
                        {
                            // 处理当前语句
                            this.ProcessEnglish();
                        }
                        break;
                    /*********** 无意义的字符 ***********/
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
                // 记忆当前字符的类别
                cat_last = cat;
                // 前进一个字符
                m_pos++;
            }
            // 如果语句缓冲中有剩余内容，进行处理
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
        /// 处理非中文语句
        /// </summary>
        private void ProcessEnglish()
        {
            // 如果无需要处理的语句
            if (m_buf_len == 0)
            {
                // 不进行处理
                return;
            }
            // 如果对英文语句的处理模式为丢弃
            if (m_english_mode == EnglishMode.ThrowAway)
            {
                // 清空缓存
                m_buf_len = 0;
                // 不进行处理
                return;
            }
            // 计算当前语句开始处的位置
            int pos_base = m_pos - m_buf_len;

            /*
            if (m_single_mode == SingleMode.Dualize)
            {
            */
            // 获取当前词小写形式的字符串
            string word = (new string(m_buf_char, 0, m_buf_len)).ToLower();
            // 如果当前词不是噪音词
            if (m_stoptable[word] == null)
            {
                // 将当前词添加到最终 Tokens 队列中
                Debug("Enqueue {0}, {1}, {2}", new string(m_buf_char, 0, m_buf_len), pos_base, pos_base + m_buf_len);
                m_tokens.Add(new Token(word, pos_base, pos_base + m_buf_len));
            }
            /*
            }
            */
            /*
            // 处理字符连写的问题
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

            // 获取当前词小写形式的字符串
            string temp = (new string(buf, 0, pos)).ToLower();
            string[] words = temp.Split(new char[] { '\uF001' }, StringSplitOptions.RemoveEmptyEntries);

            int ofs = 0;

            for (int i = 0; i < words.Length; i++)
            {
                // 如果当前词不是噪音词
                if (m_stoptable[words[i]] == null)
                {
                    // 将当前词添加到最终 Tokens 队列中
                    Debug("Enqueue {0}, {1}, {2}", words[i], pos_base + ofs, pos_base + ofs + words[i].Length);
                    m_tokens.Add(new Token(words[i], pos_base + ofs, pos_base + ofs + words[i].Length));
                }
                ofs += words[i].Length;
            }
            */
            // 清空语句缓冲
            m_buf_len = 0;
        }

        /// <summary>
        /// 处理中文语句
        /// </summary>
        private void ProcessChinese()
        {
            // 如果无需要处理的语句
            if (m_buf_len == 0)
            {
                // 不进行处理
                return;
            }
            // 计算当前语句开始处的位置
            int pos_base = m_pos - m_buf_len;
            Debug("CN: {0}", new string(m_buf_char, 0, m_buf_len));
            Debug("==============================");
            // 清空逆向分词结果列表
            m_list_backward.Clear();
            // 对当前语句进行逆向分词
            this.ProcessChinese_Backward();
            if (m_match_mode == MatchMode.Fast)
            {
                // 若使用快速匹配模式，直接采用逆向分词结果
                m_list_final = m_list_backward;
            }
            else
            {
                // 若使用标准匹配模式
                // 清空正向分词结果列表
                m_list_forward.Clear();
                // 对当前语句进行正向分词
                this.ProcessChinese_Forward();
                // 清空最终结果列表
                m_list_final.Clear();
                // 处理歧义部分
                this.ProcessChinese_Ambiguities();
            }
            // 生成 Tokens 队列
            switch (m_single_mode)
            {
                case SingleMode.Dualize:
                    // 对连续单字使用二元法切分
                    this.ProcessChinese_Tokens_Dualize(pos_base);
                    break;
                case SingleMode.Concat:
                    // 将连续的单字连接成一个词
                    this.ProcessChinese_Tokens_Concat(pos_base);
                    break;
                case SingleMode.ThrowAway:
                    // 丢弃所有单字
                    this.ProcessChinese_Tokens_Throwaway(pos_base);
                    break;
                default:
                    // 将单字看成词
                    this.ProcessChinese_Tokens_Normal(pos_base);
                    break;
            }
            // 清空语句缓冲
            m_buf_len = 0;
        }

        /// <summary>
        /// 逆向分词
        /// </summary>
        abstract protected void ProcessChinese_Backward();

        /// <summary>
        /// 正向分词
        /// </summary>
        abstract protected void ProcessChinese_Forward();

        /// <summary>
        /// 处理歧异部分
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
            // 当前在正、逆向分词结果文本中的位置
            int ofs_f = 0, ofs_b = 0;
            // 正、逆向分词结果中当前位置的词
            string str_f, str_b;
            float exp_f = 0f, exp_b = 0f;
            // 当前在正、逆向分词结果列表中的位置
            int idx_f = 0, idx_b = 0;
            // 处理歧异时所用在正、逆向分词结果列表中的位置
            int idx_f_2, idx_b_2;
            string str_f_2, str_b_2;
            // 如果在正向分词结果文本中的位置没有到结尾，则处理剩余部分
            while (ofs_f < m_buf_len)
            {
                // 获取正向分词结果中当前位置的词，并前进 1 个位置
                str_f = m_list_forward[idx_f++];
                // 获取逆向分词结果中当前位置的词，并前进 1 个位置
                str_b = m_list_backward[idx_b++];
                if (str_f.Length == str_b.Length)
                {
                    // 如果当前位置正、逆向分词结果无歧异，则直接将词添加到最终结果列表中
                    //Debug("Enqueue {0}, {1}, {2}", str_b, t_start, t_start + str_b.Length);
                    m_list_final.Add(str_b);
                    ofs_f += str_f.Length;
                    ofs_b += str_b.Length;
                }
                else
                {
                    // 如果结果有歧异
                    // 初始化**
                    exp_f = 1f;
                    exp_b = 1f;
                    // 初始化**，由于前面有 idx_f++ 操作，故此处 -1
                    idx_f_2 = idx_f - 1;
                    // 初始化**，由于前面有 idx_b++ 操作，故此处 -1
                    idx_b_2 = idx_b - 1;
                    do
                    {
                        if (ofs_f > ofs_b)
                        {
                            if (idx_b_2 == m_list_backward.Count)
                            {
                                // 如果逆向分词结果已处理完毕，终止循环
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
                                // 如果正向分词结果已处理完毕，终止循环
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
        /// 生成最终 Tokens 队列
        /// (将单字看成词)
        /// </summary>
        /// <param name="start">当前语句起始位置</param>
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
        /// 生成最终 Tokens 队列
        /// (将连续的单字连接成一个词)
        /// </summary>
        /// <param name="start">当前语句起始位置</param>
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
            // 收尾工作？refer to php version
        }

        /// <summary>
        /// 生成最终 Tokens 队列
        /// (对连续单字使用二元法切分)
        /// </summary>
        /// <param name="start">当前语句起始位置</param>
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
        /// 生成最终 Tokens 队列
        /// (丢弃所有单字)
        /// </summary>
        /// <param name="start">当前语句起始位置</param>
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
        /// 计算单字成词频率
        /// </summary>
        /// <param name="str">单字</param>
        /// <param name="exp">最终频率</param>
        abstract protected void ProcessChinese_Frequency(string str, ref float exp);

        /// <summary>
        /// 输出出错信息
        /// </summary>
        /// <param name="value">信息</param>
        [System.Diagnostics.Conditional("DEBUG")]
        protected void Debug(string value)
        {
            Console.WriteLine(value);
        }

        /// <summary>
        /// 输出出错信息
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="arg">参数</param>
        [System.Diagnostics.Conditional("DEBUG")]
        protected void Debug(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
