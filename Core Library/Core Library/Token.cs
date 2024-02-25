using System.Text;

namespace Core.Library;

public class Token : Node {
    private TokenPattern pattern;
    private string image;
    private int startLine;
    private int startColumn;
    private int endLine;
    private int endColumn;
    private Token previous = null;
    private Token next = null;

    public Token(TokenPattern pattern, string image, int line, int col) {
        this.pattern = pattern;
        this.image = image;
        this.startLine = line;
        this.startColumn = col;
        this.endLine = line;
        this.endColumn = col + image.Length - 1;
        for (int pos = 0; image.IndexOf('\n', pos) >= 0;) {
            pos = image.IndexOf('\n', pos) + 1;
            this.endLine++;
            endColumn = image.Length - pos;
        }
    }

    public override int Id {
        get {
            return pattern.Id;
        }
    }

    public override string Name {
        get {
            return pattern.Name;
        }
    }

    public override int StartLine {
        get {
            return startLine;
        }
    }

    public override int StartColumn {
        get {
            return startColumn;
        }
    }

    public override int EndLine {
        get {
            return endLine;
        }
    }

    public override int EndColumn {
        get {
            return endColumn;
        }
    }

    public string Image {
        get {
            return image;
        }
    }

    public string GetImage() {
        return Image;
    }

    internal TokenPattern Pattern {
        get {
            return pattern;
        }
    }

    public Token Previous {
        get {
            return previous;
        }
        set {
            if (previous != null) {
                previous.next = null;
            }
            previous = value;
            if (previous != null) {
                previous.next = this;
            }
        }
    }

    public Token GetPreviousToken() {
        return Previous;
    }

    public Token Next {
        get {
            return next;
        }
        set {
            if (next != null) {
                next.previous = null;
            }
            next = value;
            if (next != null) {
                next.previous = this;
            }
        }
    }

    public Token GetNextToken() {
        return Next;
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();
        int            newline = image.IndexOf('\n');

        buffer.Append(pattern.Name);
        buffer.Append("(");
        buffer.Append(pattern.Id);
        buffer.Append("): \"");
        if (newline >= 0) {
            if (newline > 0 && image[newline - 1] == '\r') {
                newline--;
            }
            buffer.Append(image.Substring(0, newline));
            buffer.Append("(...)");
        } else {
            buffer.Append(image);
        }
        buffer.Append("\", line: ");
        buffer.Append(startLine);
        buffer.Append(", col: ");
        buffer.Append(startColumn);

        return buffer.ToString();
    }

    public string ToShortString() {
        StringBuilder  buffer = new StringBuilder();
        int            newline = image.IndexOf('\n');

        buffer.Append('"');
        if (newline >= 0) {
            if (newline > 0 && image[newline - 1] == '\r') {
                newline--;
            }
            buffer.Append(image.Substring(0, newline));
            buffer.Append("(...)");
        } else {
            buffer.Append(image);
        }
        buffer.Append('"');
        if (pattern.Type == TokenPattern.PatternType.REGEXP) {
            buffer.Append(" <");
            buffer.Append(pattern.Name);
            buffer.Append(">");
        }

        return buffer.ToString();
    }
}