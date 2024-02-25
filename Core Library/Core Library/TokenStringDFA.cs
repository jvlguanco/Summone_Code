using System.Text;

namespace Core.Library;

internal class TokenStringDFA {
    private DFAState[] ascii = new DFAState[128];
    private DFAState nonAscii = new DFAState();
    public TokenStringDFA() {
    }

    public void AddMatch(string str, bool caseInsensitive, TokenPattern value) {
        DFAState  state;
        DFAState  next;
        char      c = str[0];
        int       start = 0;

        if (caseInsensitive) {
            c = Char.ToLower(c);
        }
        if (c < 128) {
            state = ascii[c];
            if (state == null) {
                state = ascii[c] = new DFAState();
            }
            start++;
        } else {
            state = nonAscii;
        }
        for (int i = start; i < str.Length; i++) {
            next = state.tree.Find(str[i], caseInsensitive);
            if (next == null) {
                next = new DFAState();
                state.tree.Add(str[i], caseInsensitive, next);
            }
            state = next;
        }
        state.value = value;
    }

    public TokenPattern Match(ReaderBuffer buffer, bool caseInsensitive) {
        TokenPattern  result = null;
        DFAState      state;
        int           pos = 0;
        int           c;

        c = buffer.Peek(0);
        if (c < 0) {
            return null;
        }
        if (caseInsensitive) {
            c = Char.ToLower((char) c);
        }
        if (c < 128) {
            state = ascii[c];
            if (state == null) {
                return null;
            } else if (state.value != null) {
                result = state.value;
            }
            pos++;
        } else {
            state = nonAscii;
        }
        while ((c = buffer.Peek(pos)) >= 0) {
            state = state.tree.Find((char) c, caseInsensitive);
            if (state == null) {
                break;
            } else if (state.value != null) {
                result = state.value;
            }
            pos++;
        }
        return result;
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        for (int i = 0; i < ascii.Length; i++) {
            if (ascii[i] != null) {
                buffer.Append((char) i);
                if (ascii[i].value != null) {
                    buffer.Append(": ");
                    buffer.Append(ascii[i].value);
                    buffer.Append("\n");
                }
                ascii[i].tree.PrintTo(buffer, " ");
            }
        }
        nonAscii.tree.PrintTo(buffer, "");
        return buffer.ToString();
    }
}

internal class DFAState {
    internal TokenPattern value = null;

    internal TransitionTree tree = new TransitionTree();
}

internal class TransitionTree {
    private char value = '\0';

    private DFAState state = null;

    private TransitionTree left = null;

    private TransitionTree right = null;

    public TransitionTree() {
    }

    public DFAState Find(char c, bool lowerCase) {
        if (lowerCase) {
            c = Char.ToLower(c);
        }
        if (value == '\0' || value == c) {
            return state;
        } else if (value > c) {
            return left.Find(c, false);
        } else {
            return right.Find(c, false);
        }
    }

    public void Add(char c, bool lowerCase, DFAState state) {
        if (lowerCase) {
            c = Char.ToLower(c);
        }
        if (value == '\0') {
            this.value = c;
            this.state = state;
            this.left = new TransitionTree();
            this.right = new TransitionTree();
        } else if (value > c) {
            left.Add(c, false, state);
        } else {
            right.Add(c, false, state);
        }
    }

    public void PrintTo(StringBuilder buffer, String indent) {
        if (this.left != null) {
            this.left.PrintTo(buffer, indent);
        }
        if (this.value != '\0') {
            if (buffer.Length > 0 && buffer[buffer.Length - 1] == '\n') {
                buffer.Append(indent);
            }
            buffer.Append(this.value);
            if (this.state.value != null) {
                buffer.Append(": ");
                buffer.Append(this.state.value);
                buffer.Append("\n");
            }
            this.state.tree.PrintTo(buffer, indent + " ");
        }
        if (this.right != null) {
            this.right.PrintTo(buffer, indent);
        }
    }
}