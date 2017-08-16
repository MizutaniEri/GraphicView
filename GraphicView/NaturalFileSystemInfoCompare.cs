using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphicView
{
    public class NaturalFileSystemInfoCompare : IComparer<FileSystemInfo>
    {
        private IEnumerable<FileSystemInfo> TraceRoot(FileInfo target)
        {
            yield return target;

            foreach (var info in TraceRoot(target.Directory))
            {
                yield return info;
            }
        }

        private IEnumerable<FileSystemInfo> TraceRoot(DirectoryInfo target)
        {
            var parent = target;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

        private IEnumerable<FileSystemInfo> TraceRoot(FileSystemInfo target)
        {
            var collection = (target is FileInfo)
                                ? TraceRoot(target as FileInfo)
                                : TraceRoot(target as DirectoryInfo);
            foreach (var info in collection)
            {
                yield return info;
            }
        }

        public int Compare(FileSystemInfo x, FileSystemInfo y)
        {
            // x と y のドライブルートまでを取得
            var xArray = TraceRoot(x).Reverse().ToArray();
            var yArray = TraceRoot(y).Reverse().ToArray();

            // 短い方の長さでループする
            var minLength = Math.Min(xArray.Length, yArray.Length);

            foreach (var i in Enumerable.Range(0, minLength))
            {
                // Directory < File
                if (xArray[i] is DirectoryInfo != yArray[i] is DirectoryInfo)
                {
                    return xArray[i] is DirectoryInfo ? -1 : 1;
                }

                // 名称の比較
                var result = NaturalCompare(xArray[i].Name, yArray[i].Name);

                if (result != 0)
                {
                    return result;
                }
            }

            return xArray.Length.CompareTo(yArray.Length);
        }

        private int NaturalCompare(string x, string y)
        {
            if (x == y)
            {
                return 0;
            }

            var ix = 0;
            var iy = 0;

            if (int.TryParse(x, out ix) && int.TryParse(y, out iy))
            {
                return ix.CompareTo(iy);
            }

            var xs = Regex.Split(x.Replace(" ", ""), @"(\d+)");
            var ys = Regex.Split(y.Replace(" ", ""), @"(\d+)");

            if (xs.Length == 1 && ys.Length == 1)
            {
                return x.CompareTo(y);
            }

            var minLength = Math.Min(xs.Length, ys.Length);

            foreach (var i in Enumerable.Range(0, minLength))
            {
                if (xs[i] != ys[i])
                {
                    return NaturalCompare(xs[i], ys[i]);
                }
            }

            return xs.Length.CompareTo(ys.Length);
        }
    }
}
