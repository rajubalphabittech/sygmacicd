using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class PayrollFormsPage
    {
        //Navbar Buttons for Payroll Forms
        public IWebElement PayrollButton { get { return this.driver.FindElement(By.LinkText("Payroll")); } }
        public IWebElement PayrollFormsButton { get { return this.driver.FindElement(By.LinkText("Forms")); } }
        public IWebElement PayrollFormsTable { get { return this.driver.FindElement(By.Id("body_gvForms")); } }
        public IWebElement CustomizeColumnsButton { get { return driver.FindElement(By.Id("anchorColumnOptions")); } }
        private IWebElement CreateNewForm { get { return driver.FindElement(By.Id("body_hlCreateNew")); } }

        ////PayrollForms Page Search Fields
        public override IWebElement CenterDropDown => driver.FindElement(By.Id("body_ddSygmaCenterNo"));
        public IWebElement WeekendingDateSearch { get { return driver.FindElement(By.Id("body_dteWeekending_txtDate")); } }
        public IWebElement PayrollFormIdSearch { get { return driver.FindElement(By.Id("body_txtFormId")); } }
       
        ////Customize Columns Functionality
        public IWebElement AvailableColumnsList { get { return this.driver.FindElement(By.Id("lstBoxAvailableColumns")); } }
        public IWebElement SelectedColumnsList { get { return this.driver.FindElement(By.Id("lstBoxSelectedColumns")); } }
        //Select or Deselect a column from the available columns list
        public IWebElement ColumnOptionsPopupWindow { get { return this.driver.FindElement(By.Id("columnOptionsDialog")); } }

        public IWebElement AddColumn { get { return this.driver.FindElement(By.Id("body_ColumnOptionsData_btnSelectAvailableColumn")); } }
        public IWebElement AddAllColumns { get { return this.driver.FindElement(By.Id("body_ColumnOptionsData_btnSelectAllAvailableColumn")); } }
        public IWebElement RemoveColumn { get { return this.driver.FindElement(By.Id("body_ColumnOptionsData_btnUnSelectSelectedColumn")); } }
        public IWebElement SelectedColumnWidth { get { return this.driver.FindElement(By.Id("txtSelectedColumnWidth")); } }

        //Move A column in the selected columns list up or down in order
        public IWebElement MoveSelectedColumnUp { get { return this.driver.FindElement(By.Id("body_ColumnOptionsData_btnSelectedColumnMoveUp")); } }
        public IWebElement MoveSelectedColumDown { get { return this.driver.FindElement(By.Id("body_ColumnOptionsData_btnSelectedColumnMoveDown")); } }
        public IWebElement SaveColumnSelection { get { return this.driver.FindElement(By.Id("columnOptionsDialogOk")); } }
        public IWebElement CancelColumnSelection { get { return this.driver.FindElement(By.Id("columnOptionsDialogCancel")); } }


    }
}
