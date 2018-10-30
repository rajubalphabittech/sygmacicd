using System;

namespace atm
{
	/// <summary>
	/// Summary description for ATMPage
	/// </summary>
	public abstract class ATMPage : BasePage {
		protected abstract void LoadATMPage();

		protected override void LoadBasePage() {
			//check that the user has access to the page
			if (!Convert.ToBoolean(ATMDB.GetScalar("up_sec_isValidUser", UserName, Request.AppRelativeCurrentExecutionFilePath))) {
                
                //Since the user doesn't have access to desire page, route them to home controller 
                //home controller will check what page they have access to and handle the routing appropriatelly
                routeToHomeController();

			} else {
				LoadATMPage();
			}
		}
	}
}
