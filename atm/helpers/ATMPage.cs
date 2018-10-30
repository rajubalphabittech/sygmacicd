using System;


namespace atm.web
{
    /// <summary>
    /// Summary description for ATMPage
    /// </summary>
    public abstract class ATMPage : BasePage
    {
        protected abstract void LoadATMPage();

        protected override void LoadBasePage()
        {
            //check that the user has access to the page
            if (!Convert.ToBoolean(ATMDB.GetScalar("up_sec_isValidUser", UserName,
                Request.AppRelativeCurrentExecutionFilePath)))
            {
                GotoInvalidUserPage();
            }
            else
            {
                LoadATMPage();
            }
        }

        public abstract class ATMPageView : BasePageView
        {
            protected abstract void LoadATMPage();

            protected override void LoadBasePage()
            {
                //check that the user has access to the page
                if (!Convert.ToBoolean(ATMDB.GetScalar("up_sec_isValidUser", UserName,
                    Request.AppRelativeCurrentExecutionFilePath)))
                {
                    GotoInvalidUserPage();
                }
                else
                {
                    LoadATMPage();
                }
            }
        }
    }
}
