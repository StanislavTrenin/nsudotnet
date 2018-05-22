using System.Threading;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Watchdog
{
    internal class PhotoCatcher
    {
        private static PhotoCatcher _photoCatcher;
        private readonly object _objectLock = new object();
        private Bitmap _photo;
        private volatile int _clients;
        private VideoCaptureDevice _device;

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
                    TakePhotoFromCam();
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

        private void TakePhotoFromCam()
        {
            var filter = new FilterInfoCollection(FilterCategory.VideoInputDevice)[0];
            _device = new VideoCaptureDevice(filter.MonikerString);
            _device.NewFrame += DeviceNewFrame;
            _device.Start();
        }

        private void DeviceNewFrame(object sender, NewFrameEventArgs e)
        {
            _photo?.Dispose();
            _photo = (Bitmap)e.Frame.Clone();
            if (_device == null) return;
            _device.NewFrame -= DeviceNewFrame;
            _device.SignalToStop();
            _device = null;
        }

    }
}
