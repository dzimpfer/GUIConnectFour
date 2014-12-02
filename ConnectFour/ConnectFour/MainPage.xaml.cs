using ConnectFour.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ConnectFour
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int BOARD_WIDTH = 6;
        public int BOARD_HEIGHT = 7;
        public bool firstPlayerTurn = true;
        public bool gameOver = false;
        public string firstPlayerName = "Player One";
        public string secondPlayerName = "Player Two";
        public Color firstPlayerColor = Colors.Blue;
        public Color secondPlayerColor = Colors.Red;
        public int firstPlayerScore = 0;
        public int secondPlayerScore = 0;
        public string[] topPlayers = { "One", "Two", "Three", "Four", "Five" };
        public int[] topPlayerScores = { 0, 0, 0, 0, 0 };
        public int[,] grid = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };
        //public int[,] grid = { { 0, 1, 2, 0, 1, 2 }, { 1, 0, 2, 2, 1, 0 }, { 2, 2, 1, 0, 0, 1 }, { 0, 1, 0, 2, 1, 2 }, { 0, 2, 2, 1, 0, 0 }, { 0, 2, 1, 2, 1, 0 }, { 1, 1, 2, 2, 0, 1 } };
        public MainPage()
        {
            this.InitializeComponent();
            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    Border border = new Border();
                    border.Width = 100;
                    border.Height = border.Width;
                    Canvas.SetTop(border, 100 * r);
                    Canvas.SetLeft(border, 100 * c);
                    //border.Name = generateName(c, r);
                    //border.PointerPressed += showborder;
                    border.BorderBrush = new SolidColorBrush(Colors.White);
                    border.BorderThickness = new Thickness(1, 1, 1, 1);
                    if (r == 0 && c == 0)
                        border.CornerRadius = new CornerRadius(20, 0, 0, 0);
                    if (r == 0 && c == BOARD_WIDTH - 1)
                        border.CornerRadius = new CornerRadius(0, 20, 0, 0);
                    if (r == BOARD_HEIGHT - 1 && c == 0)
                        border.CornerRadius = new CornerRadius(0, 0, 0, 20);
                    if (r == BOARD_HEIGHT - 1 && c == BOARD_WIDTH - 1)
                        border.CornerRadius = new CornerRadius(0, 0, 20, 0);

                    if (c == 0)
                        border.BorderThickness = new Thickness(2, 1, 1, 1);
                    if (c == BOARD_WIDTH - 1)
                        border.BorderThickness = new Thickness(1, 1, 2, 1);
                    if (r == BOARD_HEIGHT - 1)
                        border.BorderThickness = new Thickness(1, 1, 1, 2);
                    if (r == 0)
                        border.BorderThickness = new Thickness(1, 2, 1, 1);

                    boardCanvas.Children.Add(border);
                }
            }

            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 80;
                    ellipse.Height = ellipse.Width;
                    Canvas.SetTop(ellipse, 100 * r + 10);
                    Canvas.SetLeft(ellipse, 100 * c + 10);
                    //rect.Name = generateName(c, r);
                    //rect.PointerPressed += showRect;
                    boardCanvas.Children.Add(ellipse);
                }
            }

            for (int c = 0; c < BOARD_WIDTH; c++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 100;
                textBlock.Width = 100;
                Canvas.SetLeft(textBlock, 100 * c);
                textBlock.Text = (c + 1).ToString();
                textBlock.FontFamily = new FontFamily("Kristen ITC");
                textBlock.FontSize = 36;
                textBlock.TextAlignment = TextAlignment.Center;

                textBlock.Name = c.ToString();
                textBlock.PointerPressed += columnClicked;

                columnCanvas.Children.Add(textBlock);
            }

            columnCanvas.HorizontalAlignment = HorizontalAlignment.Center;
            columnCanvas.Margin = new Thickness(0, 125, 0, 0);
            columnCanvas.Height = 100;
            columnCanvas.Width = BOARD_WIDTH * 100;

            boardCanvas.HorizontalAlignment = HorizontalAlignment.Center;
            boardCanvas.VerticalAlignment = VerticalAlignment.Center;
            boardCanvas.Height = BOARD_HEIGHT * 100;
            boardCanvas.Width = BOARD_WIDTH * 100;
            DrawGrid();

            firstPlayerNameTextBlock.Text = firstPlayerName + " Wins:  ";
            firstPlayerScoreTextBlock.Text = firstPlayerScore.ToString();

            secondPlayerNameTextBlock.Text = secondPlayerName + " Wins:  ";
            secondPlayerScoreTextBlock.Text = secondPlayerScore.ToString();

            topScorerTextBlock1.Text = topPlayers[0] + ":";
            topScoreTextBlock1.Text = "  " + topPlayerScores[0].ToString();

            topScorerTextBlock2.Text = topPlayers[1] + ":";
            topScoreTextBlock2.Text = "  " + topPlayerScores[1].ToString();

            topScorerTextBlock3.Text = topPlayers[2] + ":";
            topScoreTextBlock3.Text = "  " + topPlayerScores[2].ToString();

            topScorerTextBlock4.Text = topPlayers[3] + ":";
            topScoreTextBlock4.Text = "  " + topPlayerScores[3].ToString();

            topScorerTextBlock5.Text = topPlayers[4] + ":";
            topScoreTextBlock5.Text =  "  " + topPlayerScores[4].ToString();
        }

        private void DrawGrid()
        {
            int index = BOARD_HEIGHT * BOARD_WIDTH;

            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    Ellipse ellipse = boardCanvas.Children[index] as Ellipse;
                    index++;

                    ellipse.StrokeThickness = 3;
                    if (grid[r, c] == 1)
                    {
                        ellipse.Stroke = new SolidColorBrush(firstPlayerColor);
                    }
                    else if (grid[r, c] == 2)
                    {
                        ellipse.Stroke = new SolidColorBrush(secondPlayerColor);
                    }
                    else
                    {
                        ellipse.Stroke = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

        private void newGame()
        {
            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    grid[r, c] = 0;
                }
            }

            interactionTextBlock.Foreground = new SolidColorBrush(firstPlayerColor);
            interactionTextBlock.Text = firstPlayerName + "'s Turn!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newGame();
            DrawGrid();
        }


        private void columnClicked(object sender, PointerRoutedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            makePlay(Convert.ToInt32(textBlock.Name));
        }
        private void makePlay(int column)
        {
            if (!gameIsOver(grid))
            {
                interactionTextBlock.Text = column.ToString();

                if (grid[0, column] != 0)
                {
                    interactionTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                    interactionTextBlock.Text = "Invalid Move";
                }
                else
                {
                    for (int r = BOARD_HEIGHT - 1; r > -1; r--)
                    {
                        if (grid[r, column] == 0)
                        {
                            if (firstPlayerTurn)
                            {
                                grid[r, column] = 1;

                            }
                            if (!firstPlayerTurn)
                            {
                                grid[r, column] = 2;

                            }
                            break;
                        }
                    }
                    firstPlayerTurn = !firstPlayerTurn;
                    DrawGrid();
                    if (firstPlayerTurn)
                    {
                        interactionTextBlock.Foreground = new SolidColorBrush(firstPlayerColor);
                        interactionTextBlock.Text = firstPlayerName + "'s Turn!";
                    }
                    else
                    {
                        interactionTextBlock.Foreground = new SolidColorBrush(secondPlayerColor);
                        interactionTextBlock.Text = secondPlayerName + "'s Turn!";
                    }
                }
                if (firstPlayerTurn)
                    garbage.Text = evaluateFor1(grid).ToString();
                else
                    garbage.Text = evaluateFor2(grid).ToString();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage), null);
        }

        private void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Number1: makePlay(0); break;
                case VirtualKey.Number2: makePlay(1); break;
                case VirtualKey.Number3: makePlay(2); break;
                case VirtualKey.Number4: makePlay(3); break;
                case VirtualKey.Number5: makePlay(4); break;
                case VirtualKey.Number6: makePlay(5); break;
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditScoresPage), null);
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditPlayersPage), null);
        }

        private bool gameIsOver(int[,] grid)
        {
            bool result = false;

            if (playerWon(grid, 1))
                result = true;
            if (playerWon(grid, 2))
                result = true;
            
            return result;
        }

        private bool playerWon(int[,] grid, int player)
        {
            bool result = false;

            for (int r = 0; r < BOARD_HEIGHT; r++)
            {
                for (int c = 0; c < BOARD_WIDTH; c++)
                {
                    if(r <= BOARD_HEIGHT - 4)
                    {
                        if(grid[r,c] == player && grid[r+1,c] == player && grid[r+2,c] == player && grid[r+3,c] == player)
                            result = true;
                    }
                    if(c <= BOARD_WIDTH - 4)
                    {
                        if(grid[r,c] == player && grid[r,c + 1] == player && grid[r,c + 2] == player && grid[r,c + 3] == player)
                            result = true;
                    }
                    if (c <= BOARD_WIDTH - 4 && r <= BOARD_HEIGHT - 4)
                    {
                        if (grid[r, c] == player && grid[r + 1, c + 1] == player && grid[r + 2, c + 2] == player && grid[r + 3, c + 3] == player)
                            result = true;
                    }
                    if (c <= BOARD_WIDTH - 4 && r > 2)
                    {
                        if (grid[r, c] == player && grid[r - 1, c + 1] == player && grid[r - 2, c + 2] == player && grid[r - 3, c + 3] == player)
                            result = true;
                    }
                }
            }

            return result;
        }

        private int evaluateFor1(int[,] grid)
        {
            int result = 0;

            if (playerWon(grid, 1))
                result = 1000;
            if (playerWon(grid, 2))
                result = -1000;


            if (!gameIsOver(grid))
            {
                for (int r = 0; r < BOARD_HEIGHT; r++)
                {
                    for (int c = 0; c < BOARD_WIDTH; c++)
                    {
                        if (r <= BOARD_HEIGHT - 3)
                        {
                            if (grid[r, c] == 1 && grid[r + 1, c] == 1 && grid[r + 2, c] == 1)
                                result += 3;
                            if (grid[r, c] == 2 && grid[r + 1, c] == 2 && grid[r + 2, c] == 2)
                                result -= 3;
                        }
                        if (c <= BOARD_WIDTH - 3)
                        {
                            if (grid[r, c] == 1 && grid[r, c + 1] == 1 && grid[r, c + 2] == 1)
                                result += 3;
                            if (grid[r, c] == 2 && grid[r, c + 1] == 2 && grid[r, c + 2] == 2)
                                result -= 3;
                        }
                        if (c <= BOARD_WIDTH - 3 && r <= BOARD_HEIGHT - 3)
                        {
                            if (grid[r, c] == 1 && grid[r + 1, c + 1] == 1 && grid[r + 2, c + 2] == 1)
                                result += 3;
                            if (grid[r, c] == 2 && grid[r + 1, c + 1] == 2 && grid[r + 2, c + 2] == 2)
                                result -= 3;
                        }
                        if (c <= BOARD_WIDTH - 3 && r > 1)
                        {
                            if (grid[r, c] == 1 && grid[r - 1, c + 1] == 1 && grid[r - 2, c + 2] == 1)
                                result += 3;
                            if (grid[r, c] == 2 && grid[r - 1, c + 1] == 2 && grid[r - 2, c + 2] == 2)
                                result -= 3;
                        }
                    }
                }

                for (int r = 0; r < BOARD_HEIGHT; r++)
                {
                    for (int c = 0; c < BOARD_WIDTH; c++)
                    {
                        if (r <= BOARD_HEIGHT - 2)
                        {
                            if (grid[r, c] == 1 && grid[r + 1, c] == 1)
                                result += 2;
                            if (grid[r, c] == 2 && grid[r + 1, c] == 2)
                                result -= 2;
                        }
                        if (c <= BOARD_WIDTH - 2)
                        {
                            if (grid[r, c] == 1 && grid[r, c + 1] == 1)
                                result += 2;
                            if (grid[r, c] == 2 && grid[r, c + 1] == 2)
                                result -= 2;
                        }
                        if (c <= BOARD_WIDTH - 2 && r <= BOARD_HEIGHT - 2)
                        {
                            if (grid[r, c] == 1 && grid[r + 1, c + 1] == 1)
                                result += 2;
                            if (grid[r, c] == 2 && grid[r + 1, c + 1] == 2)
                                result -= 2;
                        }
                        if (c <= BOARD_WIDTH - 2 && r > 0)
                        {
                            if (grid[r, c] == 1 && grid[r - 1, c + 1] == 1)
                                result += 2;
                            if (grid[r, c] == 2 && grid[r - 1, c + 1] == 2)
                                result += 2;
                        }
                    }
                }
            }

            return result;
        }

        private int evaluateFor2(int[,] grid)
        {
            int result = 0;

            if (playerWon(grid, 2))
                result = 1000;
            if (playerWon(grid, 2))
                result = -1000;

            if (!gameIsOver(grid))
            {
                for (int r = 0; r < BOARD_HEIGHT; r++)
                {
                    for (int c = 0; c < BOARD_WIDTH; c++)
                    {
                        if (r <= BOARD_HEIGHT - 3)
                        {
                            if (grid[r, c] == 2 && grid[r + 1, c] == 2 && grid[r + 2, c] == 2)
                                result += 3;
                            if (grid[r, c] == 1 && grid[r + 1, c] == 1 && grid[r + 2, c] == 1)
                                result -= 3;
                        }
                        if (c <= BOARD_WIDTH - 3)
                        {
                            if (grid[r, c] == 2 && grid[r, c + 1] == 2 && grid[r, c + 2] == 2)
                                result += 3;
                            if (grid[r, c] == 1 && grid[r, c + 1] == 1 && grid[r, c + 2] == 1)
                                result -= 3;
                        }
                        if (c <= BOARD_WIDTH - 3 && r <= BOARD_HEIGHT - 3)
                        {
                            if (grid[r, c] == 2 && grid[r + 1, c + 1] == 2 && grid[r + 2, c + 2] == 2)
                                result += 3;
                            if (grid[r, c] == 1 && grid[r + 1, c + 1] == 1 && grid[r + 2, c + 2] == 1)
                                result -= 3;
                        }
                        if (c <= BOARD_WIDTH - 3 && r > 1)
                        {
                            if (grid[r, c] == 2 && grid[r - 1, c + 1] == 2 && grid[r - 2, c + 2] == 2)
                                result += 3;
                            if (grid[r, c] == 1 && grid[r - 1, c + 1] == 1 && grid[r - 2, c + 2] == 1)
                                result += 3;
                        }
                    }
                }

            
                for (int r = 0; r < BOARD_HEIGHT; r++)
                {
                    for (int c = 0; c < BOARD_WIDTH; c++)
                    {
                        if (r <= BOARD_HEIGHT - 2)
                        {
                            if (grid[r, c] == 2 && grid[r + 1, c] == 2)
                                result += 2;
                            if (grid[r, c] == 1 && grid[r + 1, c] == 1)
                                result -= 2;
                        }
                        if (c <= BOARD_WIDTH - 2)
                        {
                            if (grid[r, c] == 2 && grid[r, c + 1] == 2)
                                result += 2;
                            if (grid[r, c] == 1 && grid[r, c + 1] == 1)
                                result -= 2;
                        }
                        if (c <= BOARD_WIDTH - 2 && r <= BOARD_HEIGHT - 2)
                        {
                            if (grid[r, c] == 2 && grid[r + 1, c + 1] == 2)
                                result += 2;
                            if (grid[r, c] == 1 && grid[r + 1, c + 1] == 1)
                                result -= 2;
                        }
                        if (c <= BOARD_WIDTH - 2 && r > 0)
                        {
                            if (grid[r, c] == 2 && grid[r - 1, c + 1] == 2)
                                result += 2;
                            if (grid[r, c] == 1 && grid[r - 1, c + 1] == 1)
                                result -= 2;
                        }
                    }
                }
            }

            return result;
        }

    }
}
