using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShop.AdditionalClasses
{
    public static class Password // Validate and hash password
    {
        public static bool ValidatePassword(string password)
        {
            return !String.IsNullOrWhiteSpace(password) 
                    && password.Any(ch => char.IsUpper(ch))
                    && password.Any(ch => char.IsLower(ch))
                    && !password.All(ch => char.IsLetterOrDigit(ch))
                    && !password.Any(ch => char.IsWhiteSpace(ch)
                    && password.Length >= 8);
        }

        static uint CircularLeftShift(uint value, int shift)
        {
            return (value << shift) | (value >> (32 - shift));
        }

        static string ByteArrToString(byte[] num)
        {
            string str = "";
            foreach (byte b in num)
            {
                switch (b >> 4)
                {
                    case 15: { str += 'f'; break; }
                    case 14: { str += 'e'; break; }
                    case 13: { str += 'd'; break; }
                    case 12: { str += 'c'; break; }
                    case 11: { str += 'b'; break; }
                    case 10: { str += 'a'; break; }
                    default:
                        {
                            str += (b >> 4).ToString();
                            break;
                        }
                }

                byte b2 = (byte)(b << 4);

                switch (b2 >> 4)
                {
                    case 15: { str += 'f'; break; }
                    case 14: { str += 'e'; break; }
                    case 13: { str += 'd'; break; }
                    case 12: { str += 'c'; break; }
                    case 11: { str += 'b'; break; }
                    case 10: { str += 'a'; break; }
                    default:
                        {
                            str += (b2 >> 4).ToString();
                            break;
                        }
                }

            }
            return str;
        }

        public static string SHA1(string message)
        {
            uint h0 = 0x67452301;
            uint h1 = 0xEFCDAB89;
            uint h2 = 0x98BADCFE;
            uint h3 = 0x10325476;
            uint h4 = 0xC3D2E1F0;

            ulong lengthBeforePadding = (ulong)message.Length * 8;
            message += (char)0x80;

            while (message.Length % 64 != 56)
            {
                message += (char)0;
            }
            long length = message.Length * 8;

            for (int i = 7; i >= 0; i--)
            {
                message += (char)((lengthBeforePadding >> (8 * i) & 0xFF));
            }

            for (int i = 0; i < Math.Ceiling(message.Length / (512.0 / 8.0)); i++)
            {
                string tmp = message.Substring(i * (512 / 8), (512 / 8));
                List<uint> M = new List<uint>();
                for (int j = 0; j <= 15; j++)
                {
                    uint value = 0;
                    for (int k1 = 0; k1 < 4; k1++)
                    {
                        if (j * 4 + k1 < tmp.Length)
                        {
                            value = (value << 8) | (char)tmp[j * 4 + k1];
                        }
                    }
                    M.Add(value);
                }
                List<uint> W = new List<uint>(M);

                for (int j = 16; j <= 79; j++)
                {
                    W.Add(CircularLeftShift(W[j - 3] ^ W[j - 8] ^ W[j - 14] ^ W[j - 16], 1));
                }

                var a = h0;
                var b = h1;
                var c = h2;
                var d = h3;
                var e = h4;

                uint f = 0;
                uint k = 0;

                for (int j = 0; j <= 79; j++)
                {
                    if (j >= 0 && j <= 19)
                    {
                        f = (b & c) | ((~b) & d);
                        k = 0x5A827999;
                    }
                    else if (j >= 20 && j <= 39)
                    {
                        f = b ^ c ^ d;
                        k = 0x6ED9EBA1;
                    }
                    else if (j >= 40 && j <= 59)
                    {
                        f = (b & c) | (b & d) | (c & d);
                        k = 0x8F1BBCDC;
                    }
                    else if (j >= 60 && j <= 79)
                    {
                        f = b ^ c ^ d;
                        k = 0xCA62C1D6;
                    }

                    uint temp = CircularLeftShift(a, 5) + f + e + k + W[j];
                    e = d;
                    d = c;
                    c = CircularLeftShift(b, 30);
                    b = a;
                    a = temp;
                }

                h0 = h0 + a;
                h1 = h1 + b;
                h2 = h2 + c;
                h3 = h3 + d;
                h4 = h4 + e;
            }

            List<uint> tmparr = new List<uint> { h0, h1, h2, h3, h4 };

            string str = "";

            foreach (var i in tmparr)
            {
                byte[] ConvArray = BitConverter.GetBytes(i);
                Array.Reverse(ConvArray);
                str += ByteArrToString(ConvArray);
            }

            return str;
        }
    }
}
