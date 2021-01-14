using System;
using System.Collections;
using System.Collections.Generic;

namespace wdc3.net.Table
{
    public class TableTypeParser
    {
        public static Db2ValueTypes Parse(string typeName) => (typeName) switch
        {
            "int" => Db2ValueTypes.Number,
            "float" => Db2ValueTypes.Number,
            "string" => Db2ValueTypes.Text,
            "locstring" => Db2ValueTypes.Text,
            _ => throw new NotImplementedException(),
        };
    }
}