using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopImage {
    public partial class MainForm : Form {
        private Point mousePoint; //마우스 좌표
     
        public MainForm() {
            InitializeComponent();
        }

        public void DrawImage(object? sender, ImagePathArg e) {
            //실질적으로 그려지는 부분
            pictureBox1.ImageLocation = e.path;
        }

        private void ImageForm_MouseDown(object? sender, MouseEventArgs e) {
            mousePoint = new Point(e.X, e.Y); //움직이는 당시 위치
        }

        private void ImageForm_MouseMove(object? sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                //좌클릭중일때
                move(false, new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y)));
                //Location의 좌표는 모니터상 좌표 (user32...) Left는 현재 폼의 width 위치, top은 현재 폼의 height 위치
                //마우스 클릭 당시 지정된 포인터는 => 폼안에서의 위치 (height, width)
                //따라서 기존 좌표 (150,150) => 마우스 움직이면서 (100, 100) => 괄호안 계산식은 기존 폼 절대 좌표 - 50, 절대좌표 -50 즉 50만큼 동일하게 이동

            }
        }


       public void move(bool otherThread, Point point) {
            if (otherThread && IsHandleCreated) //show가 호출된경우 
                Invoke(() => Location = point);
            else
                Location = point;
        }

        private void MainForm_Load(object? sender, EventArgs e) {
            pictureBox1.MouseMove += ImageForm_MouseMove;
            pictureBox1.MouseDown += ImageForm_MouseDown;
            Shown += MainForm_Shown; //show 호출시
            
        }

        private void MainForm_Shown(object? sender, EventArgs e) {
            Focus(); //포커싱
            TopMost = true; //항상 위에 올라오게 설정
        }

        public void Resize_Form(Size v) {
            Size = v;
        }

        public Twin<Point> GetSize() {
            return new Twin<Point>(new Point(Left, Top), new Point(Right, Bottom));
        }


    }

   
}
