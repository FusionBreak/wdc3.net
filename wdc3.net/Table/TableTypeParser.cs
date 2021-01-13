using System;
using System.Collections;
using System.Collections.Generic;

namespace wdc3.net.Table
{
    public class TableTypeParser
    {
        public static Type Parse(string typeName) => typeName switch
        {
            "int" => typeof(int),
            "float" => typeof(float),
            "string" => typeof(string),
            "locstring" => typeof(string),
            _ => throw new NotImplementedException(),
        };

        public static Type Parse(string typeName, bool isSigned, int size, bool isArray = false) => (typeName, isSigned, size, isArray) switch
        {
            ("int", true, 64, false) => typeof(long),
            ("int", false, 64, false) => typeof(ulong),
            ("int", true, 32, false) => typeof(int),
            ("int", false, 32, false) => typeof(uint),
            ("int", true, 16, false) => typeof(short),
            ("int", false, 16, false) => typeof(ushort),
            ("int", true, 8, false) => typeof(byte),
            ("int", false, 8, false) => typeof(byte),
            ("float", true, 8, false) => typeof(float),
            ("float", false, 0, false) => typeof(float),
            ("float", false, 4, false) => typeof(float),
            ("float", false, 8, false) => typeof(double),
            ("float", false, 16, false) => typeof(decimal),
            ("string", false, 0, false) => typeof(string),
            ("locstring", false, 0, false) => typeof(string),
            ("int", true, 64, true) => typeof(IEnumerable<long>),
            ("int", false, 64, true) => typeof(IEnumerable<ulong>),
            ("int", true, 32, true) => typeof(IEnumerable<int>),
            ("int", false, 32, true) => typeof(IEnumerable<uint>),
            ("int", true, 16, true) => typeof(IEnumerable<short>),
            ("int", false, 16, true) => typeof(IEnumerable<ushort>),
            ("int", true, 8, true) => typeof(IEnumerable<byte>),
            ("int", false, 8, true) => typeof(IEnumerable<byte>),
            ("float", true, 8, true) => typeof(IEnumerable<float>),
            ("float", false, 0, true) => typeof(IEnumerable<float>),
            ("float", false, 4, true) => typeof(IEnumerable<float>),
            ("float", false, 8, true) => typeof(IEnumerable<double>),
            ("float", false, 16, true) => typeof(IEnumerable<decimal>),
            ("string", false, 0, true) => typeof(IEnumerable<string>),
            ("locstring", false, 0, true) => typeof(IEnumerable<string>),
            _ => throw new NotImplementedException(),
        };
    }
}