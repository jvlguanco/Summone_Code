using System.Collections;

namespace Core.Library;

public class RecursiveDescentParser : Parser {
    public RecursiveDescentParser(TextReader input) : base(input) {
    }

    public RecursiveDescentParser(TextReader input, Analyzer analyzer)
        : base(input, analyzer) {
    }

    public RecursiveDescentParser(Tokenizer tokenizer)
        : base(tokenizer) {
    }

    public RecursiveDescentParser(Tokenizer tokenizer,
                                  Analyzer analyzer)
        : base(tokenizer, analyzer) {
    }

    public override void AddPattern(ProductionPattern pattern) {
        if (pattern.IsMatchingEmpty()) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PRODUCTION,
                pattern.Name,
                "zero elements can be matched (minimum is one)");
        }

        if (pattern.IsLeftRecursive()) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PRODUCTION,
                pattern.Name,
                "left recursive patterns are not allowed");
        }

        base.AddPattern(pattern);
    }

    public override void Prepare() {
        IEnumerator e;
        base.Prepare();
        SetInitialized(false);

        e = GetPatterns().GetEnumerator();
        while (e.MoveNext()) {
            CalculateLookAhead((ProductionPattern)e.Current);
        }

        SetInitialized(true);
    }

    protected override Node ParseStart() {
        Token token;
        Node node;
        ArrayList list;

        node = ParsePattern(GetStartPattern());
        token = PeekToken(0);
        if (token != null) {
            list = new ArrayList(1);
            list.Add("<EOF>");
            throw new ParseException(
                ParseException.ErrorType.UNEXPECTED_TOKEN,
                token.ToShortString(),
                list,
                token.StartLine,
                token.StartColumn);
        }
        return node;
    }

    private Node ParsePattern(ProductionPattern pattern) {
        ProductionPatternAlternative alt;
        ProductionPatternAlternative defaultAlt;

        defaultAlt = pattern.DefaultAlternative;
        for (int i = 0; i < pattern.Count; i++) {
            alt = pattern[i];
            if (defaultAlt != alt && IsNext(alt)) {
                return ParseAlternative(alt);
            }
        }
        if (defaultAlt == null || !IsNext(defaultAlt)) {
            ThrowParseException(FindUnion(pattern));
        }
        return ParseAlternative(defaultAlt);
    }

    private Node ParseAlternative(ProductionPatternAlternative alt) {
        Production node;

        node = NewProduction(alt.Pattern);
        EnterNode(node);
        for (int i = 0; i < alt.Count; i++) {
            try {
                ParseElement(node, alt[i]);
            } catch (ParseException e) {
                AddError(e, true);
                NextToken();
                i--;
            }
        }
        return ExitNode(node);
    }

    private void ParseElement(Production node,
                              ProductionPatternElement elem) {

        Node child;

        for (int i = 0; i < elem.MaxCount; i++) {
            string pr = Enum.GetName(typeof(SyntaxConstants), elem.GetId());
            if (i < elem.MinCount || IsNext(elem)) {
                if (elem.IsToken()) {
                    child = NextToken(elem.Id);
                    EnterNode(child);
                    AddNode(node, ExitNode(child));
                    if(ExitNode(child) != null)
                    production.AddRecursiveProduction("Enter: " + pr + "\n");
                    production.AddProductionCode(elem.GetId());
                    production.AddProductionState("Enter: " + pr + "\n");
                }
                else {
                    pr = pr.Substring(5);
                    production.AddRecursiveProduction("Enter: <" + pr + ">\n");
                    production.AddProductionCode(elem.GetId());
                    production.AddProductionState("Enter: <" + pr + ">\n");
                    child = ParsePattern(GetPattern(elem.Id));
                    AddNode(node, child);
                }
            } else {
                pr = pr.Substring(5);
                production.AddRecursiveProduction("Enter: NULL <" + pr + ">\n");
                production.AddProductionState("NULL");
                production.AddProductionCode(elem.GetId());
                break;
            }
        }
    }

    private bool IsNext(ProductionPattern pattern) {
        LookAheadSet  set = pattern.LookAhead;

        if (set == null) {
            return false;
        } else {
            return set.IsNext(this);
        }
    }

    private bool IsNext(ProductionPatternAlternative alt) {
        LookAheadSet  set = alt.LookAhead;

        if (set == null) {
            return false;
        } else {
            return set.IsNext(this);
        }
    }

    private bool IsNext(ProductionPatternElement elem) {
        LookAheadSet  set = elem.LookAhead;

        if (set != null) {
            return set.IsNext(this);
        } else if (elem.IsToken()) {
            return elem.IsMatch(PeekToken(0));
        } else {
            return IsNext(GetPattern(elem.Id));
        }
    }

    private void CalculateLookAhead(ProductionPattern pattern) {
        ProductionPatternAlternative  alt;
        LookAheadSet                  result;
        LookAheadSet[]                alternatives;
        LookAheadSet                  conflicts;
        LookAheadSet                  previous = new LookAheadSet(0);
        int                           length = 1;
        int                           i;
        CallStack                     stack = new CallStack();

        stack.Push(pattern.Name, 1);
        result = new LookAheadSet(1);
        alternatives = new LookAheadSet[pattern.Count];
        for (i = 0; i < pattern.Count; i++) {
            alt = pattern[i];
            alternatives[i] = FindLookAhead(alt, 1, 0, stack, null);
            alt.LookAhead = alternatives[i];
            result.AddAll(alternatives[i]);
        }
        if (pattern.LookAhead == null) {
            pattern.LookAhead = result;
        }
        conflicts = FindConflicts(pattern, 1);

        while (conflicts.Size() > 0) {
            length++;
            stack.Clear();
            stack.Push(pattern.Name, length);
            conflicts.AddAll(previous);
            for (i = 0; i < pattern.Count; i++) {
                alt = pattern[i];
                if (alternatives[i].Intersects(conflicts)) {
                    alternatives[i] = FindLookAhead(alt,
                                                    length,
                                                    0,
                                                    stack,
                                                    conflicts);
                    alt.LookAhead = alternatives[i];
                }
                if (alternatives[i].Intersects(conflicts)) {
                    if (pattern.DefaultAlternative == null) {
                        pattern.DefaultAlternative = alt;
                    } else if (pattern.DefaultAlternative != alt) {
                        result = alternatives[i].CreateIntersection(conflicts);
                        ThrowAmbiguityException(pattern.Name,
                                                null,
                                                result);
                    }
                }
            }
            previous = conflicts;
            conflicts = FindConflicts(pattern, length);
        }

        for (i = 0; i < pattern.Count; i++) {
            CalculateLookAhead(pattern[i], 0);
        }
    }

    private void CalculateLookAhead(ProductionPatternAlternative alt,
                                    int pos) {

        ProductionPattern         pattern;
        ProductionPatternElement  elem;
        LookAheadSet              first;
        LookAheadSet              follow;
        LookAheadSet              conflicts;
        LookAheadSet              previous = new LookAheadSet(0);
        String                    location;
        int                       length = 1;

        if (pos >= alt.Count) {
            return;
        }

        pattern = alt.Pattern;
        elem = alt[pos];
        if (elem.MinCount == elem.MaxCount) {
            CalculateLookAhead(alt, pos + 1);
            return;
        }

        first = FindLookAhead(elem, 1, new CallStack(), null);
        follow = FindLookAhead(alt, 1, pos + 1, new CallStack(), null);

        location = "at position " + (pos + 1);
        conflicts = FindConflicts(pattern.Name,
                                  location,
                                  first,
                                  follow);
        while (conflicts.Size() > 0) {
            length++;
            conflicts.AddAll(previous);
            first = FindLookAhead(elem,
                                  length,
                                  new CallStack(),
                                  conflicts);
            follow = FindLookAhead(alt,
                                   length,
                                   pos + 1,
                                   new CallStack(),
                                   conflicts);
            first = first.CreateCombination(follow);
            elem.LookAhead = first;
            if (first.Intersects(conflicts)) {
                first = first.CreateIntersection(conflicts);
                ThrowAmbiguityException(pattern.Name, location, first);
            }
            previous = conflicts;
            conflicts = FindConflicts(pattern.Name,
                                      location,
                                      first,
                                      follow);
        }

        CalculateLookAhead(alt, pos + 1);
    }

    private LookAheadSet FindLookAhead(ProductionPattern pattern,
                                       int length,
                                       CallStack stack,
                                       LookAheadSet filter) {

        LookAheadSet  result;
        LookAheadSet  temp;

        // Check for infinite loop
        if (stack.Contains(pattern.Name, length)) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INFINITE_LOOP,
                pattern.Name,
                (String) null);
        }

        // Find pattern look-ahead
        stack.Push(pattern.Name, length);
        result = new LookAheadSet(length);
        for (int i = 0; i < pattern.Count; i++) {
            temp = FindLookAhead(pattern[i],
                                 length,
                                 0,
                                 stack,
                                 filter);
            result.AddAll(temp);
        }
        stack.Pop();

        return result;
    }

    private LookAheadSet FindLookAhead(ProductionPatternAlternative alt,
                                       int length,
                                       int pos,
                                       CallStack stack,
                                       LookAheadSet filter) {

        LookAheadSet  first;
        LookAheadSet  follow;
        LookAheadSet  overlaps;

        // Check trivial cases
        if (length <= 0 || pos >= alt.Count) {
            return new LookAheadSet(0);
        }

        // Find look-ahead for this element
        first = FindLookAhead(alt[pos], length, stack, filter);
        if (alt[pos].MinCount == 0) {
            first.AddEmpty();
        }

        // Find remaining look-ahead
        if (filter == null) {
            length -= first.GetMinLength();
            if (length > 0) {
                follow = FindLookAhead(alt, length, pos + 1, stack, null);
                first = first.CreateCombination(follow);
            }
        } else if (filter.IsOverlap(first)) {
            overlaps = first.CreateOverlaps(filter);
            length -= overlaps.GetMinLength();
            filter = filter.CreateFilter(overlaps);
            follow = FindLookAhead(alt, length, pos + 1, stack, filter);
            first.RemoveAll(overlaps);
            first.AddAll(overlaps.CreateCombination(follow));
        }

        return first;
    }

    private LookAheadSet FindLookAhead(ProductionPatternElement elem,
                                       int length,
                                       CallStack stack,
                                       LookAheadSet filter) {

        LookAheadSet  result;
        LookAheadSet  first;
        LookAheadSet  follow;
        int           max;

        first = FindLookAhead(elem, length, 0, stack, filter);
        result = new LookAheadSet(length);
        result.AddAll(first);
        if (filter == null || !filter.IsOverlap(result)) {
            return result;
        }

        if (elem.MaxCount == Int32.MaxValue) {
            first = first.CreateRepetitive();
        }
        max = elem.MaxCount;
        if (length < max) {
            max = length;
        }
        for (int i = 1; i < max; i++) {
            first = first.CreateOverlaps(filter);
            if (first.Size() <= 0 || first.GetMinLength() >= length) {
                break;
            }
            follow = FindLookAhead(elem,
                                   length,
                                   0,
                                   stack,
                                   filter.CreateFilter(first));
            first = first.CreateCombination(follow);
            result.AddAll(first);
        }

        return result;
    }

    private LookAheadSet FindLookAhead(ProductionPatternElement elem,
                                       int length,
                                       int dummy,
                                       CallStack stack,
                                       LookAheadSet filter) {

        LookAheadSet       result;
        ProductionPattern  pattern;

        if (elem.IsToken()) {
            result = new LookAheadSet(length);
            result.Add(elem.Id);
        } else {
            pattern = GetPattern(elem.Id);
            result = FindLookAhead(pattern, length, stack, filter);
            if (stack.Contains(pattern.Name)) {
                result = result.CreateRepetitive();
            }
        }

        return result;
    }

    private LookAheadSet FindConflicts(ProductionPattern pattern,
                                       int maxLength) {

        LookAheadSet  result = new LookAheadSet(maxLength);
        LookAheadSet  set1;
        LookAheadSet  set2;

        for (int i = 0; i < pattern.Count; i++) {
            set1 = pattern[i].LookAhead;
            for (int j = 0; j < i; j++) {
                set2 = pattern[j].LookAhead;
                result.AddAll(set1.CreateIntersection(set2));
            }
        }
        if (result.IsRepetitive()) {
            ThrowAmbiguityException(pattern.Name, null, result);
        }
        return result;
    }

    private LookAheadSet FindConflicts(string pattern,
                                       string location,
                                       LookAheadSet set1,
                                       LookAheadSet set2) {

        LookAheadSet  result;

        result = set1.CreateIntersection(set2);
        if (result.IsRepetitive()) {
            ThrowAmbiguityException(pattern, location, result);
        }
        return result;
    }

    private LookAheadSet FindUnion(ProductionPattern pattern) {
        LookAheadSet  result;
        int           length = 0;
        int           i;

        for (i = 0; i < pattern.Count; i++) {
            result = pattern[i].LookAhead;
            if (result.GetMaxLength() > length) {
                length = result.GetMaxLength();
            }
        }
        result = new LookAheadSet(length);
        for (i = 0; i < pattern.Count; i++) {
            result.AddAll(pattern[i].LookAhead);
        }

        return result;
    }

    private void ThrowParseException(LookAheadSet set) {
        Token      token;
        ArrayList  list = new ArrayList();
        int[]      initials;

        while (set.IsNext(this, 1)) {
            set = set.CreateNextSet(NextToken().Id);
        }

        initials = set.GetInitialTokens();
        for (int i = 0; i < initials.Length; i++) {
            list.Add(GetTokenDescription(initials[i]));
        }

        token = NextToken();
        throw new ParseException(ParseException.ErrorType.UNEXPECTED_TOKEN,
                                 token.ToShortString(),
                                 list,
                                 token.StartLine,
                                 token.StartColumn);
    }

    private void ThrowAmbiguityException(string pattern,
                                         string location,
                                         LookAheadSet set) {

        ArrayList  list = new ArrayList();
        int[]      initials;

        initials = set.GetInitialTokens();
        for (int i = 0; i < initials.Length; i++) {
            list.Add(GetTokenDescription(initials[i]));
        }

        // Create exception
        throw new ParserCreationException(
            ParserCreationException.ErrorType.INHERENT_AMBIGUITY,
            pattern,
            location,
            list);
    }

    private class CallStack {
        private ArrayList nameStack = new ArrayList();
        private ArrayList valueStack = new ArrayList();

        public bool Contains(string name) {
            return nameStack.Contains(name);
        }

        public bool Contains(string name, int value) {
            for (int i = 0; i < nameStack.Count; i++) {
                if (nameStack[i].Equals(name)
                 && valueStack[i].Equals(value)) {

                    return true;
                }
            }
            return false;
        }

        public void Clear() {
            nameStack.Clear();
            valueStack.Clear();
        }

        public void Push(string name, int value) {
            nameStack.Add(name);
            valueStack.Add(value);
        }

        public void Pop() {
            if (nameStack.Count > 0) {
                nameStack.RemoveAt(nameStack.Count - 1);
                valueStack.RemoveAt(valueStack.Count - 1);
            }
        }
    }
}