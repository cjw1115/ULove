using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ULove.Controls
{
    public sealed class NotificationBar : Control
    {
        private TextBlock notifyBlock;
        private Grid mainGrid;
        private Storyboard storyBoard;

        public NotificationBar()
        {
            this.DefaultStyleKey = typeof(NotificationBar);
        }
        private void GetTextBlockControl()
        {
            if (this.notifyBlock == null)
            {
                this.notifyBlock = this.GetTemplateChild("tbMsg") as TextBlock;
            }
        }
        private void GetStoryBoardControl(string name)
        {
            if (this.storyBoard == null)
            {
                this.storyBoard = this.GetTemplateChild(name) as Storyboard;
            }
        }

        public void ShowMessage(string message)
        {
            GetTextBlockControl();
            GetStoryBoardControl("Storyboard1");
            if (notifyBlock != null && storyBoard != null)
            {
                notifyBlock.Text = message;
                storyBoard.Begin();
            }
        }
    }
}
