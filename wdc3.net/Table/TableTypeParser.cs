using System;
using System.Collections;
using System.Collections.Generic;
using wdc3.net.Enums;

namespace wdc3.net.Table
{
    public class TableTypeParser
    {
        public static Db2ValueTypes Parse(string typeName) => (typeName) switch
        {
            "int" => Db2ValueTypes.Integer,
            "float" => Db2ValueTypes.Float,
            "string" => Db2ValueTypes.Text,
            "locstring" => Db2ValueTypes.Text,
            _ => throw new NotImplementedException(),
        };
    }
}