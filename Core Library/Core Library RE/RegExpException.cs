using System.Text;

namespace Core.Library.RE;

public class RegExpException : Exception {
    public enum ErrorType {
        UNEXPECTED_CHARACTER,
        UNTERMINATED_PATTERN,
        UNSUPPORTED_SPECIAL_CHARACTER,
        UNSUPPORTED_ESCAPE_CHARACTER,
        INVALID_REPEAT_COUNT
    }

    private ErrorType type;

    private int position;

    private string pattern;

    public RegExpException(ErrorType type, int pos, string pattern) {
        this.type = type;
        this.position = pos;
        this.pattern = pattern;
    }

    public override string Message {
        get{
            return GetMessage();
        }
    }

    public string GetMessage() {
        StringBuilder  buffer = new StringBuilder();

        // Append error type name
        switch (type) {
        case ErrorType.UNEXPECTED_CHARACTER:
            buffer.Append("unexpected character");
            break;
        case ErrorType.UNTERMINATED_PATTERN:
            buffer.Append("unterminated pattern");
            break;
        case ErrorType.UNSUPPORTED_SPECIAL_CHARACTER:
            buffer.Append("unsupported character");
            break;
        case ErrorType.UNSUPPORTED_ESCAPE_CHARACTER:
            buffer.Append("unsupported escape character");
            break;
        case ErrorType.INVALID_REPEAT_COUNT:
            buffer.Append("invalid repeat count");
            break;
        default:
            buffer.Append("internal error");
            break;
        }

        // Append erroneous character
        buffer.Append(": ");
        if (position < pattern.Length) {
            buffer.Append('\'');
            buffer.Append(pattern.Substring(position));
            buffer.Append('\'');
        } else {
            buffer.Append("<end of pattern>");
        }

        // Append position
        buffer.Append(" at position ");
        buffer.Append(position);

        return buffer.ToString();
    }
}