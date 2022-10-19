namespace DesktopImage {

    //MVC Pattern - Controller
    public class ImageController {

        //실질적 접근은 컨트롤러에서 하므로 생성자에서 생성하는쪽으로..
        private ImageData data; 
        private MainForm mainForm;
       

        public ImageController() {
            data = new ImageData();
            mainForm = new MainForm();
        }

        public void Start() {
            if (CanStart() && !data.IsRunning()) {
                mainForm.Show();
                data.ImageChangeEvent += new EventHandler<ImagePathArg>(mainForm.DrawImage);
                data.NotifyChange(); //해주는 이유? 폼 실행전 이미지 경로가 지정되었으므로.
                data.Start();
            }
            else if (data.IsRunning()) {
                MessageBox.Show("이미 실행중입니다.");
            }
            else {
                MessageBox.Show("파일이 지정되어 있지 않거나 존재하지 않습니다. 파일을 다시 지정해주세요.");
            }
        }

        private bool CanStart() {
            return data.ImageExist();
        }

        public void Stop() {
            data.Stop();
            Application.Exit();
        }

        public void SetImagePath(string path) {
            data.SetImagePath(path);
        }
    }
}
