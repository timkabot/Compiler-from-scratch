using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

namespace Dlanguage
{
    class Tokenizer
    {
        /*
            states =>
                0 - clear
                1 - int
                2 - real
                3 - letters
                4 - string lit
                5 - delim
         */
        static private Dictionary<string, string> tokens = Tokens.initTokens();
        static private List<string> toks = new List<string>();
        static private StringBuilder s = new StringBuilder();

        static private StreamReader sr = new StreamReader(File.OpenRead("./input"));
        // static private StreamWriter sw = new StreamWriter(File.OpenWrite("./src/Parser/input"));

        static private int cur_state = 0;
        static private string buffer = "";
        static private string input;
        static private int ptr = 0;
        static private string delimeters = "{}()<>=:[]*/+-;,.";
        static private bool isDelimeter(char ch)
        {
            return delimeters.Contains(ch);
        }
        static private void process(string s, int state)
        {
            if (tokens.ContainsKey(buffer))
                toks.Add(tokens.GetValueOrDefault(buffer));
            else
                toks.Add("id__" + buffer);
            // Console.WriteLine("after if contains" + buffer);
            cur_state = state;
            buffer = s;
        }
        static private bool isDouble(char ch)
        {
            return "><:/=.".Contains(ch) && ptr < input.Length - 1 && tokens.ContainsKey(ch.ToString() + input[ptr + 1].ToString());
        }
        static public List<string> Parse()
        {
            input = sr.ReadToEnd();
            while (ptr < input.Length)
            {
                // Console.WriteLine("cur_state: " + cur_state + " current buf: " + buffer);
                char ch = input[ptr];
                if (cur_state == 0)
                {
                    if (ch == '\"' || ch == '\'')
                        cur_state = 4;
                    if (ch == ' ') { }

                    if (Char.IsDigit(ch))
                    {
                        cur_state = 1;
                        buffer += ch;
                    }
                    if (Char.IsLetter(ch))
                    {
                        cur_state = 3;
                        buffer += ch;
                    }
                    if (isDelimeter(ch))
                    {
                        if (isDouble(ch))
                        {
                            buffer = ch.ToString();
                            cur_state = 5;
                        }
                        else
                            toks.Add(tokens.GetValueOrDefault(ch.ToString()));
                    }
                    ptr++;
                    continue;

                }
                else if (cur_state == 1)
                {
                    if (Char.IsDigit(ch))
                        buffer += ch;
                    if (ch == '.')
                    {
                        buffer += ".";
                        cur_state = 2;
                        ptr++;
                        continue;
                    }
                    if (ch == ' ')
                    {
                        toks.Add("int__" + buffer);
                        buffer = "";
                        cur_state = 0;
                    }
                    if (isDelimeter(ch))
                    {
                        toks.Add("int__" + buffer);
                        if (isDouble(ch))
                        {
                            buffer = ch.ToString();
                            cur_state = 5;
                        }
                        else
                        {
                            toks.Add(tokens.GetValueOrDefault(ch.ToString()));
                            cur_state = 0;
                            buffer = "";
                        }
                    }
                    ptr++;
                    continue;
                }
                else if (cur_state == 2)
                {
                    Console.WriteLine("Entered 2 state: {0} {1}", buffer, ch);
                    if (Char.IsDigit(ch))
                        buffer += ch;
                    if (ch == '.' && buffer[buffer.Length - 1] == '.')
                    {
                        toks.Add("int__" + buffer.Substring(0, buffer.Length - 1));
                        toks.Add("dots_t");
                        buffer = "";
                        cur_state = 0;
                        ptr++;
                        continue;
                    }
                    if (ch == ' ' || ch == '\n')
                    {
                        toks.Add("real__" + buffer);
                        buffer = "";
                        cur_state = 0;
                    }
                    if (isDelimeter(ch))
                    {
                        toks.Add("real__" + buffer);
                        if (isDouble(ch))
                        {
                            buffer = ch.ToString();
                            cur_state = 5;
                        }
                        else
                        {
                            toks.Add(tokens.GetValueOrDefault(ch.ToString()));
                            cur_state = 0;
                            buffer = "";
                        }

                    }
                    ptr++;
                    continue;
                }
                else if (cur_state == 3)
                {
                    if (Char.IsLetter(ch))
                        buffer += ch;
                    if (ch == ' ')
                        process("", 0);
                    if (isDelimeter(ch))
                    {
                        if (isDouble(ch))
                            process(ch.ToString(), 5);
                        else
                        {
                            process("", 0);
                            toks.Add(tokens.GetValueOrDefault(ch.ToString()));
                        }
                    }
                    ptr++;
                    continue;
                }
                else if (cur_state == 4)
                {
                    if (ch == '\"' || ch == '\'')
                    {
                        toks.Add("str__" + buffer);
                        cur_state = 0;
                        buffer = "";
                    }
                    else
                        buffer += ch;
                    ptr++;
                    continue;
                }
                // 5 state
                else
                {
                    // Console.WriteLine(buffer + ch);
                    if (tokens.ContainsKey(buffer + ch.ToString()))
                    {
                        toks.Add(tokens.GetValueOrDefault(buffer + ch.ToString()));
                        buffer = "";
                        cur_state = 0;
                    }
                    ptr++;
                }

            }
            if (tokens.ContainsKey(buffer))
                toks.Add(tokens.GetValueOrDefault(buffer));

            foreach (var item in toks)
            {
                // Console.WriteLine(item);
            }
            sr.Close();
            return toks;
        }
    }
}