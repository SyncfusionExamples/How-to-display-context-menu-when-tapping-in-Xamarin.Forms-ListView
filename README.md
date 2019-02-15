# How-to-display-context-menu-when-tapping-in-Xamarin.Forms-ListView?

This example demonstrates how to display a pop-up menu with different menu items to an item when it is long pressed by customizing the SfListView and by using custom view in it. For UWP platform, you can also display the pop-up menu when pressing right click button over the item. Display the pop-up menu by accessed the touch position in the item based on [Position](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.ItemHoldingEventArgs~Position.html) property from [ItemHolding](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.SfListView~ItemHolding_EV.html) event.

Defining SfPopUpView

```
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
            popupLayout.Dismiss();
        }

        private void ListView_ScrollStateChanged(object sender, ScrollStateChangedEventArgs e)
        {
            popupLayout.Dismiss();
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

```

Defining the SfListView

```
<ContentPage xmlns:syncfusion="clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms">
  <ContentPage.BindingContext>
    <local:ContactsViewModel x:Name="viewModel"/>
  </ContentPage.BindingContext>
    <ContentPage.Content>
        <Grid>
            <listView:SfListView x:Name="listView" ItemsSource="{Binding Items}" >
                <listView:SfListView.ItemTemplate>
                    <DataTemplate>
                        <Grid>
                            <Image Source="{Binding ContactImage}"/>
                            <Label Text="{Binding ContactName}" />
                            <Label Text="{Binding ContactNumber}" />
                        </Grid>
                    </DataTemplate>
                </listView:SfListView.ItemTemplate>
            <listView:SfListView>
        </Grid>
    </ContentPage.Content>
</ContentPage>

```
## Requirements to run the demo

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) or [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
* Xamarin add-ons for Visual Studio (available via the Visual Studio installer).

## Troubleshooting

### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.
