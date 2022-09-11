using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xperimen.Model;

namespace Xperimen.ViewModel.Dashboard
{
    public class DetailspageViewmodel : BaseViewModel
    {
        public List<MobileApp> ListItems { get; set; }
        public ICommand LoadDataCommand { get; set; }

        public DetailspageViewmodel()
        {
            ListItems = new List<MobileApp>();
            LoadDataCommand = new Command(SetupView);
            SetupView();
        }

        public void SetupView()
        {
            try
            {
                IsLoading = true;
                ListItems.Add(new MobileApp { Id = Guid.NewGuid().ToString().Substring(0, 5), AppName = "Applab", AppSize = "35.4 MB", Description = "electrical plants is about 1,100,000 megawatts.The Sierra Club says the campaign has already prevented “100,792 asthma attacks, 9,420 heart attacks and 6,097 premature deaths” every year." });
                ListItems.Add(new MobileApp { Id = Guid.NewGuid().ToString().Substring(0, 5), AppName = "Skytrex", AppSize = "15.9 MB", Description = "The White House says the president had hoped to pay more attention to a region with 1.3 billion potential customers for American goods, and a continent with bases that play a role in counterterrorism operations." });
                ListItems.Add(new MobileApp { Id = Guid.NewGuid().ToString().Substring(0, 5), AppName = "Enginex", AppSize = "53.3 MB", Description = "To give her an opportunity to catch up, our daughter was placed in a class with one of the top teachers in the school and in a reduced sized classroom (16 kids)." });
                ListItems.Add(new MobileApp { Id = Guid.NewGuid().ToString().Substring(0, 5), AppName = "Fplex", AppSize = "44.2 MB", Description = "At the inaugural Coucou8 event, Edmunds found that the Chinese men were low-key and passive, often staring at their phones rather than getting to know the women in the room." });
                ListItems.Add(new MobileApp { Id = Guid.NewGuid().ToString().Substring(0, 5), AppName = "Slyrak", AppSize = "27.1 MB", Description = "This appears to be in the Mission Bay area, not far from Sea World. It's not size that matters. Sometimes, reprieves make sense." });
                IsLoading = false;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var desc = ex.StackTrace;
            }
        }
    }
}
