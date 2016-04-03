using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ULove.Controls
{
    public class GridViewEx : DependencyObject
    {
        public static readonly DependencyProperty OpenCommandProperty = DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(GridViewEx), new PropertyMetadata(null,
            (o, e) =>
            {
                var gridview = o as GridView;
                gridview.ItemClick += Gridview_ItemClick;
            }));
        private static void Gridview_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView gridview = sender as GridView;
            var command = GetOpenCommand(gridview);
            command.Execute(e.ClickedItem);
        }

        public static ICommand GetOpenCommand(DependencyObject o)
        {
            return (ICommand)o.GetValue(OpenCommandProperty);

        }
        public static void SetOpenCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(OpenCommandProperty, value);
        }
    }
}
