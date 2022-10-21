namespace DesktopImage {

    //MVC Pattern - Controller
    public class ImageController {

        //실질적 접근은 컨트롤러에서 하므로 생성자에서 생성하는쪽으로..
        private ImageData data;
        private MainForm mainForm;
        private Thread? formMovingThread;


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
            interrupt();
            Application.Exit();
        }

        private void interrupt() {
            if (formMovingThread != null)
                formMovingThread.Interrupt();
        }

        public void SetImagePath(string path) {
            data.SetImagePath(path);
        }


        public void applySettings(Move moveType, int speed, int delay, Size formSize) {
            if (!data.IsRunning()) 
                MessageBox.Show("사진이 떠있는 상태에서 설정을 진행해주세요.");
            else {
                MovingStrategy? strategy;
                mainForm.Resize_Form(formSize);
                interrupt();
                switch (moveType) {
                    case Move.UPDOWN:
                        strategy = new UpDownStrategy();
                        break;
                    case Move.LEFTRIGHT:
                        strategy = new LeftRightStrategy();
                        break;
                    case Move.BOUNCE:
                        strategy = new BouncingStrategy();
                        break;
                    case Move.RANDOM:
                        strategy = new RandomStrategy(delay);
                        break;
                    default:
                        strategy = null;
                        break;
                }
                if (strategy != null) {
                    data.strategy = strategy;
                    data.speed = speed;
                    formMovingThread = new Thread(run);
                    formMovingThread.Start();
                }
            }
        }

        public void run() {
            while (true) {
                //무한루프
                try {
                    MovingStrategy? strategy = data.strategy;
                    if (strategy != null) {
                        strategy.setBound(mainForm.GetSize(), Screen.PrimaryScreen.WorkingArea.Size);
                        if (!strategy.CanMove())
                            strategy.ChangeDirection();
                        mainForm.move(true, strategy.Move()); //임시로 조치
                        Thread.Sleep(100 / data.speed);
                    }
                    else
                        Thread.CurrentThread.Interrupt(); //강제종료
                }
                catch (ThreadInterruptedException e) {
                    //sleep도중 중단시
                    break;
                }
            }
        }
    }
}
