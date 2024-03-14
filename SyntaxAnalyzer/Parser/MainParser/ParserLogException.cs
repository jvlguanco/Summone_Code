using System.Collections;
using System.Text;

namespace Core.Library;

public class ParserLogException : Exception {
    private ArrayList errors = new ArrayList();

    public ParserLogException() {
    }

    public override string Message {
        get{
            StringBuilder  buffer = new StringBuilder();

            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    buffer.Append("\n");
                }
                buffer.Append(this[i].Message);
            }
            return buffer.ToString();
        }
    }

    public int Count {
        get {
            return errors.Count;
        }
    }

    public int GetErrorCount() {
        return Count;
    }

    public ParseException this[int index] {
        get {
            return (ParseException) errors[index];
        }
    }

    public ParseException GetError(int index) {
        return this[index];
    }

    public void AddError(ParseException e) {
        errors.Add(e);
    }

    public string GetMessage() {
        return Message;
    }
}