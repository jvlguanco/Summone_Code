namespace Core.Library;

internal class TokenNFA {
    private NFAState[] initialChar = new NFAState[128];

    private NFAState initial = new NFAState();

    private NFAStateQueue queue = new NFAStateQueue();

    public void AddTextMatch(string str, bool ignoreCase, TokenPattern value) {
        NFAState  state;
        char      ch = str[0];

        if (ch < 128 && !ignoreCase) {
            state = initialChar[ch];
            if (state == null) {
                state = initialChar[ch] = new NFAState();
            }
        } else {
            state = initial.AddOut(ch, ignoreCase, null);
        }
        for (int i = 1; i < str.Length; i++) {
            state = state.AddOut(str[i], ignoreCase, null);
        }
        state.value = value;
    }

    public void AddRegExpMatch(string pattern,
                               bool ignoreCase,
                               TokenPattern value) {

        TokenRegExpParser  parser = new TokenRegExpParser(pattern, ignoreCase);
        string             debug = "DFA regexp; " + parser.GetDebugInfo();
        bool               isAscii;

        isAscii = parser.start.IsAsciiOutgoing();
        for (int i = 0; isAscii && i < 128; i++) {
            bool match = false;
            for (int j = 0; j < parser.start.outgoing.Length; j++) {
                if (parser.start.outgoing[j].Match((char) i)) {
                    if (match) {
                        isAscii = false;
                        break;
                    }
                    match = true;
                }
            }
            if (match && initialChar[i] != null) {
                isAscii = false;
            }
        }
        if (parser.start.incoming.Length > 0) {
            initial.AddOut(new NFAEpsilonTransition(parser.start));
            debug += ", uses initial epsilon";
        } else if (isAscii && !ignoreCase) {
            for (int i = 0; isAscii && i < 128; i++) {
                for (int j = 0; j < parser.start.outgoing.Length; j++) {
                    if (parser.start.outgoing[j].Match((char) i)) {
                        initialChar[i] = parser.start.outgoing[j].state;
                    }
                }
            }
            debug += ", uses ASCII lookup";
        } else {
            parser.start.MergeInto(initial);
            debug += ", uses initial state";
        }
        parser.end.value = value;
        value.DebugInfo = debug;
    }

    public int Match(ReaderBuffer buffer, TokenMatch match) {
        int       length = 0;
        int       pos = 1;
        int       peekChar;
        NFAState  state;

        this.queue.Clear();
        peekChar = buffer.Peek(0);
        if (0 <= peekChar && peekChar < 128) {
            state = this.initialChar[peekChar];
            if (state != null) {
                this.queue.AddLast(state);
            }
        }
        if (peekChar >= 0) {
            this.initial.MatchTransitions((char) peekChar, this.queue, true);
        }
        this.queue.MarkEnd();
        peekChar = buffer.Peek(1);

        while (!this.queue.Empty) {
            if (this.queue.Marked) {
                pos++;
                peekChar = buffer.Peek(pos);
                this.queue.MarkEnd();
            }
            state = this.queue.RemoveFirst();
            if (state.value != null) {
                match.Update(pos, state.value);
            }
            if (peekChar >= 0) {
                state.MatchTransitions((char) peekChar, this.queue, false);
            }
        }
        return length;
    }
}

internal class NFAState {
    internal TokenPattern value = null;

    internal NFATransition[] incoming = new NFATransition[0];

    internal NFATransition[] outgoing = new NFATransition[0];

    internal bool epsilonOut = false;

    public bool HasTransitions() {
        return incoming.Length > 0 || outgoing.Length > 0;
    }

    public bool IsAsciiOutgoing() {
        for (int i = 0; i < outgoing.Length; i++) {
            if (!outgoing[i].IsAscii()) {
                return false;
            }
        }
        return true;
    }

    public void AddIn(NFATransition trans) {
        Array.Resize(ref incoming, incoming.Length + 1);
        incoming[incoming.Length - 1] = trans;
    }

    public NFAState AddOut(char ch, bool ignoreCase, NFAState state) {
        if (ignoreCase) {
            if (state == null) {
                state = new NFAState();
            }
            AddOut(new NFACharTransition(Char.ToLower(ch), state));
            AddOut(new NFACharTransition(Char.ToUpper(ch), state));
            return state;
        } else {
            if (state == null) {
                state = FindUniqueCharTransition(ch);
                if (state != null) {
                    return state;
                }
                state = new NFAState();
            }
            return AddOut(new NFACharTransition(ch, state));
        }
    }

    public NFAState AddOut(NFATransition trans) {
        Array.Resize(ref outgoing, outgoing.Length + 1);
        outgoing[outgoing.Length - 1] = trans;
        if (trans is NFAEpsilonTransition) {
            epsilonOut = true;
        }
        return trans.state;
    }

    public void MergeInto(NFAState state) {
        for (int i = 0; i < incoming.Length; i++) {
            state.AddIn(incoming[i]);
            incoming[i].state = state;
        }
        incoming = null;
        for (int i = 0; i < outgoing.Length; i++) {
            state.AddOut(outgoing[i]);
        }
        outgoing = null;
    }

    private NFAState FindUniqueCharTransition(char ch) {
        NFATransition  res = null;
        NFATransition  trans;

        for (int i = 0; i < outgoing.Length; i++) {
            trans = outgoing[i];
            if (trans.Match(ch) && trans is NFACharTransition) {
                if (res != null) {
                    return null;
                }
                res = trans;
            }
        }
        for (int i = 0; res != null && i < outgoing.Length; i++) {
            trans = outgoing[i];
            if (trans != res && trans.state == res.state) {
                return null;
            }
        }
        return (res == null) ? null : res.state;
    }

    public void MatchTransitions(char ch, NFAStateQueue queue, bool initial) {
        NFATransition  trans;
        NFAState       target;

        for (int i = 0; i < outgoing.Length; i++) {
            trans = outgoing[i];
            target = trans.state;
            if (initial && trans is NFAEpsilonTransition) {
                target.MatchTransitions(ch, queue, true);
            } else if (trans.Match(ch)) {
                queue.AddLast(target);
                if (target.epsilonOut) {
                    target.MatchEmpty(queue);
                }
            }
        }
    }

    public void MatchEmpty(NFAStateQueue queue) {
        NFATransition  trans;
        NFAState       target;

        for (int i = 0; i < outgoing.Length; i++) {
            trans = outgoing[i];
            if (trans is NFAEpsilonTransition) {
                target = trans.state;
                queue.AddLast(target);
                if (target.epsilonOut) {
                    target.MatchEmpty(queue);
                }
            }
        }
    }
}

internal abstract class NFATransition {
    internal NFAState state;

    public NFATransition(NFAState state) {
        this.state = state;
        this.state.AddIn(this);
    }

    public abstract bool IsAscii();

    public abstract bool Match(char ch);

    public abstract NFATransition Copy(NFAState state);
}

internal class NFAEpsilonTransition : NFATransition {
    public NFAEpsilonTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return false;
    }

    public override bool Match(char ch) {
        return false;
    }

    public override NFATransition Copy(NFAState state) {
        return new NFAEpsilonTransition(state);
    }
}

internal class NFACharTransition : NFATransition {
    protected char match;

    public NFACharTransition(char match, NFAState state) : base(state) {
        this.match = match;
    }

    public override bool IsAscii() {
        return 0 <= match && match < 128;
    }

    public override bool Match(char ch) {
        return this.match == ch;
    }

    public override NFATransition Copy(NFAState state) {
        return new NFACharTransition(match, state);
    }
}

internal class NFACharRangeTransition : NFATransition {
    protected bool inverse;

    protected bool ignoreCase;

    private object[] contents = new object[0];

    public NFACharRangeTransition(bool inverse,
                                  bool ignoreCase,
                                  NFAState state) : base(state) {
        this.inverse = inverse;
        this.ignoreCase = ignoreCase;
    }

    public override bool IsAscii() {
        object  obj;
        char    c;

        if (inverse) {
            return false;
        }
        for (int i = 0; i < contents.Length; i++) {
            obj = contents[i];
            if (obj is char) {
                c = (char) obj;
                if (c < 0 || 128 <= c) {
                    return false;
                }
            } else if (obj is Range) {
                if (!((Range) obj).IsAscii()) {
                    return false;
                }
            }
        }
        return true;
    }

    public void AddCharacter(char c) {
        if (ignoreCase) {
            c = Char.ToLower(c);
        }
        AddContent(c);
    }

    public void AddRange(char min, char max) {
        if (ignoreCase) {
            min = Char.ToLower(min);
            max = Char.ToLower(max);
        }
        AddContent(new Range(min, max));
    }

    private void AddContent(Object obj) {
        Array.Resize(ref contents, contents.Length + 1);
        contents[contents.Length - 1] = obj;
    }

    public override bool Match(char ch) {
        object  obj;
        char    c;
        Range   r;

        if (ignoreCase) {
            ch = Char.ToLower(ch);
        }
        for (int i = 0; i < contents.Length; i++) {
            obj = contents[i];
            if (obj is char) {
                c = (char) obj;
                if (c == ch) {
                    return !inverse;
                }
            } else if (obj is Range) {
                r = (Range) obj;
                if (r.Inside(ch)) {
                    return !inverse;
                }
            }
        }
        return inverse;
    }

    public override NFATransition Copy(NFAState state) {
        NFACharRangeTransition  copy;

        copy = new NFACharRangeTransition(inverse, ignoreCase, state);
        copy.contents = contents;
        return copy;
    }

    private class Range {
        private char min;

        private char max;

        public Range(char min, char max) {
            this.min = min;
            this.max = max;
        }

        public bool IsAscii() {
            return 0 <= min && min < 128 &&
                   0 <= max && max < 128;
        }

        public bool Inside(char c) {
            return min <= c && c <= max;
        }
    }
}

internal class NFADotTransition : NFATransition {
    public NFADotTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return false;
    }

    public override bool Match(char ch) {
        switch (ch) {
        case '\n':
        case '\r':
        case '\u0085':
        case '\u2028':
        case '\u2029':
            return false;
        default:
            return true;
        }
    }

    public override NFATransition Copy(NFAState state) {
        return new NFADotTransition(state);
    }
}

internal class NFADigitTransition : NFATransition {
    public NFADigitTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return true;
    }

    public override bool Match(char ch) {
        return '0' <= ch && ch <= '9';
    }

    public override NFATransition Copy(NFAState state) {
        return new NFADigitTransition(state);
    }
}

internal class NFANonDigitTransition : NFATransition {
    public NFANonDigitTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return false;
    }

    public override bool Match(char ch) {
        return ch < '0' || '9' < ch;
    }

    public override NFATransition Copy(NFAState state) {
        return new NFANonDigitTransition(state);
    }
}

internal class NFAWhitespaceTransition : NFATransition {
    public NFAWhitespaceTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return true;
    }

    public override bool Match(char ch) {
        switch (ch) {
        case ' ':
        case '\t':
        case '\n':
        case '\f':
        case '\r':
        case (char) 11:
            return true;
        default:
            return false;
        }
    }

    public override NFATransition Copy(NFAState state) {
        return new NFAWhitespaceTransition(state);
    }
}

internal class NFANonWhitespaceTransition : NFATransition {
    public NFANonWhitespaceTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return false;
    }

    public override bool Match(char ch) {
        switch (ch) {
        case ' ':
        case '\t':
        case '\n':
        case '\f':
        case '\r':
        case (char) 11:
            return false;
        default:
            return true;
        }
    }

    public override NFATransition Copy(NFAState state) {
        return new NFANonWhitespaceTransition(state);
    }
}

internal class NFAWordTransition : NFATransition {
    public NFAWordTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return true;
    }

    public override bool Match(char ch) {
        return ('a' <= ch && ch <= 'z')
            || ('A' <= ch && ch <= 'Z')
            || ('0' <= ch && ch <= '9')
            || ch == '_';
    }

    public override NFATransition Copy(NFAState state) {
        return new NFAWordTransition(state);
    }
}

internal class NFANonWordTransition : NFATransition {
    public NFANonWordTransition(NFAState state) : base(state) {
    }

    public override bool IsAscii() {
        return false;
    }

    public override bool Match(char ch) {
        bool word = ('a' <= ch && ch <= 'z')
                 || ('A' <= ch && ch <= 'Z')
                 || ('0' <= ch && ch <= '9')
                 || ch == '_';
        return !word;
    }

    public override NFATransition Copy(NFAState state) {
        return new NFANonWordTransition(state);
    }
}

internal class NFAStateQueue {
    private NFAState[] queue = new NFAState[2048];

    private int first = 0;

    private int last = 0;

    private int mark = 0;

    public bool Empty {
        get {
            return (last <= first);
        }
    }

    public bool Marked {
        get {
            return first == mark;
        }
    }

    public void Clear() {
        first = 0;
        last = 0;
        mark = 0;
    }

    public void MarkEnd() {
        mark = last;
    }

    public NFAState RemoveFirst() {
        if (first < last) {
            first++;
            return queue[first - 1];
        } else {
            return null;
        }
    }

    public void AddLast(NFAState state) {
        if (last >= queue.Length) {
            if (first <= 0) {
                Array.Resize(ref queue, queue.Length * 2);
            } else {
                Array.Copy(queue, first, queue, 0, last - first);
                last -= first;
                mark -= first;
                first = 0;
            }
        }
        queue[last++] = state;
    }
}