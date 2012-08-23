// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using WudiStudio.Chestnut.Utilities.Resources;

namespace WudiStudio.Chestnut.Utilities.Lexicons
{
    /// <summary>
    /// 词典所包含的内容
    /// </summary>
    [Flags]
    public enum Contents
    {
        WordsBackward = 1,
        WordsForward = 2,
        FreqsWords = 4,
        FreqsChars = 8,
        All = WordsBackward | WordsForward | FreqsWords | FreqsChars
    }

    /// <summary>
    /// 词典的接口
    /// </summary>
    public interface Lexicon
    {
        MetaInfo MetaInfo { get; set; }
        WordList WordList { get; set; }
        FreqList FreqList { get; set; }
        void Build(string path, Contents contents);
        void Read(string path);
    }
}
