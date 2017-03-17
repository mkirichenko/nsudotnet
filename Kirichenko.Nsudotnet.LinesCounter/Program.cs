using System;
using System.IO;

namespace Kirichenko.Nsudotnet.LinesCounter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Missing source files extension argument: [.<extension>]");
                return;
            }

            ICommentsDescriptor descriptor;
            try
            {
                descriptor = CommentsDescriptors.GetDescriptor(args[0]);
            }
            catch (ExtensionNotAvailableException _)
            {
                Console.WriteLine("Extension {0} is not available", args[0]);
                return;
            }

            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var totalLines = ScanDirectory(currentDirectory, descriptor);
            Console.WriteLine("{0} lines in {1} files at {2}", totalLines, args[0], currentDirectory);
        }

        private static int ScanDirectory(DirectoryInfo directory, ICommentsDescriptor descriptor)
        {
            var totalLines = 0;
            foreach (var f in directory.GetFiles())
            {
                if (f.Extension.Equals(descriptor.Extension))
                {
                    totalLines += ScanFile(f, descriptor);
                }
            }
            foreach (var d in directory.GetDirectories())
            {
                totalLines += ScanDirectory(d, descriptor);
            }
            return totalLines;
        }

        private static int ScanFile(FileInfo file, ICommentsDescriptor descriptor)
        {
            var totalLines = 0;
            using (var reader = new StreamReader(file.OpenRead()))
            {
                LinesScanningState state = LinesScanningState.OutsideComments;
                var startIndex = 0;
                string line = reader.ReadLine();
                bool lineCounted = false;

                while (line != null)
                {
                    switch (state)
                    {
                        case LinesScanningState.OutsideComments:
                            var start = line.IndexOf(descriptor.MultiLineCommentBegin, startIndex);
                            if (start >= 0)
                            {
                                if (start == startIndex)
                                {
                                    state = LinesScanningState.InsideComments;
                                }
                                else
                                {
                                    var oneLineStart = line.IndexOf(descriptor.OneLineCommentBegin, startIndex);
                                    if (oneLineStart >= 0)
                                    {
                                        if (oneLineStart < start)
                                        {
                                            if (oneLineStart > startIndex)
                                            {
                                                lineCounted = true;
                                            }
                                            totalLines += lineCounted ? 1 : 0;
                                            line = reader.ReadLine();
                                            lineCounted = false;
                                            startIndex = 0;
                                        }
                                        else
                                        {
                                            lineCounted = true;
                                            state = LinesScanningState.InsideComments;
                                            startIndex = start;
                                        }
                                    }
                                    else
                                    {
                                        lineCounted = true;
                                        state = LinesScanningState.InsideComments;
                                        startIndex = start;
                                    }
                                }
                            }
                            else
                            {
                                var oneLineStart = line.IndexOf(descriptor.OneLineCommentBegin, startIndex);
                                if (oneLineStart >= 0)
                                {
                                    if (oneLineStart > startIndex)
                                    {
                                        lineCounted = true;
                                    }
                                    totalLines += lineCounted ? 1 : 0;
                                    line = reader.ReadLine();
                                    lineCounted = false;
                                    startIndex = 0;
                                }
                                else
                                {
                                    lineCounted = true;
                                    totalLines += lineCounted ? 1 : 0;
                                    line = reader.ReadLine();
                                    lineCounted = false;
                                    startIndex = 0;
                                }
                            }
                            break;
                        case LinesScanningState.InsideComments:
                            var end = line.IndexOf(descriptor.MultiLineCommentEnd, startIndex);
                            if (end >= 0)
                            {
                                state = LinesScanningState.OutsideComments;
                                if (end < line.Length - 1)
                                {
                                    startIndex = end;
                                }
                                else
                                {
                                    totalLines += lineCounted ? 1 : 0;
                                    line = reader.ReadLine();
                                    lineCounted = false;
                                    startIndex = 0;
                                }
                            }
                            else
                            {
                                totalLines += lineCounted ? 1 : 0;
                                line = reader.ReadLine();
                                lineCounted = false;
                                startIndex = 0;
                            }
                            break;
                    }
                }
            }
            return totalLines;
        }
    }

    internal enum LinesScanningState
    {
        OutsideComments,
        InsideComments
    }
}