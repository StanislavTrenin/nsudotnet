﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Watchdog
{
    internal sealed class PhotoCatcher : IDisposable
    {
        private static volatile PhotoCatcher _photoCatcher;
        private static readonly Capture _capture = new Capture(); 
        private static readonly object SyncRoot = new object();
        private static readonly object ObjectLock = new object();
        private static volatile Bitmap _photo;
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
            lock (ObjectLock)
            {
                _clients++;
                if (_photo == null)
                {
                    _photo = TakePhotoFromCam();
                }
            }
            lock (ObjectLock)
            {
                if (_clients != 1)
                {
                    _clients--;
                    return _photo;
                }
                else
                {
                    var resultPhoto = _photo;
                    _photo = null;
                    _clients--;
                    return resultPhoto;
                }
            }
        }

        private static Bitmap TakePhotoFromCam()
        {
            var image = _capture.GetImage();
            Bitmap bitmap = new Bitmap(_capture.Width, _capture.Height, _capture.Stride, PixelFormat.Format24bppRgb, image);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }

        public void Dispose()
        {
            _capture.Dispose();
            if (_photo != null) {
                _photo.Dispose();
            }
        }

    }
}
