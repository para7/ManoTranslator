﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.VisualBasic;

namespace TweetConverter
{
    static class Program
    {
        // https://dobon.net/vb/dotnet/string/ishiragana.html#islatin

        /// <summary>
        /// 指定した Unicode 文字が、ひらがなかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がひらがなである場合は true。それ以外の場合は false。</returns>
        public static bool IsHiragana(char c)
        {
            //「ぁ」～「より」までと、「ー」「ダブルハイフン」をひらがなとする
            return ('\u3041' <= c && c <= '\u309F')
                || c == '\u30FC' || c == '\u30A0';
        }

        /// <summary>
        /// 指定した Unicode 文字が、英字の大文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsUpperLatin(char c)
        {
            //半角英字と全角英字の大文字の時はTrue
            return ('A' <= c && c <= 'Z') || ('Ａ' <= c && c <= 'Ｚ');
        }

        /// <summary>
        /// 指定した Unicode 文字が、英字の小文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の小文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsLowerLatin(char c)
        {
            //半角英字と全角英字の小文字の時はTrue
            return ('a' <= c && c <= 'z') || ('ａ' <= c && c <= 'ｚ');
        }
        
        /// <summary>
        /// 記号
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>記号ならtrue</returns>
        public static bool IsSign(char c)
        {
            var x = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

            return x.Contains(c);
        }

        public static bool IsNumber(char c)
        {
            return ('0' <= c && c <= '9') || ('０' <= c && c <= '９');
        }

        public static char ByteConverter(char c)
        {
            if(IsLowerLatin(c) || IsUpperLatin(c) || IsNumber(c))
            {
                return Microsoft.VisualBasic.Strings.StrConv(c.ToString(), VbStrConv.Narrow).First();
            }

            return c;
        }
        
        //ハッシュタグの継続判定
        static bool EscapeJudge(char _c)
        {
            return IsHiragana(_c) || IsLowerLatin(_c) || IsUpperLatin(_c) || IsNumber(_c);
        }

        static void Func(TextReader reader)
        {
            bool skip = false;

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 3 && line.Substring(0, 2) == "RT")
                {
                    continue;
                }

                foreach (char c in line)
                {
                    if (skip)
                    {
                        if (!EscapeJudge(c))
                        {
                            skip = false;
                        }
                        continue;
                    }

                    //ハッシュタグの読み飛ばし
                    if (c == '#')
                    {
                        skip = true;
                        continue;
                    }

                    //辞書化予定の文字なら出力
                    if (EscapeJudge(c) || IsSign(c))
                    {
                        Console.Write(ByteConverter(c));
                    }
                }
                skip = false;
            }
        }

        static void Main(string[] args)
        {
            // http://www.atmarkit.co.jp/fdotnet/dotnettips/681stdin/stdin.html
            TextReader input;

            if (args.Length == 0)
            {
                // 読み込み元は標準入力
                input = Console.In;
            }
            else
            {
                // 読み込み元はファイル
                input = new StreamReader(args[0],
                          System.Text.Encoding.GetEncoding("Shift_JIS"));
            }

            Func(input);

            input.Dispose();
        }
    }
}
