using System.Windows.Forms;

namespace Common.Methods
{
    public static class MDI
    {
        public static void SetMDIChild(Form sMDIChild, Form sMDIParent)
        {
            sMDIChild.MdiParent = sMDIParent;
            sMDIChild.Show();
            sMDIChild.Activate();
        }
    }
}
