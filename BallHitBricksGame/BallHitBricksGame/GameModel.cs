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
using System.Resources;

using AddObject;

namespace AddModel
{
    ///Ball Hit Object Side
    public enum BallHitObject
    {
        NONE = 0,
        UP_DOWN = 1,
        LEFT_RIGHT = 2,
        BOARD_LEFT_RIGHT = 3
    }

    ///Buff Hit Board Side
    public enum BuffHitBoard
    {
        NONE = 0,
        UP = 1
    }

    ///Bullet Hit Brick Side
    public enum BulletHitObject
    {
        NONE = 0,
        BRICKDOWN = 1,
        FORMUP = 2
    }

    ///Object Life
    public enum ObjectLife
    {
        NONE = 0,
        ALIFE = 1,
        DEAD = 2
    }

    ///Brick Color
    public enum BrickColor
    {
        RED = 1,
        GREEN = 2,
        BLUE = 3
    }

    ///Brick Visible
    public enum BrickVisible
    {
        VISIBLE = 1,
        UNVISIBLE = 2
    }

    ///Buff Type
    public enum BuffType
    {
        NONE = 0,
        FAST = 1,
        SLOW = 2,
        ELONGATION = 3,
        SHORTEN = 4,
        BULLET = 5,
        FIVE_BALL = 6,
        TEN_BALL = 7,
        NONE_1 = 8,
        NONE_2 = 9,
        NONE_3 = 10
    }


    public enum SoundType
    {
        NONE = 0,
        BALLHIT = 1,
        BUFFHIT = 2,
        BULLEFIRE = 3,
        DEAD = 4
    }


    class GameModel
    {
        public Round CurRound;

        public Board CurBoard;

        public Ball[] CurBall;
        public List<Ball> listCurBall;

        public Buff[] CurBuff;
        public List<Buff> listCurBuff;

        public List<Bullet> listCurBullet_Left;
        public List<Bullet> listCurBullet_Right;

        public Status_Column CurStatusColumn;

        public GameResultView GameResult;

        ///Game Start View
        public Button Button_Start;
        public Label Label_PressEnter;

        ///Game Pause View
        public Label Label_Pause;

        ///Game Sound
        public SoundPlayer Sound_BallHitObject;
        public SoundPlayer Sound_BuffHitBoard;
        public SoundPlayer Sound_BulletFirt;
        public SoundPlayer Sound_BallOut;
        public SoundPlayer Sound_GameWin;
        public SoundPlayer Sound_GameLose;


        ///For Algorithm       
        public int HitRow;
        public int HitColumn;
        public Point HitPosition;

        public BallHitObject BallHitFormRound;
        public BallHitObject BallHitBricks;
        public BallHitObject BallHitBoard;

        public BuffHitBoard Buff_HitBoard;

        public BulletHitObject BulletHitBrick;
        public BulletHitObject BulletHitForm;

        public BuffType BuffHitBoardType;

        public ObjectLife BallLife;
        public ObjectLife BuffLife;
        public ObjectLife BulletLife;

        public int BallNumber = 1;
        public int BallVisibleNumber = 1;
        public int BallCreateNumber = 0;

        public Stopwatch sw = new Stopwatch();
        public int BulletSec = 5;
        //public DateTime MyEndDate;
        public bool FormTimerStartStop;

        ///Constructor
        public GameModel(int formWidth, int formHeight)
        {
            this.Button_Start = new Button();
            this.Button_Start.Width = 280;
            this.Button_Start.Height = 90;
            this.Button_Start.Text = "Start";
            this.Button_Start.Location = new Point(formWidth / 2 - this.Button_Start.Width / 2, formHeight / 2 - this.Button_Start.Height / 2);
            this.Button_Start.ForeColor = Color.White;
            //this.Button_Start.BackColor = Color.Transparent;
            this.Button_Start.Font = new Font("Microsoft JhengHei", 50, FontStyle.Bold);

            this.Label_PressEnter = new Label();
            this.Label_PressEnter.Width = 150;
            this.Label_PressEnter.Height = 20;
            this.Label_PressEnter.Text = "---Press Enter---";
            this.Label_PressEnter.Location = new Point(formWidth / 2 - this.Label_PressEnter.Width / 2, formHeight / 2 - this.Label_PressEnter.Height / 2 + 50);
            this.Label_PressEnter.ForeColor = Color.White;
            //this.Button_Start.BackColor = Color.Transparent;
            this.Label_PressEnter.Font = new Font("Times New Roman", 15, FontStyle.Bold);


            
            Sound_BallHitObject = new SoundPlayer(BallHitBricksGame.Properties.Resources.objecthitbricks);
            Sound_BuffHitBoard = new SoundPlayer(BallHitBricksGame.Properties.Resources.buffhitboard);
            Sound_BulletFirt = new SoundPlayer(BallHitBricksGame.Properties.Resources.bullet);
            Sound_BallOut = new SoundPlayer(BallHitBricksGame.Properties.Resources.ballout);
            Sound_GameWin = new SoundPlayer(BallHitBricksGame.Properties.Resources.gamewin);
            Sound_GameLose = new SoundPlayer(BallHitBricksGame.Properties.Resources.gamelose);

        }

        ///Set Ball Hit Form Bound
        public void SetBallHitFormBound(int formWidth, int formHeight, Ball ball)
        {
            Point CenPt = ball.CenPt;

            BallHitFormRound = BallHitObject.NONE;
            BallLife = ObjectLife.NONE;

            if (CenPt.X - ball.Radius <= 0 || (CenPt.X + ball.Radius) >= formWidth)  //左右
            {
                BallHitFormRound = BallHitObject.LEFT_RIGHT;
                this.Sound_BallHitObject.Play();
            }
            else if (CenPt.Y - ball.Radius <= 0)  //上
            {
                BallHitFormRound = BallHitObject.UP_DOWN;
                this.Sound_BallHitObject.Play();
            }
            else if (CenPt.Y - ball.Radius >= formHeight)  //下
            {
                this.Sound_BallOut.Play();
                //ball.X_Variable = 0;
                //ball.Y_Variable = 0;
                //ball.Dispose();
                //ball.Visible = false;
                //ball = null;  
                BallLife = ObjectLife.DEAD;
                //ballhit = true;
                this.BallVisibleNumber--;
                this.BallCreateNumber--;
            }
            else
                return;
        }

        ///Set Ball HIt Board
        public void SetBallHitBoard(Label board, Ball ball)
        {
            BallHitBoard = BallHitObject.NONE;
            ball.BallHitBoardDelayTime++;

            if (ball.BallHitBoardDelayTime > 5)
            {
                BallA_Collision_LabelB(ball, board, false);
            }
            else
                return;
        }

        ///Set Ball Hit Bricks
        public void SetBallHitBricks(Label[,] bricks, Ball ball)
        {
            BallHitBricks = BallHitObject.NONE;
            ball.BallHitBoardDelayTime++;

            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].Visible == true)
                    {
                        if (ball.BallHitBoardDelayTime > 5)
                        {
                            if (BallA_Collision_LabelB(ball, bricks[i, j], true))
                            {
                                HitRow = i;
                                HitColumn = j;
                                HitPosition = new Point(bricks[i, j].Location.X + bricks[i, j].Width / 2, bricks[i, j].Location.Y + bricks[i, j].Height);
                                //PlayHitBrickSound();

                                //this.Sound_BallHitObject.Play();
                            }
                        }
                    }
                }
            }
        }

        ///Ball Collision Label (Ball Hit Bricks [hitwhat = true] or Ball Hit Board [hitwhat = false]) > [For "SetBallHitBricks()" Use]
        //private bool BallA_Collision_LabelB(Ball ball, Label labelIn, bool hitwhat)
        //{
        //    if (ball.Location.X > labelIn.Location.X - ball.Width
        //        && ball.Location.X < labelIn.Location.X + labelIn.Width
        //        && ball.Location.Y > labelIn.Location.Y - ball.Height
        //        && ball.Location.Y < labelIn.Location.Y + labelIn.Height)
        //    {
        //        ball.BallHitBoardDelayTime = 0;
        //        if (ball.Location.Y < labelIn.Location.Y && ball.Location.Y + ball.Height > labelIn.Location.Y)  //磚塊上邊
        //        {
        //            if (ball.Location.X < labelIn.Location.X && ball.Location.X + ball.Width > labelIn.Location.X)  //左上
        //            {
        //                if (ball.Location.X + ball.Width - labelIn.Location.X >= ball.Location.Y + ball.Height - labelIn.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.UP_DOWN;
        //                    else
        //                        BallHitBoard = BallHitObject.UP_DOWN;
        //                    return true;
        //                }
        //                else if (ball.Location.X + ball.Width - labelIn.Location.X < ball.Location.Y + ball.Height - labelIn.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.LEFT_RIGHT;
        //                    else
        //                        BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
        //                    return true;
        //                }
        //                else
        //                    return false;
        //            }
        //            else if (ball.Location.X >= labelIn.Location.X && ball.Location.X + ball.Width <= labelIn.Location.X + labelIn.Width)  //中間
        //            {
        //                if (hitwhat)
        //                    BallHitBricks = BallHitObject.UP_DOWN;
        //                else
        //                    BallHitBoard = BallHitObject.UP_DOWN;
        //                return true;
        //            }
        //            else if (ball.Location.X < labelIn.Location.X + labelIn.Width && ball.Location.X + ball.Width > labelIn.Location.X + labelIn.Width)  //右上
        //            {
        //                if (labelIn.Location.X + labelIn.Width - ball.Location.X >= ball.Location.Y + ball.Height - labelIn.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.UP_DOWN;
        //                    else
        //                        BallHitBoard = BallHitObject.UP_DOWN;
        //                    return true;
        //                }
        //                else if (labelIn.Location.X + labelIn.Width - ball.Location.X < ball.Location.Y + ball.Height - labelIn.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.LEFT_RIGHT;
        //                    else
        //                        BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
        //                    return true;
        //                }
        //                else
        //                    return false;
        //            }
        //            else
        //                return false;
        //        }
        //        else if (ball.Location.Y < labelIn.Location.Y + labelIn.Height && ball.Location.Y + ball.Height > labelIn.Location.Y + labelIn.Height)  //磚塊下邊
        //        {
        //            if (ball.Location.X < labelIn.Location.X && ball.Location.X + ball.Width > labelIn.Location.X)  //左下
        //            {
        //                if (ball.Location.X + ball.Width - labelIn.Location.X >= labelIn.Location.Y + labelIn.Height - ball.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.UP_DOWN;
        //                    else
        //                        BallHitBoard = BallHitObject.UP_DOWN;
        //                    return true;
        //                }
        //                else if (ball.Location.X + ball.Width - labelIn.Location.X < labelIn.Location.Y + labelIn.Height - ball.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.LEFT_RIGHT;
        //                    else
        //                        BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
        //                    return true;
        //                }
        //                else
        //                    return false;
        //            }
        //            else if (ball.Location.X >= labelIn.Location.X && ball.Location.X + ball.Width <= labelIn.Location.X + labelIn.Width)  //中間
        //            {
        //                if (hitwhat)
        //                    BallHitBricks = BallHitObject.UP_DOWN;
        //                else
        //                    BallHitBoard = BallHitObject.UP_DOWN;
        //                return true;
        //            }
        //            else if (ball.Location.X < labelIn.Location.X + labelIn.Width && ball.Location.X + ball.Width > labelIn.Location.X + labelIn.Width)  //右下
        //            {
        //                if (labelIn.Location.X + labelIn.Width - ball.Location.X >= labelIn.Location.Y + labelIn.Height - ball.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.UP_DOWN;
        //                    else
        //                        BallHitBoard = BallHitObject.UP_DOWN;
        //                    return true;
        //                }
        //                else if (labelIn.Location.X + labelIn.Width - ball.Location.X < labelIn.Location.Y + labelIn.Height - ball.Location.Y)
        //                {
        //                    if (hitwhat)
        //                        BallHitBricks = BallHitObject.LEFT_RIGHT;
        //                    else
        //                        BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
        //                    return true;
        //                }
        //                else
        //                    return false;
        //            }
        //            else
        //                return false;
        //        }
        //        else if (ball.Location.X < labelIn.Location.X && ball.Location.X + ball.Width > labelIn.Location.X)  //磚塊左邊
        //        {
        //            if (ball.Location.Y >= labelIn.Location.Y && ball.Location.Y + ball.Height <= labelIn.Location.Y + labelIn.Height)  //中間
        //            {
        //                if (hitwhat)
        //                    BallHitBricks = BallHitObject.LEFT_RIGHT;
        //                else
        //                    BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
        //                return true;
        //            }
        //            else
        //                return false;
        //        }
        //        else if (ball.Location.X < labelIn.Location.X + labelIn.Width && ball.Location.X + ball.Width > labelIn.Location.X + labelIn.Width)  //磚塊右邊
        //        {
        //            if (ball.Location.Y >= labelIn.Location.Y && ball.Location.Y + ball.Height <= labelIn.Location.Y + labelIn.Height)  //中間
        //            {
        //                if (hitwhat)
        //                    BallHitBricks = BallHitObject.LEFT_RIGHT;
        //                else
        //                    BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
        //                return true;
        //            }
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //    else
        //        return false;
        //}

        ///Ball Collision Label (Ball Hit Bricks [hitwhat = true] or Ball Hit Board [hitwhat = false]) > [For "SetBallHitBricks()" Use]
        private bool BallA_Collision_LabelB(Ball ball, Label labelIn, bool hitwhat)
        {
            if (ACollisionB(ball, labelIn))
            {
                ball.BallHitBoardDelayTime = 0;
                // X Direction
                if (PreBallA_Collision_B(ball, labelIn, true))
                {
                    if (hitwhat)
                        BallHitBricks = BallHitObject.LEFT_RIGHT;
                    else
                        BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
                }
                else if (PreBallA_Collision_B(ball, labelIn, false))
                {
                    if (hitwhat)
                        BallHitBricks = BallHitObject.UP_DOWN;
                    else
                        BallHitBoard = BallHitObject.UP_DOWN;
                }
                else
                {
                    if (hitwhat)
                        BallHitBricks = BallHitObject.BOARD_LEFT_RIGHT;
                    else
                        BallHitBoard = BallHitObject.BOARD_LEFT_RIGHT;
                }
                this.Sound_BallHitObject.Play();
                return true;
            }
            else
                return false;
        }



        private bool PreBallA_Collision_B(Ball A, Control B, bool IsXDirection)
        {
            Control PreA = new Control();
            PreA.Location = new Point(A.Location.X - A.X_Variable, A.Location.Y - A.Y_Variable);
            PreA.Width = A.Width;
            PreA.Height = A.Height;

            if (IsXDirection)
            {
                if (PreA.Location.Y > B.Location.Y - PreA.Height && PreA.Location.Y < B.Location.Y + B.Height)
                {
                    PreA.Dispose();
                    return true;
                }
                else
                {
                    PreA.Dispose();
                    return false;
                }
            }
            else
            {
                if (PreA.Location.X > B.Location.X - PreA.Width && PreA.Location.X < B.Location.X + B.Width)
                {
                    PreA.Dispose();
                    return true;
                }
                else
                {
                    PreA.Dispose();
                    return false;
                }
            }
        }

        ///Set Buff Hit Board
        public void SetBuffHitBoard(Buff buff, Label board)
        {
            //int boardWidth = labelIn.Width;
            //int boardHeight = labelIn.Height;
            //int boardX = labelIn.Location.X;
            //int boardY = labelIn.Location.Y;
            //Point CenPt = buff.CenPt;

            Buff_HitBoard = BuffHitBoard.NONE;
            BuffHitBoardType = BuffType.NONE;
            BuffLife = ObjectLife.NONE;

            //if ((CenPt.X - buff.Radius) > labelIn.Location.X - buff.Width
            //    && (CenPt.X - buff.Radius) < labelIn.Location.X + labelIn.Width
            //    && (CenPt.Y - buff.Radius) > labelIn.Location.Y - buff.Height
            //    && (CenPt.Y - buff.Radius) < labelIn.Location.Y + labelIn.Height

            //    /*buff.Location.X > labelIn.Location.X - buff.Width 
            //    && buff.Location.X < labelIn.Location.X + labelIn.Width 
            //    && buff.Location.Y > labelIn.Location.Y - buff.Height 
            //    && buff.Location.Y < labelIn.Location.Y + labelIn.Height*/)
            if (ACollisionB(buff, board))
            {
                this.Sound_BuffHitBoard.Play();

                if (buff.CurBuffType == BuffType.FAST)
                {
                    BuffHitBoardType = BuffType.FAST;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;
                }
                else if (buff.CurBuffType == BuffType.SLOW)
                {
                    BuffHitBoardType = BuffType.SLOW;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;
                }
                else if (buff.CurBuffType == BuffType.ELONGATION)
                {
                    BuffHitBoardType = BuffType.ELONGATION;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;
                }
                else if (buff.CurBuffType == BuffType.SHORTEN)
                {
                    BuffHitBoardType = BuffType.SHORTEN;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;
                }
                else if (buff.CurBuffType == BuffType.BULLET)
                {
                    BuffHitBoardType = BuffType.BULLET;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;

                    //sw.Reset();
                    //sw.Start();

                    //要倒數的時間，也可以倒數5秒，例：DateTime MyEndDate = DateTime.Now.AddSeconds(5);
                    //MyEndDate = new DateTime();

                    //this.CurStatusColumn.BulletTimeSec.Start();
                    //this.CurStatusColumn.BulletTimeStartStop = true;



                }
                else if (buff.CurBuffType == BuffType.FIVE_BALL)
                {
                    BuffHitBoardType = BuffType.FIVE_BALL;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;
                }
                else if (buff.CurBuffType == BuffType.TEN_BALL)
                {
                    BuffHitBoardType = BuffType.TEN_BALL;
                    Buff_HitBoard = BuffHitBoard.UP;
                    BuffLife = ObjectLife.DEAD;
                }
                else
                    return;
            }
            else
                return;
        }

        ///
        private bool ACollisionB(Control buff, Control board)
        {
            if (buff.Location.X >= board.Location.X - buff.Width
                && buff.Location.X <= board.Location.X + board.Width
                && buff.Location.Y >= board.Location.Y - buff.Height
                && buff.Location.Y <= board.Location.Y + board.Height)
            {
                return true;
            }
            else
                return false;
        }

        ///
        //private bool ACollisionB(Label brick, Label bullet)
        //{
        //    if (bullet.Location.X > brick.Location.X - bullet.Width
        //        && bullet.Location.X < brick.Location.X + brick.Width
        //        && bullet.Location.Y > brick.Location.Y - bullet.Height
        //        && bullet.Location.Y < brick.Location.Y + brick.Height)
        //    {
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        ///Set Buff Out Of Window
        public void SetBuffOutWindow(Buff buff, int formHeight)
        {
            Point CenPt = buff.CenPt;
            if (CenPt.Y - buff.Radius >= formHeight)
            {
                BuffLife = ObjectLife.DEAD;
            }
            else
                return;
        }

        ///Set Ball Number
        public void SetBallNumber(BuffType bufftype)
        {
            switch (bufftype)
            {
                case BuffType.FIVE_BALL:
                    BallNumber += 5;
                    BallVisibleNumber += 5;
                    break;
                case BuffType.TEN_BALL:
                    //BallNumber += 10;
                    //BallVisibleNumber += 10;
                    break;
            }
        }

        ///Set Bullet Hit Bricks
        public void SetBulletHitBrick(Label[,] bricks, Bullet bullet)
        {
            BulletHitBrick = BulletHitObject.NONE;
            BulletLife = ObjectLife.NONE;
            //ball.BallHitBoardDelayTime++;

            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].Visible == true)
                    {
                        //if (ball.BallHitBoardDelayTime > 5)
                        //{
                        if (ACollisionB(bricks[i, j], bullet))
                        {
                            BulletHitBrick = BulletHitObject.BRICKDOWN;
                            BulletLife = ObjectLife.DEAD;
                            HitRow = i;
                            HitColumn = j;
                            HitPosition = new Point(bricks[i, j].Location.X + bricks[i, j].Width / 2, bricks[i, j].Location.Y + bricks[i, j].Height);
                        }
                        //}
                    }
                }
            }
        }

        ///Set Bullet Hit Form Bound
        public void SetBulletHitFormBound(Bullet bullet)
        {
            BulletHitBrick = BulletHitObject.NONE;
            BulletLife = ObjectLife.NONE;

            if (bullet.Y_Variable + bullet.Height <= 0)  //上
            {
                BulletHitBrick = BulletHitObject.FORMUP;
                BulletLife = ObjectLife.DEAD;
            }
            else
                return;
        }

        ///判斷遊戲結束
        public void SetGameWinOver(Label[,] bricks)
        {
            if (this.CurRound.visiblecount == bricks.GetLength(0) * bricks.GetLength(1))
            {
                GameResult = new GameResultView();
                GameResult.SetGameWin();
                this.CurStatusColumn.BulletTimeSec.Stop();
                this.Sound_GameWin.Play();
                this.DeleteObject();
            }
            else if (this.BallNumber == 0 && BallVisibleNumber == 0)
            {
                GameResult = new GameResultView();
                GameResult.SetGameLose();
                this.CurStatusColumn.BulletTimeSec.Stop();
                this.Sound_GameLose.Play();                
                this.DeleteObject();
            }
            else
                return;
        }

        ///Delete Object
        private void DeleteObject()
        {
            this.CurBoard.Dispose();
            this.CurStatusColumn.Dispose();
            for (int i = 0; i < this.CurRound.Label_Bricks.GetLength(0); i++)
            {
                for (int j = 0; j < this.CurRound.Label_Bricks.GetLength(1); j++)
                {
                    this.CurRound.Label_Bricks[i, j].Dispose();                    
                }
            }            

            for (int i = 0; i < this.listCurBall.Count; i++)
            {
                this.listCurBall.RemoveAt(i);
                //this.listCurBall.ElementAt(i).Dispose();
            }

            for (int j = 0; j < this.listCurBuff.Count; j++)
            {
                this.listCurBuff.RemoveAt(j);
                //this.listCurBuff.ElementAt(j).Dispose();
            }
            GC.Collect();
        }

        ///Play Hit Brick Sound
        public void PlayHitBrickSound(BallHitObject ballhitbrick, BallHitObject ballhitform, BallHitObject ballhitboard, BuffHitBoard buffhitboard, BulletHitObject bullethitbrick) //播放撞擊音樂方法
        {
            //var HitBricks = new WMPLib.WindowsMediaPlayer();
            //HitBricks.URL = @"C:\Users\czh\Desktop\Course\碩二下\機械所物件導向程式設計\oop_game\sound\objecthitbricks.wav";      

            if (ballhitbrick != BallHitObject.NONE)
            {
                this.Sound_BallHitObject.Play();
            }
            else if (ballhitform != BallHitObject.NONE)
            {
                this.Sound_BallHitObject.Play();
            }
            else if (ballhitboard != BallHitObject.NONE)
            {
                this.Sound_BallHitObject.Play();
            }

            else if (buffhitboard != BuffHitBoard.NONE)
            {
                //this.Sound_BuffHitBoard.Play();
            }
            else if (bullethitbrick == BulletHitObject.BRICKDOWN)
            {
                this.Sound_BallHitObject.Play();
            }

        }


        ///Game Update
        public void GameUpdate(int formwidth, int formheight)
        {
            ///
            for (int i = 0; i < this.listCurBall.Count; i++)
            {
                if (this.listCurBall.ElementAt(i) != null)
                {
                    int FormWidth = formwidth - 200;
                    int FormHeight = formheight;
                    int BoardWidth = this.CurBoard.Width;
                    int BoardHeight = this.CurBoard.Height;
                    int BoardX = this.CurBoard.Board_X;
                    int BoardY = this.CurBoard.Board_Y;

                    //Set Ball Hit Form Bound
                    this.SetBallHitFormBound(FormWidth, FormHeight, this.listCurBall.ElementAt(i));

                    //Set Ball Hit Board
                    this.SetBallHitBoard(this.CurBoard, this.listCurBall.ElementAt(i));

                    //Set Ball Hit Bricks
                    this.SetBallHitBricks(this.CurRound.Label_Bricks, this.listCurBall.ElementAt(i));

                    //Set Bricks Status
                    this.CurRound.SetBrickStatus(this.BallHitBricks, this.HitRow, this.HitColumn);

                    //Set Ball Move
                    this.listCurBall.ElementAt(i).SetBallMove(this.BallHitFormRound, this.BallHitBricks, this.BallHitBoard);

                    //Set Ball Status
                    this.listCurBall.ElementAt(i).SetBallStatus(this.BallLife);

                    //If Ball Out Of Form Bound Remove From Ball List
                    if (this.listCurBall.ElementAt(i).BallVisible == false)
                    {
                        this.listCurBall.ElementAt(i).Dispose();
                        this.listCurBall.RemoveAt(i);
                        GC.Collect();
                    }

                    //this.listCurBall.ElementAt(i).SetBallStatus(this.CurLife);
                    //if (this.listCurBall.ElementAt(i).BallVisible == false)
                    //{
                    //    //bool remove = true;
                    //    //if (remove == true)
                    //    //{                            
                    //    this.listCurBall.RemoveAt(i);
                    //    //}
                    //}

                    //Set Game Result
                    this.SetGameWinOver(this.CurRound.Label_Bricks);
                }
            }

            ///
            for (int j = 0; j < this.listCurBuff.Count; j++)
            {
                if (this.listCurBuff.ElementAt(j) != null)
                {
                    if (this.listCurBuff.ElementAt(j).Visible == true)
                    {
                        int BoardWidth = this.CurBoard.Width;
                        int BoardHeight = this.CurBoard.Height;
                        int BoardX = this.CurBoard.Board_X;
                        int BoardY = this.CurBoard.Board_Y;

                        //Set Buff Move
                        this.listCurBuff.ElementAt(j).SetBuffMove();

                        //Set Buff Hit Board
                        this.SetBuffHitBoard(this.listCurBuff.ElementAt(j), this.CurBoard);

                        //Set Buff Out Of Window
                        this.SetBuffOutWindow(this.listCurBuff.ElementAt(j), formheight);

                        //Set Buff Status
                        this.listCurBuff.ElementAt(j).SetBuffStatus(this.Buff_HitBoard, this.BuffLife);

                        //If Buff Out Of Form Bound Or Hit Board, Remove From Ball List
                        if (this.listCurBuff.ElementAt(j).Visible == false)
                        {
                            this.listCurBuff.ElementAt(j).Dispose();
                            this.listCurBuff.RemoveAt(j);
                            GC.Collect();
                        }

                        //Set Board Status
                        this.CurBoard.SetBoardStatus(this.BuffHitBoardType);

                        //Set Ball Number
                        this.SetBallNumber(this.BuffHitBoardType);

                        //Set Ball Status [BuffHitBoardType]
                        for (int k = 0; k < this.listCurBall.Count; k++)
                        {
                            if (this.listCurBall.ElementAt(k) != null)
                            {
                                this.listCurBall.ElementAt(k).SetBallStatus(this.BuffHitBoardType);
                            }
                        }

                        this.CurStatusColumn.SetBulletTimerStart(this.BuffHitBoardType);
                        //this.CurStatusColumn.SetBulletTimerStop(this.CurStatusColumn.int_BulletTimeSec);
                    }
                }
            }

            //this.CurStatusColumn.SetBulletTimerStart(this.BuffHitBoardType);
            //this.CurStatusColumn.SetBulletTimerStop(this.CurStatusColumn.int_BulletTimeSec);

            for (int i = 0; i < this.listCurBullet_Left.Count; i++)
            {
                if (this.listCurBullet_Left.ElementAt(i) != null)
                {
                    this.listCurBullet_Left.ElementAt(i).SetBulletMove(this.CurBoard.Location.X);
                    SetBulletHitBrick(this.CurRound.Label_Bricks, this.listCurBullet_Left.ElementAt(i));
                    this.listCurBullet_Left.ElementAt(i).SetBulletStatus(this.BulletHitBrick, this.BulletLife);
                    this.CurRound.SetBrickStatus(this.BulletHitBrick, this.HitRow, this.HitColumn);
                    this.SetBulletHitFormBound(this.listCurBullet_Left.ElementAt(i));
                    if (this.listCurBullet_Left.ElementAt(i).Visible == false)
                    {
                        this.listCurBullet_Left.ElementAt(i).Dispose();
                        this.listCurBullet_Left.RemoveAt(i);
                        GC.Collect();
                    }
                }
            }


            for (int i = 0; i < this.listCurBullet_Right.Count; i++)
            {
                if (this.listCurBullet_Right.ElementAt(i) != null)
                {
                    this.listCurBullet_Right.ElementAt(i).SetBulletMove(this.CurBoard.Location.X);
                    SetBulletHitBrick(this.CurRound.Label_Bricks, this.listCurBullet_Right.ElementAt(i));
                    this.listCurBullet_Right.ElementAt(i).SetBulletStatus(this.BulletHitBrick, this.BulletLife);
                    this.CurRound.SetBrickStatus(this.BulletHitBrick, this.HitRow, this.HitColumn);
                    this.SetBulletHitFormBound(this.listCurBullet_Right.ElementAt(i));
                    if (this.listCurBullet_Right.ElementAt(i).Visible == false)
                    {
                        this.listCurBullet_Right.ElementAt(i).Dispose();
                        this.listCurBullet_Right.RemoveAt(i);
                        GC.Collect();
                    }
                }
            }

            //Set Ball Number In Status Column
            this.CurStatusColumn.SetBallNumberInStatusColumn(this.BallNumber);

            //this.PlayHitBrickSound(BallHitBricks, BallHitFormRound, BallHitBoard, Buff_HitBoard, BulletHitBrick);


        }
    }
}
