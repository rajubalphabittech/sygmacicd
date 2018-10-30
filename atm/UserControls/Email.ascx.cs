using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class UserControls_Email : System.Web.UI.UserControl {
    private bool gAllowMultiple = false;
    private string singleValidEmail = string.Concat(@"^", WebCommon.REGEXP_VALID_EMAIL, "$");
    private string multipleValidEmail = string.Concat(@"^(", WebCommon.REGEXP_VALID_EMAIL, @"){1}(;", WebCommon.REGEXP_VALID_EMAIL, @")*$");

    protected void Page_Load(object sender, System.EventArgs e) {
		if (gForceValidationSummmary || this.NamingContainer.FindControl("ValidationSummary1") != null) {
            revValue.Text = "*";
            rfvValue.Text = "*";
        }
    }
    protected void Page_PreRender(object sender, System.EventArgs e) {
        if (AllowMultiple) {
            revValue.ValidationExpression = multipleValidEmail;
            multiFootNote.Visible = true;
            if (Name == "Email")
                Name = string.Concat(Name, "s");
        } else {
            revValue.ValidationExpression = singleValidEmail;
            multiFootNote.Visible = false;
        
        }

        rfvValue.ErrorMessage = string.Format("'{0}' is Required!", Name);
        revValue.ErrorMessage = string.Format("'{0}' is NOT in Correct Format!", Name);
    }

    #region Public Methods

    public void Validate() {
        revValue.Validate();
        rfvValue.Validate();
    }

    public override void Focus() {
        txtValue.Focus();
    }

    #endregion

    #region Public Properties

    public string Value {
        get { return txtValue.Text.Replace(";", "; ").Trim(); }
        set { txtValue.Text = value; }
    }

    public bool AllowMultiple {
        get { return gAllowMultiple; }
        set { gAllowMultiple = value; }
    }

    public Unit Width {
        get { return txtValue.Width; }
        set { txtValue.Width = value; }
    }

    public int MaxLength {
        get { return txtValue.MaxLength; }
        set { txtValue.MaxLength = value; }
    }

    public short TabIndex {
        get { return txtValue.TabIndex; }
        set { txtValue.TabIndex = value; }
    }

    public bool IsValid {
        get { return (rfvValue.IsValid && revValue.IsValid); }
    }

    public bool IsRequired {
        get { return rfvValue.Enabled; }
        set { rfvValue.Enabled = value; }
    }

    public string ValidationGroup {
        get { return revValue.ValidationGroup; }
        set {
            revValue.ValidationGroup = value;
            rfvValue.ValidationGroup = value;
        }
    }
    private string gName = "Email";
    public string Name {
        get { return gName ; }
        set { gName = value; }
    }
	private bool gForceValidationSummmary;

	public bool ForceValidationSummary {
		get { return gForceValidationSummmary; }
		set { gForceValidationSummmary = value; }
	}

    #endregion

}
