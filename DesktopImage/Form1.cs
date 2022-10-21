using System.Diagnostics;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace DesktopImage {
    public partial class Form1 : Form {
        private ImageController controller;
        public Form1(ImageController controller) {
            this.controller = controller;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e) {
            controller.Start();
        }


        private void button2_Click(object sender, EventArgs e) {
            controller.Stop();
        }

        private void button3_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog(); //파일 선택기
            dialog.Filter = "이미지 (*.jpg, *.gif, *.png)| *.jpg; *.gif; *.png;";
            if (dialog.ShowDialog() == DialogResult.OK) {
                //파일을 정상적으로 선택했다면
                controller.SetImagePath(dialog.FileName); //파일 경로를 선택한다.
            }
        }


        private void Form1_Load(object sender, EventArgs e) {
            //아이콘 설정
            Icon = Properties.Resources.icon;
            notifyIcon1.Icon = Properties.Resources.icon;
            numericUpDown4.Enabled = false; //랜덤값에서만 조절가능하게
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason != CloseReason.ApplicationExitCall) { //강제종료 요청 (컨트롤러)가 아닐경우
                e.Cancel = true; //종료하지 않는다.
                Hide(); //현재폼을 숨기며
                notifyIcon1.ShowBalloonTip(3); //3초간 아이콘 보여줌
            }

        }

        private void SettingItem_Click(object sender, EventArgs e) {
            Show();
            Focus();
            //창을 다시 띄워줌
        }

        private void ExitItem_Click(object sender, EventArgs e) {
            controller.Stop();
        }

        private void button4_Click(object sender, EventArgs e) {
            int x = (int)numericUpDown1.Value;
            int y = (int)numericUpDown2.Value;
            int speed = (int)numericUpDown3.Value;
            int delay = (int)numericUpDown4.Value;
            Move move = GetMoveType();
            if (x < 1 || y < 1)
                MessageBox.Show("사이즈는 0이하의 값으로 지정할 수 없습니다.");
            else if (speed < 1 || delay < 1)
                MessageBox.Show("속도나 딜레이는 0이하로 지정할 수 없습니다.");
            else {
                controller.applySettings(move, speed, delay, new Size(x, y));
            }

        }

        private Move GetMoveType() {
            if (radioButton1.Checked)
                return DesktopImage.Move.STATIC;
            else if (radioButton2.Checked)
                return DesktopImage.Move.UPDOWN;
            else if (radioButton3.Checked)
                return DesktopImage.Move.LEFTRIGHT;
            else if (radioButton4.Checked)
                return DesktopImage.Move.BOUNCE;
            else
                return DesktopImage.Move.RANDOM;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) {
            numericUpDown4.Enabled = true; //설정할 수 있게
        }
    }

}