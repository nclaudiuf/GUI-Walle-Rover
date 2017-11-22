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
     // hand tracking class which implements the hand tracking algorithm  
    class HandTracking
    {
        /// <Variables>
        int kernel_size = 3;
        DIST_TYPE dt = DIST_TYPE.CV_DIST_L2;
        /// </End Variables>
         public Contour<Point> ExtractBiggestContour(Image<Gray, byte> local)
        {
            Contour<Point> biggestContour = null;
            MemStorage storage = new MemStorage();
            Contour<Point> contours = FindContours(local, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);

            Double Result1 = 0;
            Double Result2 = 0;
            while (contours != null)
            {
                Result1 = contours.Area;
                if (Result1 > Result2)
                {
                    Result2 = Result1;
                    biggestContour = contours;
                }
                contours = contours.HNext;
            }
            return biggestContour;
        }

        public Contour<Point> FindContours(Image<Gray, byte> local, CHAIN_APPROX_METHOD cHAIN_APPROX_METHOD, RETR_TYPE rETR_TYPE, MemStorage stor)
        {
            using (Image<Gray, byte> imagecopy = local.Copy()) //since cvFindContours modifies the content of the source, we need to make a clone
            {
                IntPtr seq = IntPtr.Zero;
                CvInvoke.cvFindContours(
                    imagecopy.Ptr,
                    stor.Ptr,
                    ref seq,
                    StructSize.MCvContour,
                    rETR_TYPE,
                    cHAIN_APPROX_METHOD,
                    new Point(local.ROI.X, local.ROI.Y));// because of ROI, the contour is offset or shifted 

                return (seq == IntPtr.Zero) ? null : new Contour<Point>(seq, stor);
            }
        }

        /// <summary>
        /// find the center of an object of a transform image
        /// </summary>
        /// <param name="binary_image"></param>
        /// <returns></returns>
        public PointF FindCentroidByDistanceTrans(Image<Gray, byte> binary_image)
        {
            double max_value = 0.0d,
                   min_value = 0.0d;
            Point max_location = new Point(0, 0),
                  min_location = new Point(0, 0);
            using (Image<Gray, float> distTransform = new Image<Gray, float>(binary_image.Width, binary_image.Height))
            {
                CvInvoke.cvDistTransform(binary_image, distTransform, dt, kernel_size, null, IntPtr.Zero);
                CvInvoke.cvMinMaxLoc(distTransform, ref min_value, ref max_value, ref min_location, ref max_location, IntPtr.Zero);
            }
            return max_location;
        }             
    }
}

