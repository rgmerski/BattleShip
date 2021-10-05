using BattleShip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BattleShip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player p1 = new Player("p1");
        private Player p2 = new Player("p2");

        string[] cols = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        string[] rows = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        bool p1_ready;
        bool p2_ready;

        public MainWindow()
        {
            InitializeComponent();
            p1_ready = false;
            p2_ready = false;
        }

        private void RND_P1_Click(object sender, RoutedEventArgs e)
        {
            Rand_ship(p1);
        }

        private void RND_P2_Click(object sender, RoutedEventArgs e)
        {
            Rand_ship(p2);
        }

        private void P1_Shot_Click(object sender, RoutedEventArgs e)
        {
            // Shoot until you miss
            Rand_shoot(p1);
        }

        private void P2_Shot_Click(object sender, RoutedEventArgs e)
        {
            // Shoot until you miss
            Rand_shoot(p2);
        }


        // Reset grids
        private void Reset(Player player, Grid[] grids)
        {
            
            if (player.Name == "p1")
            {
                P1G_Ships.Children.Clear();
                for (int i = 0; i < 10; i++)
                {
                    TextBlock temp = new TextBlock();
                    temp.Text = cols[i];
                    Grid.SetColumn(temp, i + 1);
                    Grid.SetRow(temp, 0);
                    temp.HorizontalAlignment = HorizontalAlignment.Center;
                    temp.VerticalAlignment = VerticalAlignment.Center;
                    grids[0].Children.Add(temp);
                }
                for (int i = 0; i < 10; i++)
                {
                    TextBlock temp = new TextBlock();
                    temp.Text = rows[i];
                    Grid.SetColumn(temp, 0);
                    Grid.SetRow(temp, i + 1);
                    temp.HorizontalAlignment = HorizontalAlignment.Center;
                    temp.VerticalAlignment = VerticalAlignment.Center;
                    grids[0].Children.Add(temp);
                }
            }
            else
            {
                P2G_Ships.Children.Clear();
                for (int i = 0; i < 10; i++)
                {
                    TextBlock temp = new TextBlock();
                    temp.Text = cols[i];
                    Grid.SetColumn(temp, i + 1);
                    Grid.SetRow(temp, 0);
                    temp.HorizontalAlignment = HorizontalAlignment.Center;
                    temp.VerticalAlignment = VerticalAlignment.Center;
                    grids[1].Children.Add(temp);
                }
                for (int i = 0; i < 10; i++)
                {
                    TextBlock temp = new TextBlock();
                    temp.Text = rows[i];
                    Grid.SetColumn(temp, 0);
                    Grid.SetRow(temp, i + 1);
                    temp.HorizontalAlignment = HorizontalAlignment.Center;
                    temp.VerticalAlignment = VerticalAlignment.Center;
                    grids[1].Children.Add(temp);
                }
            }
        }

        
        private void Rand_ship(Player player)
        {
            Random rand = new Random();
            // Generating coordinates of ships
            // random row -> random column -> orientation

            // Reset everything on grid and restore it
            // Grids to reset -> grids
            Grid[] grids = { P1G_Ships, P2G_Ships };
            Reset(player, grids);

            foreach (var ship in player.Ships)
            {
                bool loop = true;
                while (loop)
                {
                    var startcolumn = rand.Next(1, 11);
                    var startrow = rand.Next(1, 11);

                    int endrow = startrow, endcolumn = startcolumn;
                    var orientation = rand.Next(0, 2); // 0 -> horizonal, 1 -> vertical

                    // We need to check, if ship can be placed in drawn coords
                    // Cords - (startcolumn, startrow,
                    // if orientation = 0
                    // startcolumn, startrow + ship.Lenght (endrow)
                    // if orientation = 1
                    // startcolumn + ship.Length (endcolumn), startrow)

                    if (orientation == 0)
                    {
                        endrow += ship.Length;
                    }
                    else
                    {
                        endcolumn += ship.Length;
                    }
                    // EMPTY | A | B | C | D | E | F | G | H | I | J
                    /*  1
                     *  2
                     *  3
                     *  4
                     *  5
                     *  6
                     *  7
                     *  8
                     *  9
                     *  10
                     *
                     */
                    if (endrow > 11 || endcolumn > 11)
                    {
                        loop = true;
                        continue;
                        // try next placement
                    }

                    // ---------------------------------
                    // Here we have proper coords - now we need to check if there are any ships already
                    var usedAreas = player.Board.Areas.Check(startrow, startcolumn, endrow, endcolumn);

                    // if any area in needed for ship is used
                    if (usedAreas.Any(x => x.IsUsed))
                    {
                        loop = true;
                        continue;
                        // try next placement
                    }
                    foreach (var area in usedAreas)
                    {
                        int row = area.Row;
                        int col = area.Column;
                        // draw in grid
                        if (player.Name == "p1")
                        {
                            // draw for 1st player
                            TextBlock txt = new TextBlock();
                            txt.Text = ("#");
                            Grid.SetColumn(txt, col);
                            Grid.SetRow(txt, row);
                            txt.HorizontalAlignment = HorizontalAlignment.Center;
                            txt.VerticalAlignment = VerticalAlignment.Center;
                            P1G_Ships.Children.Add(txt);
                        }
                        else
                        {
                            // draw for 2nd player
                            TextBlock txt = new TextBlock();
                            txt.Text = ("#");
                            Grid.SetColumn(txt, col);
                            Grid.SetRow(txt, row);
                            txt.HorizontalAlignment = HorizontalAlignment.Center;
                            txt.VerticalAlignment = VerticalAlignment.Center;
                            P2G_Ships.Children.Add(txt);
                        }
                        area.IsUsed = true;
                    }
                    loop = false;
                }
            }
            // Now we need to lock possibility of shooting when ships aren't randomized yet
            // After randomizing ships for both players enable shot buttons.
            if (player.Name == "p1") p1_ready = true;
            else p2_ready = true;

            if (p1_ready && p2_ready)
            {
                MessageBox.Show("Both players have their boards randomized.");
                P1_Shot.IsEnabled = true;
                P2_Shot.IsEnabled = true;
            }
        }

        private void Rand_shoot(Player player)
        {
            Random rand = new Random();
            Board enemy_board;
            Grid grid;
            if (player.Name == "p1")
            {
                grid = P1G_Shots;
                enemy_board = p2.Board;
            }
            else 
            {
                grid = P2G_Shots;
                enemy_board = p1.Board;
            }


            // Game board that contains shots
            Board shoots = new();

            bool hit = true;
            // Random shoot
            while(hit)
            {
                var shot_col = rand.Next(1, 11);
                var shot_row = rand.Next(1, 11);
                // Get current area
                var current = shoots.Areas.Check(shot_row, shot_col, shot_row, shot_col);


                // if area was already chosen - try next area
                if (current.Any(x => x.IsUsed))
                {
                    MessageBox.Show($"This area was already chosen: {shot_row} row and {shot_col} column");
                    continue;
                }

                // shoot
                foreach (var item in current)
                {
                    item.IsUsed = true;
                }

                // check, if area is occupied by any ship
                // iteration in list (Board class) goes like this -> row 1 col 1 2 3 4 .... , row 2 , col 1 2 3 4 5 etc
                // row 1 col 1 -> Area(1,1); row 1 col 2 -> Area(1,2) 
                foreach (var item in current)
                {
                    var clone_hit = new Area(shot_row, shot_col);
                    clone_hit.IsUsed = true;
                    var board = enemy_board.Areas;
                    if (board.Contains(clone_hit))
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = ("+");
                        Grid.SetColumn(txt, shot_col);
                        Grid.SetRow(txt, shot_row);
                        txt.HorizontalAlignment = HorizontalAlignment.Center;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        grid.Children.Add(txt);
                        MessageBox.Show($"Hit! {shot_row} row and {shot_col} column");
                        hit = true;
                    }
                    else
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = ("-");
                        Grid.SetColumn(txt, shot_col);
                        Grid.SetRow(txt, shot_row);
                        txt.HorizontalAlignment = HorizontalAlignment.Center;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        grid.Children.Add(txt);
                        MessageBox.Show($"Miss. {shot_row} row and {shot_col} column");
                        hit = false;
                    }
                }

            }

        }
    }

    public class Area
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsUsed { get; set; }

        public Area(int row, int col)
        {
            Row = row;
            Column = col;
            IsUsed = false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Area objAsArea = obj as Area;
            if (objAsArea == null) return false;
            else return Equals(objAsArea);
        }

        public bool Equals(Area other)
        {
            if (other == null) return false;
            return (this.Row.Equals(other.Row) && 
                this.Column.Equals(other.Column) &&
                this.IsUsed.Equals(other.IsUsed));
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public List<Ship> Ships { get; set; }

        public bool Lost
        {
            get
            {
                return Ships.All(x => x.Sunk);
            }
        }

        public Board Board { get; set; }

        public Player(string name)
        {
            Name = name;
            Ships = new List<Ship>()
            {
                new Carrier(),
                new Battleship(),
                new Destroyer(),
                new Submarine(),
                new PatrolBoat()
            };
            Board = new Board();
        }
    }
}