using System.Text;

namespace Core.Library;

public class ProductionPatternElement {
    private bool token;
    private int id;
    private int min;
    private int max;
    private LookAheadSet lookAhead;

    public ProductionPatternElement(bool isToken,
                                    int id,
                                    int min,
                                    int max) {

        this.token = isToken;
        this.id = id;
        if (min < 0) {
            min = 0;
        }
        this.min = min;
        if (max <= 0) {
            max = Int32.MaxValue;
        } else if (max < min) {
            max = min;
        }
        this.max = max;
        this.lookAhead = null;
    }

    public int Id {
        get {
            return id;
        }
    }

    public int GetId() {
        return Id;
    }

    public int MinCount {
        get {
            return min;
        }
    }

    public int GetMinCount() {
        return MinCount;
    }

    public int MaxCount {
        get {
            return max;
        }
    }

    public int GetMaxCount() {
        return MaxCount;
    }

    internal LookAheadSet LookAhead {
        get {
            return lookAhead;
        }
        set {
            lookAhead = value;
        }
    }

    public bool IsToken() {
        return token;
    }

    public bool IsProduction() {
        return !token;
    }

    public bool IsMatch(Token token) {
        return IsToken() && token != null && token.Id == id;
    }

    public override bool Equals(object obj) {
        ProductionPatternElement  elem;

        if (obj is ProductionPatternElement) {
            elem = (ProductionPatternElement) obj;
            return this.token == elem.token
                && this.id == elem.id
                && this.min == elem.min
                && this.max == elem.max;
        } else {
            return false;
        }
    }

    public override int GetHashCode() {
        return this.id * 37;
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        buffer.Append(id);
        if (token) {
            buffer.Append("(Token)");
        } else {
            buffer.Append("(Production)");
        }
        if (min != 1 || max != 1) {
            buffer.Append("{");
            buffer.Append(min);
            buffer.Append(",");
            buffer.Append(max);
            buffer.Append("}");
        }
        return buffer.ToString();
    }
}