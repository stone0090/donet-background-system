using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using stonefw.Utility;

namespace stonefw.Web.Utility.UserControl
{
    public partial class SelectListBox : System.Web.UI.UserControl
    {
        public string Text
        {
            get { return this.txtText.Text; }
            set { this.txtText.Text = value; }
        }

        public string Value
        {
            get { return this.hdValue.Value; }
            set { this.hdValue.Value = value; }
        }

        public int Width
        {
            get
            {
                var width = this.txtText.Style["width"];
                return string.IsNullOrEmpty(width) ? 0 : int.Parse(width.Replace("px", ""));
            }
            set
            {
                this.txtText.Style["width"] = value + "px";
            }
        }

        public string DialogTitle
        {
            get { return this.hdTitle.Value; }
            set { this.hdTitle.Value = value; }
        }

        public int DialogWidth
        {
            get { return int.Parse(this.hdDialogWidth.Value); }
            set { this.hdDialogWidth.Value = value.ToString(); }
        }

        public int DialogHeight
        {
            get { return int.Parse(this.hdDialogHeight.Value); }
            set { this.hdDialogHeight.Value = value.ToString(); }
        }

        public bool SingleSelect
        {
            get { return bool.Parse(this.hdSingleSelect.Value); }
            set { this.hdSingleSelect.Value = value.ToString(); }
        }
        public bool ShowSearchBox
        {
            get { return bool.Parse(this.hdShowSearchBox.Value); }
            set { this.hdShowSearchBox.Value = value.ToString(); }
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public object DataSource { get; set; }

        private List<Column> _columns = new List<Column>();
        public List<Column> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Attributes["size"]))
                {
                    this.txtText.Attributes.Add("size", this.Attributes["size"]);
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
                        this.hdData.Value = JsonHelper.DataTableToJson(dt);
                    }
                    var list = DataSource as IList;
                    if (list != null)
                    {
                        this.hdData.Value = JsonHelper.ObjectToJson(list);
                    }
                }

                var sbCol = new StringBuilder();
                sbCol.Append("[[{ \"field\": 'ck', \"checkbox\": true },");
                this.selectFiled.Items.Add(new ListItem("请选择...", "0"));
                foreach (Column column in Columns)
                {
                    sbCol.Append("{");
                    sbCol.AppendFormat(" \"align\":'center', \"field\": '{0}', \"title\": '{1}'", column.FieldValue, column.FieldText);
                    if (column.Width > 0) sbCol.AppendFormat(" ,\"width\": {0}", column.Width);
                    if (column.Hidden) sbCol.Append(" ,\"hidden\": true");
                    else this.selectFiled.Items.Add(new ListItem(column.FieldText, column.FieldValue));
                    sbCol.Append("},");
                    if (column.IsText) this.hdTextField.Value = column.FieldValue;
                    if (column.IsValue) this.hdValueField.Value = column.FieldValue;

                }
                sbCol.AppendLine("]]");
                this.hdColumns.Value = sbCol.ToString();
            }
        }

    }
}