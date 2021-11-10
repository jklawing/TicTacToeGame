using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using System;

namespace Tic_Tac_Toe_Game
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      #region Private Members

      /// <summary>
      /// Holds the current results of cells in the game
      /// </summary>
      private MarkType[] results;

      /// <summary>
      /// True if it is player 1's turn (X)
      /// </summary>
      private bool player1Turn;

      /// <summary>
      /// True if the game is ended
      /// </summary>
      private bool gameEnded;

      public bool vsCPU;

      public Random rnd = new Random();
      public int rndNumber;

      private Stack GameHistory { get; set; }

      #endregion

      #region Constructor

      /// <summary>
      /// Default constructor
      /// </summary>
      public MainWindow()
      {
         InitializeComponent();

         NewGame();

         GameHistory = new Stack();
      }

      #endregion

      /// <summary>
      /// Starts a new game
      /// </summary>
      private void NewGame()
      {
         // Create a new blank array of free cells
         results = new MarkType[9];

         for (var i = 0; i < results.Length; i++)
            results[i] = MarkType.Free;

         // Make sure player 1 starts the game
         player1Turn = true;

         // Iterate every button on the Container grid
         Container.Children.Cast<Button>().ToList().ForEach(button =>
         {
               // Change colors to default values
               button.Content = ' ';
            button.Background = Brushes.White;
            button.Foreground = Brushes.Black;
         });

         // Game hasn't finished
         gameEnded = false;

         if (CheckboxCPU.IsChecked == true)
            vsCPU = true;
         else
            vsCPU = false;
      }

      /// <summary>
      /// Button click event
      /// </summary>
      /// <param name="sender">Button clicked</param>
      /// <param name="e">Event of click</param>
      private void Button_Click(object sender, RoutedEventArgs e)
      {
         StoreBoardstate();

         // After game ended, start a new game with click
         if (gameEnded)
         {
            NewGame();
            return;
         }

         if (CheckboxCPU.IsChecked == true)
            vsCPU = true;
         else
            vsCPU = false;

         // Cast the sender to a button
         var button = (Button)sender;

         // Find the button position in array
         var column = Grid.GetColumn(button);
         var row = Grid.GetRow(button);

         var index = column + (row * 3);

         // Nothing happens if button has already been clicked
         if (results[index] != MarkType.Free)
            return;

         // Set value based on which players turn
         results[index] = player1Turn ? MarkType.Ex : MarkType.Oh;

         // Set button content to result
         button.Content = player1Turn ? "X" : "O";

         // Change Ex's to blue
         if (player1Turn)
            button.Foreground = Brushes.Blue;

         // Change Oh's to red
         if (!player1Turn)
            button.Foreground = Brushes.Red;

         // Check for winner
         CheckForWinner();

         // If playing vs CPU
         if (vsCPU == true && gameEnded == false)
         {
            StoreBoardstate();
            CPU_Opponent();
            player1Turn = true;
         }

         // If playing vs Human
         if (vsCPU == false && player1Turn == true)
                player1Turn = false;
         else if (vsCPU == false && player1Turn == false)
                 player1Turn = true;

         // Check for winner
         CheckForWinner();
      }

      private void SetBoardState(BoardState state)
      {
         NewGame();

         Button0_0.Content = state.zeroZero;
         Button1_0.Content = state.oneZero;
         Button2_0.Content = state.twoZero;
         Button0_1.Content = state.zeroOne;
         Button1_1.Content = state.oneOne;
         Button2_1.Content = state.twoOne;
         Button0_2.Content = state.zeroTwo;
         Button1_2.Content = state.oneTwo;
         Button2_2.Content = state.twoTwo;
         player1Turn = state.player1Turn;

         foreach (Button button in Container.Children)
         {
            if (button.Content.ToString().ToCharArray()[0] == 'O' && CheckboxCPU.IsChecked == false)
               button.Foreground = Brushes.Red;
            if (button.Content.ToString().ToCharArray()[0] == 'X')
               button.Foreground = Brushes.Blue;
         }
      }

      private void StoreBoardstate()
      
      {
         BoardState state = new BoardState
         {
            zeroZero = Button0_0.Content.ToString().ToCharArray()[0],
            oneZero = Button1_0.Content.ToString().ToCharArray()[0],
            twoZero = Button2_0.Content.ToString().ToCharArray()[0],
            zeroOne = Button0_1.Content.ToString().ToCharArray()[0],
            oneOne = Button1_1.Content.ToString().ToCharArray()[0],
            twoOne = Button2_1.Content.ToString().ToCharArray()[0],
            zeroTwo = Button0_2.Content.ToString().ToCharArray()[0],
            oneTwo = Button1_2.Content.ToString().ToCharArray()[0],
            twoTwo = Button2_2.Content.ToString().ToCharArray()[0],
            player1Turn = player1Turn
         };

         GameHistory.Push(state);
      }

      /// <summary>
      /// Checks if there is a winner
      /// </summary>
      private void CheckForWinner()
      {
         #region Horizontal Wins
         // Check for horizontal wins

         // Row 0
         if (results[0] != MarkType.Free && (results[0] & results[1] & results[2]) == results[0])
         {
            gameEnded = true;

            // Highlight winning cells
            Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Yellow;
         }

         // Row 1
         if (results[3] != MarkType.Free && (results[3] & results[4] & results[5]) == results[3])
         {
            gameEnded = true;

            // Highlight winning cells
            Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Yellow;
         }

         // Row 2
         if (results[6] != MarkType.Free && (results[6] & results[7] & results[8]) == results[6])
         {
            gameEnded = true;

            // Highlight winning cells
            Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Yellow;
         }
         #endregion

         #region Vertical Wins
         // Check for vertical wins

         // Column 0
         if (results[0] != MarkType.Free && (results[0] & results[3] & results[6]) == results[0])
         {
            gameEnded = true;

            // Highlight winning cells
            Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Yellow;
         }

         // Column 1
         if (results[1] != MarkType.Free && (results[1] & results[4] & results[7]) == results[1])
         {
            gameEnded = true;

            // Highlight winning cells
            Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Yellow;
         }

         // Column 2
         if (results[2] != MarkType.Free && (results[2] & results[5] & results[8]) == results[2])
         {
            gameEnded = true;

            // Highlight winning cells
            Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Yellow;
         }
         #endregion

         #region Diagonal Wins
         // Check for diagonal wins

         // Top left to Bottom right
         if (results[0] != MarkType.Free && (results[0] & results[4] & results[8]) == results[0])
         {
            gameEnded = true;

            // Highlight winning cells
            Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Yellow;
         }

         // Top right to Bottom left
         if (results[2] != MarkType.Free && (results[2] & results[4] & results[6]) == results[2])
         {
            gameEnded = true;

            // Highlight winning cells
            Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Yellow;
         }
         #endregion

         #region No Winner
         // Check for full board and no winner
         if (!results.Any(f => f == MarkType.Free))
         {
            gameEnded = true;

            // Turn marks green
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
               button.Foreground = Brushes.Green;
            });

         }
         #endregion

      }

      private void ButtonReset_Click(object sender, RoutedEventArgs e)
      {
         NewGame();
      }

      private void ButtonUndo_Click(object sender, RoutedEventArgs e)
      {
         if (GameHistory.Count > 0)
         {
            BoardState state = (BoardState)Convert.ChangeType(GameHistory.Pop(), typeof(BoardState));
            SetBoardState(state);
         }
      }

      private void CPU_Opponent()
      {

         #region Horizontal Checks
         // Row 0
         if (results[0] == MarkType.Free && results[1] == results[2] && results[1] != MarkType.Free)
         {
            Button0_0.Content = "O";
            results[0] = MarkType.Oh;
         }
         else if (results[1] == MarkType.Free && results[0] == results[2] && results[0] != MarkType.Free)
         {
            Button1_0.Content = "O";
            results[1] = MarkType.Oh;
         }
         else if (results[2] == MarkType.Free && results[0] == results[1] && results[0] != MarkType.Free)
         {
            Button2_0.Content = "O";
            results[2] = MarkType.Oh;
         }

         // Row 1
         else if (results[3] == MarkType.Free && results[4] == results[5] && results[4] != MarkType.Free)
         {
            Button0_1.Content = "O";
            results[3] = MarkType.Oh;
         }
         else if (results[4] == MarkType.Free && results[3] == results[5] && results[3] != MarkType.Free)
         {
            Button1_1.Content = "O";
            results[4] = MarkType.Oh;
         }
         else if (results[5] == MarkType.Free && results[3] == results[4] && results[3] != MarkType.Free)
         {
            Button2_1.Content = "O";
            results[5] = MarkType.Oh;
         }

         // Row 2
         else if (results[6] == MarkType.Free && results[7] == results[8] && results[7] != MarkType.Free)
         {
            Button0_2.Content = "O";
            results[6] = MarkType.Oh;
         }
         else if (results[7] == MarkType.Free && results[6] == results[8] && results[6] != MarkType.Free)
         {
            Button1_2.Content = "O";
            results[7] = MarkType.Oh;
         }
         else if (results[8] == MarkType.Free && results[6] == results[7] && results[6] != MarkType.Free)
         {
            Button2_2.Content = "O";
            results[8] = MarkType.Oh;
         }
         #endregion

         #region Vertical Checks
         // Column 0
         else if (results[0] == MarkType.Free && results[3] == results[6] && results[3] != MarkType.Free)
         {
            Button0_0.Content = "O";
            results[0] = MarkType.Oh;
         }
         else if (results[3] == MarkType.Free && results[0] == results[6] && results[0] != MarkType.Free)
         {
            Button0_1.Content = "O";
            results[3] = MarkType.Oh;
         }
         else if (results[6] == MarkType.Free && results[0] == results[3] && results[0] != MarkType.Free)
         {
            Button0_2.Content = "O";
            results[6] = MarkType.Oh;
         }

         // Column 1
         else if (results[1] == MarkType.Free && results[4] == results[7] && results[4] != MarkType.Free)
         {
            Button1_0.Content = "O";
            results[1] = MarkType.Oh;
         }
         else if (results[4] == MarkType.Free && results[1] == results[7] && results[1] != MarkType.Free)
         {
            Button1_1.Content = "O";
            results[4] = MarkType.Oh;
         }
         else if (results[7] == MarkType.Free && results[1] == results[4] && results[1] != MarkType.Free)
         {
            Button1_2.Content = "O";
            results[7] = MarkType.Oh;
         }

         // Column 2
         else if (results[2] == MarkType.Free && results[5] == results[8] && results[5] != MarkType.Free)
         {
            Button2_0.Content = "O";
            results[2] = MarkType.Oh;
         }
         else if (results[5] == MarkType.Free && results[2] == results[8] && results[2] != MarkType.Free)
         {
            Button2_1.Content = "O";
            results[5] = MarkType.Oh;
         }
         else if (results[8] == MarkType.Free && results[2] == results[5] && results[2] != MarkType.Free)
         {
            Button2_2.Content = "O";
            results[8] = MarkType.Oh;
         }
         #endregion

         #region Diagonal Checks
         // Top Left to Bottom Right
         else if (results[0] == MarkType.Free && results[4] == results[8] && results[4] != MarkType.Free)
         {
            Button0_0.Content = "O";
            results[0] = MarkType.Oh;
         }
         else if (results[4] == MarkType.Free && results[0] == results[8] && results[0] != MarkType.Free)
         {
            Button1_1.Content = "O";
            results[4] = MarkType.Oh;
         }
         else if (results[8] == MarkType.Free && results[0] == results[4] && results[0] != MarkType.Free)
         {
            Button2_2.Content = "O";
            results[8] = MarkType.Oh;
         }

         // Top Right to Bottom Left
         else if (results[2] == MarkType.Free && results[4] == results[6] && results[4] != MarkType.Free)
         {
            Button2_0.Content = "O";
            results[2] = MarkType.Oh;
         }
         else if (results[4] == MarkType.Free && results[2] == results[6] && results[2] != MarkType.Free)
         {
            Button1_1.Content = "O";
            results[4] = MarkType.Oh;
         }
         else if (results[6] == MarkType.Free && results[2] == results[4] && results[2] != MarkType.Free)
         {
            Button0_2.Content = "O";
            results[6] = MarkType.Oh;
         }
         #endregion

         #region Alternative CPU Space
         // Otherwise CPU picks space
         else
         {
            #region Random Picker
            rndNumber = rnd.Next(results.Length);

            while (results[rndNumber] != MarkType.Free || results[rndNumber] == MarkType.Ex)
            {
               rndNumber = rnd.Next(results.Length);
            }

            int x = 0;
            foreach (Button button in Container.Children)
            {
               if (x == rndNumber)
               {
                  button.Content = "O";
                  results[x] = MarkType.Oh;
                  return;
               }
               else
                  x++;
            }

            //if (results[rndNumber] == results[0])
            //{
            //   Button0_0.Content = "O";
            //   results[0] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[1])
            //{
            //   Button1_0.Content = "O";
            //   results[1] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[2])
            //{
            //   Button2_0.Content = "O";
            //   results[2] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[3])
            //{
            //   Button0_1.Content = "O";
            //   results[3] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[4])
            //{
            //   Button1_1.Content = "O";
            //   results[4] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[5])
            //{
            //   Button2_1.Content = "O";
            //   results[5] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[6])
            //{
            //   Button0_2.Content = "O";
            //   results[6] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[7])
            //{
            //   Button1_2.Content = "O";
            //   results[7] = MarkType.Oh;
            //}
            //else if (results[rndNumber] == results[8])
            //{
            //   Button2_2.Content = "O";
            //   results[8] = MarkType.Oh;
            //}
            #endregion

            #region Priority Picker
            // Pick based on space priority
            /* if (results[4] == MarkType.Free)
                Button1_1.Content = "O";
            else if (results[0] == MarkType.Free)
                Button0_0.Content = "O";
            else if (results[2] == MarkType.Free)
                Button2_0.Content = "O";
            else if (results[6] == MarkType.Free)
                Button0_2.Content = "O";
            else if (results[8] == MarkType.Free)
                Button2_2.Content = "O";
            else if (results[5] == MarkType.Free)
                Button2_1.Content = "O";
            else if (results[3] == MarkType.Free)
                Button0_1.Content = "O";
            else if (results[7] == MarkType.Free)
                Button1_2.Content = "O";
            else if (results[1] == MarkType.Free)
                Button1_0.Content = "O";
            else
                return; */
            #endregion
         }
         #endregion

      }

      private void CheckboxCPU_Checked(object sender, RoutedEventArgs e)
      {

      }
   }
}
