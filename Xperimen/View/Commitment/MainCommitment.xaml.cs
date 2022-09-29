using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xperimen.ViewModel.Commitment;

namespace Xperimen.View.Commitment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainCommitment : ContentPage
    {
        public CommitmentViewmodel viewmodel;

        public MainCommitment()
        {
            InitializeComponent();
            viewmodel = new CommitmentViewmodel();
            BindingContext = viewmodel;
            SetupView();

            MessagingCenter.Subscribe<AddRecord>(this, "CommitmentAdded", (sender) => { SetupView(); });
            MessagingCenter.Subscribe<Details>(this, "CommitmentUpdated", (sender) => { SetupView(); });
        }

        public void SetupView()
        {
            var result = viewmodel.GetCommitmentList();
            if (result == 2) SetDisplayAlert("Error", "Technical error retrieving commitment list.", "", "", "");
        }

        public async void EditIncomeTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            stack_income.IsVisible = false;
            stack_editincome.IsVisible = true;
        }

        public async void SaveIncomeTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;

            var result = viewmodel.SaveIncome();
            if (result == 2) SetDisplayAlert("Error", "Technical error saving income amount.", "", "", "");
            viewmodel.GetCommitmentList();
            stack_income.IsVisible = true;
            stack_editincome.IsVisible = false;
        }

        public async void ItemCommitmentTapped(object sender, EventArgs e)
        {
            var view = (StackLayout)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            var grid = (Grid)view.Children[0];
            var stack = (StackLayout)grid.Children[0];
            var lbl_id = (Label)stack.Children[2];
            await Navigation.PushAsync(new Details(lbl_id.Text));
        }

        public async void BackTapped(object sender, EventArgs e)
        {
            var view = (Image)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PopAsync();
        }

        public async void AddCommitmentClicked(object sender, EventArgs e)
        {
            var view = (Frame)sender;
            await view.ScaleTo(0.9, 100);
            view.Scale = 1;
            await Navigation.PushAsync(new AddRecord());
        }

        public void SetDisplayAlert(string title, string description, string btn1, string btn2, string obj)
        {
            //if string1 empty will not display btn1, if string2 empty will not display btn2
            //if both string1 & string2 empty will not display all buttons
            //all buttons tapped will send 'DisplayAlertSelection' with text of the button
            //close button tapped will send 'DisplayAlertSelection' with empty text
            alert.Title = title;
            alert.Description = description;
            alert.TxtBtn1 = btn1;
            alert.TxtBtn2 = btn2;
            alert.IsVisible = true;
            alert.CodeObject = obj;
        }
    }
}