namespace Core.Library.RE;

internal abstract class Element : ICloneable {
    public abstract object Clone();

    public abstract int Match(Matcher m,
        ReaderBuffer buffer,
        int start,
        int skip);

    public abstract void PrintTo(TextWriter output, string indent);
}