using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SrcChess2
{
    public class BoardLines : Border
    { 
    }

    public class BoardLabel : TextBlock
    { 
    }
    public class CommentText : FlowDocument
    {
    }
    public class TitParagraph : Paragraph
    {

    }
    public class ContentParagraph : Paragraph
    {

    }
    public class CommentBoad : Grid, INotifyPropertyChanged
    {
        #region Game Info
        private string _players;
        public string Players
        {
            get { return _players; }
            set { _players = value; OnPropertyChanged("Players"); }
        }
        
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private FlowDocument _text;
        public FlowDocument Text 
        {
            get { return _text; }
            set { _text = value; } 
        }
        private string _title;
        public string Title 
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); } 
        }

        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }
        private string _content;
        public string Content 
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged("Content"); } 
        }
        public Button CloseButton { get; set; }
        Paragraph TitleParagraph { get; set; }
        Paragraph ContentParagraph { get; set; }
        FlowDocumentScrollViewer docContainer { get; set; }
        public TextBlock PlayersText { get; set; }
        public CommentBoad()
        {
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });

            // Game Info
            Players = "Player 1 (Black) vs Player 2 (White)";
            PlayersText = new TextBlock()
            {
                Text = Players,
                FontSize = 20,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(10, 5, 5, 10),
            };
            this.Children.Add(PlayersText);

            docContainer = new FlowDocumentScrollViewer() 
            { 
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto, 
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                
            };

            Text = new CommentText();
            docContainer.Document = Text;
            
            this.Children.Add(docContainer);

            CloseButton = new Button() 
            { 
                Content = "Hide", HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Width=100, Height=25, Margin = new Thickness(5,5,5,5),
                Visibility = System.Windows.Visibility.Collapsed
            };
            CloseButton.Click += CloseButton_Click;
            this.Children.Add(CloseButton);

            //
            Grid.SetRow(PlayersText, 0);
            Grid.SetRow(docContainer, 1);
            Grid.SetRow(CloseButton, 2);
        }
        public void SetPlayers(string players)
        {
            PlayersText.Text = players.ToUpper();
        }
        /// <summary>
        /// Hiển thị bình luận
        /// </summary>
        /// <param name="title">Tiêu đề bình luận</param>
        /// <param name="content">Nội dung bình luận</param>
        public void RaiseComment(string title, string content)
        {
            Text.Blocks.Clear();

            TitleParagraph = new Paragraph(new Run(title));
            TitleParagraph.FontSize = 30;
            TitleParagraph.Foreground = Brushes.Red;
            Text.Blocks.Add(TitleParagraph);
            
            ContentParagraph = new Paragraph(new Run(content));
            ContentParagraph.FontSize = 25;
            ContentParagraph.Foreground = Brushes.Black;
            Text.Blocks.Add(ContentParagraph);
            CloseButton.Visibility = Text.Blocks.Count > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ClearComment();
            CloseButton.Visibility = Text.Blocks.Count > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        public void ClearComment()
        {
            Text.Blocks.Clear();
            CloseButton.Visibility = Text.Blocks.Count > 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
    }

    public class Comment
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Comment(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }

    public class Board : Grid
    {
        public Border Container = new Border();
        public double ActualWidth { get; set; }
        public double ActualHeight { get; set; }
        public Board(Border parent) 
        {
            this.Container = parent;
            this.ActualHeight = Container.Height;
            this.ActualWidth = Container.Width;
            AddLines();
        }

        public void AddLines()
        {
            for(int i=0; i<17; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 20; i++)
            {
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            
            //
            for (int iRow = 0; iRow < 20; iRow++)
            {
                for (int iCol = 0; iCol < 18; iCol++)
                {
                    BoardLines border = new BoardLines();
                    if (iRow == 0)
                    {
                    }
                    else if (iRow == 19)
                    {
                        if (iCol == 0 || iCol == 17)
                        {
                            border.BorderThickness = new Thickness(0);
                        }
                        else
                        {
                            border.BorderThickness = new Thickness(0, 0.5, 0, 0);
                        }
                    }
                    else if (iRow % 2 != 0)
                    {
                        if (iCol == 0)
                        {
                            border.BorderThickness = new Thickness(0, 0, 0, 0);
                        }
                        else if (iCol == 17)
                        {
                            border.BorderThickness = new Thickness(0.5, 0, 0, 0);
                        }
                        else
                        {
                            if (iCol % 2 == 0)
                            {

                                border.BorderThickness = new Thickness(0, 0.5, 0, 0);
                            }
                            else
                            {
                                border.BorderThickness = new Thickness(0.5, 0.5, 0, 0);
                            }

                        }
                    }
                    else
                    {
                        if (iCol % 2 != 0)
                        {
                            border.BorderThickness = new Thickness(0.5, 0, 0, 0);
                        }
                    }
                    border.Opacity = 0.65;
                    border.SetValue(Grid.ColumnProperty, iCol);
                    border.SetValue(Grid.RowProperty, iRow);
                    this.Children.Add(border);
                }
            }
            
            
        }

    }
}
