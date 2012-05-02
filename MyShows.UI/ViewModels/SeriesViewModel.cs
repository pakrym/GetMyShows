using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using MyShows.Core;

namespace MyShows.UI.ViewModels
{
    [Export(typeof(SeriesViewModel))]
    public class SeriesViewModel : PropertyChangedBase
    {
        private readonly Series _series;

        public SeriesViewModel(Series series)
        {
            _series = series;
            Episodes =
                new BindableCollection<EpisodeViewModel>(
                    series.Episodes.Where(e => !e.Watched).Select(e => new EpisodeViewModel(e)).OrderBy(e => e.CodedName));
        }

        public BindableCollection<EpisodeViewModel> Episodes { get;private set; }

        public string Title
        {
            get { return _series.Title; }
        }


        public byte[] Image
        {
            get { return _series.Image; }
        }

        public Series Model
        {
            get {
                return _series;
            }
        }

    }
}