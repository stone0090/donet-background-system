using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Stonefw.Utility;

namespace Stonefw.Web.Utility.UserControl
{
    public partial class SelectListBox : System.Web.UI.UserControl
    {
        public string Text
        {
            get { return txtText.Text; }
            set { txtText.Text = value; }
        }

        public string Value
        {
            get { return hdValue.Value; }
            set { hdValue.Value = value; }
        }

        public int Width
        {
            get
            {
                var width = txtText.Style["width"];
                return string.IsNullOrEmpty(width) ? 0 : int.Parse(width.Replace("px", ""));
            }
            set { txtText.Style["width"] = value + "px"; }
        }

        public string DialogTitle
        {
            get { return hdTitle.Value; }
            set { hdTitle.Value = value; }
        }

        public int DialogWidth
        {
            get { return int.Parse(hdDialogWidth.Value); }
            set { hdDialogWidth.Value = value.ToString(); }
        }

        public int DialogHeight
        {
            get { return int.Parse(hdDialogHeight.Value); }
            set { hdDialogHeight.Value = value.ToString(); }
        }

        public bool SingleSelect
        {
            get { return bool.Parse(hdSingleSelect.Value); }
            set { hdSingleSelect.Value = value.ToString(); }
        }

        public bool ShowSearchBox
        {
            get { return bool.Parse(hdShowSearchBox.Value); }
            set { hdShowSearchBox.Value = value.ToString(); }
        }

        public bool Enabled { get; set; } = true;

        public object DataSource { get; set; }

        public List<Column> Columns { get; set; } = new List<Column>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Attributes["size"]))
                {
                    txtText.Attributes.Add("size", Attributes["size"]);
                }

                if (!Enabled)
                {
                    btnShowDialog.Enabled = false;
                    return;
                }

                if (DataSource != null)
                {
                    var dt = DataSource as DataTable;
                    if (dt != null)
                    {
                        hdData.Value = JsonHelper.DataTableToJson(dt);
                    }
                    var list = DataSource as IList;
                    if (list != null)
                    {
                        hdData.Value = JsonHelper.ObjectToJson(list);
                    }
                }

                var sbCol = new StringBuilder();
                sbCol.Append("[[{ \"field\": 'ck', \"checkbox\": true },");
                selectFiled.Items.Add(new ListItem("请选择...", "0"));
                foreach (var column in Columns)
                {
                    sbCol.Append("{");
                    sbCol.AppendFormat(" \"align\":'center', \"field\": '{0}', \"title\": '{1}'", column.FieldValue,
                        column.FieldText);
                    if (column.Width > 0) sbCol.AppendFormat(" ,\"width\": {0}", column.Width);
                    if (column.Hidden) sbCol.Append(" ,\"hidden\": true");
                    else selectFiled.Items.Add(new ListItem(column.FieldText, column.FieldValue));
                    sbCol.Append("},");
                    if (column.IsText) hdTextField.Value = column.FieldValue;
                    if (column.IsValue) hdValueField.Value = column.FieldValue;
                }
                sbCol.AppendLine("]]");
                hdColumns.Value = sbCol.ToString();
            }
        }
    }
}