using System;
using System.Collections.Generic;

namespace Kirichenko.Nsudotnet.LinesCounter
{
    public class CommentsDescriptors
    {
        private static Dictionary<string, ICommentsDescriptor> AvailableFileExtensions;

        static CommentsDescriptors()
        {
            AvailableFileExtensions = new Dictionary<string, ICommentsDescriptor>();
            AvailableFileExtensions.Add(".cs", new CSharpCommentsDescriptor());
        }

        public static ICommentsDescriptor GetDescriptor(string extension)
        {
            try
            {
                return AvailableFileExtensions[extension];
            }
            catch (KeyNotFoundException _)
            {
                throw new ExtensionNotAvailableException();
            }
        }
    }

    public class ExtensionNotAvailableException : Exception
    {
    }
}