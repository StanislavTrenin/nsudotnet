using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace Watchdog
{
    internal sealed class PhotoCatcher
    {
        private static volatile PhotoCatcher _photoCatcher;
        private static readonly object SyncRoot = new object();
        private static readonly object ObjectLock = new object();
        private Bitmap _photo;
        private volatile int _clients;

        private PhotoCatcher()
        {
        }

        public static PhotoCatcher Instance
        {
            get
            {
                if (_photoCatcher != null) return _photoCatcher;
                lock (SyncRoot)
                {
                    if (_photoCatcher == null)
                    {
                        _photoCatcher = new PhotoCatcher();
                    }
                }

                return _photoCatcher;
            }
        }

        public Bitmap GetPhoto()
        {
            _clients++;
            if (_clients == 1)
            {
                lock (ObjectLock)
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
                lock (ObjectLock)
                {
                    return _photo;
                }
            }
            Bitmap resultPhoto;
            lock (ObjectLock)
            {
                resultPhoto = _photo;
                _photo = null;
            }
            return resultPhoto;
        }

        private static Bitmap TakePhotoFromCam()
        {
            Bitmap bitmap;
            using (var capture = new Capture())
            {
                var image = capture.GetImage();
                bitmap = new Bitmap(capture.Width, capture.Height, capture.Stride, PixelFormat.Format24bppRgb, image);
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            return bitmap;
        }

    }
}
