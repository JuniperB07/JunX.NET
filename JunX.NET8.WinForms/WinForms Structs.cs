using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunX.NET8.WinForms
{
    /// <summary>
    /// Encapsulates metadata for binding a specific <see cref="DataTable"/> from a <see cref="DataSet"/> to a <see cref="DataGridView"/>.
    /// </summary>
    public struct SetDGVMetadata
    {
        /// <summary>
        /// The <see cref="DataSet"/> containing one or more tables to bind.
        /// </summary>
        public DataSet DataSet { get; set; }

        /// <summary>
        /// The zero-based index of the <see cref="DataTable"/> within the <see cref="DataSet"/> to bind.
        /// </summary>
        public int Table { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetDGVMetadata"/> struct with the specified dataset and table index.
        /// </summary>
        /// <param name="UseDataSet">The <see cref="DataSet"/> containing the target table.</param>
        /// <param name="UseTable">The index of the table within the dataset to bind.</param>
        public SetDGVMetadata(DataSet UseDataSet, int UseTable)
        {
            DataSet = UseDataSet;
            Table = UseTable;
        }
    }
}
