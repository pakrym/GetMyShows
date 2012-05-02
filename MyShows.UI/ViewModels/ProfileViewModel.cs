using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MyShows.Core;

namespace MyShows.UI.ViewModels
{
    public class ProfileViewModel
    {
        private DataContext _dataContext;

        public ProfileViewModel()
        {
            _dataContext = new DataContext();
            Series = new BindableCollection<SeriesViewModel>(_dataContext.GetUnwatchedSeries(0).Select(s => new SeriesViewModel(s)));

        }

        public BindableCollection<SeriesViewModel> Series { get; set; }
    }

}