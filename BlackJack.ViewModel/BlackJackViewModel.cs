﻿using System;
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
        private const int CNT_FIRST_DIS = 2;
        private const int TWENTY_ONE = 21;
        private const int TEN = 10;
        private const int DEALER_TOTAL_MAX = 16;

        private const int SCORE_10 = 10;
        private const int SCORE_15 = 15;

        private const int SUIT_COUNT_MIN = 15;

        private BlackJackModel _model;

        private int _score;
        private int _dockerCount;
        private bool _started;

        private int _total_dealer = 0;
        private int _total_player = 0;

        private ObservableCollection<CardEntity> _cardsOnDeck;

        private ObservableCollection<CardEntity> _cardsPlayer;
        private ObservableCollection<CardEntity> _cardsDealer;

        #region Ctor

        public BlackJackViewModel()
        {
            // Initial whole Cards
            CardsOnDeck = new ObservableCollection<CardEntity>();
            CardsOnDeck.CollectionChanged += CardsOnDeck_CollectionChanged;
            CardsDealer = new ObservableCollection<CardEntity>();
            CardsDealer.CollectionChanged += CardsDealer_CollectionChanged;
            CardsPlayer = new ObservableCollection<CardEntity>();
            CardsPlayer.CollectionChanged += CardsPlayer_CollectionChanged;

            Init();
        }

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

        #endregion


        #region Property

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
        public ObservableCollection<CardEntity> CardsDealer
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
        public ObservableCollection<CardEntity> CardsPlayer
        {
            get { return _cardsPlayer; }
            set
            {
                _cardsPlayer = value;
                OnPropertyChanged();
                CalculateCardsTotalValue(CardsPlayer);
            }
        }


        // ReSharper disable once MemberCanBePrivate.Global
        public BlackJackModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged();
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

        private void Reset()
        {
            Init();
        }

        /// <summary>
        /// Player to pick one suit
        /// </summary>
        private void Hit()
        {
            Dispatch(CardsPlayer);

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
            while (CalculateCardsTotalValue(CardsDealer) <= DEALER_TOTAL_MAX)
                Dispatch(CardsDealer);

            Total_Dealer = CalculateCardsTotalValue(CardsDealer);

            if(Total_Dealer > TWENTY_ONE)
            {
                // Dealer lost for this match
                HandleWinProcess();
            }
            else
            {
                // Compare with Player
                if(Total_Dealer > Total_Player)
                {
                    HandleLoseProcess();  
                }
                else if(Total_Dealer < Total_Player)
                {
                    HandleWinProcess();
                }
                else
                {
                    HandleDualProcess();
                }
            }
            PrepareNewMatch();
        }

        private void HandleDualProcess()
        {
            
        }

        private void HandleWinProcess()
        {
            Score += SCORE_10;
        }

        private void HandleLoseProcess()
        {
            Score -= SCORE_10;
        }

        private void PrepareNewMatch()
        {
            Started = false;

            // Check remainding suits count less than SUIT_COUNT_MIN
            if (CardsOnDeck.Count < SUIT_COUNT_MIN)
                ResetDeck();
        }


        /// <summary>
        /// Calculate cards total value
        /// </summary>
        /// <param name="cardsList"></param>
        /// <returns></returns>
        private int CalculateCardsTotalValue(IEnumerable<CardEntity> cardsList)
        {
            var total = 0;
            foreach (var card in cardsList)
            {
                if (card.Number > TEN)
                    total += TEN;
                else
                    total += card.Number;
            }

            return total;
        }

        /// <summary>
        /// Start new match during current game
        /// </summary>
        private void Start()
        {
            Started = true;

            Total_Dealer = 0;
            Total_Player = 0;
            CardsDealer.Clear();
            CardsPlayer.Clear();

            // Give 2 Cards to dealer and player, 
            // For dealer, the first card should be hidden
            for (var i = 0; i < CNT_FIRST_DIS; i++)
            {
                Dispatch(CardsDealer);
                Dispatch(CardsPlayer);
            }
        }

        private void Dispatch(ICollection<CardEntity> list)
        {
            var random = new Random();
            var i = random.Next(0, CardsOnDeck.Count);
            var card = CardsOnDeck[i];

            CardsOnDeck.Remove(card);
            list.Add(card);
        }

        private void Init()
        {
            // Player score reset
            Score = 0;

            // Game status reset
            Started = false;

            Model = new BlackJackModel();
            CardsDealer.Clear();
            CardsPlayer.Clear();

            ResetDeck();
        }

        private void ResetDeck()
        {
            CardsOnDeck.Clear();
            foreach (CardType type in Enum.GetValues(typeof(CardType)))
            {
                for (var i = 0; i < 13; i++)
                {
                    CardsOnDeck.Add(new CardEntity(type, i + 1));
                }
            }
        }

        #endregion
    }
}
