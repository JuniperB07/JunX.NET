using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace JunX.NET8.Excel
{
    /// <summary>
    /// Provides .NET functionalities for interacting with Excel Workbooks and Worksheets.
    /// </summary>
    [ComVisible(true)]
    [Guid("F8372207-D405-4E5A-AAC9-5C2789CF8406")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class JXcel
    {
        private Worksheet _worksheet = new Worksheet();
        private Workbook _workbook = new Workbook();
        private string _filePath = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="JXcel"/> class using the specified worksheet.
        /// </summary>
        /// <param name="SetWorksheet">
        /// The <see cref="Worksheet"/> instance to operate on. Must not be <c>null</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="SetWorksheet"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This constructor sets the internal worksheet context for all subsequent operations.
        /// </remarks>
        public JXcel(Worksheet SetWorksheet)
        {
            _worksheet = SetWorksheet ?? throw new ArgumentNullException(nameof(SetWorksheet));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JXcel"/> class using the specified workbook.
        /// </summary>
        /// <param name="SetWorkbook">
        /// The <see cref="Workbook"/> instance to operate on. Must not be <c>null</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="SetWorkbook"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This constructor sets the internal workbook context for all subsequent worksheet-level operations.
        /// </remarks>
        public JXcel(Workbook SetWorkbook)
        {
            _workbook = SetWorkbook ?? throw new ArgumentNullException(nameof(SetWorkbook));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JXcel"/> class using the specified workbook and worksheet.
        /// </summary>
        /// <param name="SetWorkbook">
        /// The <see cref="Workbook"/> instance to operate on. Must not be <c>null</c>.
        /// </param>
        /// <param name="SetWorksheet">
        /// The <see cref="Worksheet"/> instance to operate on. Must not be <c>null</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="SetWorkbook"/> or <paramref name="SetWorksheet"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This constructor sets both workbook and worksheet contexts for subsequent operations.
        /// Use this overload when both scopes are required for initialization.
        /// </remarks>
        public JXcel(Workbook SetWorkbook, Worksheet SetWorksheet)
        {
            _workbook = SetWorkbook ?? throw new ArgumentNullException(nameof(SetWorkbook));
            _worksheet = SetWorksheet ?? throw new ArgumentNullException(nameof(SetWorksheet));
        }

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the current <see cref="Worksheet"/> context used by this instance.
        /// </summary>
        /// <value>
        /// The <see cref="Worksheet"/> instance associated with this object. This property can be updated dynamically.
        /// </value>
        /// <remarks>
        /// Changing this property will redirect all subsequent operations to the new worksheet.
        /// Ensure the assigned worksheet is properly initialized before use.
        /// </remarks>
        public Worksheet Worksheet {  get { return _worksheet; }  set { _worksheet = value; } }
        /// <summary>
        /// Gets or sets the current <see cref="Workbook"/> context used by this instance.
        /// </summary>
        /// <value>
        /// The <see cref="Workbook"/> instance associated with this object. This property can be reassigned at runtime.
        /// </value>
        /// <remarks>
        /// Changing this property will redirect all workbook-scoped operations to the new instance.
        /// Ensure the assigned workbook is properly initialized before use.
        /// </remarks>
        public Workbook Workbook { get { return _workbook; } set { _workbook = value; } }
        /// <summary>
        /// Gets or sets the absolute file path associated with this instance.
        /// </summary>
        /// <value>
        /// A fully qualified file path derived from the internal <c>_filePath</c> field.
        /// </value>
        /// <remarks>
        /// The getter returns the canonical full path using <see cref="Path.GetFullPath(string)"/>.
        /// The setter stores the raw input path without validation.
        /// </remarks>
        public string FilePath { get { return Path.GetFullPath(_filePath); } set {  _filePath = value; } }
        #endregion

        #region PUBLIC CLASSES
        /// <summary>
        /// Returns the next available row index in the specified column by scanning upward from the bottom.
        /// </summary>
        /// <param name="ColumnIndex">
        /// The column index to scan, typically an integer (e.g., <c>1</c> for column A).
        /// </param>
        /// <returns>
        /// The row index where new data can be inserted. If the last non-empty cell is at row <c>n</c>, returns <c>n + 1</c>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the internal worksheet reference is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method uses Excel's <see cref="XlDirection.xlUp"/> to find the last non-empty cell in the column.
        /// If the last cell is empty, it returns that row; otherwise, it returns the next row.
        /// </remarks>
        public int LastRow(object ColumnIndex)
        {
            if (_worksheet == null)
                throw new Exception("Worksheet is null.");

            int lRC = _worksheet.Rows.Count; //last Row Count
            int lr = _worksheet.Cells[lRC, ColumnIndex].End(XlDirection.xlUp).Row;

            if (Convert.ToString(_worksheet.Cells[lr, ColumnIndex].Value) == "")
                return lr;
            else
                return lr + 1;
        }
        /// <summary>
        /// Retrieves the value of a cell at the specified row and column index.
        /// </summary>
        /// <param name="RowIndex">
        /// The row index of the target cell. Typically an integer starting from <c>1</c>.
        /// </param>
        /// <param name="ColumnIndex">
        /// The column index of the target cell. Can be an integer or string (e.g., <c>"A"</c>).
        /// </param>
        /// <returns>
        /// The value of the specified cell, or <c>string.Empty</c> if the cell is <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method provides a dynamic return type to accommodate various Excel cell contents.
        /// </remarks>
        public dynamic CellValue(object RowIndex, object ColumnIndex)
        {
            return _worksheet.Cells[RowIndex, ColumnIndex].Value ?? string.Empty;
        }
        /// <summary>
        /// Retrieves the value of the specified Excel range.
        /// </summary>
        /// <param name="Range">
        /// The range reference, typically a string like <c>"A1:B2"</c> or a named range.
        /// </param>
        /// <returns>
        /// The value of the specified range, or <c>string.Empty</c> if the range is <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method supports dynamic return types to accommodate single-cell values, arrays, or complex range contents.
        /// </remarks>
        public dynamic RangeValue(object Range)
        {
            return _worksheet.Range[Range].Value ?? string.Empty;
        }
        /// <summary>
        /// Determines whether a specified value exists in a given column, starting from a specified row index.
        /// </summary>
        /// <param name="CheckValue">
        /// The value to search for within the column.
        /// </param>
        /// <param name="FirstRowIndex">
        /// The starting row index for the search. Typically begins at <c>1</c>.
        /// </param>
        /// <param name="CheckColumn">
        /// The column index to search within. Can be an integer or string (e.g., <c>"A"</c>).
        /// </param>
        /// <returns>
        /// <c>true</c> if the value is found in the column; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method scans upward from the bottom of the worksheet using <see cref="LastRow"/> to determine the search boundary.
        /// It performs a linear search and stops at the first match.
        /// </remarks>
        public bool Exists(dynamic CheckValue, int FirstRowIndex, object CheckColumn)
        {
            int lr = LastRow(CheckColumn);
            bool e = false;

            for(int i=FirstRowIndex; i<lr; i++)
            {
                if(CellValue(i, CheckColumn) == CheckValue)
                {
                    e = true;
                    break;
                }
            }

            return e;
        }
        /// <summary>
        /// Inserts a row of values into the worksheet across a specified column range.
        /// </summary>
        /// <param name="RowIndex">
        /// The row index where the values will be inserted.
        /// </param>
        /// <param name="FromColumnIndex">
        /// The starting column index for insertion.
        /// </param>
        /// <param name="ToColumnIndex">
        /// The ending column index for insertion. Must be greater than or equal to <paramref name="FromColumnIndex"/>.
        /// </param>
        /// <param name="Values">
        /// An array of values to insert into the specified row and column range. Its length must match the column span.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the column range is invalid or the number of values does not match the number of columns.
        /// </exception>
        /// <remarks>
        /// This method performs a direct cell assignment across the specified range. It assumes the worksheet is initialized and accessible.
        /// </remarks>
        public void InsertRow(object RowIndex, int FromColumnIndex, int ToColumnIndex, dynamic[] Values)
        {
            if (ToColumnIndex < FromColumnIndex)
                throw new Exception("ToColumnIndex must be greater than or equal to FromColumnIndex.");

            if ((ToColumnIndex - FromColumnIndex) + 1 != Values.Length)
                throw new Exception("The number of columns doesn't match the number of Values in the array.");

            int a = 0;
            for(int b=FromColumnIndex; b <= ToColumnIndex; b++)
            {
                _worksheet.Cells[RowIndex, b].Value = Values[b];
                a++;
            }
        }
        /// <summary>
        /// Inserts a block of values into the worksheet across a specified row and column range.
        /// </summary>
        /// <param name="FromRowIndex">
        /// The starting row index for insertion.
        /// </param>
        /// <param name="ToRowIndex">
        /// The ending row index for insertion. Must be greater than or equal to <paramref name="FromRowIndex"/>.
        /// </param>
        /// <param name="FromColumnIndex">
        /// The starting column index for insertion.
        /// </param>
        /// <param name="ToColumnIndex">
        /// The ending column index for insertion. Must be greater than or equal to <paramref name="FromColumnIndex"/>.
        /// </param>
        /// <param name="Values">
        /// A two-dimensional array of values to insert. The first dimension represents rows, and the second represents columns.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the specified range is invalid or the dimensions of <paramref name="Values"/> do not match the target range.
        /// </exception>
        /// <remarks>
        /// This method performs a direct cell assignment for each value in the array. It assumes the worksheet is initialized and accessible.
        /// </remarks>
        public void InsertRow(int FromRowIndex, int ToRowIndex, int FromColumnIndex, int ToColumnIndex, dynamic[][] Values)
        {
            if (ToRowIndex < FromRowIndex || ToColumnIndex < FromColumnIndex)
                throw new Exception("Invalid ToRowIndex or ToColumnIndex values.");

            if ((ToRowIndex - FromRowIndex) + 1 != Values.GetLength(0))
                throw new Exception("The number of rows doesn't match the 1st Dimension (Row) of the Values array.");

            int row = 0, col = 0;
            for(int a =  FromRowIndex; a < ToRowIndex; a++)
            {
                col = 0;
                for(int b= FromColumnIndex; b < ToColumnIndex; b++)
                {
                    _worksheet.Cells[a, b].Value = Values[row][col];
                    col++;
                }
                row++;
            }
        }
        /// <summary>
        /// Updates the value of a specific cell in the worksheet.
        /// </summary>
        /// <param name="Row">
        /// The row index of the target cell.
        /// </param>
        /// <param name="Column">
        /// The column index of the target cell. Can be an integer or string (e.g., <c>"A"</c>).
        /// </param>
        /// <param name="Value">
        /// The value to assign to the specified cell.
        /// </param>
        /// <remarks>
        /// This method performs a direct assignment to the cell's <c>Value</c> property. It assumes the worksheet is initialized and accessible.
        /// </remarks>
        public void UpdateValue(object Row, object Column, dynamic Value)
        {
            _worksheet.Cells[Row, Column].Value = Value;
        }
        /// <summary>
        /// Updates the value of a specified range in the worksheet.
        /// </summary>
        /// <param name="Range">
        /// The target range to update. Can be a string reference (e.g., <c>"A1:B2"</c>) or a named range.
        /// </param>
        /// <param name="Value">
        /// The value to assign to the specified range. Can be a scalar, array, or formula-compatible input.
        /// </param>
        /// <remarks>
        /// This method performs a direct assignment to the range's <c>Value</c> property. It assumes the worksheet is initialized and accessible.
        /// </remarks>
        public void UpdateValue(object Range, dynamic Value)
        {
            _worksheet.Range[Range].Value = Value;
        }
        /// <summary>
        /// Updates multiple ranges in the worksheet with corresponding values.
        /// </summary>
        /// <param name="Ranges">
        /// An array of range references to update. Each entry can be a string (e.g., <c>"A1"</c>, <c>"B2:C3"</c>) or a named range.
        /// </param>
        /// <param name="Values">
        /// An array of values to assign to the corresponding ranges. The length must match <paramref name="Ranges"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the array lengths mismatch or a range update fails.
        /// </exception>
        /// <remarks>
        /// This method performs a one-to-one assignment of values to ranges. It throws a detailed exception if any update fails.
        /// </remarks>
        public void UpdateValues(object[] Ranges, dynamic[] Values)
        {
            if (Ranges.Length != Values.Length)
                throw new Exception("Ranges & Values array length mismatch.");

            for(int a=0; a<Ranges.Length; a++)
            {
                try
                {
                    _worksheet.Range[Ranges[a]].Value = Values[a];
                }
                catch(Exception e)
                {
                    throw new Exception("Cell Value Update Error:\n\n" + e.Message.ToString());
                }
            }
        }
        #endregion
    }
}
