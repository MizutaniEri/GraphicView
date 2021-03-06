﻿using System;
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
using FindAssocExe;

namespace GraphicView
{
    public partial class Form1 : Form
    {
        private string[] ActiveExt = { ".jpg", ".bmp", ".png", "gif" };
        private List<ZipArchiveEntry> zipList = null;
        int index = -1;
        string zipFileName;
        string imageFIleName;
        private Point mouseDownLocation;
        private bool exec;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var cmd = System.Environment.GetCommandLineArgs()[1];
            ZipFileLoader(cmd);
            if (zipList == null || zipList.Count <= 0)
            {
                // 既定の方法で開く
                var proc = System.Diagnostics.Process.Start(cmd);
                var procName = Path.GetFileName(Path.GetExtension(cmd).FindAssociatedExecutable()).ToLower();
                var myProcName = Path.GetFileName(System.Environment.GetCommandLineArgs()[0]).ToLower();
                if (procName == myProcName)
                {
                    proc.Kill();
                }
                Close();
                return;
            }
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
                zipList = zipArc.Entries.Where(ent => ActiveExt.Contains(Path.GetExtension(ent.Name).ToLower())).OrderBy(s => new FileInfo(s.FullName), new NaturalFileSystemInfoCompare()).ToList();
                if (zipList == null || zipList.Count <= 0)
                {
                    return;
                }
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
                this.AutoScrollPosition = new Point(0, 0);
            }
            Text = Path.GetFileName(zipFileName) + " (" + (index + 1) + "/" + zipList.Count + ") - " + zipList[index].FullName;
        }

        public static Image GetZoomImageFromStream(Stream fs)
        {
            var img = Image.FromStream(fs, false, false);
            // Exif情報に基づく画像方向修正
            var pitem = img.PropertyItems.Where(item => item.Id == 0x112 && item.Type == 3 && item.Value[0] != 1).FirstOrDefault();
            if (pitem != null)
            {
                Debug.WriteLine("Exif id = [" + pitem.Id + "]" + " type=[" + pitem.Type + "]");
                var rotate = RotateFlipType.RotateNoneFlipNone;
                switch (pitem.Value[0])
                {
                    case 2:
                        rotate = RotateFlipType.RotateNoneFlipX;
                        break;
                    case 3:
                        rotate = RotateFlipType.Rotate180FlipNone;
                        break;
                    case 4:
                        rotate = RotateFlipType.RotateNoneFlipY;
                        break;
                    case 5:
                        rotate = RotateFlipType.Rotate270FlipX;
                        break;
                    case 6:
                        rotate = RotateFlipType.Rotate90FlipNone;
                        break;
                    case 7:
                        rotate = RotateFlipType.Rotate90FlipX;
                        break;
                    case 8:
                        rotate = RotateFlipType.Rotate270FlipNone;
                        break;
                }
                img.RotateFlip(rotate);
            }
            return (img);
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nextImageView();
        }

        private void nextImageView()
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
            fitScreensizeImageView();
        }

        private void beforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            beforeImageView();
        }

        private void beforeImageView()
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
            fitScreensizeImageView();
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
                    using (var writer = entry.Open())
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
                index = form2.selectImageIndex;
                zipView(index);
                fitScreensizeImageView();
            }
            form2.Dispose();
            form2 = null;
        }

        private void fitScreenSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fitScreenSizeToolStripMenuItem.Checked && widthFitZoomToolStripMenuItem.Checked)
            {
                widthFitZoomToolStripMenuItem.Checked = false;
            }
            fitScreensizeImageView();
        }

        private void fitScreensizeImageView()
        {
            if (fitScreenSizeToolStripMenuItem.Checked)
            {
                screenFitZoom();
            }
            else if (widthFitZoomToolStripMenuItem.Checked)
            {
                ScreenFitWidthZoom();
            }
            else
            {
                zipView(index);
            }
        }

        private void widthFitZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fitScreenSizeToolStripMenuItem.Checked && widthFitZoomToolStripMenuItem.Checked)
            {
                widthFitZoomToolStripMenuItem.Checked = false;
            }
            fitScreensizeImageView();
        }

        private void screenFitZoom()
        {
            var imageSize = new Size();
            imageSize.Width = pictureBox1.Image.Width;
            imageSize.Height = pictureBox1.Image.Height;
            getScreenFitSize(ref imageSize);
            pictureBox1.Image = ZoomGraphic(imageSize);
        }

        private Image ZoomGraphic(System.Drawing.Size newSize)
        {
            //描画先とするImageオブジェクトを作成する
            var canvas = new Bitmap(newSize.Width, newSize.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //画像ファイルを読み込んで、Imageオブジェクトとして取得する
            Image img = pictureBox1.Image;
            //画像のサイズを2倍にしてcanvasに描画する
            g.DrawImage(img, 0, 0, newSize.Width, newSize.Height);
            //Imageオブジェクトのリソースを解放する
            img.Dispose();

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            return canvas;
        }

        private void getScreenFitSize(ref System.Drawing.Size imageSize)
        {
            // スクリーンサイズの取得;
            Rectangle Rect = Screen.GetWorkingArea(new Point(0, 0));
            // スクリーンサイズの幅と高さ;
            int screenX = Rect.Size.Width;
            int screenY = Rect.Size.Height;
            // 画像の幅と高さ;
            int imageX = imageSize.Width;
            int imageY = imageSize.Height;
            int newX = 0;
            int newY = 0;
            int RX = 0;
            int RY = 0;

            // 画像の比率に沿った幅と高さ計算;
            RX = imageX * screenY / imageY;
            RY = imageY * screenX / imageX;

            if ((RX < screenX) && (RY > screenY))
            {
                newX = RX;
                newY = screenY;
            }
            else
            {
                newX = screenX;
                newY = RY;
            }
            imageSize.Width = newX;
            imageSize.Height = newY;
        }

        private void ScreenFitWidthZoom()
        {
            var imageSize = new Size();
            imageSize.Width = pictureBox1.Image.Width;
            imageSize.Height = pictureBox1.Image.Height;
            getScreenWidthFitSize(ref imageSize);
            pictureBox1.Image = ZoomGraphic(imageSize);
        }

        private void getScreenWidthFitSize(ref System.Drawing.Size imageSize)
        {
            // スクリーンサイズの取得;
            Rectangle Rect = Screen.GetWorkingArea(new Point(0, 0));
            // スクリーンサイズの幅と高さ;
            int screenX = Rect.Size.Width;
            int screenY = Rect.Size.Height;
            // 画像の幅と高さ;
            int imageX = imageSize.Width;
            int imageY = imageSize.Height;
            int newX = 0;
            int newY = 0;
            int RX = 0;
            int RY = 0;

            if (imageX < imageY && imageX > screenX)
            {
                screenX -= SystemInformation.VerticalScrollBarWidth;
            }

            // 画像の比率に沿った幅と高さ計算;
            RX = imageX * screenY / imageY;
            RY = imageY * screenX / imageX;

            //if ((RX < screenX) && (RY > screenY))
            //{
            //    newX = RX;
            //    newY = screenY;
            //}
            //else
            //{
            newX = screenX;
            newY = RY;
            //}
            imageSize.Width = newX;
            imageSize.Height = newY;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            mouseDownLocation = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            int mouseMoveX = e.X - mouseDownLocation.X;
            // 横方向に移動なし、または、移動距離が短い
            if (mouseMoveX == 0 || Math.Abs(mouseMoveX) < 100)
            {
                return;
            }
            if (mouseMoveX > 0)
            {
                mouseSwipe(1);
            }
            else
            {
                mouseSwipe(-1);
            }
        }

        private void mouseSwipe(int indexAdd)
        {
            // 2重起動防止(1回の処理中に同イベントが発生しても何もしない)
            if (exec)
            {
                return;
            }
            exec = true;
            zipView((index += indexAdd));
            exec = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                nextImageView();
            }
            else if (e.KeyCode == Keys.Left)
            {
                beforeImageView();
            }
            if (e.KeyCode == Keys.Up)
            {
                this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, -this.AutoScrollPosition.Y - 100);
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, -this.AutoScrollPosition.Y + 100);
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, -this.AutoScrollPosition.Y - 500);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, -this.AutoScrollPosition.Y + 500);
            }
            else if (e.KeyCode == Keys.Home)
            {
                this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, 0);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
