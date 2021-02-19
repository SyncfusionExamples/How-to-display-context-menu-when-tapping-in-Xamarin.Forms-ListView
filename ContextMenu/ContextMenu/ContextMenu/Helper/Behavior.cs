using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SfListViewSample
{
    public class Behavior : Behavior<SfListView>
    {
        SfListView ListView;
        int sortorder = 0;
        Contacts item;
        SfPopupLayout popupLayout;
        protected override void OnAttachedTo(SfListView listView)
        {
            ListView = listView;
            ListView.ItemHolding += ListView_ItemHolding;
            ListView.ScrollStateChanged += ListView_ScrollStateChanged;
            ListView.ItemTapped += ListView_ItemTapped;
            base.OnAttachedTo(listView);
        }

        private void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }
        }

        private void ListView_ScrollStateChanged(object sender, ScrollStateChangedEventArgs e)
        {
            if (popupLayout != null)
            {
                popupLayout.Dismiss();
            }
        }

        private void ListView_ItemHolding(object sender, ItemHoldingEventArgs e)
        {
            item = e.ItemData as Contacts;
            popupLayout = new SfPopupLayout();
            popupLayout.PopupView.HeightRequest = 100;
            popupLayout.PopupView.WidthRequest = 100;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();
                mainStack.BackgroundColor = Color.Teal;

                var deletedButton = new Button()
                {
                    Text = "Delete",
                    HeightRequest=50,
                    BackgroundColor=Color.Teal,
                    TextColor = Color.White
                };
                deletedButton.Clicked += DeletedButton_Clicked;
                var sortButton = new Button()
                {
                    Text = "Sort",
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor=Color.White
                };
                sortButton.Clicked += SortButton_Clicked;
                mainStack.Children.Add(deletedButton);
                mainStack.Children.Add(sortButton);
                return mainStack;

            });
            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.ShowFooter = false;
            if (e.Position.Y + 100 <= ListView.Height && e.Position.X + 100 > ListView.Width)
                popupLayout.Show((double)(e.Position.X - 100), (double)(e.Position.Y));
            else if (e.Position.Y + 100 > ListView.Height && e.Position.X + 100 < ListView.Width)
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y - 100));
            else if (e.Position.Y + 100 > ListView.Height && e.Position.X + 100 > ListView.Width)
                popupLayout.Show((double)(e.Position.X - 100), (double)(e.Position.Y - 100));
            else
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y));

        }

        private void SortButton_Clicked(object sender, EventArgs e)
        {
            if (ListView == null)
                return;

            ListView.DataSource.SortDescriptors.Clear();
            popupLayout.Dismiss();
            ListView.DataSource.LiveDataUpdateMode = LiveDataUpdateMode.AllowDataShaping;
            if (sortorder == 0)
            {
                ListView.DataSource.SortDescriptors.Add(new SortDescriptor { PropertyName = "ContactName", Direction = ListSortDirection.Descending });
                sortorder = 1;
            }
            else
            {
                ListView.DataSource.SortDescriptors.Add(new SortDescriptor { PropertyName = "ContactName", Direction = ListSortDirection.Ascending });
                sortorder = 0;
            }
        }

        private void Dismiss()
        {
            popupLayout.IsVisible = false;
        }

        private void DeletedButton_Clicked(object sender, EventArgs e)
        {
            
            if (ListView == null)
                return;

            var source = ListView.ItemsSource as IList;

            if (source != null && source.Contains(item))
                source.Remove(item);
            else
                App.Current.MainPage.DisplayAlert("Alert", "Unable to delete the item", "OK");

            item = null;
            source = null;
        }
    }
}
