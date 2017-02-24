using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BlackJack.Model;

namespace BlackJack.ViewModel
{
    public class BlackJackViewModel : ViewModelBase
    {
        private string _someText;
        private readonly ObservableCollection<string> _history
            = new ObservableCollection<string>();
        private BlackJackModel _model;

        public BlackJackViewModel()
        {
            Model = new BlackJackModel();
        }
        #region Property

        // ReSharper disable once MemberCanBePrivate.Global
        public BlackJackModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public string SomeText
        {
            get { return _someText; }
            set
            {
                _someText = value;
                OnPropertyChanged();
            }
        }

        // Readonly History List
        public IEnumerable<string> History => _history;

        #endregion

        #region ICommand

        public ICommand ConvertTextCommand => new DelegateCommand(ConvertText);

        #endregion

        #region Private Method

        private void ConvertText()
        {
            if (string.IsNullOrWhiteSpace(SomeText)) return;
            AddToHistory(SomeText);
            Model.Title = SomeText;
            SomeText = string.Empty;
        }

        private void AddToHistory(string item)
        {
            if (!_history.Contains(item))
                _history.Add(item);
        }

        #endregion
    }
}
