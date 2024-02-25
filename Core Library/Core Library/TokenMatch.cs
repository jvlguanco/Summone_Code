namespace Core.Library;

internal class TokenMatch {
    private int length = 0;
    private TokenPattern pattern = null;
    public void Clear() {
        length = 0;
        pattern = null;
    }

    public int Length {
        get {
            return length;
        }
    }

    public TokenPattern Pattern {
        get {
            return pattern;
        }
    }

    public void Update(int length, TokenPattern pattern) {
        if (this.length < length) {
            this.length = length;
            this.pattern = pattern;
        } else if (this.length == length && this.pattern.Id > pattern.Id) {
            this.length = length;
            this.pattern = pattern;
        }
    }
}