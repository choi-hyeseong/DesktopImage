using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopImage {

    public class ImageData {
        //nullable, path가 들어올때 무조건 이미지 파일.

        //private MoveStrategy strategy;
        private string? imgPath {
            get;
            set;
        }

        private bool isRunning = false;

        public event EventHandler<ImagePathArg>? ImageChangeEvent;

        public bool ImageExist() {
            return imgPath != null && File.Exists(imgPath);
        }

        public void Start() {
            isRunning = true;
        }

        public void Stop() {
            isRunning = false;
        }

        public bool IsRunning() {
            return isRunning;
        }
        public void NotifyChange() {
            if (ImageChangeEvent != null)
                ImageChangeEvent.Invoke(this, new ImagePathArg(imgPath));
        }
        public void SetImagePath(string path) {
            imgPath = path;
            if (isRunning)
                NotifyChange();
        }
       

    }

    public class ImagePathArg : EventArgs{
        public string? path {
            get;
            set;
        }

        public ImagePathArg(string? v) {
            path = v;
        }
    }

}
