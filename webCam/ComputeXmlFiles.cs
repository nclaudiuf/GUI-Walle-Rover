using System;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using System.Windows.Forms;

namespace RoverGUI.webCam
{
    class ComputeXmlFiles
    {
        /// <Variables>
        DetectMatches dm = new DetectMatches();
        /// </Variables>
       
        public int FaceDetection(Image<Bgr, Byte> imgface)
        {
            List<Rectangle> faces = new List<Rectangle>();                                       //hold the face dimension
            try
            {
                dm.detectFaces(imgface, "xmlFiles/haarcascade_frontalface.xml", faces);          //call method to detect the face 
            }
            catch (Exception ex) { MessageBox.Show("" + ex); }                                   //error catch 
            foreach (Rectangle face in faces)                                                    //loop throught 
            {
                imgface.Draw(face, new Bgr(Color.Red), 2);                                       //if face found draw rectangle of the image
            }
            return faces.Count;                                                                  //return nr of faces found 
        }

        public int HandDetection(Image<Bgr, Byte> imghand)
        {
            List<Rectangle> hands = new List<Rectangle>();                                       //hold the hand dimension
            try
            {
                dm.detectHands(imghand, "xmlFiles/palm.xml", hands);                                      //call method to detect the face 
            }
            catch (Exception ex) { MessageBox.Show("" + ex); }                                   //error catch 
            
            return hands.Count;                                                                  //return nr of hands found 
        }

        public int HandDetectionClosed(Image<Bgr, Byte> imghandClosed)
        {
            List<Rectangle> hands = new List<Rectangle>();                                       //hold the face dimension
            try
            {
                dm.detectHandClosed(imghandClosed, "xmlFiles/closed_palm.xml", hands);                    //call method to detect the face 
            }
            catch (Exception ex) { MessageBox.Show("" + ex); }                                   //error catch 
            
            return hands.Count;                                                                  //return nr of hands found 
        }

        public int Letters(Image<Bgr, Byte> imgHandLetterA)
        {
            List<Rectangle> handletter = new List<Rectangle>();                                  //hold the hand letter A of American Alphabet dimension
            try
            {
                dm.detectHandLetterA(imgHandLetterA, "xmlFiles/haarcascade_letterA.xml", handletter);       //call method to detect the face 
            }
            catch (Exception ex) { MessageBox.Show("" + ex); }                                    //error catch 
            
            return handletter.Count;                                                              //return 1 if found 0 if not 
        }

        #region display on image 
        public void displayOnImageFace(Image<Bgr, Byte> img, String msg)                                                     //for displaying on the image
        {
            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_DUPLEX, 0.5d, 0.5d);
            img.Draw(msg, ref font, new Point(10, 20), new Bgr(Color.White));
        }

        public void displayOnImageHand(Image<Bgr, Byte> img, String msg)                                                     //for displaying on the image
        {
            MCvFont font = new MCvFont(                                                                 //create the font
                Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX,                                            //image text font style           
                0.5f,                                                                                   //declare hsScale 
                0.5f);                                                                                  //declare vsScale 
            img.Draw(msg, ref font, new Point(10, 40), new Bgr(Color.White));                   //draw the message on the image 
        }

        public void displayOnImageHandLetterA(Image<Bgr, Byte> img, String msg)                                               //for displaying on the image
        {
            MCvFont font = new MCvFont(                                                                 //create the font
                Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX,                                            //image text font style           
                0.5f,                                                                                   //declare hsScale 
                0.5f);                                                                                  //declare vsScale 
            img.Draw(msg, ref font, new Point(10, 60), new Bgr(Color.White));                   //draw the message on the image 
        }
        #endregion
    }
}
