using System;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace Watchdog
{
    internal class PhotoCatcher
    {
        private static PhotoCatcher _photoCatcher;
        private readonly object _objectLock = new object();
        private Bitmap _photo;
        private volatile int _clients;

        public static PhotoCatcher GetPhotoCatcher()
        {
            return _photoCatcher ?? (_photoCatcher = new PhotoCatcher());
        }

        public Bitmap GetPhoto()
        {
            _clients++;
            if (_clients == 1)
            {
                lock (_objectLock)
                {
                    _photo = TakePhotoFromCam();
                }
            }
            while (_photo == null)
            {
                Thread.Sleep(500);
            }
            _clients--;
            if (_clients != 0)
            {
                lock (_objectLock)
                {
                    return _photo;
                }
            }
            Bitmap resultPhoto;
            lock (_objectLock)
            {
                resultPhoto = _photo;
                _photo = null;
            }
            return resultPhoto;
        }

        private Bitmap TakePhotoFromCam()
        {
            Capture capture = new Capture();
            IntPtr image = capture.GetImage();
            return new Bitmap(capture.Width, capture.Height, capture.Stride, PixelFormat.Format24bppRgb, image);
        }

    }
}
