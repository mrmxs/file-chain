using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Nethereum.RLP;

namespace EthereumLibrary.Helper
{
    public static class CastHelper
    {
        public static byte[] StringToBytes32(string input)
        {
            var bytes = input.ToBytesForRLPEncoding();
            var result = new byte[32];
            Array.Copy(bytes, result, bytes.Length >= 32 ? 32 : bytes.Length);

            return result;
        }

        public static string Bytes32ToString(byte[] input)
        {
            return input.ToStringFromRLPDecoded().TrimEnd('\0');
        }

        public static byte[][] StringToBytes32ArrayOf(int dimensions, string input)
        {
            var bytes = input.ToBytesForRLPEncoding();
            var result = new byte[dimensions][];
            for (var i = 0; i < dimensions; i++)
            {
                result[i] = new byte[32];
                var fromParentI = i * 32;
                var tailIndex = 32 * (i + 1);
                if (bytes.Length > fromParentI)
                    Array.Copy(bytes, fromParentI, result[i], 0, bytes.Length >= tailIndex ? 32 : bytes.Length);
            }

            return result;
        }

        public static string Bytes32ArrayToString(IEnumerable<byte[]> input)
        {
            return input.Aggregate("", (current, t) => current + t.ToStringFromRLPDecoded()).TrimEnd('\0');
        }
    }
}