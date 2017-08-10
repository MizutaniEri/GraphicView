using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicView
{
    public partial class Form1 : Form
    {
        private string[] ActiveExt = { ".jpg", ".bmp", ".png", "gif" };
        private List<ZipArchiveEntry> zipList = null;
        int index = -1;
        string zipFileName;
        string imageFIleName;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = Image.FromFile(@"C:\Users\tanakken\Pictures\HSC_M31.jpg");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "ZIP File|*.zip";
            var res = openDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                ZipFileLoader(openDialog.FileName);
            }
        }

        private void ZipFileLoader(string fileName)
        {
            var zipArc = ZipFile.OpenRead(fileName);
            {
                zipFileName = fileName;
                zipList = zipArc.Entries.Where(ent => ActiveExt.Contains(Path.GetExtension(ent.Name).ToLower())).ToList();
                zipView(++index);
            }
        }

        private void zipView(int index)
        {
            imageFIleName = zipList[index].FullName;
            //ZIP書庫を開く
            using (ZipArchive a = ZipFile.OpenRead(zipFileName))
            {
                // ZipArchiveEntryを取得する
                ZipArchiveEntry e = a.GetEntry(imageFIleName);
                var stream = e.Open();
                pictureBox1.Image = GetZoomImageFromStream(stream);
            }
            Text = zipFileName + " (" + (index + 1) + "/" + zipList.Count + ") - " + zipList[index].Name;
        }

        public static Image GetZoomImageFromStream(Stream fs)
        {
            return Image.FromStream(fs, false, false);
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((index + 1) >= zipList.Count)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            zipView(index);
        }

        private void beforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((index - 1) < 0)
            {
                index = zipList.Count - 1;
            }
            else
            {
                index--;
            }
            zipView(index);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "ZIP file|*.zip|Jpeg File|*.jpg|Bitmap File|*.bmp|Portable Network Graphics|*.png";
            saveDialog.DefaultExt = "zip";
            saveDialog.CheckFileExists = false;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(saveDialog.FileName).ToLower() == ".zip")
                {
                    saveZipFile(saveDialog.FileName);
                }
                else
                {
                    pictureBox1.Image.Save(saveDialog.FileName, getImageFormatFromFileName(saveDialog.FileName));
                }
            }
        }

        private ImageFormat getImageFormatFromFileName(string fileName)
        {
            var iformat = ImageFormat.Jpeg;
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".png":
                    iformat = ImageFormat.Png;
                    break;
                case ".bmp":
                    iformat = ImageFormat.Bmp;
                    break;
                case ".gif":
                    iformat = ImageFormat.Gif;
                    break;
            }
            return (iformat);
        }

        private void saveZipFile(string FileName)
        {
            using (var zto = new FileStream(FileName, FileMode.Create))
            {
                using (var zipArc = new ZipArchive(zto, ZipArchiveMode.Create))
                {
                    var entry = zipArc.CreateEntry(imageFIleName);
                    using(var writer = entry.Open())
                    {
                        var memStream = new MemoryStream();
                        var iformat = getImageFormatFromFileName(imageFIleName);
                        // 画像をメモリストリームに保存する(指定の画像形式で)
                        pictureBox1.Image.Save(memStream, iformat);
                        long len = memStream.Length;
                        int baseSize = int.MaxValue;
                        int offset = 0;
                        var buf = memStream.ToArray();
                        while (len > 0)
                        {
                            int wlen = len > baseSize ? baseSize : (int)len;
                            writer.Write(buf, offset, wlen);
                            len -= wlen;
                            offset += wlen;
                        }
                    }
                }
            }
        }

        private void imageListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.zipList = zipList;
            var result = form2.ShowDialog();
            if (result == DialogResult.OK)
            {
                zipView(form2.selectImageIndex);
            }
            form2.Dispose();
            form2 = null;
        }
    }
}
