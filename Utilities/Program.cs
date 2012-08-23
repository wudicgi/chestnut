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
                * /
                
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
                * /
                //string text = "XXI php-gtk get_args Hello getField PHPClasses HDCD image012 tImage028";
                //string text = "这使他迸发出旺盛活力。第一次登上海拔八千米的山。";
                //string text = "2007年2月17日是中国的除夕";//，中国是世界上5个常任理事国之一";
                string text = "五六七八 image001 hello002the003world";
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
            */
            ArgumentParser parser = new ArgumentParser();

            parser.AddArguments(
                new Argument(ArgumentType.Command, "?", "help", null, "Display this help and exit"),
                new Argument(ArgumentType.Command, "b", "build", null, "Build a lexicon from the specified files"),
                new Argument(ArgumentType.Command, "x", "export", null, "Export all words in a lexion"),
                new Argument(ArgumentType.Command, "c", "check", null, "Check if the specified file is a valid lexicon"),
                new Argument(ArgumentType.Command, "i", "info", null, "Display the meta data of specified lexicon"),
                new Argument(ArgumentType.Value, "o", "output", "filename", "Specify the output filename"),
                new Argument(ArgumentType.Value, "f", "format", null, "词典格式 (bin, php, dotnet)"),
                new Argument(ArgumentType.Switch, null, "english", null, "English interface"),
                new Argument(ArgumentType.Value, null, "title", null, "词典的标题"),
                new Argument(ArgumentType.Value, null, "author", null, "词典的作者"),
                new Argument(ArgumentType.Params, null, "files", "files...", "词汇文件列表，或词典文件")
            );

            //args = new string[] { "--build", "-o=_words.lex", "_words.txt" };

            // 如果没有给定任何参数
            if (args.Length == 0)
            {
                // 显示版本信息
                ShowVersion();
                // 显示参数信息
                parser.ShowArguments();
#if DEBUG
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
#endif
//                return;
            }

            // 获取参数解析结果
            Dictionary<string, object> result = parser.Parse(args);

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

                string[] files = (string[])result["files"];
                string output = result["output"].ToString();

                if (files.Length == 0)
                {
                    ShowException("no files");
                }
                if (output == "")
                {
                    ShowException("no output");
                }

                MetaInfo meta_info = new MetaInfo();

                WordList word_list = new WordList();
                for (int i = 0; i < files.Length; i++)
                {
                    word_list.ReadFromFile(files[i]);
                }
                word_list.UpdateMetainfo(meta_info);

                FreqList freq_list = new FreqList();
                for (int i = 0; i < files.Length; i++)
                {
                    freq_list.ReadFromFile(files[i]);
                }
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
                lexicon.Build(output, Contents.All);
            }
            else if ((bool)result["export"])
            {
                // 导出词典中的所有词汇
                //Reader reader = new Reader();
                //reader.Output = result["output"].ToString();
                //reader.Input = ((string[])result["files"])[0];
                //reader.Export();
            }
            else if ((bool)result["info"])
            {
                //显示指定词典的元信息
                //Reader reader = new Reader();
                //reader.Output = null;
                //reader.Input = ((string[])result["files"])[0];
                //reader.Info();
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
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
#endif
        }

        private static void ShowVersion()
        {
            Console.WriteLine("Wudi's Lexicon Utilities");
            Console.WriteLine("(C) Copyright 2007-2008 Wudi Studio");
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
