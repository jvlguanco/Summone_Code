namespace Core.Library.RE;

public class Matcher {
    private Element element;

    private ReaderBuffer buffer;

    private bool ignoreCase;

    private int start;

    private int length;

    private bool endOfString;

    internal Matcher(Element e, ReaderBuffer buffer, bool ignoreCase) {
        this.element = e;
        this.buffer = buffer;
        this.ignoreCase = ignoreCase;
        this.start = 0;
        Reset();
    }

    public bool IsCaseInsensitive() {
        return ignoreCase;
    }

    public void Reset() {
        length = -1;
        endOfString = false;
    }

    public void Reset(string str) {
        Reset(new ReaderBuffer(new StringReader(str)));
    }

    public void Reset(ReaderBuffer buffer) {
        this.buffer = buffer;
        Reset();
    }

    public int Start() {
        return start;
    }

    public int End() {
        if (length > 0) {
            return start + length;
        } else {
            return start;
        }
    }

    public int Length() {
        return length;
    }

    public bool HasReadEndOfString() {
        return endOfString;
    }

    public bool MatchFromBeginning() {
        return MatchFrom(0);
    }

    public bool MatchFrom(int pos) {
        Reset();
        start = pos;
        length = element.Match(this, buffer, start, 0);
        return length >= 0;
    }

    public override string ToString() {
        if (length <= 0) {
            return "";
        } else {
            return buffer.Substring(buffer.Position, length);
        }
    }

    internal void SetReadEndOfString() {
        endOfString = true;
    }
}