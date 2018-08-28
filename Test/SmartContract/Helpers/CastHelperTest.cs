using System.Collections.Generic;
using EthereumLibrary.Helper;
using Xunit;

namespace Test.SmartContract.Helpers
{
    public class CastHelperTest
    {
        private const string LongString =
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.";

        private const string ShortString = "#bb44a6";

        private static readonly byte[] TestBytes32 = new byte[32]
        {
            116, 101, 115, 116, 83, 116, 114, 105, 110, 103, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0
        };

        private readonly byte[][] _testBytes32Array = new[] {TestBytes32, TestBytes32, TestBytes32};

        private readonly string _expectedStringFromBytes32Array =
            "testString\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0testString\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0testString";


        [Fact]
        public void CastStringToBytes32()
        {
            // longer then 32 bytes : 245 bytes
            var result = CastHelper.StringToBytes32(LongString);
            Assert.Equal(32, result.Length);

            // shorter then 32 bytes : 7 bytes 
            result = CastHelper.StringToBytes32(ShortString);
            Assert.Equal(32, result.Length);
        }

        [Fact]
        public void CastBytes32ToString()
        {
            var result = CastHelper.Bytes32ToString(TestBytes32);
            Assert.Equal("testString", result);
        }

        [Fact]
        public void CastStringToBytes32ArrayOf3()
        {
            const int dimensions = 3;

            // longer then 96 bytes : 245 bytes
            var result = CastHelper.StringToBytes32ArrayOf(dimensions, LongString);
            Assert.Equal(dimensions, result.Length);
            for (var i = 0; i < dimensions; i++)
            {
                Assert.Equal(32, result[i].Length);
            }

            // shorter then 96 bytes : 7 bytes 
            result = CastHelper.StringToBytes32ArrayOf(3, ShortString);
            Assert.Equal(dimensions, result.Length);
            for (var i = 0; i < dimensions; i++)
            {
                Assert.Equal(32, result[i].Length);
            }
        }

        [Fact]
        public void CastStringToBytes32ArrayOf6()
        {
            const int dimensions = 6;

            // longer then 192 bytes : 245 bytes
            var result = CastHelper.StringToBytes32ArrayOf(dimensions, LongString);
            Assert.Equal(dimensions, result.Length);
            for (var i = 0; i < dimensions; i++)
            {
                Assert.Equal(32, result[i].Length);
            }
        }

        [Fact]
        public void CastBytes32ArrayToString()
        {
            var result = CastHelper.Bytes32ArrayToString(_testBytes32Array);
            Assert.Equal(_expectedStringFromBytes32Array, result);
        }

        [Fact]
        public void CastBackForward()
        {
            var bytes = CastHelper.StringToBytes32(LongString);
            Assert.StartsWith(CastHelper.Bytes32ToString(bytes), LongString);

            var bytes3 = CastHelper.StringToBytes32ArrayOf(3, LongString);
            Assert.StartsWith(CastHelper.Bytes32ArrayToString(bytes3), LongString);

            var bytes6 = CastHelper.StringToBytes32ArrayOf(6, LongString);
            Assert.StartsWith(CastHelper.Bytes32ArrayToString(bytes6), LongString);

            var bytesOrigin = new byte[]
            {
                12, 16, 125, 13, 9, 10, 12, 16, 125, 13, 9, 10, 12, 16, 125, 13, 9, 10, 12, 16, 125, 13, 9, 10, 12, 16,
                125, 13, 9, 10, 9, 10
            };
            var stringFromBytes = CastHelper.Bytes32ToString(bytesOrigin);
            var doubleCasting = CastHelper.StringToBytes32(stringFromBytes);
            Assert.Equal(bytesOrigin, doubleCasting);
        }
    }
}