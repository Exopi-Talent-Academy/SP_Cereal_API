using System.Windows.Controls;

namespace Cereal_API.WPF.Views;

public partial class CerealDataTableView : UserControl
{
    public CerealDataTableView()
    {
        InitializeComponent();
    }

    private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyName == "Id")
        {
            e.Column.Header = "ID";
            e.Column.IsReadOnly = true;
        }

        if (e.PropertyName == "Manufacturer" && e.Column is DataGridComboBoxColumn comboBoxColumn)
        {
            //comboBoxColumn.DisplayMemberPath = "ManufacturerName";
        }
    }
}
