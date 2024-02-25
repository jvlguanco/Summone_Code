using System.Collections;

namespace Core.Library;

public class Production : Node {
    private ProductionPattern pattern;

    private ArrayList children;

    public Production(ProductionPattern pattern) {
        this.pattern = pattern;
        this.children = new ArrayList();
    }

    public override int Id {
        get {
            return pattern.Id;
        }
    }

    public override string Name {
        get {
            return pattern.Name;
        }
    }

    public override int Count {
        get {
            return children.Count;
        }
    }

    public override Node this[int index] {
        get {
            if (index < 0 || index >= children.Count) {
                return null;
            } else {
                return (Node) children[index];
            }
        }
    }

    public void AddChild(Node child) {
        if (child != null) {
            child.SetParent(this);
            children.Add(child);
        }
    }

    public ProductionPattern Pattern {
        get {
            return pattern;
        }
    }

    public ProductionPattern GetPattern() {
        return Pattern;
    }

    internal override bool IsHidden() {
        return pattern.Synthetic;
    }

    public override string ToString() {
        return pattern.Name + '(' + pattern.Id + ')';
    }
}