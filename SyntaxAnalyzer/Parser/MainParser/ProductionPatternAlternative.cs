using System.Collections;
using System.Text;

namespace Core.Library;

public class ProductionPatternAlternative {
    private ProductionPattern pattern;

    private ArrayList elements = new ArrayList();

    private LookAheadSet lookAhead = null;

    public ProductionPatternAlternative() {
    }

    public ProductionPattern Pattern {
        get {
            return pattern;
        }
    }

    public ProductionPattern GetPattern() {
        return Pattern;
    }

    internal LookAheadSet LookAhead {
        get {
            return lookAhead;
        }
        set {
            lookAhead = value;
        }
    }

    public int Count {
        get {
            return elements.Count;
        }
    }

    public int GetElementCount() {
        return Count;
    }

    public ProductionPatternElement this[int index] {
        get {
            return (ProductionPatternElement) elements[index];
        }
    }

    public ProductionPatternElement GetElement(int pos) {
        return this[pos];
    }

    public bool IsLeftRecursive() {
        ProductionPatternElement  elem;

        for (int i = 0; i < elements.Count; i++) {
            elem = (ProductionPatternElement) elements[i];
            if (elem.Id == pattern.Id) {
                return true;
            } else if (elem.MinCount > 0) {
                break;
            }
        }
        return false;
    }

    public bool IsRightRecursive() {
        ProductionPatternElement  elem;

        for (int i = elements.Count - 1; i >= 0; i--) {
            elem = (ProductionPatternElement) elements[i];
            if (elem.Id == pattern.Id) {
                return true;
            } else if (elem.MinCount > 0) {
                break;
            }
        }
        return false;
    }

    public bool IsMatchingEmpty() {
        return GetMinElementCount() == 0;
    }

    internal void SetPattern(ProductionPattern pattern) {
        this.pattern = pattern;
    }

    public int GetMinElementCount() {
        ProductionPatternElement  elem;
        int                       min = 0;

        for (int i = 0; i < elements.Count; i++) {
            elem = (ProductionPatternElement) elements[i];
            min += elem.MinCount;
        }
        return min;
    }

    public int GetMaxElementCount() {
        ProductionPatternElement  elem;
        int                       max = 0;

        for (int i = 0; i < elements.Count; i++) {
            elem = (ProductionPatternElement) elements[i];
            if (elem.MaxCount >= Int32.MaxValue) {
                return Int32.MaxValue;
            } else {
                max += elem.MaxCount;
            }
        }
        return max;
    }

    public void AddToken(int id, int min, int max) {
        AddElement(new ProductionPatternElement(true, id, min, max));
    }

    public void AddProduction(int id, int min, int max) {
        AddElement(new ProductionPatternElement(false, id, min, max));
    }

    public void AddElement(ProductionPatternElement elem) {
        elements.Add(elem);
    }

    public void AddElement(ProductionPatternElement elem,
                           int min,
                           int max) {

        if (elem.IsToken()) {
            AddToken(elem.Id, min, max);
        } else {
            AddProduction(elem.Id, min, max);
        }
    }

    public override bool Equals(object obj) {
        if (obj is ProductionPatternAlternative) {
            return Equals((ProductionPatternAlternative) obj);
        } else {
            return false;
        }
    }

    public bool Equals(ProductionPatternAlternative alt) {
        if (elements.Count != alt.elements.Count) {
            return false;
        }
        for (int i = 0; i < elements.Count; i++) {
            if (!elements[i].Equals(alt.elements[i])) {
                return false;
            }
        }
        return true;
    }

    public override int GetHashCode() {
        return elements.Count.GetHashCode();
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        for (int i = 0; i < elements.Count; i++) {
            if (i > 0) {
                buffer.Append(" ");
            }
            buffer.Append(elements[i]);
        }
        return buffer.ToString();
    }
}