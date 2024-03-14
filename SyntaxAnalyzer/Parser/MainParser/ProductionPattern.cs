using System.Collections;
using System.Text;

namespace Core.Library;

public class ProductionPattern {
    private int id;

    private string name;

    private bool synthetic;

    private ArrayList alternatives;

    private int defaultAlt;

    private LookAheadSet lookAhead;

    public ProductionPattern(int id, string name) {
        this.id = id;
        this.name = name;
        this.synthetic = false;
        this.alternatives = new ArrayList();
        this.defaultAlt = -1;
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

    public string Name {
        get {
            return name;
        }
    }

    public string GetName() {
        return Name;
    }

    public bool Synthetic {
        get {
            return synthetic;
        }
        set {
            synthetic = value;
        }
    }

    public bool IsSyntetic() {
        return Synthetic;
    }

    public void SetSyntetic(bool synthetic) {
        Synthetic = synthetic;
    }

    internal LookAheadSet LookAhead {
        get {
            return lookAhead;
        }
        set {
            lookAhead = value;
        }
    }

    internal ProductionPatternAlternative DefaultAlternative {
        get {
            if (defaultAlt >= 0) {
                object obj = alternatives[defaultAlt];
                return (ProductionPatternAlternative) obj;
            } else {
                return null;
            }
        }
        set {
            defaultAlt = 0;
            for (int i = 0; i < alternatives.Count; i++) {
                if (alternatives[i] == value) {
                    defaultAlt = i;
                }
            }
        }
    }

    public int Count {
        get {
            return alternatives.Count;
        }
    }

    public int GetAlternativeCount() {
        return Count;
    }

    public ProductionPatternAlternative this[int index] {
        get {
            return (ProductionPatternAlternative) alternatives[index];
        }
    }

    public ProductionPatternAlternative GetAlternative(int pos) {
        return this[pos];
    }

    public bool IsLeftRecursive() {
        ProductionPatternAlternative  alt;

        for (int i = 0; i < alternatives.Count; i++) {
            alt = (ProductionPatternAlternative) alternatives[i];
            if (alt.IsLeftRecursive()) {
                return true;
            }
        }
        return false;
    }

    public bool IsRightRecursive() {
        ProductionPatternAlternative  alt;

        for (int i = 0; i < alternatives.Count; i++) {
            alt = (ProductionPatternAlternative) alternatives[i];
            if (alt.IsRightRecursive()) {
                return true;
            }
        }
        return false;
    }

    public bool IsMatchingEmpty() {
        ProductionPatternAlternative  alt;

        for (int i = 0; i < alternatives.Count; i++) {
            alt = (ProductionPatternAlternative) alternatives[i];
            if (alt.IsMatchingEmpty()) {
                return true;
            }
        }
        return false;
    }

    public void AddAlternative(ProductionPatternAlternative alt) {
        if (alternatives.Contains(alt)) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PRODUCTION,
                name,
                "two identical alternatives exist");
        }
        alt.SetPattern(this);
        alternatives.Add(alt);
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();
        StringBuilder  indent = new StringBuilder();
        int            i;

        buffer.Append(name);
        buffer.Append("(");
        buffer.Append(id);
        buffer.Append(") ");
        for (i = 0; i < buffer.Length; i++) {
            indent.Append(" ");
        }
        for (i = 0; i < alternatives.Count; i++) {
            if (i == 0) {
                buffer.Append("= ");
            } else {
                buffer.Append("\n");
                buffer.Append(indent);
                buffer.Append("| ");
            }
            buffer.Append(alternatives[i]);
        }
        return buffer.ToString();
    }
}