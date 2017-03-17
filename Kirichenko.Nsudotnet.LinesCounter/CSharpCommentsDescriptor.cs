namespace Kirichenko.Nsudotnet.LinesCounter
{
    public class CSharpCommentsDescriptor : ICommentsDescriptor
    {
        public CSharpCommentsDescriptor()
        {
            Extension = ".cs";
            OneLineCommentBegin = "//";
            MultiLineCommentBegin = "/*";
            MultiLineCommentEnd = "*/";
        }

        public string Extension { get; }

        public string OneLineCommentBegin { get; }

        public string MultiLineCommentBegin { get; }

        public string MultiLineCommentEnd { get; }
    }
}