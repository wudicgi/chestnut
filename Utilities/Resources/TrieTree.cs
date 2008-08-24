// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WudiStudio.Chestnut.Utilities.Resources
{
    /// <summary>
    /// TRIE 树结构
    /// </summary>
    public class TrieTree
    {
        /// <summary>
        /// 树根
        /// </summary>
        private TrieNode m_root;
        /// <summary>
        /// 终止节点
        /// </summary>
        private TrieNode m_terminal;

        /// <summary>
        /// 字符列表
        /// </summary>
        private Dictionary<int, string> m_dict_chars;
        /// <summary>
        /// 索引列表
        /// </summary>
        private Dictionary<int, int[]> m_dict_idxes;

        private int m_index;

        /// <summary>
        /// 树根
        /// </summary>
        public TrieNode Root
        {
            get { return m_root; }
        }

        /// <summary>
        /// 终止节点
        /// </summary>
        public TrieNode Terminal
        {
            get { return m_terminal; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TrieTree()
        {
            // 初始化树根，T 为无效字符
            m_root = new TrieNode('T');
            // 初始化终止节点，T 为无效字符
            m_terminal = new TrieNode('T');
            // 设置终止节点为成词点
            m_terminal.IsK = true;
        }

        /// <summary>
        /// 添加字符串到树中
        /// </summary>
        /// <param name="str">字符串</param>
        public void AddString(string str)
        {
            // 如果字符串长度为 0
            if (str.Length == 0)
            {
                // 不进行处理
                return;
            }
            // 设置当前节点为树根
            TrieNode node = m_root;
            // 逐个处理字符串中的字符
            for (int i = 0; i < str.Length; i++)
            {
                // 如果当前节点不包含字符 str[i]
                if (!node.Children.ContainsKey(str[i]))
                {
                    // 新建字符 str[i] 的节点作为当前节点的子节点
                    node.Children.Add(str[i], new TrieNode(str[i]));
                }
                // 设置当前节点为新建节点
                node = node.Children[str[i]];
            }
            // 设置当前节点为成词点
            node.IsK = true;
        }

        public ArrayList GetDoubleArrays()
        {
            this.BuildDicts();
            return this.BuildArrays();
        }

        private void BuildDicts()
        {
            // 初始化字符列表
            m_dict_chars = new Dictionary<int, string>();
            // 初始化索引列表
            m_dict_idxes = new Dictionary<int, int[]>();

            // ?
            for (int i = 0; i < 10; i++)
            {
                m_dict_chars[i] = "";
                m_dict_idxes[i] = new int[0];
            }

            m_index = 10;

            m_index = this.BuildDicts_ProcessNode(m_root, 10);
        }

        private int BuildDicts_ProcessNode(TrieNode node, int index)
        {
            if (node.Children.Count == 0)
            {
                return index;
            }
            // 确定成词标志，K 表示成词，U 表示不成词
            string mark = node.IsK ? "K" : "U";
            // 获取所处理节点的所有子节点的字符
            Dictionary<char, TrieNode>.KeyCollection kc = node.Children.Keys;
            // 将上一步所获取的字符复制到字符数组中
            char[] arr = new char[kc.Count];
            kc.CopyTo(arr, 0);
            // 将所有字符连接成一个字符串
            string chars = new string(arr);
            // int index = m_index++;
            // 将成此标志和字符串添加到字符列表中
            m_dict_chars[index] = mark + chars;
            // 建立临时整数数组
            List<int> temp = new List<int>();
            temp.Add(-1);
            // 开始处理所处理节点的子节点
            int node_index = index + 1;
            int node_index_new;
            foreach (char chr in kc)
            {
                // 处理字符为 chr 的子节点，并获得新的可用索引值
                node_index_new = this.BuildDicts_ProcessNode(node.Children[chr], node_index);
                if (node.Children[chr].Children.Count == 0)
                {
                    // 如果该子节点无子节点，设置它所对应的下一个查找节点为终止节点
                    temp.Add(1);
                }
                else
                {
                    // 设置下一个查找节点
                    temp.Add(node_index);
                }
                // 为下一个节点设置可用索引值为新的可用索引值
                node_index = node_index_new;
            }
            m_dict_idxes[index] = temp.ToArray();
            // 返回新的可用索引值
            return node_index;
        }

        private ArrayList BuildArrays()
        {
            List<string> al_chars = new List<string>(m_index);
            List<int[]> al_idxes = new List<int[]>(m_index);

            int count = m_index;

            // 添加索引值为 10 的项 (根)
            al_chars.Add(m_dict_chars[10]);
            al_idxes.Add(m_dict_idxes[10]);

            // 添加终止节点
            al_chars.Add("K");
            al_idxes.Add(new int[1] { -1 });

            // 添加保留项
            for (int i = 2; i < 11; i++)
            {
                al_chars.Add("R");
                al_idxes.Add(new int[1] { -1 });
            }

            for (int i = 11; i < count; i++)
            {
                al_chars.Add(m_dict_chars[i]);
                al_idxes.Add(m_dict_idxes[i]);
            }

            ArrayList al = new ArrayList(2);
            al.Add(al_chars.ToArray());
            al.Add(al_idxes.ToArray());

            return al;
        }
    }

    /// <summary>
    /// TRIE 节点结构
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// 是否为成词点
        /// </summary>
        private bool m_is_k = false;
        /// <summary>
        /// 节点的字符
        /// </summary>
        private char m_char;
        /// <summary>
        /// 子节点
        /// </summary>
        private Dictionary<char, TrieNode> m_children;

        /// <summary>
        /// 是否为成词点
        /// </summary>
        public bool IsK
        {
            get { return m_is_k; }
            set { m_is_k = value; }
        }

        /// <summary>
        /// 节点的字符
        /// </summary>
        public char Char
        {
            get { return m_char; }
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public Dictionary<char, TrieNode> Children
        {
            get { return m_children; }
            set { m_children = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chr">节点的字符</param>
        public TrieNode(char chr)
        {
            m_char = chr;
            m_children = new Dictionary<char, TrieNode>();
            Global.CountNodes++;
        }
    }
}
