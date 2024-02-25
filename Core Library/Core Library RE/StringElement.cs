namespace Core.Library.RE;

internal class StringElement : Element {
    private string value;

    public StringElement(char c)
        : this(c.ToString()) {
    }

    public StringElement(string str) {
        value = str;
    }

    public string GetString() {
        return value;
    }

    public override object Clone() {
        return this;
    }

    public override int Match(Matcher m,
                              ReaderBuffer buffer,
                              int start,
                              int skip) {

        int  c;

        if (skip != 0) {
            return -1;
        }
        for (int i = 0; i < value.Length; i++) {
            c = buffer.Peek(start + i);
            if (c < 0) {
                m.SetReadEndOfString();
                return -1;
            }
            if (m.IsCaseInsensitive()) {
                c = (int) Char.ToLower((char) c);
            }
            if (c != (int) value[i]) {
                return -1;
            }
        }
        return value.Length;
    }

    public override void PrintTo(TextWriter output, string indent) {
        output.WriteLine(indent + "'" + value + "'");
    }
}