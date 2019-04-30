using System;
using System.Collections.Generic;
using System.Text;

namespace JsonFileDB
{
    /// <summary>
    /// Makes a class suitable to be used as a table.
    /// </summary>
    public interface ITable
    {
        object Id { get; set; }
    }
}
