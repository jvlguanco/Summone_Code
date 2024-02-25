using System.Text;

namespace Core.Library;

public class TokenPattern {
    public enum PatternType {
        STRING,
        REGEXP
    }

    private int id;
    private string name;
    private PatternType type;
    private string pattern;
    private bool error = false;
    private string errorMessage = null;
    private bool ignore = false;
    private string ignoreMessage = null;
    private string debugInfo = null;
    public TokenPattern(int id,
                        string name,
                        PatternType type,
                        string pattern) {

        this.id = id;
        this.name = name;
        this.type = type;
        this.pattern = pattern;
    }

    public int Id {
        get {
            return id;
        }
    }

    public int GetId() {
        return id;
    }

    public string Name {
        get {
            return name;
        }
    }

    public string GetName() {
        return name;
    }

    public PatternType Type {
        get {
            return type;
        }
    }

    public PatternType GetPatternType() {
        return type;
    }

    public string Pattern {
        get {
            return pattern;
        }
    }

    public string GetPattern() {
        return pattern;
    }

    public bool Error {
        get {
            return error;
        }
        set {
            error = value;
            if (error && errorMessage == null) {
                errorMessage = "unrecognized token found";
            }
        }
    }

    public string ErrorMessage {
        get {
            return errorMessage;
        }
        set {
            error = true;
            errorMessage = value;
        }
    }

    public bool IsError() {
        return Error;
    }

    public string GetErrorMessage() {
        return ErrorMessage;
    }

    public void SetError() {
        Error = true;
    }

    public void SetError(string message) {
        ErrorMessage = message;
    }

    public bool Ignore {
        get {
            return ignore;
        }
        set {
            ignore = value;
        }
    }

    public string IgnoreMessage {
        get {
            return ignoreMessage;
        }
        set {
            ignore = true;
            ignoreMessage = value;
        }
    }

    public bool IsIgnore() {
        return Ignore;
    }

    public string GetIgnoreMessage() {
        return IgnoreMessage;
    }

    public void SetIgnore() {
        Ignore = true;
    }

    public void SetIgnore(string message) {
        IgnoreMessage = message;
    }

    public string DebugInfo {
        get {
            return debugInfo;
        }
        set {
            debugInfo = value;
        }
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        buffer.Append(name);
        buffer.Append(" (");
        buffer.Append(id);
        buffer.Append("): ");
        switch (type) {
        case PatternType.STRING:
            buffer.Append("\"");
            buffer.Append(pattern);
            buffer.Append("\"");
            break;
        case PatternType.REGEXP:
            buffer.Append("<<");
            buffer.Append(pattern);
            buffer.Append(">>");
            break;
        }
        if (error) {
            buffer.Append(" ERROR: \"");
            buffer.Append(errorMessage);
            buffer.Append("\"");
        }
        if (ignore) {
            buffer.Append(" IGNORE");
            if (ignoreMessage != null) {
                buffer.Append(": \"");
                buffer.Append(ignoreMessage);
                buffer.Append("\"");
            }
        }
        if (debugInfo != null) {
            buffer.Append("\n  ");
            buffer.Append(debugInfo);
        }
        return buffer.ToString();
    }

    public string ToShortString() {
        StringBuilder  buffer = new StringBuilder();
        int            newline = pattern.IndexOf('\n');

        if (type == PatternType.STRING) {
            buffer.Append("\"");
            if (newline >= 0) {
                if (newline > 0 && pattern[newline - 1] == '\r') {
                    newline--;
                }
                buffer.Append(pattern.Substring(0, newline));
                buffer.Append("(...)");
            } else {
                buffer.Append(pattern);
            }
            buffer.Append("\"");
        } else {
            buffer.Append("<");
            buffer.Append(name);
            buffer.Append(">");
        }

        return buffer.ToString();
    }
}