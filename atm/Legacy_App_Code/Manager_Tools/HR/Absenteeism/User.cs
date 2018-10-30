using System;

namespace SygmaIntranet.Manager_Tools.HR.Absenteeism
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	public class User
	{
		public enum UserAttribute:short{
			UserId, DisplayName, IsAdmin, EditableWeeks
		}
		int gUserId;
		string gUserName, gDisplayName;
		bool gIsAdmin;
		int gEditableWeeks;
		bool gIsValidUser;

		public User(System.Web.HttpContext context){
			System.Web.HttpRequest request = context.Request;
			gUserName = WebCommon.GetUserName(context);
			//if (gUserName.ToLower() == "bfking")
			//  if (request.QueryString["user"] != null)
			//    gUserName = request.QueryString.Get("user");
			SetUserInfo();
		}
		public User(string fullUserName){
			gUserName = WebCommon.GetUserName(fullUserName);
			SetUserInfo();
		}
		private void SetUserInfo(){
			SygmaFramework.Database db = new SygmaFramework.Database("Absenteeism");
			System.Data.DataView dv = db.GetDataView("up_GetUserInfo", new object[]{gUserName});	
			if (dv.Count > 0){
				System.Data.DataRowView drv = dv[0];
				gUserId = Convert.ToInt32(drv[(short)UserAttribute.UserId]);
				gDisplayName = drv[(short)UserAttribute.DisplayName].ToString();
				gIsAdmin = Convert.ToBoolean(drv[(short)UserAttribute.IsAdmin]);
				gEditableWeeks = Convert.ToInt16(drv[(short)UserAttribute.EditableWeeks]);
				gIsValidUser = true;
			}else{
				gIsValidUser = false;
			}
		}
		public int ID{
			get{return gUserId;}
		}
		public string Name{
			get{return gUserName;}
		}
		public string DisplayName{
			get{return gDisplayName;}
		}
		public bool IsAdmin{
			get{return gIsAdmin;}
		}
		public int EditableWeeks{
			get{return gEditableWeeks;}
		}
		public bool IsValidUser{
			get{return gIsValidUser;}
		}
	}
}
