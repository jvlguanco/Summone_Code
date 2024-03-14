namespace Core.Library;

public class ReaderBuffer {
    public const int BLOCK_SIZE = 1024;

    private char[] buffer = new char[BLOCK_SIZE * 4];

    private int pos = 0;

    private int length = 0;

    private TextReader input = null;

    private int line = 1;

    private int column = 1;

    public ReaderBuffer(TextReader input) {
        this.input = input;
    }

    public void Dispose() {
        buffer = null;
        pos = 0;
        length = 0;
        if (input != null) {
            try {
                input.Close();
            } catch (Exception) {
                // Do nothing
            }
            input = null;
        }
    }

    public int Position {
        get {
            return pos;
        }
    }

    public int LineNumber {
        get {
            return line;
        }
    }

    public int ColumnNumber {
        get {
            return column;
        }
    }

    public int Length {
        get {
            return length;
        }
    }

    public string Substring(int index, int length) {
        return new string(buffer, index, length);
    }

    public override string ToString() {
        return new string(buffer, 0, length);
    }

    public int Peek(int offset) {
        int  index = pos + offset;

        if (index >= length) {
            EnsureBuffered(offset + 1);
            index = pos + offset;
        }
        return (index >= length) ? -1 : buffer[index];
    }

    public string Read(int offset) {
        int     count;
        string  result;

        EnsureBuffered(offset + 1);
        if (pos >= length) {
            return null;
        } else {
            count = length - pos;
            if (count > offset) {
                count = offset;
            }
            UpdateLineColumnNumbers(count);
            result = new string(buffer, pos, count);
            pos += count;
            if (input == null && pos >= length) {
                Dispose();
            }
            return result;
        }
    }

    private void UpdateLineColumnNumbers(int offset) {
        for (int i = 0; i < offset; i++) {
            if (buffer[pos + i] == '\n') {
                line++;
                column = 1;
            } else {
                column++;
            }
        }
    }

    private void EnsureBuffered(int offset) {
        int  size;
        int  readSize;

        if (input == null || pos + offset < length) {
            return;
        }

        if (pos > BLOCK_SIZE) {
            length -= (pos - 16);
            Array.Copy(buffer, pos - 16, buffer, 0, length);
            pos = 16;
        }

        size = pos + offset - length + 1;
        if (size % BLOCK_SIZE != 0) {
            size = (1 + size / BLOCK_SIZE) * BLOCK_SIZE;
        }
        EnsureCapacity(length + size);

        // Read characters
        try {
            while (input != null && size > 0) {
                readSize = input.Read(buffer, length, size);
                if (readSize > 0) {
                    length += readSize;
                    size -= readSize;
                } else {
                    input.Close();
                    input = null;
                }
            }
        } catch (IOException e) {
            input = null;
            throw e;
        }
    }

    private void EnsureCapacity(int size) {
        if (buffer.Length >= size) {
            return;
        }
        if (size % BLOCK_SIZE != 0) {
            size = (1 + size / BLOCK_SIZE) * BLOCK_SIZE;
        }
        Array.Resize(ref buffer, size);
    }
}