// $Id$

using System;
using System.Collections.Generic;
using System.Text;
using WudiStudio.Chestnut.Utilities.Resources;
using WudiStudio.Chestnut.Utilities.Lexicons;

namespace WudiStudio.Chestnut.Utilities
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
            string str = "_-.";
            for (int i = 0; i < str.Length; i++)
            {
                Console.WriteLine("{0} => {1}", str[i], Char.GetUnicodeCategory(str[i]));
            }

            Console.ReadKey();

            return;
            */
            Global.SetCulture("zh-CN");

            string tmp = "_words_bin";

            bool build = false;

            if (build)
            {
                MetaInfo meta_info = new MetaInfo();

                WordList word_list = new WordList();
                word_list.ReadFromFile(tmp + ".txt");
                word_list.UpdateMetainfo(meta_info);

                FreqList freq_list = new FreqList();
                freq_list.ReadFromFile(tmp + ".txt");
                freq_list.UpdateMetainfo(meta_info);
                /*
                BinaryLexion lexicon = new BinaryLexion();
                lexicon.MetaInfo = meta_info;
                lexicon.WordList = word_list;
                lexicon.FreqList = freq_list;
                lexicon.Build(tmp + ".lex", Contents.All);
                */
                
                DotnetLexion lexicon = new DotnetLexion();
                lexicon.MetaInfo = meta_info;
                lexicon.WordList = word_list;
                lexicon.FreqList = freq_list;
                lexicon.Build(tmp + ".lex", Contents.All);
            }
            else
            {
                string path_lex = @"D:\Projects\Assemblies\data\" + tmp + ".lex";
                Tokenizer tokenizer = Tokenizer.factory(path_lex);
                tokenizer.MatchMode = MatchMode.Normal;
                tokenizer.SingleMode = SingleMode.Normal;
                tokenizer.EnglishMode = EnglishMode.Normal;
                /*
                System.IO.FileStream fs = new System.IO.FileStream("1070.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                byte[] buf = new byte[fs.Length];
                fs.Read(buf, 0, (int)fs.Length);
                string text = Encoding.UTF8.GetString(buf);
                */
                string text = "zend_function_get_args Hello getField PHPClasses HDCD image012 tImage028";
                //string text = "这使他迸发出旺盛活力。第一次登上海拔八千米的山。";
                //string text = "2007年2月17日是中国的除夕";//，中国是世界上5个常任理事国之一";
                Token[] tokens = tokenizer.Tokenize(text);
                StringBuilder sb = new StringBuilder();
                foreach (Token token in tokens)
                {
                    sb.Append(token.Text);
                    sb.Append("/ ");
                }
                Console.WriteLine();
                Console.WriteLine(sb.ToString());
                Console.WriteLine("Completed.");

                Console.ReadKey();
            }
            /*
            ArgumentParser parser = new ArgumentParser();
            parser.AddArgument(ArgumentType.Command, "?", "help", null);
            parser.AddArgument(ArgumentType.Command, "b", "build", null);
            parser.AddArgument(ArgumentType.Command, "x", "export", null);
            //parser.AddArgument(ArgumentType.Command, null, "upgrade", null);
            parser.AddArgument(ArgumentType.Command, "c", "check", null);
            parser.AddArgument(ArgumentType.Command, "i", "info", null);
            parser.AddArgument(ArgumentType.Value, "o", "output", "filename");
            parser.AddArgument(ArgumentType.Value, "f", "format", null);
            //parser.AddArgument(ArgumentType.Switch, null, "dotnet", null);
            parser.AddArgument(ArgumentType.Switch, null, "gbk", null);
            parser.AddArgument(ArgumentType.Switch, null, "english", null);
            parser.AddArgument(ArgumentType.Value, null, "title", null);
            parser.AddArgument(ArgumentType.Value, null, "author", null);
            parser.AddArgument(ArgumentType.Params, null, "files", "files...");

            args = new string[] { "--build", "-o=_words.lex", "_words.txt" };

            // 如果没有给定任何参数
            if (args.Length == 0)
            {
                // 显示版本信息
                ShowVersion();
                // 显示参数信息
                parser.ShowArguments();
#if DEBUG
                Console.WriteLine(Global.GetText("press_any_key_to_exit"));
                Console.ReadKey();
#endif
                return;
            }

            // 获取参数解析结果
            Dictionary<string, object> result = parser.Parse(args);

            // 如果要求使用英文界面
            if ((bool)result["english"])
            {
                // 设定当前区域设置为美国英语
                Global.SetCulture("en-US");
            }

            // 显示版本信息
            ShowVersion();

            if ((bool)result["help"])
            {
                // 显示参数信息
                parser.ShowArguments();
            }
            else if ((bool)result["build"])
            {
                // 从指定文件生成词典
                Builder builder = new Builder();
                builder.Output = result["output"].ToString();
                builder.Files = (string[])result["files"];
                builder.Format = Format.Dotnet;
                builder.Build();
            }
            else if ((bool)result["export"])
            {
                // 导出词典中的所有词汇
                Reader reader = new Reader();
                reader.Output = result["output"].ToString();
                reader.Input = ((string[])result["files"])[0];
                reader.Export();
            }
            else if ((bool)result["upgrade"])
            {
                // 升级词典到版本二
                Reader reader = new Reader();
                reader.Output = result["output"].ToString();
                reader.Input = ((string[])result["files"])[0];
                reader.Upgrade();
            }
            else if ((bool)result["info"])
            {
                // 显示指定词典的元信息
                Reader reader = new Reader();
                reader.Output = null;
                reader.Input = ((string[])result["files"])[0];
                reader.Info();
            }
            else
            {
#if DEBUG
                // 显示所有输出参数
                foreach (string key in result.Keys)
                {
                    Console.WriteLine("{0} = {1}", key, ((result[key] == null) ? "NULL" : result[key].ToString()));
                }
                Console.WriteLine();
#else
                    // 显示参数信息
                    parser.ShowArguments();
#endif
            }
#if DEBUG
            Console.WriteLine(Global.GetText("press_any_key_to_exit"));
            Console.ReadKey();
#endif
            */
        }

        private static void ShowVersion()
        {
            Console.WriteLine(Global.GetText("title"));
            Console.WriteLine(Global.GetText("copyright"));
            Console.WriteLine();
        }

        private static void ShowException(string message)
        {
            Console.WriteLine(message);
#if DEBUG
            Console.ReadKey();
#endif
            System.Environment.Exit(0);
        }

        private static void ShowException(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
#if DEBUG
            Console.ReadKey();
#endif
            System.Environment.Exit(0);
        }
    }
}
