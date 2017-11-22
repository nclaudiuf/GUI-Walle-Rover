
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using RoverGUI.SocketServerTCP;
using RoverGUI.webCam;

namespace RoverGUI
{
    public partial class Rover : Form
    {
        /// <Declaring Variable> 
        Server config = new Server();
        Boolean connectionStatus = false;
        Boolean _bFullScreenMode = false;
        ComputeXmlFiles computeXML;
        HandTracking ht;
        Capture capWebCam = null;
        bool blnCapturingInProcess = false;
        Image<Bgr, Byte> imgOriginal;
        Image<Gray, Byte> imgProcessed;                                                             //for testing purpose
        List<Rectangle> faces;
        IColorSkinDetector skinDetector;
        AdaptiveSkinDetector detector;
        int frameWidth;
        int frameHeight;
        Ycc YCrCb_min;
        Ycc YCrCb_max;
        Seq<Point> hull;
        Seq<Point> filteredHull;
        Seq<Point> hand;
        Seq<MCvConvexityDefect> defects;
        MCvConvexityDefect[] defectArray;
        Rectangle handRect;
        MCvBox2D box;
        Ellipse ellip;
        MemStorage storage;
        int totalDefects = 0;
        Contour<Point> biggestContour;
        /// </End declaretion>
        
        public Rover()
        {        
            InitializeComponent();
            connectButton.BackColor = Color.Red;

 /*           while (!config.clientConnected())
            {
                if (connectButton.Text.Equals("Connect"))
                {
                    disconnect();
                }
            }
*/
            #region menu Display
            ibOriginal.Visible = true;
            btnPauseOrResume.Visible = true;
            ibOriginal.Size = new System.Drawing.Size(491, 428);
            Load();
            #endregion

            #region Keyboard&Mouse Event Handlers
            this.KeyDown += new KeyEventHandler(keyboardControl);                                   //Event for keyboard control
            this.KeyPreview = true;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);    //Event hanlder for keyboard f11
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);               //Event hanlder for keyboard f11        
            #endregion 
        }

        private void Rover_Load(object sender, EventArgs e)
        {
            try
            {
                ibOriginal.Visible = true;
                capWebCam = new Capture();                                                              //associate capture object to the default webcam                                                                    //associate Memory storage to the deafult memory 
                //capWebCam.Start();                                            
                frameWidth = capWebCam.Width;                                                           //get Width of Camera 
                frameHeight = capWebCam.Height;                                                         //get Height of Camera
                detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);
                YCrCb_min = new Ycc(0, 137, 77);                                                        // set the min values for Y luma , cr red difference, cb blue difference
                YCrCb_max = new Ycc(255, 170, 127);                                                     //set the max values for Y, cr, cb      
                box = new MCvBox2D();
                ellip = new Ellipse();
                storage = new MemStorage();
                ht = new HandTracking();
                computeXML = new ComputeXmlFiles();
            }
            catch (NullReferenceException ex)                                                           //catch error if unssucesfull 
            {
                MessageBox.Show("" + ex);
            };
            //once we have capture object           
            Application.Idle += procesFrameAndUpdateGUI;                                                //add process imgae function to the application`s list of tasks
            blnCapturingInProcess = true;
        }

        private void Load()
        {
            try
            {
                ibOriginal.Visible = true;
                capWebCam = new Capture();                                                              //associate capture object to the default webcam                                                                    //associate Memory storage to the deafult memory 
                //capWebCam.Start();                                            
                frameWidth = capWebCam.Width;                                                           //get Width of Camera 
                frameHeight = capWebCam.Height;                                                         //get Height of Camera
                detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);
                YCrCb_min = new Ycc(0, 137, 77);                                                        // set the min values for Y luma , cr red difference, cb blue difference
                YCrCb_max = new Ycc(255, 170, 127);                                                     //set the max values for Y, cr, cb      
                box = new MCvBox2D();
                ellip = new Ellipse();
                storage = new MemStorage();
                ht = new HandTracking();
                computeXML = new ComputeXmlFiles();
            }
            catch (NullReferenceException ex)                                                           //catch error if unssucesfull 
            {
                MessageBox.Show("" + ex);
            };
            //once we have capture object           
            Application.Idle += procesFrameAndUpdateGUI;                                                //add process imgae function to the application`s list of tasks
            blnCapturingInProcess = true;

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)                                     //Compute a full Screen by pressing f11
        {
            if (e.KeyData == Keys.F11)
            {
                if (_bFullScreenMode == false)
                {
                    Rover f = new Rover();
                    this.Owner = f;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.Left = (Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2);
                    this.Top = (Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
                    _bFullScreenMode = true;
                    f.KeyUp += new KeyEventHandler(Form1_KeyUp);
                }
                else
                {
                    Form f = this.Owner;
                    this.Owner = null;
                    f.Close();

                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    _bFullScreenMode = false;
                }
            }
        }

        private void Main_Closed(Object sender, FormClosedEventArgs e)
        {
            if (capWebCam != null)
            {
                capWebCam.Dispose();                                                                     //update memeber flag variable
                config.sendAction("X");
            }
        }

        private void procesFrameAndUpdateGUI(Object sender, EventArgs arg)
        {
            String letter = "";
            skinDetector = new YCrCbSkinDetector();
            imgOriginal = capWebCam.QueryFrame();                                                       //get the next Frame from the webcam                                   
            if (imgOriginal == null) return;                                                             //if we did not get a frame       
            Image<Gray, Byte> skin = skinDetector.DetectSkin(imgOriginal, YCrCb_min, YCrCb_max);        //use YCrCb family of color spaces to detect the skin differences with other objects                                              
            ibOriginal.Image = imgOriginal;
            #region testing
            /*
             *for testing the Gray image after aplying the skin detector 
             *replace the imgOriginal with imgProcessed 
             *skin dector has light problems, so computing the biggest contour of the hand will be afect because of the light, for computing correctly the counting of the fingers need a not very lighted room; 
             * 
            imgProcessed = skin.Copy();
            ibOriginal.Image = imgProcessed;
            //ibOriginal.Image = imgOriginal;
            */
            #endregion
            ExtractContourAndHull(skin);                                                                 //extract the contour and hull

            #region computing multiple detections using xml haar cascade
            int faces, hands, handclosed, handLetter = 0;
            Boolean stop = true ;
            Boolean go = false;
            faces = computeXML.FaceDetection(imgOriginal);
            hands = computeXML.HandDetection(imgOriginal);
            handclosed = computeXML.HandDetectionClosed(imgOriginal);
            handLetter = computeXML.Letters(imgOriginal);

            if (faces != 0)                                                                             //if face is found 
            {
                computeXML.displayOnImageFace(imgOriginal, "Face Detected");                            //display message on the screen                                                           
            }
            else if (faces == 0 && hands != 0 || handLetter != 0 || handLetter == 0)
            {
                ComputeNrFingersAndDraw();                                                              //compute number of fingers 
            }
            if (hands != 0 && handLetter == 0)                                                                           //if hand is found 
            {
                computeXML.displayOnImageHand(imgOriginal, "Hand Detected - Stop");                            //STOP ROVER
                if (connectionStatus && blnCapturingInProcess && go && !stop )
                   // config.sendAction("O");
                    go = false;
                    stop = true;
                    Thread.Sleep(1000);
            }
            if (handLetter != 0 && hands == 0)                                                                         //if hand Letter A found 
            {  
                computeXML.displayOnImageHandLetterA(imgOriginal, "Letter A Detected - Move");                  //MOVING ROVER 
                 if (connectionStatus && blnCapturingInProcess && stop && !go )
                 //   config.sendAction("F");
                    go = true;
                    stop = false;
                    Thread.Sleep(1000);
            }
            #endregion
        }

        private void ExtractContourAndHull(Image<Gray, byte> skin)
        {
            biggestContour = ht.ExtractBiggestContour(skin);

            if (biggestContour != null)
            {
                //imgOriginal.Draw(biggestContour, new Bgr(Color.DarkViolet), 2);                                            //draw the biggest contour 
                Contour<Point> currentContour = biggestContour.ApproxPoly(biggestContour.Perimeter * 0.0025, storage);
                //imgOriginal.Draw(currentContour, new Bgr(Color.LimeGreen), 2);                                             //draw the contour 
                biggestContour = currentContour;

                hull = biggestContour.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);                                //use the convex hull algorithm to find the points of the biggest contour          
                box = biggestContour.GetMinAreaRect();
                PointF[] points = box.GetVertices();
                Point[] ps = new Point[points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    ps[i] = new Point((int)points[i].X, (int)points[i].Y);
                }

                hull = new Seq<Point>(storage);
                for (int i = 0; i < hull.Total; i++)
                {
                    if (Math.Sqrt(Math.Pow(hull[i].X - hull[i + 1].X, 2) + Math.Pow(hull[i].Y - hull[i + 1].Y, 2)) > box.size.Width / 10)
                    {
                        hull.Push(filteredHull[i]);
                    }
                }

                defects = biggestContour.GetConvexityDefacts(storage, Emgu.CV.CvEnum.ORIENTATION.CV_COUNTER_CLOCKWISE);
                totalDefects = defects.Total;
                defectArray = defects.ToArray();

                imgOriginal.Draw(new CircleF(ht.FindCentroidByDistanceTrans(skin), 5), new Bgr(200, 125, 75), 2);             //draw the center of dectected object                        

            }
        }

        private void ComputeNrFingersAndDraw()
        {
            int fingerNum = 0;

            #region defects drawing
            for (int i = 0; i < totalDefects; i++)
            {
                PointF startPoint = new PointF((float)defectArray[i].StartPoint.X,
                                                (float)defectArray[i].StartPoint.Y);

                PointF depthPoint = new PointF((float)defectArray[i].DepthPoint.X,
                                                (float)defectArray[i].DepthPoint.Y);

                PointF endPoint = new PointF((float)defectArray[i].EndPoint.X,
                                                (float)defectArray[i].EndPoint.Y);

                LineSegment2D startDepthLine = new LineSegment2D(defectArray[i].StartPoint, defectArray[i].DepthPoint);

                LineSegment2D depthEndLine = new LineSegment2D(defectArray[i].DepthPoint, defectArray[i].EndPoint);

                CircleF startCircle = new CircleF(startPoint, 5f);

                CircleF depthCircle = new CircleF(depthPoint, 5f);

                CircleF endCircle = new CircleF(endPoint, 5f);

                //Custom heuristic based on some experiment, double check it before use
                if ((startCircle.Center.Y < box.center.Y || depthCircle.Center.Y < box.center.Y) && (startCircle.Center.Y < depthCircle.Center.Y) && (Math.Sqrt(Math.Pow(startCircle.Center.X - depthCircle.Center.X, 2) + Math.Pow(startCircle.Center.Y - depthCircle.Center.Y, 2)) > box.size.Height / 6.5))
                {
                    fingerNum++;
                    //imgOriginal.Draw(startDepthLine, new Bgr(Color.Green), 2);                                                        //testing purposes 
                    //imgOriginal.Draw(depthEndLine, new Bgr(Color.Magenta), 2);                                                        //testing purposes                 
                }
                //imgOriginal.Draw(startCircle, new Bgr(Color.Red), 2);                                                                 //testing purposes 
                //imgOriginal.Draw(depthCircle, new Bgr(Color.Yellow), 5);                                                              //testing purposes           
                //imgOriginal.Draw(endCircle, new Bgr(Color.DarkBlue), 4);                                                              //testing purposes 
            }
            #endregion
            /*
             * Computing the number of fingers 
             * For a good and acurate computing 
             */
            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_DUPLEX, 0.5d, 0.5d);
            imgOriginal.Draw("Numbers of fingers: " + fingerNum.ToString(), ref font, new Point(10, 400), new Bgr(Color.White));     //display the number of fingers on imagebox                           
        }

        private void btnPauseOrResume_Click(object sender, EventArgs e)
        {
            if (blnCapturingInProcess == true)                                                           //if we are currently processing an image, user choose to pause
            {
                Application.Idle -= procesFrameAndUpdateGUI;                                             //remove the process image function from the application`s list of tasks
                blnCapturingInProcess = false;                                                           //update flag variable
                btnPauseOrResume.Text = "Resume";                                                        //update button text 
            }
            else                                                                                         //if we are currently processing an image, user choose to resume
            {
                Application.Idle += procesFrameAndUpdateGUI;                                             //add process imgae function to the application`s list of tasks
                blnCapturingInProcess = true;                                                            //update flag variable
                btnPauseOrResume.Text = "Pause";                                                         //update button tex                
            }
        }

        private void webCamTools(object sender, EventArgs e)
        {
            if (ibOriginal.Visible.Equals(false))
            {
                ibOriginal.Visible = true;
                ibOriginal.Size = new System.Drawing.Size(613, 449);
                Load();
                btnPauseOrResume.Visible = true;
            }
            else if (ibOriginal.Visible.Equals(true))
            {
                btnPauseOrResume.Visible = false;
                ibOriginal.Visible = false;
            }
        }

        private void sendMouseClick(object sender, EventArgs e)
        {
            Boolean statusAction;

            if (sendTextBox.Text.Equals(""))
                MessageBox.Show("No action inserted!");
            else
                if (statusAction = config.sendAction(sendTextBox.Text))
                    MessageBox.Show("Action " + sendTextBox.Text);
                else
                    MessageBox.Show("Action faild to send!");               
        }

        /*
        private void updateUI(String s)
        {
            Func<int> del = delegate()
            {
                listBox.Text = s + System.Environment.NewLine;
                return 0;
            };
            Invoke(del);
        }
         * */

        public void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("You pressed a keyboard key." + e.KeyCode.ToString());
        }

        public void keyboardControl(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
            try
            {
                MessageBox.Show("" + config.checkConnection());
               // if (config.checkConnection())
               // {
                    if (e.KeyCode.ToString().Equals("W"))
                    {
                        // MessageBox.Show("W Front");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("W");
                    }
                    if (e.KeyCode.ToString().Equals("A"))
                    {
                        // MessageBox.Show("A Left");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("A");
                    }
                    if (e.KeyCode.ToString().Equals("S"))
                    {
                        //MessageBox.Show("S Back");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("S");
                    }
                    if (e.KeyCode.ToString().Equals("D"))
                    {
                        // MessageBox.Show("D Right");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("D");
                    }
                    if (e.KeyCode.ToString().Equals("H"))
                    {
                        // MessageBox.Show("G Shift Gear Down");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("H");
                    }
                    if (e.KeyCode.ToString().Equals("G"))
                    {
                        // MessageBox.Show("G Shift Gear Up");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("G");
                    }
                    if (e.KeyCode.ToString().Equals("C"))
                    {
                        // MessageBox.Show("C Spin");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("C");
                    }
                    if (e.KeyCode.ToString().Equals("Space"))       //Check space
                    {
                        // MessageBox.Show("Stop");
                        // sendTextBox.Text = e.KeyCode.ToString();
                        config.sendAction("O");
                    }
               //}
            }
            catch (Exception ex) { }

            if (e.KeyCode.ToString().Equals("Return"))                                                       //Connect the Rover
            {
                connectRover();
            }
        }

        private void connectMouseClick(object sender, EventArgs e)
        {
            connectRover();
        }

        private void connectRover()
        {          
            if (connectButton.Text.Equals("Connect"))
            {            
                try
                {
                    connectButton.Text = "Disconnect";
                    connectButton.BackColor = Color.Green;
                    config.runServer();
                    config.startServer();                  
                }
                catch (NullReferenceException ex)
                {
                    disconnect();
                    MessageBox.Show(ex.ToString(), "Error during the connection!");
                }
            }
            else if (connectButton.Text.Equals("Disconnect"))
            {
                disconnect();
            }
        }

        private void disconnect()
        {
            config.closeServer();
            config.sendAction("X");
            connectionStatus = false;
            connectButton.Text = "Connect";
            connectButton.BackColor = Color.Red;
            sendTextBox.Clear();           
        }
    }
}
