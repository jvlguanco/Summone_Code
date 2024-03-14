using System.Collections;
using System.Text;

namespace Core.Library;

public class ParserCreationException : Exception {

    public enum ErrorType {
        INTERNAL,
        INVALID_PARSER,
        INVALID_TOKEN,
        INVALID_PRODUCTION,
        INFINITE_LOOP,
        INHERENT_AMBIGUITY
    }

    private ErrorType type;

    private string name;

    private string info;

    private ArrayList details;

    public ParserCreationException(ErrorType type,
                                   String info)
        : this(type, null, info) {
    }

    public ParserCreationException(ErrorType type,
                                   String name,
                                   String info)
        : this(type, name, info, null) {
    }

    public ParserCreationException(ErrorType type,
                                   String name,
                                   String info,
                                   ArrayList details) {

        this.type = type;
        this.name = name;
        this.info = info;
        this.details = details;
    }

    public ErrorType Type {
        get {
            return type;
        }
    }

    public ErrorType GetErrorType() {
        return Type;
    }

    public string Name {
        get {
            return name;
        }
    }

    public string GetName() {
        return Name;
    }

    public string Info {
        get {
            return info;
        }
    }

    public string GetInfo() {
        return Info;
    }

    public string Details {
        get {
            StringBuilder  buffer = new StringBuilder();

            if (details == null) {
                return null;
            }
            for (int i = 0; i < details.Count; i++) {
                if (i > 0) {
                    buffer.Append(", ");
                    if (i + 1 == details.Count) {
                        buffer.Append("and ");
                    }
                }
                buffer.Append(details[i]);
            }

            return buffer.ToString();
        }
    }

    public string GetDetails() {
        return Details;
    }

    public override string Message {
        get{
            StringBuilder  buffer = new StringBuilder();

            switch (type) {
            case ErrorType.INVALID_PARSER:
                buffer.Append("parser is invalid, as ");
                buffer.Append(info);
                break;
            case ErrorType.INVALID_TOKEN:
                buffer.Append("token '");
                buffer.Append(name);
                buffer.Append("' is invalid, as ");
                buffer.Append(info);
                break;
            case ErrorType.INVALID_PRODUCTION:
                buffer.Append("production '");
                buffer.Append(name);
                buffer.Append("' is invalid, as ");
                buffer.Append(info);
                break;
            case ErrorType.INFINITE_LOOP:
                buffer.Append("infinite loop found in production pattern '");
                buffer.Append(name);
                buffer.Append("'");
                break;
            case ErrorType.INHERENT_AMBIGUITY:
                buffer.Append("inherent ambiguity in production '");
                buffer.Append(name);
                buffer.Append("'");
                if (info != null) {
                    buffer.Append(" ");
                    buffer.Append(info);
                }
                if (details != null) {
                    buffer.Append(" starting with ");
                    if (details.Count > 1) {
                        buffer.Append("tokens ");
                    } else {
                        buffer.Append("token ");
                    }
                    buffer.Append(Details);
                }
                break;
            default:
                buffer.Append("internal error");
                break;
            }
            return buffer.ToString();
        }
    }

    public string GetMessage() {
        return Message;
    }
}