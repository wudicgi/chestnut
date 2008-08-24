// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using WudiStudio.Chestnut.Utilities.Resources;

namespace WudiStudio.Chestnut.Utilities.Lexicons
{
    /// <summary>
    /// ´ÊµäÄÚÈÝ
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

    public interface Lexicon
    {
        MetaInfo MetaInfo { get; set; }
        WordList WordList { get; set; }
        FreqList FreqList { get; set; }
        void Build(string path, Contents contents);
        void Read(string path);
    }
}
