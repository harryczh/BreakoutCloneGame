using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AddModel;
using AddObject;

namespace BallHitBricksGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(1200, 700);
            this.BackColor = Color.Black;
            this.Location = new Point(10, 10);
            //Image image = Image.FromFile("C:\\Users\\czh\\Desktop\\Course\\碩二下\\機械所物件導向程式設計\\oop_game\\stars-1245902_960_720.jpg");
            //this.BackgroundImage = image;           


            this.CurModel = new GameModel(this.ClientSize.Width, this.ClientSize.Height);
            //this.CurModel.Sound_GameLoop.PlayLooping();



            this.Controls.Add(this.CurModel.Button_Start);
            this.Controls.Add(this.CurModel.Label_PressEnter);

            this.CurModel.Button_Start.Click += new EventHandler(Button_Start_Click);
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            //開始畫面刪除
            this.CurModel.Button_Start.Hide();
            this.CurModel.Button_Start.Enabled = false;
            this.CurModel.Button_Start.Dispose();
            this.CurModel.Label_PressEnter.Hide();
            this.CurModel.Label_PressEnter.Enabled = false;
            this.CurModel.Label_PressEnter.Dispose();
            this.Controls.Remove(this.CurModel.Button_Start);
            this.Controls.Remove(this.CurModel.Label_PressEnter);

            //實例化關卡(磚塊)
            this.CurModel.CurRound = new Round();
            //實例化擊球板
            this.CurModel.CurBoard = new Board(this.ClientSize.Width, this.ClientSize.Height);
            //實例化球
            this.CurModel.CurBall = new Ball[99];
            this.CurModel.listCurBall = new List<Ball>();
            //實例化效果
            this.CurModel.CurBuff = new Buff[30];
            this.CurModel.listCurBuff = new List<Buff>();
            //實例化子彈
            this.CurModel.listCurBullet_Left = new List<Bullet>();
            this.CurModel.listCurBullet_Right = new List<Bullet>();
            //實例化狀態列
            this.CurModel.CurStatusColumn = new Status_Column();

            //this.CurModel.CurStatusColumn.Label_BallNumber.Text = this.CurModel.BallNumber.ToString();            

            //Form加入物件
            for (int i = 0; i < this.CurModel.CurRound.Label_Bricks.GetLength(0); i++)  //磚塊
            {
                for (int j = 0; j < this.CurModel.CurRound.Label_Bricks.GetLength(1); j++)
                {
                    this.Controls.Add(CurModel.CurRound.Label_Bricks[i, j]);
                }
            }
            this.Controls.Add(this.CurModel.CurBoard);  //擊球板            
            this.Controls.Add(this.CurModel.CurStatusColumn);  //狀態列

            this.CurModel.CurStatusColumn.Button_Stop.Click += new EventHandler(Button_Stop_Click);  //暫停鈕

            this.timer1.Start();
            this.timer1.Interval = 1;
            this.CurModel.FormTimerStartStop = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ///Set Position and Status
            this.CurModel.GameUpdate(this.ClientSize.Width, this.ClientSize.Height);

            ///Create Buff
            if (this.CurModel.CurRound.Brick_Visible == BrickVisible.UNVISIBLE)
            {                
                this.CurModel.listCurBuff.Add(new Buff(this.CurModel.HitPosition, 20));

                for (int i = 0; i < this.CurModel.listCurBuff.Count; i++)
                {
                    if (this.CurModel.listCurBuff.ElementAt(i) != null)
                    {
                        this.Controls.Add(this.CurModel.listCurBuff.ElementAt(i));
                        //this.CurModel.listCurBuff.ElementAt(i).BringToFront();
                    }
                }
            }

            ///Game Result
            if (this.CurModel.GameResult != null)
            {
                this.Controls.Add(this.CurModel.GameResult);
                this.timer1.Stop();
                this.CurModel.GameResult.BringToFront();
                //this.CurModel.CurStatusColumn.SendToBack();  

                //this.CurModel = null;
            }

            ///Twinkle Problem
            //this.DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景
            SetStyle(ControlStyles.DoubleBuffer, true); // 雙緩衝
            this.UpdateStyles();
            //this.Update();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.CurModel.FormTimerStartStop)
                {
                    if (this.CurModel.CurBoard != null)
                    {
                        if (this.CurModel.BallNumber == 1)
                        {
                            this.CurModel.listCurBall.Add(new Ball(new Point(this.CurModel.CurBoard.Location.X + this.CurModel.CurBoard.Width / 2, 670), 5));
                            foreach (Ball ball in this.CurModel.listCurBall)
                            {
                                this.Controls.Add(ball);
                            }
                            this.CurModel.BallNumber--;
                            this.CurModel.BallCreateNumber++;
                        }
                        else if (this.CurModel.BallNumber > 0 && this.CurModel.BallCreateNumber <= 20)
                        {
                            this.CurModel.listCurBall.Add(new Ball(new Point(this.CurModel.CurBoard.Location.X + this.CurModel.CurBoard.Width / 2, 670), 5));
                            foreach (Ball ball in this.CurModel.listCurBall)
                            {
                                this.Controls.Add(ball);
                            }
                            this.CurModel.BallNumber--;
                            this.CurModel.BallCreateNumber++;
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.CurModel.CurBoard != null)
            {
                this.CurModel.CurBoard.SetBoardPosition(e.X, this.ClientSize.Width - 200, this.ClientSize.Height);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (this.CurModel.FormTimerStartStop)
                {
                    this.timer1.Stop();
                    this.CurModel.FormTimerStartStop = false;

                    if (this.CurModel.CurStatusColumn.BulletTimeStartStop)
                    {
                        this.CurModel.CurStatusColumn.BulletTimeSec.Stop();
                    }

                }
                else
                {
                    this.timer1.Start();
                    this.CurModel.FormTimerStartStop = true;

                    if (this.CurModel.CurStatusColumn.BulletTimeStartStop)
                    {
                        this.CurModel.CurStatusColumn.BulletTimeSec.Start();
                    }
                }
            }
            else if (e.Shift)
            {
                if (this.CurModel.FormTimerStartStop)
                {
                    if (this.CurModel.CurStatusColumn.BulletTimeStartStop)
                    {
                        this.CurModel.listCurBullet_Left.Add(new Bullet(this.CurModel.CurBoard.Location.X, this.CurModel.CurBoard.Location.Y));
                        this.CurModel.listCurBullet_Right.Add(new Bullet(this.CurModel.CurBoard.Location.X + this.CurModel.CurBoard.Width - 5, this.CurModel.CurBoard.Location.Y));


                        //this.CurModel.Sound_BulletFirt.SoundLocation = @"C:\Users\czh\Desktop\Course\碩二下\機械所物件導向程式設計\oop_game\sound\buffhitboard.wav";
                        this.CurModel.Sound_BulletFirt.Play();

                        foreach (Bullet bullet in this.CurModel.listCurBullet_Left)
                        {
                            this.Controls.Add(bullet);
                        }

                        foreach (Bullet bullet in this.CurModel.listCurBullet_Right)
                        {
                            this.Controls.Add(bullet);
                        }
                    }
                    else
                        return;
                }
                else
                    return;
            }
            else if (e.KeyCode == Keys.R)
            {
                if (this.CurModel.GameResult != null)
                {
                    Application.Restart();
                }
                else
                    return;
            }
            else
                return;
        }


        private void Button_Stop_Click(object sender, EventArgs e)
        {
            if (this.CurModel.FormTimerStartStop)
            {
                this.timer1.Stop();
                this.CurModel.FormTimerStartStop = false;
                if (this.CurModel.CurStatusColumn.BulletTimeStartStop)
                {
                    this.CurModel.CurStatusColumn.BulletTimeSec.Stop();
                }
            }
            else
            {
                this.timer1.Start();
                this.CurModel.FormTimerStartStop = true;
                if (this.CurModel.CurStatusColumn.BulletTimeStartStop)
                {
                    this.CurModel.CurStatusColumn.BulletTimeSec.Stop();
                }
            }
        }


    }
}
