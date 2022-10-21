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
            OpenFileDialog dialog = new OpenFileDialog(); //���� ���ñ�
            dialog.Filter = "�̹��� (*.jpg, *.gif, *.png)| *.jpg; *.gif; *.png;";
            if (dialog.ShowDialog() == DialogResult.OK) {
                //������ ���������� �����ߴٸ�
                controller.SetImagePath(dialog.FileName); //���� ��θ� �����Ѵ�.
            }
        }


        private void Form1_Load(object sender, EventArgs e) {
            //������ ����
            Icon = Properties.Resources.icon;
            notifyIcon1.Icon = Properties.Resources.icon;
            numericUpDown4.Enabled = false; //������������ ���������ϰ�
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason != CloseReason.ApplicationExitCall) { //�������� ��û (��Ʈ�ѷ�)�� �ƴҰ��
                e.Cancel = true; //�������� �ʴ´�.
                Hide(); //�������� �����
                notifyIcon1.ShowBalloonTip(3); //3�ʰ� ������ ������
            }

        }

        private void SettingItem_Click(object sender, EventArgs e) {
            Show();
            Focus();
            //â�� �ٽ� �����
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
                MessageBox.Show("������� 0������ ������ ������ �� �����ϴ�.");
            else if (speed < 1 || delay < 1)
                MessageBox.Show("�ӵ��� �����̴� 0���Ϸ� ������ �� �����ϴ�.");
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
            numericUpDown4.Enabled = true; //������ �� �ְ�
        }
    }

}