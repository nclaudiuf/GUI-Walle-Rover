using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.GPU;
using System.Windows.Forms;

namespace RoverGUI.webCam
{
    public class DetectMatches
    {       
        public void detectHands(Image<Bgr, Byte> image, String xmls, List<Rectangle> hands)
        {          
                using (CascadeClassifier hand = new CascadeClassifier(xmls))                                //Read the HaarCascade objects
                {
                    using (Image<Gray, Byte> gray = image.Convert<Gray, Byte>())                            //Convert it to Grayscale
                    {
                        gray._EqualizeHist();                                                               //normalizes brightness and increases contrast of the image                    
                                                                                                            //The first dimensional is the channel
                                                                                                            //The second dimension is the index of the rectangle in the specific channel
                        Rectangle[] handsDetected = hand.DetectMultiScale(                                  //Detect the hands  from the gray scale image and 
                           gray,
                           1.1,
                           10,
                           new Size(10, 10),
                           Size.Empty);
                        hands.AddRange(handsDetected);                                                      //store the hand locations as rectangle
                    }
                }
            }
        
        public void detectHandClosed(Image<Bgr, Byte> image, String xmls, List<Rectangle> hands)
        {
            using (CascadeClassifier hand = new CascadeClassifier(xmls))                                    //Read the HaarCascade objects
            {
                using (Image<Gray, Byte> gray = image.Convert<Gray, Byte>())                                //Convert it to Grayscale
                {
                                            gray._EqualizeHist();                                           //normalizes brightness and increases contrast of the image                    
                                                                                                            //The first dimensional is the channel
                                                                                                            //The second dimension is the index of the rectangle in the specific channel
                    Rectangle[] handsDetected = hand.DetectMultiScale(                                      //Detect the hands  from the gray scale image and 
                       gray,
                       1.1,
                       10,
                       new Size(20, 20),
                       Size.Empty);
                    hands.AddRange(handsDetected);                                                          //store the hand locations as rectangle
                }
            }
        }

        public void detectFaces(Image<Bgr, Byte> image, String xmls, List<Rectangle> faces)
        {
            using (CascadeClassifier face = new CascadeClassifier(xmls))
            {
                using (Image<Gray, Byte> gray = image.Convert<Gray, Byte>())                                 //Convert it to Grayscale
                {
                                            gray._EqualizeHist();                                            //normalizes brightness and increases contrast of the image                     
                                                                                                             //The first dimensional is the channel
                                                                                                             //The second dimension is the index of the rectangle in the specific channel
                    Rectangle[] facesDetected = face.DetectMultiScale(                                       //Detect the faces  from the gray scale image
                       gray,
                       1.1,
                       10,
                       new Size(20, 20),
                       Size.Empty);
                    faces.AddRange(facesDetected);                                                          //store the face locations as rectangle
                }
            }
        }

        public void detectHandLetterA(Image<Bgr, Byte> image, String xmls, List<Rectangle> letterIn)            //detect the hand letter A of the American Alphabet 
        {
            using (CascadeClassifier letter = new CascadeClassifier(xmls))
            {
                using (Image<Gray, Byte> gray = image.Convert<Gray, Byte>())                                 //Convert it to Grayscale
                {
                    gray._EqualizeHist();                                                                    //normalizes brightness and increases contrast of the image                     
                                                                                                             //The first dimensional is the channel
                                                                                                             //The second dimension is the index of the rectangle in the specific channel
                    Rectangle[] letterDetected = letter.DetectMultiScale(                                       //Detect the faces  from the gray scale image
                       gray,
                       1.1,
                       10,
                       new Size(20, 20),
                       Size.Empty);
                    letterIn.AddRange(letterDetected);                                                           //store the face locations as rectangle
                }
            }
        }
    }
}