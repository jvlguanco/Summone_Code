using System.Collections;
using System.IO;

namespace Core.Library;

public abstract class Node {
    private Node parent = null;

    private ArrayList values = null;

    internal virtual bool IsHidden() {
        return false;
    }

    public abstract int Id {
        get;
    }

    public virtual int GetId() {
        return Id;
    }

    public abstract string Name {
        get;
    }

    public virtual string GetName() {
        return Name;
    }

    public virtual int StartLine {
        get {
            int  line;

            for (int i = 0; i < Count; i++) {
                line = this[i].StartLine;
                if (line >= 0) {
                    return line;
                }
            }
            return -1;
        }
    }

    public virtual int GetStartLine() {
        return StartLine;
    }

    public virtual int StartColumn {
        get {
            int  col;

            for (int i = 0; i < Count; i++) {
                col = this[i].StartColumn;
                if (col >= 0) {
                    return col;
                }
            }
            return -1;
        }
    }

    public virtual int GetStartColumn() {
        return StartColumn;
    }

    public virtual int EndLine {
        get {
            int  line;

            for (int i = Count - 1; i >= 0; i--) {
                line = this[i].EndLine;
                if (line >= 0) {
                    return line;
                }
            }
            return -1;
        }
    }

    public virtual int GetEndLine() {
        return EndLine;
    }

    public virtual int EndColumn {
        get {
            int  col;

            for (int i = Count - 1; i >= 0; i--) {
                col = this[i].EndColumn;
                if (col >= 0) {
                    return col;
                }
            }
            return -1;
        }
    }

    public virtual int GetEndColumn() {
        return EndColumn;
    }

    public Node Parent {
        get {
            return parent;
        }
        set {
            this.parent = value;
        }
    }

    public Node GetParent() {
        return Parent;
    }

    public void SetParent(Node parent) {
        Parent = parent;
    }

    public virtual int Count {
        get {
            return 0;
        }
    }

    public virtual int GetChildCount() {
        return Count;
    }

    public int GetDescendantCount() {
        int  count = 0;

        for (int i = 0; i < Count; i++) {
            count += 1 + this[i].GetDescendantCount();
        }
        return count;
    }

    public virtual Node this[int index] {
        get {
            return null;
        }
    }

    public virtual Node GetChildAt(int index) {
        return this[index];
    }

    public ArrayList Values {
        get {
            if (values == null) {
                values = new ArrayList();
            }
            return values;
        }
        set {
            this.values = value;
        }
    }

    public int GetValueCount() {
        if (values == null) {
            return 0;
        } else {
            return values.Count;
        }
    }

    public object GetValue(int pos) {
        return Values[pos];
    }

    public ArrayList GetAllValues() {
        return values;
    }

    public void AddValue(object value) {
        if (value != null) {
            Values.Add(value);
        }
    }

    public void AddValues(ArrayList values) {
        if (values != null) {
            Values.AddRange(values);
        }
    }

    public void RemoveAllValues() {
        values = null;
    }

    public void PrintTo(TextWriter output) {
        PrintTo(output, "");
        output.Flush();
    }

    private void PrintTo(TextWriter output, string indent) {
        output.WriteLine(indent + ToString());
        indent = indent + "  ";
        for (int i = 0; i < Count; i++) {
            this[i].PrintTo(output, indent);
        }
    }
}