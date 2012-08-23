// $Id$

using System;
using System.Collections.Generic;
using System.Text;

namespace WudiStudio.Chestnut.Utilities
{
    /// <summary>
    /// 参数类型
    /// </summary>
    public enum ArgumentType
    {
        /// <summary>
        /// 命令
        /// </summary>
        Command,
        /// <summary>
        /// 开关
        /// </summary>
        Switch,
        /// <summary>
        /// 字符串值
        /// </summary>
        Value,
        /// <summary>
        /// 数组
        /// </summary>
        Array,
        /// <summary>
        /// 不定项参数
        /// </summary>
        Params
    }

    public class Argument
    {
        private ArgumentType m_type;
        private string m_short_name;
        private string m_long_name;
        private string m_value_name;
        private string m_description;

        public ArgumentType Type
        {
            get { return m_type; }
        }

        public string ShortName
        {
            get { return m_short_name; }
        }

        public string LongName
        {
            get { return m_long_name; }
        }

        public string ValueName
        {
            get { return m_value_name; }
        }

        public string Description
        {
            get { return m_description; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="short_name">短名称</param>
        /// <param name="long_name">长名称</param>
        /// <param name="value_name">值的名称</param>
        /// <param name="description">参数描述</param>
        public Argument(ArgumentType type, string short_name, string long_name, string value_name, string description)
        {
            if (long_name == null)
            {
                if (short_name == null)
                {
                    throw new Exception("Error");
                }
                else
                {
                    long_name = short_name;
                }
            }
            m_type = type;
            m_short_name = short_name;
            m_long_name = long_name;
            m_value_name = value_name;
            m_description = description;
        }
    }

    /// <summary>
    /// 命令行参数解析器
    /// </summary>
    public class ArgumentParser
    {
        private int m_length_expression = 21;

        private Dictionary<string, Argument> m_arguments;
        private Dictionary<char, string> m_equals;

        private Dictionary<string, object> m_result;

        /// <summary>
        /// 允许的前缀符号，默认为 - 及 /
        /// </summary>
        private char[] m_allowed_prefixes = new char[] { '-', '/' };
        /// <summary>
        /// 赋值符号，默认为 = 及 :
        /// </summary>
        private char[] m_assign_symbols = new char[] { '=', ':' };

        private string[] m_args;
        private int m_argc;
        private int m_argi;

        private Argument m_params_argument;
        private List<string> m_params_list;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ArgumentParser()
        {
            m_arguments = new Dictionary<string, Argument>();
            m_equals = new Dictionary<char, string>();

            m_result = new Dictionary<string, object>();
        }

        /// <summary>
        /// 添加多个可接受的参数
        /// </summary>
        /// <param name="arguments">参数对象数组</param>
        public void AddArguments(params Argument[] arguments)
        {
            foreach (Argument argument in arguments) {
                AddArgument(argument);
            }
        }

        /// <summary>
        /// 添加一个可接受的参数
        /// </summary>
        /// <param name="argument">参数对象</param>
        public void AddArgument(Argument argument)
        {
            if (argument.Type == ArgumentType.Params)
            {
                m_params_argument = argument;
                m_params_list = new List<string>();
            }
            else
            {
                m_arguments.Add(argument.LongName, argument);
                if (argument.ShortName != null)
                {
                    m_equals.Add(argument.ShortName[0], argument.LongName);
                }
            }
        }

        public void ShowArguments()
        {
            string filename = AppDomain.CurrentDomain.FriendlyName;
            System.IO.FileInfo fileinfo = new System.IO.FileInfo(filename);
            Dictionary<string, Argument>.ValueCollection arguments = m_arguments.Values;
            List<Argument> list_commands = new List<Argument>();
            List<Argument> list_options = new List<Argument>();

            foreach (Argument argument in arguments)
            {
                if (argument.Type == ArgumentType.Command)
                {
                    list_commands.Add(argument);
                }
                else
                {
                    list_options.Add(argument);
                }
            }

            StringBuilder usage = new StringBuilder();
            usage.Append("Usage: ");
            usage.Append(fileinfo.Name.Remove(fileinfo.Name.Length - fileinfo.Extension.Length));
            if (list_commands.Count > 0)
            {
                usage.Append(" <command>");
            }
            if (list_options.Count > 0)
            {
                usage.Append(" [options]");
            }
            if (m_params_argument != null)
            {
                usage.AppendFormat(" {0}", m_params_argument.ValueName);
            }
            Console.WriteLine(usage.ToString());
            Console.WriteLine();

            if (list_commands.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Commands:");
                Console.ResetColor();
                for (int i = 0; i < list_commands.Count; i++)
                {
                    ShowArgument(list_commands[i]);
                }
                Console.WriteLine();
            }

            if (list_options.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Options:");
                Console.ResetColor();
                for (int i = 0; i < list_options.Count; i++)
                {
                    ShowArgument(list_options[i]);
                }
                Console.WriteLine();
            }

            if (m_params_argument != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Params:");
                Console.ResetColor();
                ShowArgument(m_params_argument);
                Console.WriteLine();
            }
        }

        private void ShowArgument(Argument argument)
        {
            string expression = ShowArgument_GetExpression(argument);
            Console.Write("  ");
            if (expression.Length > m_length_expression)
            {
                Console.WriteLine(expression);
                Console.WriteLine(argument.Description.PadLeft(argument.Description.Length + m_length_expression + 2, ' '));
            }
            else
            {
                Console.Write(expression.PadRight(m_length_expression + 2, ' '));
                Console.WriteLine(argument.Description.Replace("\r\n", "\r\n" + new string(' ', m_length_expression + 4)));
            }
        }

        private string ShowArgument_GetExpression(Argument argument)
        {
            if (argument.Type == ArgumentType.Params)
            {
                return argument.ValueName;
            }
            StringBuilder builder = new StringBuilder();
            if (argument.ShortName != null)
            {
                builder.Append('-');
                builder.Append(argument.ShortName);
                builder.Append(", ");
            }
            if (argument.LongName != null && argument.LongName != argument.ShortName)
            {
                builder.Append("--");
                builder.Append(argument.LongName);
            }
            if (argument.ValueName != null)
            {
                builder.Append('=');
                builder.Append(argument.ValueName);
            }
            return builder.ToString();
        }

        private void InitResult()
        {
            m_result.Clear();
            foreach (Argument argument in m_arguments.Values)
            {
                switch (argument.Type)
                {
                    case ArgumentType.Command:
                    case ArgumentType.Switch:
                        m_result.Add(argument.LongName, false);
                        break;
                    case ArgumentType.Value:
                        m_result.Add(argument.LongName, "");
                        break;
                    case ArgumentType.Array:
                    case ArgumentType.Params:
                        m_result.Add(argument.LongName, new string[0]);
                        break;
                    default:
                        ShowException("Error");
                        break;
                }
            }
        }

        public Dictionary<string, object> Parse(string[] args)
        {
            InitResult();

            m_args = args;
            m_argc = args.Length;
            m_argi = 0;
            while (m_argi < m_argc)
            {
                if (m_args[m_argi][0] != '-')
                {
                    if (m_params_argument != null)
                    {
                        List<string> list = new List<string>();
                        for (int i = m_argi; i < m_argc; i++)
                        {
                            list.Add(m_args[i]);
                        }
                        m_result[m_params_argument.LongName] = list.ToArray();
                        break;
                    }
                    else
                    {
                        ShowException("Syntax Error: {0}", m_args[m_argi]);
                    }
                }
                if (m_args[m_argi][1] == '-')
                {
                    ParseLong();
                }
                else
                {
                    ParseShort();
                }
            }
            return m_result;
        }

        private void ParseShort()
        {
            string arg = m_args[m_argi];
            int pos = arg.IndexOfAny(m_assign_symbols, 1);
            if ((pos == -1) && (arg.Length == 2))
            {
                if (!m_equals.ContainsKey(arg[1]))
                {
                    ShowException("Unknown argument \"-{0}\"", arg[1]);
                }
                m_args[m_argi] = "--" + m_equals[arg[1]];
                ParseLong();
            }
            else if ((pos != -1) && (pos == 2))
            {
                if (!m_equals.ContainsKey(arg[1]))
                {
                    ShowException("Unknown argument \"-{0}\"", arg[1]);
                }
                m_args[m_argi] = String.Concat(
                    "--",
                    m_equals[arg[1]],
                    arg.Substring(pos)
                );
                ParseLong();
            }
            else
            {
#if DEBUG
                ShowException("Syntax Error: {0}", arg);
#else
                return;
#endif
            }
        }

        private void ParseLong()
        {
            string arg = m_args[m_argi];
            int pos = arg.IndexOfAny(m_assign_symbols, 2);
            if (pos == -1)
            {
                string name = arg.Substring(2);
                if (!m_arguments.ContainsKey(name))
                {
                    ShowException("Unknown argument \"--{0}\"", name);
                }
                Argument argument = m_arguments[name];
                if (argument.Type == ArgumentType.Command || argument.Type == ArgumentType.Switch)
                {
                    m_result[name] = true;
                }
                else
                {
                    string value = (m_argi == m_argc - 1) ? "-" : m_args[m_argi + 1];
                    if (value[0] == '-')
                    {
                        ShowException("The required value for \"--{0}\" not found", name);
                    }
                    ParseLong_Process(argument, value);
                    m_argi++;
                }
            }
            else if (pos > 2)
            {
                string name = arg.Substring(2, pos - 2);
                if (!m_arguments.ContainsKey(name))
                {
                    ShowException("Unknown argument \"--{0}\"", name);
                }
                Argument argument = m_arguments[name];
                string value = arg.Substring(pos + 1);
                ParseLong_Process(argument, value);
            }
            else
            {
                ShowException("Syntax Error: {0}", arg);
            }
            m_argi++;
        }

        private void ParseLong_Process(Argument argument, string value)
        {
            string name = argument.LongName;
            object obj_value = null;
            switch (argument.Type)
            {
                case ArgumentType.Command:
                case ArgumentType.Switch:
                    obj_value = ParseSwitchValue(value);
                    break;
                case ArgumentType.Value:
                    obj_value = value;
                    break;
                case ArgumentType.Array:
                    string[] arr = value.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = arr[i].Trim();
                    }
                    obj_value = arr;
                    break;
                default:
                    ShowException("Exception!");
                    break;
            }
            if (obj_value != null)
            {
                m_result[name] = obj_value;
            }
        }

        private bool ParseSwitchValue(string value)
        {
            switch (value.ToLower())
            {
                case "0":
                case "false":
                case "off":
                case "no":
                    return false;
                default:
                    return true;
            }
        }

        private void ShowException(string message)
        {
            Console.WriteLine(message);
#if DEBUG
            Console.ReadKey();
#endif
            System.Environment.Exit(0);
        }

        private void ShowException(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
#if DEBUG
            Console.ReadKey();
#endif
            System.Environment.Exit(0);
        }
    }
}
