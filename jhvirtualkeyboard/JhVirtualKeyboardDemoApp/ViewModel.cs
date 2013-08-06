// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboardDemoApp</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using JhLib;


namespace JhVirtualKeyboardDemoApp
{
    public class ViewModel : BaseViewModel
    {
        public ViewModel()
        {
            IsEnabled = true;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    Notify("IsEnabled");
                    if (value)
                    {
                        ToolTipForTextBox = "This TextBox is enabled";
                        ToolTipForRichTextBox = "This RichTextBox is enabled";
                    }
                    else
                    {
                        ToolTipForTextBox = "This TextBox is not enabled";
                        ToolTipForRichTextBox = "This RichTextBox not is enabled";
                    }
                }
            }
        }

        public string ToolTipForTextBox
        {
            get { return _toolTipForTextBox; }
            set
            {
                if (value != _toolTipForTextBox)
                {
                    _toolTipForTextBox = value;
                    Notify("ToolTipForTextBox");
                }
            }
        }

        public string ToolTipForRichTextBox
        {
            get { return _toolTipForRichTextBox; }
            set
            {
                if (value != _toolTipForRichTextBox)
                {
                    _toolTipForRichTextBox = value;
                    Notify("ToolTipForRichTextBox");
                }
            }
        }

        private bool _isEnabled;
        private string _toolTipForTextBox;
        private string _toolTipForRichTextBox;
    }
}
