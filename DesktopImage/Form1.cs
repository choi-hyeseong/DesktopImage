using System.Drawing.Imaging;

namespace DesktopImage
{
    public partial class Form1 : Form
    {
        private ImageController controller;
        public Form1(ImageController controller)
        {
            this.controller = controller;   
            InitializeComponent();
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            controller.Start();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            controller.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog(); //파일 선택기
            dialog.Filter = "이미지 (*.jpg, *.gif, *.png)| *.jpg; *.gif; *.png;";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //파일을 정상적으로 선택했다면
                controller.SetImagePath(dialog.FileName); //파일 경로를 선택한다.
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }
    }
}