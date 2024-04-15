using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

using AddModel;

namespace AddObject
{
    ///=======================================================================================================///
                                               ///-----Round-----///
    ///=======================================================================================================///
    class Round
    {
        public Label[,] Label_Bricks;
        public int BrickWidth = 50;
        public int BrickHeight = 30;

        public int Row;
        public int Column;
        
        public BrickVisible Brick_Visible;
        //public int[] Int_BrickBuff_I;
        //public int[] Int_BrickBuff_J;

        public int visiblecount = 0;

        ///Constructor
        public Round()
        {
            Row = 12;
            Column = 4;
            Label_Bricks = new Label[Row, Column];

            SetBricks();
        }

        ///Set Bricks Color And Position
        private void SetBricks()
        {
            //Random rnd = new Random(Guid.NewGuid().GetHashCode());
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i <= Row - 1; i++) 
            {
                for (int j = 0; j <= Column - 1; j++)
                {
                    Label_Bricks[i, j] = new Label();
                    Label_Bricks[i, j].Enabled = false;
                    BrickColor brickcolor = (BrickColor)rnd.Next(1, 4);

                    switch (brickcolor)
                    {
                        case BrickColor.RED: Label_Bricks[i, j].BackColor = Color.FromArgb(255, 255, 0, 0); break;
                        case BrickColor.GREEN: Label_Bricks[i, j].BackColor = Color.FromArgb(255, 0, 255, 0); break;
                        case BrickColor.BLUE: Label_Bricks[i, j].BackColor = Color.FromArgb(255, 0, 0, 255); break;
                    }          
                    Label_Bricks[i, j].Width = BrickWidth;
                    Label_Bricks[i, j].Height = BrickHeight;
                    //this.Controls.Add(Label_Bricks[i, j]);
                }
            }
            SetBrickPosition();
        }

        ///Set Brick Position
        private void SetBrickPosition()
        {
            for (int i = 0; i <= Row - 1; i++)
            {
                for (int j = 0; j <= Column - 1; j++)
                {
                    if (i == 0)
                    {
                        Label_Bricks[i, j].Location = new Point(20 + j * 30 + 10, 260 - j * (this.BrickHeight + 20) - 10);
                    }
                    else if (i == 1)
                    {
                        Label_Bricks[i, j].Location = new Point(140 + j * 30 + 10, 40 + j * (this.BrickHeight + 20) + 10);
                    }
                    else if (i == 2)
                    {
                        Label_Bricks[i, j].Location = new Point(280 - j * 30 - 10, 240 + j * (this.BrickHeight + 20) + 10);
                    }
                    else if (i == 3)
                    {
                        Label_Bricks[i, j].Location = new Point(160 - j * 30 - 10, 460 - j * (this.BrickHeight + 20) - 10);
                    }
                    ///
                    else if (i == 4)
                    {
                        Label_Bricks[i, j].Location = new Point(350 + j * 30 + 10, 260 - j * (this.BrickHeight + 20) - 10);
                    }
                    else if (i == 5)
                    {
                        Label_Bricks[i, j].Location = new Point(470 + j * 30 + 10, 40 + j * (this.BrickHeight + 20) + 10);
                    }
                    else if (i == 6)
                    {
                        Label_Bricks[i, j].Location = new Point(610 - j * 30 - 10, 240 + j * (this.BrickHeight + 20) + 10);
                    }
                    else if (i == 7)
                    {
                        Label_Bricks[i, j].Location = new Point(490 - j * 30 - 10, 460 - j * (this.BrickHeight + 20) - 10);
                    }
                    ///
                    else if (i == 8)
                    {
                        Label_Bricks[i, j].Location = new Point(690, 470 - j * (this.BrickHeight + 30) - 20);
                    }
                    else if (i == 9)
                    {
                        Label_Bricks[i, j].Location = new Point(690, 240 - j * (this.BrickHeight + 30) - 20);
                    }
                    else if (i == 10)
                    {
                        Label_Bricks[i, j].Location = new Point(750 + j * 40 + 10, 30 + j * (this.BrickHeight + 15) + 10);
                    }
                    else if (i == 11)
                    {
                        Label_Bricks[i, j].Location = new Point(850 - j * 40 - 10, 210 + j * (this.BrickHeight + 15) + 10);
                        if (j == this.Column - 1)
                        {
                            Label_Bricks[i, j].Location = new Point(890 , 450);
                        }
                    }
                }
            }
        }


        ///Set Brick Buff (目前沒用)
        //private void SetBrickBuff()
        //{
        //    Random rnd = new Random((int)DateTime.Now.Ticks);
        //    for (int i = 0; i < 16; i++)
        //    {
        //        Int_BrickBuff_I[i] = new int();
        //        Int_BrickBuff_J[i] = new int();

        //        Int_BrickBuff_I[i] = rnd.Next(12);
        //        Int_BrickBuff_J[i] = rnd.Next(4);
        //    }
        //}

        ///Set Bricks Status
        public BrickVisible SetBrickStatus(BallHitObject ballhitbricks, int row, int column)
        {
            int i = row;
            int j = column;

            if (ballhitbricks != BallHitObject.NONE)
            {
                if (Label_Bricks[i, j].BackColor == Color.FromArgb(255, 255, 0, 0))  //紅色
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(170, 255, 10, 10);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(170, 255, 10, 10))
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(85, 255, 0, 0);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(255, 0, 255, 0))  //綠色
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(170, 10, 255, 10);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(170, 10, 255, 10))
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(85, 0, 255, 0);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(255, 0, 0, 255))  //藍色
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(170, 10, 10, 255);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(170, 10, 10, 255))
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(85, 0, 0, 255);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else
                {
                    Label_Bricks[i, j].Visible = false;
                    Brick_Visible = BrickVisible.UNVISIBLE;
                    visiblecount++;
                }
            }
            else
            {
                Brick_Visible = BrickVisible.VISIBLE;
            }
            return Brick_Visible;
        }

        ///Set Bricks Status
        public BrickVisible SetBrickStatus(BulletHitObject bullethitbricks, int row, int column)
        {
            int i = row;
            int j = column;

            if (bullethitbricks != BulletHitObject.NONE)
            {
                if (Label_Bricks[i, j].BackColor == Color.FromArgb(255, 255, 0, 0))  //紅色
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(170, 255, 50, 50);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(170, 255, 50, 50))
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(50, 255, 0, 0);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(255, 0, 255, 0))  //綠色
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(170, 50, 255, 50);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(170, 50, 255, 50))
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(50, 0, 255, 0);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(255, 0, 0, 255))  //藍色
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(170, 50, 50, 255);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else if (Label_Bricks[i, j].BackColor == Color.FromArgb(170, 50, 50, 255))
                {
                    Label_Bricks[i, j].BackColor = Color.FromArgb(50, 0, 0, 255);
                    Brick_Visible = BrickVisible.VISIBLE;
                }
                else
                {
                    Label_Bricks[i, j].Visible = false;
                    Brick_Visible = BrickVisible.UNVISIBLE;
                    visiblecount++;
                }
            }
            else
            {
                Brick_Visible = BrickVisible.VISIBLE;
            }
            return Brick_Visible;
        }
    }

    ///=======================================================================================================///
                                               ///-----Ball-----///
    ///=======================================================================================================///
    class Ball : PictureBox
    {
        //public Image image1 = Image.FromFile("C:\\Users\\czh\\Desktop\\game\\circle.png");

        public Image image1 = BallHitBricksGame.Properties.Resources.ball;

        public Point CenPt;
        public int Radius;
        public int countPastnum = 0;
        //球移動
        //public BallTimer BallTimer1;
        //public int Ball_X = 0;  //球左上X位置
        //public int Ball_Y = 0;  //球左上Y位置
        public int X_Variable = 4;  //X移動距離
        public int Y_Variable = -4;  //Y移動距離
        //public int Past_X_Variable = 5;
        //public int Past_Y_Variable = 5;
        public bool BallVisible = true;

        public int BallHitBoardDelayTime = 0;

        ///Constructor
        public Ball(Point cenPt, int radius)
        {
            //定義pictureBox
            this.Image = image1;
            //this.BackColor = System.Drawing.Color.Transparent;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Width = radius * 2;
            this.Height = radius * 2;

            //Ball位置
            this.CenPt = cenPt;
            this.Location = new Point(this.CenPt.X - Radius, this.CenPt.Y - Radius);
            this.Radius = radius;
            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true;
        }

        ///Set Ball Move
        public void SetBallMove(BallHitObject ballhitformround, BallHitObject ballhitbricks, BallHitObject ballhitboard)
        {
            //Ball Hit Form Round
            switch (ballhitformround)
            {
                case BallHitObject.UP_DOWN:
                    this.Y_Variable = -this.Y_Variable;
                    break;
                case BallHitObject.LEFT_RIGHT:
                    this.X_Variable = -this.X_Variable;
                    break;
                case BallHitObject.NONE:
                    break;
            }
            //Ball Hit Bricks
            switch (ballhitbricks)
            {
                case BallHitObject.UP_DOWN:
                    this.Y_Variable = -this.Y_Variable;
                    break;
                case BallHitObject.LEFT_RIGHT:
                    this.X_Variable = -this.X_Variable;
                    break;
                case BallHitObject.NONE:
                    break;
            }
            //Ball Hit Board
            switch (ballhitboard)
            {
                case BallHitObject.UP_DOWN:
                    this.Y_Variable = -this.Y_Variable;
                    break;
                case BallHitObject.BOARD_LEFT_RIGHT:
                    this.X_Variable = -this.X_Variable;
                    this.Y_Variable = -this.Y_Variable;
                    break;
                case BallHitObject.NONE:
                    break;
            }
            this.Location = new Point(this.CenPt.X - Radius, this.CenPt.Y - Radius);
            this.CenPt.X += X_Variable;
            this.CenPt.Y += Y_Variable;
        }

        ///Set Ball Status For Ball Life
        public void SetBallStatus(ObjectLife balllife)
        {
            switch (balllife)
            {
                case ObjectLife.ALIFE:
                    BallVisible = true;
                    //this.Dispose();
                    break;
                case ObjectLife.DEAD:
                    BallVisible = false;
                    //this.Dispose();
                    break;
            }
        }

        ///Set Ball Status For BuffType
        public void SetBallStatus(BuffType bufftype)
        {
            switch (bufftype)
            {
                case BuffType.FAST:
                    if (this.X_Variable > 0 && this.Y_Variable > 0)
                    {
                        this.X_Variable = 6;
                        this.Y_Variable = 6;
                    }
                    else if (this.X_Variable > 0 && this.Y_Variable < 0)
                    {
                        this.X_Variable = 6;
                        this.Y_Variable = -6;
                    }
                    else if (this.X_Variable < 0 && this.Y_Variable > 0)
                    {
                        this.X_Variable = -6;
                        this.Y_Variable = 6;
                    }
                    else if (this.X_Variable < 0 && this.Y_Variable < 0)
                    {
                        this.X_Variable = -6;
                        this.Y_Variable = -6;
                    }
                    break;
                case BuffType.SLOW:
                    if (this.X_Variable > 0 && this.Y_Variable > 0)
                    {
                        this.X_Variable = 2;
                        this.Y_Variable = 2;
                    }
                    else if (this.X_Variable > 0 && this.Y_Variable < 0)
                    {
                        this.X_Variable = 2;
                        this.Y_Variable = -2;
                    }
                    else if (this.X_Variable < 0 && this.Y_Variable > 0)
                    {
                        this.X_Variable = -2;
                        this.Y_Variable = 2;
                    }
                    else if (this.X_Variable < 0 && this.Y_Variable < 0)
                    {
                        this.X_Variable = -2;
                        this.Y_Variable = -2;
                    }
                    break;
            }
        }
    }

    ///=======================================================================================================///
                                               ///-----Board-----///
    ///=======================================================================================================///
    class Board : Label
    {
        public int Board_X = 0;
        public int Board_Y = 0;

        ///Constructor
        public Board(int formWidth, int formHeight)
        {
            this.Width = 100;
            this.Height = 20;
            this.BackColor = Color.Gray;
            this.Location = new Point(formWidth / 2, formHeight - this.Height);
        }

        ///Set Board Position
        public void SetBoardPosition(int mouseX, int formWidth, int formHeight)
        {
            this.Board_X = formWidth / 2 - this.Width / 2;
            this.Board_Y = formHeight - this.Height;

            //球拍的移動
            if (mouseX >= (formWidth - this.Width / 2)) //如果游標在右邊界處 
            {
                Board_X = formWidth - this.Width;//減去 球拍的水平位置，否則的話，球拍會在視窗 
                this.Location = new Point(Board_X, Board_Y);
            }
            else if (mouseX < this.Width / 2)
            {
                Board_X = 0; //球拍的水平位置 = 游標的水平位置
                this.Location = new Point(Board_X, Board_Y);
            }
            else
            {
                Board_X = mouseX - (this.Width) / 2;
                this.Location = new Point(Board_X, Board_Y);
            }

        }

        ///Set Board Status
        public void SetBoardStatus(BuffType bufftype)
        {
            switch (bufftype)
            {
                case BuffType.ELONGATION:
                    if (this.Width <= 1000)
                    {
                        this.Width += 30;
                    }
                    else
                        return;
                    break;
                case BuffType.SHORTEN:
                    if (this.Width >= 40)
                    {
                        this.Width -= 30;
                    }
                    else
                        return;
                    break;
            }
        }

    }

    ///=======================================================================================================///
                                                ///-----Buff-----///
    ///=======================================================================================================///
    class Buff : PictureBox
    {
        //BuffType bufftype;

        public string path = Application.StartupPath.ToString();
        //public Image image1 = Image.FromFile("circle.png");


        public Image image_Fast1 = BallHitBricksGame.Properties.Resources.fast_new1;
        public Image image_Slow1 = BallHitBricksGame.Properties.Resources.slow_new2;
        public Image image_Elongation1 = BallHitBricksGame.Properties.Resources.long_new1;
        public Image image_Shorten = BallHitBricksGame.Properties.Resources.short_new2;
        public Image image_Bullet = BallHitBricksGame.Properties.Resources.Bullet_new1;
        public Image image_Five = BallHitBricksGame.Properties.Resources.FiveBall_new1;
        public Image image_Ten = BallHitBricksGame.Properties.Resources.TenBall_new1;

        public BuffType CurBuffType;

        public Random rnd = new Random((int)DateTime.Now.Ticks);

        public Point CenPt;
        public int Radius;
        public int countPastnum = 0;

        //移動
        public int X_Variable = 2;  //X移動距離
        public int Y_Variable = 2;  //Y移動距離

        ///Constructor
        public Buff(Point cenPt, int radius)
        {
            //定義pictureBox
            SetBuffType();
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Width = radius * 2;
            this.Height = radius * 2;

            //Ball位置
            this.CenPt = cenPt;
            this.Radius = radius;
            //this.Location = new Point(this.CenPt.X, this.CenPt.Y);
            this.Location = new Point(-50, -50);

            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true;
        }

        ///Set Buff Type
        private void SetBuffType()
        {
            //Random rnd = new Random((int)DateTime.Now.Ticks);

            //BuffType bufftype = (BuffType)rnd.Next(1,8);
            CurBuffType = (BuffType)rnd.Next(1, 10);
            switch (CurBuffType)
            {
                case BuffType.FAST: this.Image = image_Fast1; break;
                case BuffType.SLOW: this.Image = image_Slow1; break;
                case BuffType.ELONGATION: this.Image = image_Elongation1; break;
                case BuffType.SHORTEN: this.Image = image_Shorten; break;
                case BuffType.BULLET: this.Image = image_Bullet; break;
                case BuffType.FIVE_BALL: this.Image = image_Five; break;
                //case BuffType.TEN_BALL: this.Image = image_Ten; break;
                default: break;

            }

        }

        ///Set Buff Move
        public void SetBuffMove(/*BrickVisible visible*/)
        {
            this.Location = new Point(this.CenPt.X - Radius, this.CenPt.Y - Radius);
            //this.CenPt.X += X_Variable;
            this.CenPt.Y += Y_Variable;
        }

        ///Set Buff Status
        public void SetBuffStatus(BuffHitBoard buffhitboard, ObjectLife bufflife)
        {
            switch (buffhitboard)
            {
                case BuffHitBoard.UP:
                    this.Visible = false;
                    break;
            }
            switch (bufflife)
            {
                case ObjectLife.DEAD:
                    this.Visible = false;
                    break;
            }
        }
        
    }

    ///=======================================================================================================///
                                                ///-----Bullet-----///
    ///=======================================================================================================///
    class Bullet : Label
    {

        public int X_Variable = 4;  //X移動距離
        public int Y_Variable = 4;  //Y移動距離

        private int LocX;
        private int LocY;

        public Bullet(int locx, int locy)
        {
            this.Width = 5;
            this.Height = 10;

            this.BackColor = Color.Gray;

            this.LocX = locx;
            this.LocY = locy - this.Height;
            this.Location = new Point(-50, -50);
        }

        public void SetBulletMove(int locx)
        {
            this.Location = new Point(LocX, LocY);
            this.LocY -= this.Y_Variable;
        }

        public void SetBulletStatus(BulletHitObject bullethitbrick, ObjectLife bulletlife)
        {
            switch (bullethitbrick)
            {
                case BulletHitObject.BRICKDOWN:
                    this.Visible = false;
                    break;
            }
            switch (bulletlife)
            {
                case ObjectLife.DEAD:
                    this.Visible = false;
                    break;
            }
        }
    }

    ///=======================================================================================================///
                                              ///-----Status_Column-----///
    ///=======================================================================================================///
    class Status_Column : Panel
    {
        //public Panel Panel_Status;
        public Label Label_Round;

        //球數
        public Label Label_BallNumberName;
        public Label Label_BallNumber;
        //子彈
        public Label Label_BulletName;
        public Label Label_BulletTime;
        //暫停
        public Button Button_Stop;
        //public Button Button_KeyBaord;

        //
        public Timer BulletTimeSec;
        public bool BulletTimeStartStop;

        public int int_BulletTimeSec = 5;


        public SoundPlayer Sound_BulletTime;

        public Status_Column()
        {
            ///狀態列視窗
            this.Location = new Point(1000, 0);
            this.Width = 200;
            this.Height = 700;
            this.BackColor = Color.DarkBlue;


            ///關卡
            this.Label_Round = new Label();
            this.Label_Round.Width = 200;
            this.Label_Round.Height = 50;
            this.Label_Round.Font = new Font("Microsoft JhengHei", 30, FontStyle.Bold);
            this.Label_Round.Text = "Round 1";
            this.Label_Round.ForeColor = Color.WhiteSmoke;


            ///球數名稱
            this.Label_BallNumberName = new Label();
            this.Label_BallNumberName.Width = 200;
            this.Label_BallNumberName.Height = 30;
            this.Label_BallNumberName.Text = "Ball Number :";
            this.Label_BallNumberName.Font = new Font("Microsoft JhengHei", 20, FontStyle.Bold);
            this.Label_BallNumberName.ForeColor = Color.WhiteSmoke;
            ///球數
            this.Label_BallNumber = new Label();
            this.Label_BallNumber.Width = 70;
            this.Label_BallNumber.Height = 30;
            //this.Label_BallNumber.Text = "123";
            //this.CurModel.Label_BallNumber.BackColor = Color.Blue;
            this.Label_BallNumber.Font = new Font("Microsoft JhengHei", 20, FontStyle.Bold);
            this.Label_BallNumber.ForeColor = Color.WhiteSmoke;

            this.BulletTimeSec = new Timer();
            this.BulletTimeSec.Interval = 1000;
            this.BulletTimeSec.Tick += new EventHandler(BulletTimeSec_Tick);
            ///子彈名稱
            this.Label_BulletName = new Label();
            this.Label_BulletName.Width = 200;
            this.Label_BulletName.Height = 30;
            this.Label_BulletName.Text = "Bullet sec :";
            this.Label_BulletName.Font = new Font("Microsoft JhengHei", 20, FontStyle.Bold);
            this.Label_BulletName.ForeColor = Color.WhiteSmoke;
            ///子彈剩餘秒數
            this.Label_BulletTime = new Label();
            this.Label_BulletTime.Width = 70;
            this.Label_BulletTime.Height = 30;
            this.Label_BulletTime.Text = "0 s";
            //this.CurModel.Label_BallNumber.BackColor = Color.Blue;
            this.Label_BulletTime.Font = new Font("Microsoft JhengHei", 20, FontStyle.Bold);
            this.Label_BulletTime.ForeColor = Color.WhiteSmoke;

            ///暫停按鈕
            this.Button_Stop = new Button();
            this.Button_Stop.Width = 100;
            this.Button_Stop.Height = 40;
            this.Button_Stop.Font = new Font("Microsoft JhengHei", 20, FontStyle.Bold);
            this.Button_Stop.Text = "Pause";
            this.Button_Stop.ForeColor = Color.WhiteSmoke;


            ///狀態列視窗加入物件
            this.Controls.Add(this.Label_Round);

            this.Controls.Add(this.Label_BallNumberName);
            this.Controls.Add(this.Label_BallNumber);

            this.Controls.Add(this.Label_BulletName);
            this.Controls.Add(this.Label_BulletTime);

            this.Controls.Add(this.Button_Stop);

            ///狀態列物件排板
            this.Label_Round.Location = new Point(0, 10);
            this.Label_BallNumberName.Location = new Point(0, 500);
            this.Label_BallNumber.Location = new Point(130, 530);
            this.Label_BulletName.Location = new Point(0, 400);
            this.Label_BulletTime.Location = new Point(130, 430);
            this.Button_Stop.Location = new Point(55, 650);

            Sound_BulletTime = new SoundPlayer(BallHitBricksGame.Properties.Resources.timecount);

        }

        ///Set Ball Number In Status Column
        public void SetBallNumberInStatusColumn(int ballnumber)
        {
            this.Label_BallNumber.Text = ballnumber.ToString();
        }

        public void SetBulletSecInStatusColumn(int bulletsec)
        {
            this.Label_BulletTime.Text = bulletsec.ToString();
        }

        public void SetBulletTimerStart(BuffType bullethitboard)
        {
            switch (bullethitboard)
            {
                case BuffType.BULLET:
                    this.BulletTimeSec.Start();
                    this.BulletTimeStartStop = true;
                    break;
                default:
                    break;
            }
        }

        public void SetBulletTimerStop(int timesec)
        {
            if (timesec < 0)
            {
                this.BulletTimeSec.Stop();
                this.BulletTimeStartStop = false;
                int_BulletTimeSec = 5;
            }
        }

        private void BulletTimeSec_Tick(object sender, EventArgs e)
        {
            //this.Sound_BulletTime.Play();
            this.Label_BulletTime.Text = this.int_BulletTimeSec--.ToString() + " s";
            SetBulletTimerStop(this.int_BulletTimeSec);
        }
    }

    ///=======================================================================================================///
                                            ///-----Game Result View-----///
    ///=======================================================================================================///
    class GameResultView : Panel
    {
        ///Game Result View        
        //public Panel Panel_GameWinLose;
        public Label Label_WinLose;
        public Label Label_PressR_ReStart;

        public GameResultView()
        {
            //this.Panel_GameWinLose = new Panel();
            this.Width = 1200;
            this.Height = 700;
            this.BringToFront();

            this.Label_WinLose = new Label();
            this.Label_WinLose.Width = 320;
            this.Label_WinLose.Height = 80;
            this.Label_WinLose.Location = new Point(this.Width / 2 - this.Label_WinLose.Width / 2, this.Height / 2 - this.Label_WinLose.Height / 2);
            this.Label_WinLose.ForeColor = Color.Red;
            this.Label_WinLose.BackColor = Color.Transparent;
            this.Label_WinLose.Font = new Font("Microsoft JhengHei", 50, FontStyle.Bold);
            this.Label_WinLose.AutoSize = false;
            this.Label_WinLose.TextAlign = ContentAlignment.MiddleCenter;

            this.Label_PressR_ReStart = new Label();
            this.Label_PressR_ReStart.Width = 230;
            this.Label_PressR_ReStart.Height = 20;
            this.Label_PressR_ReStart.Text = "---Press 'R' To Restart---";
            this.Label_PressR_ReStart.Location = new Point(this.Width / 2 - this.Label_PressR_ReStart.Width / 2, this.Height / 2 - this.Label_PressR_ReStart.Height / 2 + 50);
            this.Label_PressR_ReStart.ForeColor = Color.White;
            //this.Button_Start.BackColor = Color.Transparent;
            this.Label_PressR_ReStart.Font = new Font("Times New Roman", 15, FontStyle.Bold);
        }

        ///Set Game Win
        public void SetGameWin()
        {
            this.Label_WinLose.Text = "W i n !!";
            this.BackColor = Color.FromArgb(10, 0, 0, 0);
            this.Controls.Add(this.Label_WinLose);
            this.Controls.Add(this.Label_PressR_ReStart);
        }

        ///Set Game Lose
        public void SetGameLose()
        {
            this.Label_WinLose.Text = "L o s e !!";
            this.BackColor = Color.FromArgb(10, 0, 0, 0);
            this.Controls.Add(this.Label_WinLose);
            this.Controls.Add(this.Label_PressR_ReStart);
        }

    }
}
