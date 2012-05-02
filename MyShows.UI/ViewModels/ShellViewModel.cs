using System.Collections.Generic;
using Caliburn.Micro;

namespace MyShows.UI.ViewModels
{
    using System.ComponentModel.Composition;

    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<object>, IShell
    {
        private Stack<object> _screens = new Stack<object>();
        private ProfileViewModel _profileViewModel;

        public ShellViewModel()
        {
            _profileViewModel = new ProfileViewModel();
            GoTo(_profileViewModel);
        }

        public void GoBack()
        {
            if (_screens.Count == 1)
            {
                return;
            }
            else
            {
                _screens.Pop();
                ActivateItem(_screens.Peek());
            }
            
        }

        public void GoTo(object screen)
        {
            _screens.Push(screen);
            ActivateItem(screen);
        }

        public void SelectSeries(SeriesViewModel series)
        {
            GoTo(series);
        }

        public void SelectEpisode(EpisodeViewModel episode)
        {

            GoTo(episode);
        }

    }
}
