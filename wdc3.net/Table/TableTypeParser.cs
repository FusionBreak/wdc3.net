using System;

namespace wdc3.net.Table
{
    public class TableTypeParser
    {
        public Type Parse(string typeName) => typeName switch
        {
            "int" => typeof(int),
            "float" => typeof(float),
            "string" => typeof(string),
            "locstring" => typeof(string),
            _ => throw new NotImplementedException(),
        };

        public Type Parse(string typeName, bool isSigned, int size) => (typeName, isSigned, size) switch
        {
            ("int", true, 32) => typeof(int),
            ("int", false, 32) => typeof(uint),
            ("int", true, 16) => typeof(short),
            ("int", false, 16) => typeof(ushort),
            ("int", true, 8) => typeof(byte),
            ("int", false, 8) => typeof(byte),
            ("float", true, 8) => typeof(float),
            ("float", false, 0) => typeof(float),
            ("float", false, 4) => typeof(float),
            ("float", false, 8) => typeof(double),
            ("float", false, 16) => typeof(decimal),
            ("string", false, 0) => typeof(string),
            ("locstring", false, 0) => typeof(string),
            _ => throw new NotImplementedException(),
        };
    }
}