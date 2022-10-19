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
            OpenFileDialog dialog = new OpenFileDialog(); //���� ���ñ�
            dialog.Filter = "�̹��� (*.jpg, *.gif, *.png)| *.jpg; *.gif; *.png;";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //������ ���������� �����ߴٸ�
                controller.SetImagePath(dialog.FileName); //���� ��θ� �����Ѵ�.
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }
    }
}