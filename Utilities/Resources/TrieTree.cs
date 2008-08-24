// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WudiStudio.Chestnut.Utilities.Resources
{
    /// <summary>
    /// TRIE ���ṹ
    /// </summary>
    public class TrieTree
    {
        /// <summary>
        /// ����
        /// </summary>
        private TrieNode m_root;
        /// <summary>
        /// ��ֹ�ڵ�
        /// </summary>
        private TrieNode m_terminal;

        /// <summary>
        /// �ַ��б�
        /// </summary>
        private Dictionary<int, string> m_dict_chars;
        /// <summary>
        /// �����б�
        /// </summary>
        private Dictionary<int, int[]> m_dict_idxes;

        private int m_index;

        /// <summary>
        /// ����
        /// </summary>
        public TrieNode Root
        {
            get { return m_root; }
        }

        /// <summary>
        /// ��ֹ�ڵ�
        /// </summary>
        public TrieNode Terminal
        {
            get { return m_terminal; }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public TrieTree()
        {
            // ��ʼ��������T Ϊ��Ч�ַ�
            m_root = new TrieNode('T');
            // ��ʼ����ֹ�ڵ㣬T Ϊ��Ч�ַ�
            m_terminal = new TrieNode('T');
            // ������ֹ�ڵ�Ϊ�ɴʵ�
            m_terminal.IsK = true;
        }

        /// <summary>
        /// ����ַ���������
        /// </summary>
        /// <param name="str">�ַ���</param>
        public void AddString(string str)
        {
            // ����ַ�������Ϊ 0
            if (str.Length == 0)
            {
                // �����д���
                return;
            }
            // ���õ�ǰ�ڵ�Ϊ����
            TrieNode node = m_root;
            // ��������ַ����е��ַ�
            for (int i = 0; i < str.Length; i++)
            {
                // �����ǰ�ڵ㲻�����ַ� str[i]
                if (!node.Children.ContainsKey(str[i]))
                {
                    // �½��ַ� str[i] �Ľڵ���Ϊ��ǰ�ڵ���ӽڵ�
                    node.Children.Add(str[i], new TrieNode(str[i]));
                }
                // ���õ�ǰ�ڵ�Ϊ�½��ڵ�
                node = node.Children[str[i]];
            }
            // ���õ�ǰ�ڵ�Ϊ�ɴʵ�
            node.IsK = true;
        }

        public ArrayList GetDoubleArrays()
        {
            this.BuildDicts();
            return this.BuildArrays();
        }

        private void BuildDicts()
        {
            // ��ʼ���ַ��б�
            m_dict_chars = new Dictionary<int, string>();
            // ��ʼ�������б�
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
            // ȷ���ɴʱ�־��K ��ʾ�ɴʣ�U ��ʾ���ɴ�
            string mark = node.IsK ? "K" : "U";
            // ��ȡ������ڵ�������ӽڵ���ַ�
            Dictionary<char, TrieNode>.KeyCollection kc = node.Children.Keys;
            // ����һ������ȡ���ַ����Ƶ��ַ�������
            char[] arr = new char[kc.Count];
            kc.CopyTo(arr, 0);
            // �������ַ����ӳ�һ���ַ���
            string chars = new string(arr);
            // int index = m_index++;
            // ���ɴ˱�־���ַ�����ӵ��ַ��б���
            m_dict_chars[index] = mark + chars;
            // ������ʱ��������
            List<int> temp = new List<int>();
            temp.Add(-1);
            // ��ʼ����������ڵ���ӽڵ�
            int node_index = index + 1;
            int node_index_new;
            foreach (char chr in kc)
            {
                // �����ַ�Ϊ chr ���ӽڵ㣬������µĿ�������ֵ
                node_index_new = this.BuildDicts_ProcessNode(node.Children[chr], node_index);
                if (node.Children[chr].Children.Count == 0)
                {
                    // ������ӽڵ����ӽڵ㣬����������Ӧ����һ�����ҽڵ�Ϊ��ֹ�ڵ�
                    temp.Add(1);
                }
                else
                {
                    // ������һ�����ҽڵ�
                    temp.Add(node_index);
                }
                // Ϊ��һ���ڵ����ÿ�������ֵΪ�µĿ�������ֵ
                node_index = node_index_new;
            }
            m_dict_idxes[index] = temp.ToArray();
            // �����µĿ�������ֵ
            return node_index;
        }

        private ArrayList BuildArrays()
        {
            List<string> al_chars = new List<string>(m_index);
            List<int[]> al_idxes = new List<int[]>(m_index);

            int count = m_index;

            // �������ֵΪ 10 ���� (��)
            al_chars.Add(m_dict_chars[10]);
            al_idxes.Add(m_dict_idxes[10]);

            // �����ֹ�ڵ�
            al_chars.Add("K");
            al_idxes.Add(new int[1] { -1 });

            // ��ӱ�����
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
    /// TRIE �ڵ�ṹ
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// �Ƿ�Ϊ�ɴʵ�
        /// </summary>
        private bool m_is_k = false;
        /// <summary>
        /// �ڵ���ַ�
        /// </summary>
        private char m_char;
        /// <summary>
        /// �ӽڵ�
        /// </summary>
        private Dictionary<char, TrieNode> m_children;

        /// <summary>
        /// �Ƿ�Ϊ�ɴʵ�
        /// </summary>
        public bool IsK
        {
            get { return m_is_k; }
            set { m_is_k = value; }
        }

        /// <summary>
        /// �ڵ���ַ�
        /// </summary>
        public char Char
        {
            get { return m_char; }
        }

        /// <summary>
        /// �ӽڵ�
        /// </summary>
        public Dictionary<char, TrieNode> Children
        {
            get { return m_children; }
            set { m_children = value; }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="chr">�ڵ���ַ�</param>
        public TrieNode(char chr)
        {
            m_char = chr;
            m_children = new Dictionary<char, TrieNode>();
            Global.CountNodes++;
        }
    }
}
