using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JunX.NET8.WinForms
{
    /// <summary>
    /// Provides additional functions for manipulating System.Windows.Forms.Form Controls.
    /// </summary>
    public static class Forms
    {
        /// <summary>
        /// Sets the enabled state of the specified control.
        /// </summary>
        /// <param name="FormControl">The control whose enabled state is to be set. Cannot be null.</param>
        /// <param name="IsEnabled">A value indicating whether the control should be enabled. Set to <see langword="true"/> to enable the
        /// control; otherwise, <see langword="false"/>.</param>
        public static void SetControlEnabled(Control FormControl, bool IsEnabled)
        {
            FormControl.Enabled=IsEnabled;
        }
        /// <summary>
        /// Sets the enabled state of each control in the specified collection.
        /// </summary>
        /// <remarks>This method updates the Enabled property for all controls in the provided list. No
        /// action is taken if the list is empty.</remarks>
        /// <param name="FormControls">The list of controls whose Enabled property will be set. Cannot be null.</param>
        /// <param name="AreEnabled">A value indicating whether the controls should be enabled. Set to <see langword="true"/> to enable the
        /// controls; otherwise, <see langword="false"/>.</param>
        public static void SetControlEnabled(IEnumerable<Control> FormControls, bool AreEnabled)
        {
            foreach(Control ctrl in FormControls)
                ctrl.Enabled=AreEnabled;
        }
        /// <summary>
        /// Determines whether any control in the specified array has an empty Text property.
        /// </summary>
        /// <param name="FormControls">An array of Control objects to check for empty Text properties. Cannot be null.</param>
        /// <returns>true if at least one control in the array has an empty Text property; otherwise, false.</returns>
        public static bool HasEmptyFields(IEnumerable<Control> FormControls)
        {
            bool hasEmpty = false;

            foreach(Control ctrl in FormControls)
            {
                if(ctrl.Text == "")
                {
                    hasEmpty = true;
                    break;
                }
            }

            return hasEmpty;
        }
        /// <summary>
        /// Clears the text of each control in the specified collection.
        /// </summary>
        /// <param name="FormControls">A list of controls whose Text properties will be set to an empty string. Cannot be null.</param>
        public static void ClearControlText(IEnumerable<Control> FormControls)
        {
            foreach (Control ctrl in FormControls)
                ctrl.Text = "";
        }
        /// <summary>
        /// Removes all items from the specified ComboBox control.
        /// </summary>
        /// <param name="FormComboBox">The ComboBox control whose items are to be cleared. Cannot be null.</param>
        public static void ClearComboBox(ComboBox FormComboBox)
        {
            FormComboBox.Items.Clear();
        }
        /// <summary>
        /// Removes all items from each ComboBox in the specified list.
        /// </summary>
        /// <param name="FormComboBoxes">A list of ComboBox controls whose items will be cleared. Cannot be null. Each ComboBox in the list will have
        /// its Items collection cleared.</param>
        public static void ClearComboBox(IEnumerable<ComboBox> FormComboBoxes)
        {
            foreach (ComboBox cmb in FormComboBoxes)
                cmb.Items.Clear();
        }
        /// <summary>
        /// Populates the specified ComboBox with the provided list of items, replacing any existing entries.
        /// </summary>
        /// <remarks>This method clears all existing items from the ComboBox before adding the new items.
        /// The order of items in the ComboBox will match the order in the provided list.</remarks>
        /// <param name="FormComboBox">The ComboBox control to be filled with items. Cannot be null.</param>
        /// <param name="Items">The list of string items to add to the ComboBox. Cannot be null.</param>
        public static void FillComboBox(ComboBox FormComboBox, IEnumerable<string> Items)
        {
            ClearComboBox(FormComboBox);
            foreach (string item in Items)
                FormComboBox.Items.Add(item);
        }
        /// <summary>
        /// Populates each ComboBox in the specified array with the corresponding list of items.
        /// </summary>
        /// <remarks>All ComboBoxes in FormComboBoxes are cleared before new items are added. If the
        /// arrays are not of equal length, an exception may occur.</remarks>
        /// <param name="FormComboBoxes">An array of ComboBox controls to be filled with items. Each ComboBox at index i will be populated with the
        /// items from ItemsArray at the same index.</param>
        /// <param name="ItemsArray">An array of string lists, where each list contains the items to add to the corresponding ComboBox in
        /// FormComboBoxes. The length of ItemsArray must match the length of FormComboBoxes.</param>
        public static void FillComboBox(ComboBox[] FormComboBoxes, List<string>[] ItemsArray)
        {
            int cmbIndex = 0;
            ClearComboBox(FormComboBoxes);

            foreach (var items in ItemsArray)
            {
                foreach (string item in items)
                    FormComboBoxes[cmbIndex].Items.Add(item);
                cmbIndex++;
            }
        }
        /// <summary>
        /// Adds the specified items to the item collection of the given ComboBox control.
        /// </summary>
        /// <param name="FormComboBox">The ComboBox control to which the items will be added. Cannot be null.</param>
        /// <param name="Items">A list of strings representing the items to add to the ComboBox. Cannot be null.</param>
        public static void AppendComboBox(ComboBox FormComboBox, IEnumerable<string> Items)
        {
            foreach (string item in Items)
                FormComboBox.Items.Add(item);
        }
        /// <summary>
        /// Adds the items from each list in the specified array to the corresponding ComboBox in the provided array.
        /// </summary>
        /// <remarks>The number of elements in FormComboBoxes must be equal to the number of elements in
        /// ItemsArray. Each list in ItemsArray is added to the ComboBox at the same index in FormComboBoxes.</remarks>
        /// <param name="FormComboBoxes">An array of ComboBox controls to which items will be added. Each ComboBox corresponds to a list in the
        /// ItemsArray parameter.</param>
        /// <param name="ItemsArray">An array of string lists, where each list contains the items to add to the corresponding ComboBox in the
        /// FormComboBoxes array.</param>
        public static void AppendComboBox(ComboBox[] FormComboBoxes, List<string>[] ItemsArray)
        {
            int index = 0;
            foreach(List<string> Items in ItemsArray)
            {
                foreach (string item in Items)
                    FormComboBoxes[index].Items.Add(item);
                index++;
            }
        }
        /// <summary>
        /// Removes all items from the specified ListBox control.
        /// </summary>
        /// <param name="FormListBox">The ListBox control whose items are to be cleared. Cannot be null.</param>
        public static void ClearListBox(ListBox FormListBox)
        {
            FormListBox.Items.Clear();
        }
        /// <summary>
        /// Removes all items from each ListBox in the specified collection.
        /// </summary>
        /// <param name="FormListBoxes">A list of ListBox controls whose items will be cleared. Cannot be null. Each ListBox in the list must not be
        /// null.</param>
        public static void ClearListBox(IEnumerable<ListBox> FormListBoxes)
        {
            foreach (ListBox lst in FormListBoxes)
                lst.Items.Clear();
        }
        /// <summary>
        /// Populates the specified ListBox control with the provided collection of items, replacing any existing
        /// entries.
        /// </summary>
        /// <remarks>This method clears all existing items from the ListBox before adding the new items.
        /// The order of items in the ListBox will match the order in the provided collection.</remarks>
        /// <param name="FormListBox">The ListBox control to be filled with items. Cannot be null.</param>
        /// <param name="Items">The collection of strings to add to the ListBox. Cannot be null.</param>
        public static void FillListBox(ListBox FormListBox, IEnumerable<string> Items)
        {
            ClearListBox(FormListBox);

            foreach (string item in Items)
                FormListBox.Items.Add(item);
        }
        /// <summary>
        /// Populates each ListBox in the specified array with the corresponding collection of items.
        /// </summary>
        /// <remarks>All ListBox controls in FormListBoxes are cleared before new items are added. The
        /// method assumes that both arrays are of equal length; otherwise, an exception may occur.</remarks>
        /// <param name="FormListBoxes">An array of ListBox controls to be filled. Each ListBox at index i will receive items from the collection at
        /// the same index in the ItemsArray parameter.</param>
        /// <param name="ItemsArray">An array of string collections, where each collection contains the items to add to the corresponding ListBox
        /// in the FormListBoxes array. The length of this array must match the length of FormListBoxes.</param>
        public static void FillListBox(ListBox[] FormListBoxes, List<string>[] ItemsArray)
        {
            ClearListBox(FormListBoxes);

            int index = 0;
            foreach(List<string> Items in ItemsArray)
            {
                foreach(string item in Items)
                    FormListBoxes[index].Items.Add(item);
                index++;
            }
        }
        /// <summary>
        /// Appends the specified collection of strings to the items of the given ListBox control.
        /// </summary>
        /// <param name="FormListBox">The ListBox control to which the items will be added. Cannot be null.</param>
        /// <param name="Items">The collection of strings to append to the ListBox. Cannot be null.</param>
        public static void AppendListBox(ListBox FormListBox, IEnumerable<string> Items)
        {
            foreach(string item in Items)
                FormListBox.Items.Add(item);
        }
        /// <summary>
        /// Adds the items from each list in the specified array to the corresponding ListBox control in the provided
        /// array.
        /// </summary>
        /// <remarks>The number of elements in FormListBoxes must be equal to the number of elements in
        /// ItemsArray. Items from each list in ItemsArray are added to the Items collection of the ListBox at the same
        /// index in FormListBoxes.</remarks>
        /// <param name="FormListBoxes">An array of ListBox controls to which items will be added. Each ListBox corresponds to a list in the
        /// ItemsArray parameter.</param>
        /// <param name="ItemsArray">An array of string lists, where each list contains the items to add to the corresponding ListBox in the
        /// FormListBoxes array.</param>
        public static void AppendListBox(ListBox[] FormListBoxes, List<string>[] ItemsArray)
        {
            int index = 0;
            foreach(List<string> Items in ItemsArray)
            {
                foreach (string item in Items)
                    FormListBoxes[index].Items.Add(item);
                index++;
            }
        }
        /// <summary>
        /// Sets the maximum selectable date for the specified DateTimePicker control.
        /// </summary>
        /// <param name="FormDateTimePicker">The DateTimePicker control whose maximum date is to be set. Cannot be null.</param>
        /// <param name="MaxDate">The maximum date that can be selected in the DateTimePicker control.</param>
        public static void SetDateTimePickerMaxDate(DateTimePicker FormDateTimePicker, DateTime MaxDate)
        {
            FormDateTimePicker.MaxDate = MaxDate;
        }
        /// <summary>
        /// Sets the maximum selectable date for each <see cref="DateTimePicker"/> in the provided list.
        /// </summary>
        /// <param name="FormDateTimePickers">A list of <see cref="DateTimePicker"/> controls to update.</param>
        /// <param name="MaxDate">The maximum date to assign to each control.</param>
        /// <remarks>
        /// This method delegates to an overload that sets the <c>MaxDate</c> property for individual controls.
        /// Ensure that all controls in the list are properly initialized before calling.
        /// </remarks>
        public static void SetDateTimePickerMaxDate(IEnumerable<DateTimePicker> FormDateTimePickers, DateTime MaxDate)
        {
            foreach (DateTimePicker dtp in FormDateTimePickers)
                SetDateTimePickerMaxDate(dtp, MaxDate);
        }
        /// <summary>
        /// Sets the minimum selectable date for the specified DateTimePicker control.
        /// </summary>
        /// <param name="FormDateTimePicker">The DateTimePicker control whose minimum date is to be set. Cannot be null.</param>
        /// <param name="MinDate">The minimum date that can be selected in the DateTimePicker control.</param>
        public static void SetDateTimePickerMinDate(DateTimePicker FormDateTimePicker, DateTime MinDate)
        {
            FormDateTimePicker.MinDate = MinDate;
        }
        /// <summary>
        /// Sets the minimum selectable date for each <see cref="DateTimePicker"/> in the provided list.
        /// </summary>
        /// <param name="FormDateTimePickers">A list of <see cref="DateTimePicker"/> controls to update.</param>
        /// <param name="MinDate">The minimum date to assign to each control.</param>
        /// <remarks>
        /// This method delegates to an overload that sets the <c>MinDate</c> property for individual controls.
        /// Ensure that all controls in the list are properly initialized before calling.
        /// </remarks>
        public static void SetDateTimePickerMinDate(IEnumerable<DateTimePicker> FormDateTimePickers, DateTime MinDate)
        {
            foreach (DateTimePicker dtp in FormDateTimePickers)
                SetDateTimePickerMinDate(dtp, MinDate);
        }
        /// <summary>
        /// Resets the specified <see cref="DataGridView"/> by clearing its rows and removing its data source.
        /// </summary>
        /// <param name="FormDataGridView">
        /// The <see cref="DataGridView"/> control to reset. This method clears all rows using <c>Rows.Clear()</c>
        /// and sets the <c>DataSource</c> property to <c>null</c>, effectively removing bound data and manual entries.
        /// </param>
        /// <remarks>
        /// Use this method to fully reset a grid before rebinding or reinitializing. It handles both bound and unbound scenarios.
        /// Note that <c>Rows.Clear()</c> only applies to unbound rows; for bound grids, clearing the <c>DataSource</c> is sufficient.
        /// </remarks>
        public static void ResetDataGridView(DataGridView FormDataGridView)
        {
            FormDataGridView.Rows.Clear();
            FormDataGridView.DataSource = null;
        }
        /// <summary>
        /// Resets a collection of <see cref="DataGridView"/> controls by clearing their rows and removing their data sources.
        /// </summary>
        /// <param name="FormDataGridView">
        /// An <see cref="IEnumerable{T}"/> of <see cref="DataGridView"/> controls to reset. Each grid will have its <c>Rows</c> cleared
        /// and its <c>DataSource</c> property set to <c>null</c>, effectively removing both bound and unbound data.
        /// </param>
        /// <remarks>
        /// This method delegates to <c>ResetDataGridView(DataGridView)</c> for each control in the collection.
        /// Use it to batch-reset multiple grids before rebinding or reinitializing.
        /// </remarks>
        public static void ResetDataGridView(IEnumerable<DataGridView> FormDataGridView)
        {
            foreach (DataGridView dgv in FormDataGridView)
                ResetDataGridView(dgv);
        }
        /// <summary>
        /// Binds a specific table from a <see cref="DataSet"/> to the provided <see cref="DataGridView"/>.
        /// </summary>
        /// <param name="FormDataGridView">
        /// The <see cref="DataGridView"/> control to populate with data.
        /// </param>
        /// <param name="UseDataSet">
        /// The <see cref="DataSet"/> containing one or more <see cref="DataTable"/> objects to bind.
        /// </param>
        /// <param name="TableIndex">
        /// The zero-based index of the <see cref="DataTable"/> within the <paramref name="UseDataSet"/> to bind.
        /// </param>
        /// <remarks>
        /// This method sets the <c>DataSource</c> of the grid to the specified table. Ensure that <paramref name="TableIndex"/>
        /// is within bounds to avoid runtime exceptions. Use this to dynamically bind grids to different tables in a dataset.
        /// </remarks>
        public static void SetDataGridViewDataSet(DataGridView FormDataGridView, DataSet UseDataSet, int TableIndex)
        {
            FormDataGridView.DataSource = UseDataSet.Tables[TableIndex];
        }
        /// <summary>
        /// Binds a collection of <see cref="DataGridView"/> controls to corresponding tables from a sequence of <see cref="SetDGVMetadata"/>.
        /// </summary>
        /// <param name="FormDataGridViews">
        /// An <see cref="IEnumerable{T}"/> of <see cref="DataGridView"/> controls to bind.
        /// </param>
        /// <param name="UsingDataSetMetadata">
        /// An <see cref="IEnumerable{T}"/> of <see cref="SetDGVMetadata"/> objects, each containing a <see cref="DataSet"/> and table index
        /// to be bound to the corresponding grid.
        /// </param>
        /// <remarks>
        /// This method pairs each grid with its respective metadata entry by index and binds the specified table using
        /// <c>SetDataGridViewDataSet(DataGridView, DataSet, int)</c>. Ensure that both sequences are aligned and of equal length
        /// to avoid index out-of-range errors.
        /// </remarks>
        public static void SetDataGridViewDataSet(IEnumerable<DataGridView> FormDataGridViews, IEnumerable<SetDGVMetadata> UsingDataSetMetadata)
        {
            int index = 0;
            SetDGVMetadata[] dgvMetadata = UsingDataSetMetadata.ToArray();
            foreach(DataGridView dgv in FormDataGridViews)
            {
                SetDataGridViewDataSet(dgv, dgvMetadata[index].DataSet, dgvMetadata[index].Table);
                index++;
            }
        }


        /// <summary>
        /// Provides recursive extraction of controls of type <typeparamref name="T"/> from a container,
        /// optionally filtered by name prefix.
        /// </summary>
        /// <typeparam name="T">
        /// The type of control to extract, constrained to <see cref="Control"/>. Examples include <see cref="TextBox"/>,
        /// <see cref="Button"/>, <see cref="Label"/>, and other Windows Forms controls.
        /// </typeparam>
        public static class ControlType<T> where T : Control
        {
            /// <summary>
            /// Recursively extracts all controls of type <typeparamref name="T"/> from the specified container and its child controls,
            /// optionally filtering by name prefix.
            /// </summary>
            /// <param name="Container">The root <see cref="Control"/> container to search within.</param>
            /// <param name="StartsWith">
            /// An optional prefix to match against each control's <c>Name</c> property. If specified, only controls whose names
            /// begin with this value will be included; otherwise, all matching controls are returned.
            /// </param>
            /// <returns>A list of controls of type <typeparamref name="T"/> matching the specified criteria.</returns>
            /// <remarks>
            /// This method traverses the entire control hierarchy, including nested containers such as panels and group boxes.
            /// Use this to selectively extract controls based on naming conventions or to gather all for batch operations.
            /// </remarks>
            public static IEnumerable<T> Extract(Control Container, string StartsWith = "")
            {
                List<T> extracted = new List<T>();

                if(StartsWith != "")
                {
                    foreach(Control ctrl in Container.Controls)
                    {
                        if(ctrl is T typedControl && ctrl.Name.StartsWith(StartsWith))
                            extracted.Add(typedControl);

                        if (ctrl.HasChildren)
                            extracted.AddRange(Extract(ctrl, StartsWith));
                    }

                    return extracted;
                }
                else
                {
                    foreach(Control ctrl in Container.Controls)
                    {
                        if (ctrl is T typedControl)
                            extracted.Add(typedControl);

                        if (ctrl.HasChildren)
                            extracted.AddRange(Extract(ctrl));
                    }

                    return extracted;
                }
            }
        }
    }
}
