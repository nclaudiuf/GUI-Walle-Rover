using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace RoverGUI.webCam
{    
    class YCrCbSkinDetector : IColorSkinDetector
    {
        /// <Variables>
        Contour<Point> i;
        /// </End Variables>

        public override Image<Gray, byte> DetectSkin(Image<Bgr, Byte> originalImg, IColor min, IColor max)                              //overide metod DetectSkin by filtering the skin Color and return it as image 
        {
            Image<Ycc, Byte> currentYCrCbFrame = originalImg.Convert<Ycc, Byte>();                                                      //Convert the originial Image by image color YCbCr
            Image<Gray, byte> skin = new Image<Gray, byte>(originalImg.Width, originalImg.Height);
            skin = currentYCrCbFrame.InRange((Ycc)min, (Ycc)max);
            StructuringElementEx rect_12 = new StructuringElementEx(                                                                    //Create a structuring element of the specific type
                2,                                                                                                                     //define number of columns in the structuring element
                2,                                                                                                                     //define number of rows in the structuring element
                1,                                                                                                                      //define relative horizontal offset of the anchor point
                1,                                                                                                                      //define relative vertical offset of the anchor point
                Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);                                                                         //shape of the structuring element:	
            //skin.FindContours()
            skin = skin.ThresholdBinary(new Gray(100), new Gray(255));
            using (MemStorage storage = new MemStorage())
            {
                 i = skin.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                                                             Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL,
                                                             storage);
                 //MessageBox.Show(""+i.Perimeter);
            }
           // CvInvoke.cvFilter2D(
            //CvInvoke.cvBoundingRect(i, true);                                                                                                                                                                                                                                                                                                                           
            CvInvoke.cvErode(skin, skin, rect_12, 1);
            StructuringElementEx rect_6 = new StructuringElementEx(2, 2, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
            CvInvoke.cvDilate(                                                                                                                                             //apply dilation operator of methematical morphology
                skin,                                                                                                                                                      //dilatation of each pixel of the image 
                skin,
                rect_6,                                                                                                                                                    //dilatation of structuring element 
                2);   
            
            return skin;
        }
    }    
}
