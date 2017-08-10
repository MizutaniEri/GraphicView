using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicView
{
    public partial class Form2 : Form
    {
        public List<ZipArchiveEntry> zipList { get; set; }
        public string zipFileName { get; set; }
        public int selectImageIndex { get; private set; }
        private int imageSize = 128;

        public Form2()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  幅w、高さhのImageオブジェクトを作成
        /// </summary>
        /// <param name="image"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private Image createThumbnail(Image image, int w, int h)
        {
            Bitmap canvas = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            float fw = (float)w / (float)image.Width;
            float fh = (float)h / (float)image.Height;

            float scale = Math.Min(fw, fh);
            fw = image.Width * scale;
            fh = image.Height * scale;

            g.DrawImage(image, (w - fw) / 2, (h - fh) / 2, fw, fh);
            g.Dispose();

            return canvas;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (zipList == null || zipList.Count == 0)
            {
                return;
            }
            imageList1.ImageSize = new Size(imageSize, imageSize);
            zipList.Select((zipEntity, index) => new { zipEntity, index })
                .ToList()
                .ForEach(a =>
            {
                var stream = a.zipEntity.Open();
                imageList1.Images.Add(createThumbnail(Image.FromStream(stream), imageSize, imageSize));
                listView1.Items.Add(a.zipEntity.Name, a.index);
            }); ;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                return;
            }
            selectImageIndex = listView1.SelectedIndices[0];
            this.DialogResult = DialogResult.OK;
        }
    }
}
