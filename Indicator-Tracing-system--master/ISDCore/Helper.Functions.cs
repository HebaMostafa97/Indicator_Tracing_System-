using System.Text.RegularExpressions;

namespace ISDCore
{
  public static class Functions
  {
    public const string FunctionName_Browse = "Browse";
    public const string FunctionName_Add = "Add";
    public const string FunctionName_Edit = "Edit";
    public const string FunctionName_Delete = "Delete";
    public const string FunctionName_Preview = "Preview";
    public const string FunctionName_PreviewAttachment = "PreviewAttachment";
    public const string FunctionName_New = "New";
    public const string FunctionName_BrowseAllDepartments = "BrowseAllDepartments";

    public const string FunctionName_BrowseReport = "BrowseReport";

    public const string FunctionName_Activate = "Activate";
    public const string FunctionName_Bookmark = "Bookmark";
    public const string FunctionName_Focus = "Focus";
    public const string FunctionName_Done = "Done";
    public const string FunctionName_Permission = "Permission";
    public const string FunctionName_LinkGroup = "LinkGroup";

    public const string FunctionName_Save = "Save";
    public const string FunctionName_SaveAdd = "SaveAdd";
    public const string FunctionName_SaveEdit = "SaveEdit";
    public const string FunctionName_Reset = "Reset";
    public const string FunctionName_Cancel = "Cancel";
    public const string FunctionName_LinkUserClassification = "LinkUserClassification";
    public const string FunctionName_LinkParent = "LinkParent";
    public const string FunctionName_Approve = "Approve";
    public const string FunctionName_Reject = "Reject";
    //public const string FunctionName_Interview = "Interview";
    //public const string FunctionName_CarryOver = "CarryOver";

    // updated on 5/11/2014
    public const string FunctionName_AttachFile = "AttachFile";
    public const string FunctionName_LinkAttachment = "LinkAttachment";
    public static string[] ActivateColor = new string[] { "red", "green" };
    public static bool ValidatePassword(string password)
    {
      var hasNumber = new Regex(@"[0-9]+");
      var hasUpperChar = new Regex(@"[A-Z]+");
      var hasMinimum8Chars = new Regex(@".{8,}");
      var isValidated = hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
      return isValidated;
    }

  }
}
