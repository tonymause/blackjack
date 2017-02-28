using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using BlackJack.Model;
using BlackJack.Model.Command;
using BlackJack.Model.Entity;
using BlackJack.Model.Enum;

namespace BlackJack.ViewModel
{
    public class BlackJackViewModel : ViewModelBase
    {
        #region Private Field

        private const int CNT_FIRST_DIS = 2;
        private const int TWENTY_ONE = 21;
        private const int TEN = 10;
        private const int DEALER_TOTAL_MAX = 16;

        private const int SCORE_10 = 10;
        private const int SCORE_15 = 15;

        private const int SUIT_COUNT_MIN = 15;

        private BlackJackModel _model;
        private readonly Random _random = new Random();
        
        private int _decks;
        private int _score;
        private string _result;
        private int _dockerCount;
        private bool _inGame;
        private bool _started;

        private int _total_dealer;
        private int _total_player;

        private ObservableCollection<CardEntity> _cardsOnDeck;
        private ObservableCollection<CardEntity> _cardsOffDeck;

        private ObservableCollection<BlackJackModel> _cardsPlayer;
        private ObservableCollection<BlackJackModel> _cardsDealer;

        #endregion

        #region Ctor

        public BlackJackViewModel()
        {
            // Initial whole Cards
            CardsOnDeck = new ObservableCollection<CardEntity>();
            CardsOnDeck.CollectionChanged += CardsOnDeck_CollectionChanged;
            CardsDealer = new ObservableCollection<BlackJackModel>();
            CardsDealer.CollectionChanged += CardsDealer_CollectionChanged;
            CardsPlayer = new ObservableCollection<BlackJackModel>();
            CardsPlayer.CollectionChanged += CardsPlayer_CollectionChanged;

            DecksList = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

            // Deck
            _decks = 1;

            Init();
        }

        #endregion

        #region Property

        public int Decks
        {
            get { return _decks; }
            set
            {
                _decks = value;
                ResetDeck();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> DecksList { get; private set; }

        /// <summary>
        /// Player Score displayed at UI
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        public int Total_Player
        {
            get { return _total_player; }
            set
            {
                _total_player = value;
                OnPropertyChanged();
            }
        }

        public int Total_Dealer
        {
            get { return _total_dealer; }
            set
            {
                _total_dealer = value;
                OnPropertyChanged();
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Cards count on deck
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public int DockerCount
        {
            get { return _dockerCount; }
            set
            {
                _dockerCount = value;
                OnPropertyChanged();
            }
        }

        public bool InGame
        {
            get { return _inGame; }
            set
            {
                _inGame = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// To indicate game started or not
        /// </summary>
        public bool Started
        {
            get { return _started; }
            set
            {
                _started = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Remainding Cards on deck
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public ObservableCollection<CardEntity> CardsOnDeck
        {
            get { return _cardsOnDeck; }
            set
            {
                _cardsOnDeck = value;
                DockerCount = CardsOnDeck.Count;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Cards held by Dealer
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public ObservableCollection<BlackJackModel> CardsDealer
        {
            get { return _cardsDealer; }
            set
            {
                _cardsDealer = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Cards held by Player
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public ObservableCollection<BlackJackModel> CardsPlayer
        {
            get { return _cardsPlayer; }
            set
            {
                _cardsPlayer = value;
                OnPropertyChanged();
                CalculateCardsTotalValue(CardsPlayer);
            }
        }

        #endregion

        #region ICommand

        public ICommand ResetCommand => new DelegateCommand(Reset);
        public ICommand HitCommand => new DelegateCommand(Hit);
        public ICommand StayCommand => new DelegateCommand(Stay);
        public ICommand StartCommand => new DelegateCommand(Start);

        #endregion

        #region Private Method

        private void CardsPlayer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Total_Player = CalculateCardsTotalValue(CardsPlayer);
        }

        private void CardsDealer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Total_Dealer = CalculateCardsTotalValue(CardsDealer);
        }

        private void CardsOnDeck_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DockerCount = CardsOnDeck.Count;
        }

        private void Reset()
        {
            Init();
        }

        /// <summary>
        /// Player to pick one suit
        /// </summary>
        private void Hit()
        {
            Dispatch(UserType.Player);

            // Calculate total
            Total_Player = CalculateCardsTotalValue(CardsPlayer);

            if (Total_Player > TWENTY_ONE)
            {
                // Player lose for this match
                HandleLoseProcess();
                PrepareNewMatch();
            }
        }

        /// <summary>
        /// Dealer to pick suit one by one
        /// </summary>
        private void Stay()
        {
            if (Total_Player == TWENTY_ONE)
            {
                HandleWinProcess(SCORE_15);
            }
            else
            {
                while (CalculateCardsTotalValue(CardsDealer) <= DEALER_TOTAL_MAX)
                    Dispatch(UserType.Dealer);

                Total_Dealer = CalculateCardsTotalValue(CardsDealer);

                if (Total_Dealer > TWENTY_ONE)
                {
                    // Dealer lost for this match
                    HandleWinProcess(SCORE_10);
                }
                else
                {
                    // Compare with Player
                    if (Total_Dealer > Total_Player)
                    {
                        HandleLoseProcess();
                    }
                    else if (Total_Dealer < Total_Player)
                    {
                        HandleWinProcess(SCORE_10);
                    }
                    else
                    {
                        HandleDualProcess();
                    }
                }
            }

            PrepareNewMatch();
        }

        private void HandleDualProcess()
        {
            Result = "Dual";
        }

        private void HandleWinProcess(int score)
        {
            Result = "Win";
            Score += score;
        }

        private void HandleLoseProcess()
        {
            Result = "Lose";
            Score -= SCORE_10;
        }

        private void PrepareNewMatch()
        {
            Started = false;
        }


        /// <summary>
        /// Calculate cards total value
        /// </summary>
        /// <param name="cardsList"></param>
        /// <returns></returns>
        private int CalculateCardsTotalValue(IEnumerable<BlackJackModel> cardsList)
        {
            var total = 0;
            foreach (var card in cardsList)
            {
                if (card.Card.Number > TEN)
                    total += TEN;
                else
                    total += card.Card.Number;
            }

            return total;
        }

        /// <summary>
        /// Start new match during current game
        /// </summary>
        private void Start()
        {
            InGame = true;
            Started = true;
            Result = string.Empty;

            foreach (var card in CardsPlayer)
            {
                _cardsOffDeck.Add(card.Card);
            }
            foreach (var card in CardsDealer)
            {
                _cardsOffDeck.Add(card.Card);
            }

            Total_Dealer = 0;
            Total_Player = 0;
            CardsDealer.Clear();
            CardsPlayer.Clear();

            // Give 2 Cards to dealer and player, 
            // For dealer, the first card should be hidden
            for (var i = 0; i < CNT_FIRST_DIS; i++)
            {
                Dispatch(UserType.Dealer);
                Dispatch(UserType.Player);
            }
        }

        private void Dispatch(UserType userType)
        {
            // Option1: Reshuffle the cards whenever there are fewer than fifteen cards remaining in the deck.
            if (CardsOnDeck.Count < SUIT_COUNT_MIN)
                ShuffleDeck();

            var i = _random.Next(0, CardsOnDeck.Count);
            var card = CardsOnDeck[i];

            CardsOnDeck.Remove(card);
            if (userType == UserType.Dealer)
                CardsDealer.Add(new BlackJackModel(CardsDealer.Count, card, userType));
            else
                CardsPlayer.Add(new BlackJackModel(CardsPlayer.Count, card, userType));
        }

        /// <summary>
        /// Application init or reset button clicked
        /// </summary>
        private void Init()
        {
            // Player score reset
            Score = 0;

            InGame = false;
            Started = false;
            Result = string.Empty;
            
            CardsDealer.Clear();
            CardsPlayer.Clear();

            _cardsOffDeck = new ObservableCollection<CardEntity>();

            ResetDeck();
        }

        private void ResetDeck()
        {
            CardsOnDeck.Clear();
            for (var j = 0; j < Decks; j++)
                foreach (CardType type in Enum.GetValues(typeof(CardType)))
                {
                    for (var i = 0; i < 13; i++)
                    {
                        CardsOnDeck.Add(new CardEntity(type, i + 1));
                    }
                }
        }

        private void ShuffleDeck()
        {
            CardsOnDeck = CardsOnDeck.Union(_cardsOffDeck).ToObservableCollection();
        }

        #endregion
    }
}
