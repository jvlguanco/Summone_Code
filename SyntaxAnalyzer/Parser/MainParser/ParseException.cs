using System.Collections;
using System.Text;

namespace Core.Library;

public class ParseException : Exception {
    public enum ErrorType {
        INTERNAL,
        IO,
        UNEXPECTED_EOF,
        UNEXPECTED_CHAR,
        UNEXPECTED_TOKEN,
        INVALID_TOKEN,
        ANALYSIS
    }

    private ErrorType type;

    private string info;

    private ArrayList details;

    private int line;

    private int column;

    public ParseException(ErrorType type,
                          string info,
                          int line,
                          int column)
        : this(type, info, null, line, column) {
    }

    public ParseException(ErrorType type,
                          string info,
                          ArrayList details,
                          int line,
                          int column) {

        this.type = type;
        this.info = info;
        this.details = details;
        this.line = line;
        this.column = column;
    }

    public ErrorType Type {
        get {
            return type;
        }
    }

    public ErrorType GetErrorType() {
        return Type;
    }

    public string Info {
        get {
            return info;
        }
    }

    public string GetInfo() {
        return Info;
    }

    /**
     * The additional detailed error information property
     * (read-only).
     *
     * 
     */
    public ArrayList Details {
        get {
            return new ArrayList(details);
        }
    }

    public ArrayList GetDetails() {
        return Details;
    }

    public int Line {
        get {
            return line;
        }
    }

    public int GetLine() {
        return Line;
    }

    public int Column {
        get {
            return column;
        }
    }

    public int GetColumn() {
        return column;
    }

    public override string Message {
        get{
            StringBuilder  buffer = new StringBuilder();

            // Add error description
            buffer.Append(ErrorMessage);

            // Add line and column
            if (line > 0 && column > 0) {
                buffer.Append(", on line: ");
                buffer.Append(line);
                buffer.Append(" column: ");
                buffer.Append(column);
            }

            return buffer.ToString();
        }
    }

    public string GetMessage() {
        return Message;
    }

    public string ErrorMessage {
        get {
            StringBuilder  buffer = new StringBuilder();

            // Add type and info
            switch (type) {
            case ErrorType.IO:
                buffer.Append("I/O error: ");
                buffer.Append(info);
                break;
            case ErrorType.UNEXPECTED_EOF:
                buffer.Append("unexpected end of file");
                break;
            case ErrorType.UNEXPECTED_CHAR:
                buffer.Append("unexpected character '");
                buffer.Append(info);
                buffer.Append("'");
                break;
            case ErrorType.UNEXPECTED_TOKEN:
                buffer.Append("unexpected token ");
                buffer.Append(info);
                if (details != null) {
                    buffer.Append(", expected ");
                    if (details.Count > 1) {
                        buffer.Append("one of ");
                    }
                    buffer.Append(GetMessageDetails());
                }
                break;
            case ErrorType.INVALID_TOKEN:
                buffer.Append(info);
                break;
            case ErrorType.ANALYSIS:
                buffer.Append(info);
                break;
            default:
                buffer.Append("internal error");
                if (info != null) {
                	buffer.Append(": ");
                    buffer.Append(info);
                }
                break;
            }

            return buffer.ToString();
        }
    }

    public string GetErrorMessage() {
        return ErrorMessage;
    }

    private string GetMessageDetails() {
        StringBuilder  buffer = new StringBuilder();

        for (int i = 0; i < details.Count; i++) {
            if (i > 0) {
                buffer.Append(", ");
                if (i + 1 == details.Count) {
                    buffer.Append("or ");
                }
            }
            buffer.Append(details[i]);
        }

        return buffer.ToString();
    }
}