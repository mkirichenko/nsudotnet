namespace Kirichenko.Nsudotnet.LinesCounter
{
    public interface ICommentsDescriptor
    {
        string Extension { get; }

        string OneLineCommentBegin { get; }

        string MultiLineCommentBegin { get; }

        string MultiLineCommentEnd { get; }
    }
}