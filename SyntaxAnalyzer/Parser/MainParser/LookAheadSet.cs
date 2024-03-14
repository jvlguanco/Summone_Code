using System.Collections;
using System.Text;

namespace Core.Library;

internal class LookAheadSet {
    private ArrayList elements = new ArrayList();

    private int maxLength;

    public LookAheadSet(int maxLength) {
        this.maxLength = maxLength;
    }

    public LookAheadSet(int maxLength, LookAheadSet set)
        : this(maxLength) {

        AddAll(set);
    }

    public int Size() {
        return elements.Count;
    }

    public int GetMinLength() {
        Sequence  seq;
        int       min = -1;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (min < 0 || seq.Length() < min) {
                min = seq.Length();
            }
        }
        return (min < 0) ? 0 : min;
    }

    public int GetMaxLength() {
        Sequence  seq;
        int       max = 0;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (seq.Length() > max) {
                max = seq.Length();
            }
        }
        return max;
    }

    public int[] GetInitialTokens() {
        ArrayList  list = new ArrayList();
        int[]      result;
        object     token;
        int        i;

        for (i = 0; i < elements.Count; i++) {
            token = ((Sequence) elements[i]).GetToken(0);
            if (token != null && !list.Contains(token)) {
                list.Add(token);
            }
        }
        result = new int[list.Count];
        for (i = 0; i < list.Count; i++) {
            result[i] = (int) list[i];
        }
        return result;
    }

    public bool IsRepetitive() {
        Sequence  seq;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (seq.IsRepetitive()) {
                return true;
            }
        }
        return false;
    }

    public bool IsNext(Parser parser) {
        Sequence  seq;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (seq.IsNext(parser)) {
                return true;
            }
        }
        return false;
    }

    public bool IsNext(Parser parser, int length) {
        Sequence  seq;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (seq.IsNext(parser, length)) {
                return true;
            }
        }
        return false;
    }

    public bool IsOverlap(LookAheadSet set) {
        for (int i = 0; i < elements.Count; i++) {
            if (set.IsOverlap((Sequence) elements[i])) {
                return true;
            }
        }
        return false;
    }

    private bool IsOverlap(Sequence seq) {
        Sequence  elem;

        for (int i = 0; i < elements.Count; i++) {
            elem = (Sequence) elements[i];
            if (seq.StartsWith(elem) || elem.StartsWith(seq)) {
                return true;
            }
        }
        return false;
    }

    private bool Contains(Sequence elem) {
        return FindSequence(elem) != null;
    }

    public bool Intersects(LookAheadSet set) {
        for (int i = 0; i < elements.Count; i++) {
            if (set.Contains((Sequence) elements[i])) {
                return true;
            }
        }
        return false;
    }

    private Sequence FindSequence(Sequence elem) {
        for (int i = 0; i < elements.Count; i++) {
            if (elements[i].Equals(elem)) {
                return (Sequence) elements[i];
            }
        }
        return null;
    }

    private void Add(Sequence seq) {
        if (seq.Length() > maxLength) {
            seq = new Sequence(maxLength, seq);
        }
        if (!Contains(seq)) {
            elements.Add(seq);
        }
    }

    public void Add(int token) {
        Add(new Sequence(false, token));
    }

    public void AddAll(LookAheadSet set) {
        for (int i = 0; i < set.elements.Count; i++) {
            Add((Sequence) set.elements[i]);
        }
    }

    public void AddEmpty() {
        Add(new Sequence());
    }

    private void Remove(Sequence seq) {
        elements.Remove(seq);
    }

    public void RemoveAll(LookAheadSet set) {
        for (int i = 0; i < set.elements.Count; i++) {
            Remove((Sequence) set.elements[i]);
        }
    }

    public LookAheadSet CreateNextSet(int token) {
        LookAheadSet  result = new LookAheadSet(maxLength - 1);
        Sequence      seq;
        object        value;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            value = seq.GetToken(0);
            if (value != null && token == (int) value) {
                result.Add(seq.Subsequence(1));
            }
        }
        return result;
    }

    public LookAheadSet CreateIntersection(LookAheadSet set) {
        LookAheadSet  result = new LookAheadSet(maxLength);
        Sequence      seq1;
        Sequence      seq2;

        for (int i = 0; i < elements.Count; i++) {
            seq1 = (Sequence) elements[i];
            seq2 = set.FindSequence(seq1);
            if (seq2 != null && seq1.IsRepetitive()) {
                result.Add(seq2);
            } else if (seq2 != null) {
                result.Add(seq1);
            }
        }
        return result;
    }

    public LookAheadSet CreateCombination(LookAheadSet set) {
        LookAheadSet  result = new LookAheadSet(maxLength);
        Sequence      first;
        Sequence      second;

        // Handle special cases
        if (this.Size() <= 0) {
            return set;
        } else if (set.Size() <= 0) {
            return this;
        }

        // Create combinations
        for (int i = 0; i < elements.Count; i++) {
            first = (Sequence) elements[i];
            if (first.Length() >= maxLength) {
                result.Add(first);
            } else if (first.Length() <= 0) {
                result.AddAll(set);
            } else {
                for (int j = 0; j < set.elements.Count; j++) {
                    second = (Sequence) set.elements[j];
                    result.Add(first.Concat(maxLength, second));
                }
            }
        }
        return result;
    }

    public LookAheadSet CreateOverlaps(LookAheadSet set) {
        LookAheadSet  result = new LookAheadSet(maxLength);
        Sequence      seq;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (set.IsOverlap(seq)) {
                result.Add(seq);
            }
        }
        return result;
    }

    public LookAheadSet CreateFilter(LookAheadSet set) {
        LookAheadSet  result = new LookAheadSet(maxLength);
        Sequence      first;
        Sequence      second;

        // Handle special cases
        if (this.Size() <= 0 || set.Size() <= 0) {
            return this;
        }

        // Create combinations
        for (int i = 0; i < elements.Count; i++) {
            first = (Sequence) elements[i];
            for (int j = 0; j < set.elements.Count; j++) {
                second = (Sequence) set.elements[j];
                if (first.StartsWith(second)) {
                    result.Add(first.Subsequence(second.Length()));
                }
            }
        }
        return result;
    }

    public LookAheadSet CreateRepetitive() {
        LookAheadSet  result = new LookAheadSet(maxLength);
        Sequence      seq;

        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            if (seq.IsRepetitive()) {
                result.Add(seq);
            } else {
                result.Add(new Sequence(true, seq));
            }
        }
        return result;
    }

    public override string ToString() {
        return ToString(null);
    }

    public string ToString(Tokenizer tokenizer) {
        StringBuilder  buffer = new StringBuilder();
        Sequence       seq;

        buffer.Append("{");
        for (int i = 0; i < elements.Count; i++) {
            seq = (Sequence) elements[i];
            buffer.Append("\n  ");
            buffer.Append(seq.ToString(tokenizer));
        }
        buffer.Append("\n}");
        return buffer.ToString();
    }

    private class Sequence {
        private bool repeat = false;

        private ArrayList tokens = null;

        public Sequence() {
            this.repeat = false;
            this.tokens = new ArrayList(0);
        }

        public Sequence(bool repeat, int token) {
            this.repeat = false;
            this.tokens = new ArrayList(1);
            this.tokens.Add(token);
        }

        public Sequence(int length, Sequence seq) {
            this.repeat = seq.repeat;
            this.tokens = new ArrayList(length);
            if (seq.Length() < length) {
                length = seq.Length();
            }
            for (int i = 0; i < length; i++) {
                tokens.Add(seq.tokens[i]);
            }
        }

        public Sequence(bool repeat, Sequence seq) {
            this.repeat = repeat;
            this.tokens = seq.tokens;
        }

        public int Length() {
            return tokens.Count;
        }

        public object GetToken(int pos) {
            if (pos >= 0 && pos < tokens.Count) {
                return tokens[pos];
            } else {
                return null;
            }
        }

        public override bool Equals(object obj) {
            if (obj is Sequence) {
                return Equals((Sequence) obj);
            } else {
                return false;
            }
        }

        public bool Equals(Sequence seq) {
            if (tokens.Count != seq.tokens.Count) {
                return false;
            }
            for (int i = 0; i < tokens.Count; i++) {
                if (!tokens[i].Equals(seq.tokens[i])) {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode() {
            return tokens.Count.GetHashCode();
        }

        public bool StartsWith(Sequence seq) {
            if (Length() < seq.Length()) {
                return false;
            }
            for (int i = 0; i < seq.tokens.Count; i++) {
                if (!tokens[i].Equals(seq.tokens[i])) {
                    return false;
                }
            }
            return true;
        }

        public bool IsRepetitive() {
            return repeat;
        }

        public bool IsNext(Parser parser) {
            Token   token;
            int     id;

            for (int i = 0; i < tokens.Count; i++) {
                id = (int) tokens[i];
                token = parser.PeekToken(i);
                if (token == null || token.Id != id) {
                    return false;
                }
            }
            return true;
        }

        public bool IsNext(Parser parser, int length) {
            Token  token;
            int    id;

            if (length > tokens.Count) {
                length = tokens.Count;
            }
            for (int i = 0; i < length; i++) {
                id = (int) tokens[i];
                token = parser.PeekToken(i);
                if (token == null || token.Id != id) {
                    return false;
                }
            }
            return true;
        }

        public override string ToString() {
            return ToString(null);
        }

        public string ToString(Tokenizer tokenizer) {
            StringBuilder  buffer = new StringBuilder();
            string         str;
            int            id;

            if (tokenizer == null) {
                buffer.Append(tokens.ToString());
            } else {
                buffer.Append("[");
                for (int i = 0; i < tokens.Count; i++) {
                    id = (int) tokens[i];
                    str = tokenizer.GetPatternDescription(id);
                    if (i > 0) {
                        buffer.Append(" ");
                    }
                    buffer.Append(str);
                }
                buffer.Append("]");
            }
            if (repeat) {
                buffer.Append(" *");
            }
            return buffer.ToString();
        }

        public Sequence Concat(int length, Sequence seq) {
            Sequence  res = new Sequence(length, this);

            if (seq.repeat) {
                res.repeat = true;
            }
            length -= this.Length();
            if (length > seq.Length()) {
                res.tokens.AddRange(seq.tokens);
            } else {
                for (int i = 0; i < length; i++) {
                    res.tokens.Add(seq.tokens[i]);
                }
            }
            return res;
        }

        public Sequence Subsequence(int start) {
            Sequence  res = new Sequence(Length(), this);

            while (start > 0 && res.tokens.Count > 0) {
                res.tokens.RemoveAt(0);
                start--;
            }
            return res;
        }
    }
}