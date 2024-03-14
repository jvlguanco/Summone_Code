using System.Collections;

namespace Core.Library;

public class Analyzer {
    public Analyzer() {
    }

    public virtual void Reset() {
        // Default implementation does nothing
    }

    public virtual Node Analyze(Node node) {
        ParserLogException  log = new ParserLogException();

        node = Analyze(node, log);
        if (log.Count > 0) {
            throw log;
        }
        return node;
    }

    public virtual Node Analyze(Node node, ParserLogException log) {
        Production  prod;
        int         errorCount;

        Node res = null;
        errorCount = log.Count;
        if (node is Production) {
            prod = (Production) node;
            prod = NewProduction(prod.Pattern);
            try {
                Enter(prod);
            } catch (ParseException e) {
                log.AddError(e);
            }
            for (int i = 0; i < node.Count; i++) {
                try {
                    Child(prod, Analyze(node[i], log));
                } catch (ParseException e) {
                    log.AddError(e);
                }
            }
            try {
                res = Exit(prod);
                return res;
            } catch (ParseException e) {
                if (errorCount == log.Count) {
                    log.AddError(e);
                }
            }
        }
        else {
            node.Values.Clear();
            try {
                Enter(node);
            } catch (ParseException e) {
                log.AddError(e);
            }
            try {
                res = Exit(node);
                return res;
            } catch (ParseException e) {
                if (errorCount == log.Count) {
                    log.AddError(e);
                }
            }
        }
        return null;
    }

    public virtual Production NewProduction(ProductionPattern pattern) {
        return new Production(pattern);
    }

    public virtual void Enter(Node node) {
    }

    public virtual Node Exit(Node node) {
        return node;
    }

    public virtual void Child(Production node, Node child) {
        node.AddChild(child);
    }

    protected Node GetChildAt(Node node, int pos) {
        Node  child;

        if (node == null) {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "attempt to read 'null' parse tree node",
                -1,
                -1);
        }
        child = node[pos];
        if (child == null) {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "node '" + node.Name + "' has no child at " +
                "position " + pos,
                node.StartLine,
                node.StartColumn);
        }
        return child;
    }

    protected Node GetChildWithId(Node node, int id) {
        Node  child;

        if (node == null) {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "attempt to read 'null' parse tree node",
                -1,
                -1);
        }
        for (int i = 0; i < node.Count; i++) {
            child = node[i];
            if (child != null && child.Id == id) {
                return child;
            }
        }
        throw new ParseException(
            ParseException.ErrorType.INTERNAL,
            "node '" + node.Name + "' has no child with id " + id,
            node.StartLine,
            node.StartColumn);
    }

    protected object GetValue(Node node, int pos) {
        object  value;

        if (node == null) {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "attempt to read 'null' parse tree node",
                -1,
                -1);
        }
        value = node.Values[pos];
        if (value == null) {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "node '" + node.Name + "' has no value at " +
                "position " + pos,
                node.StartLine,
                node.StartColumn);
        }
        return value;
    }

    protected int GetIntValue(Node node, int pos) {
        object  value;

        value = GetValue(node, pos);
        if (value is int) {
            return (int) value;
        } else {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "node '" + node.Name + "' has no integer value " +
                "at position " + pos,
                node.StartLine,
                node.StartColumn);
        }
    }

    protected string GetStringValue(Node node, int pos) {
        object  value;

        value = GetValue(node, pos);
        if (value is string) {
            return (string) value;
        } else {
            throw new ParseException(
                ParseException.ErrorType.INTERNAL,
                "node '" + node.Name + "' has no string value " +
                "at position " + pos,
                node.StartLine,
                node.StartColumn);
        }
    }

    protected ArrayList GetChildValues(Node node) {
        ArrayList  result = new ArrayList();
        Node       child;
        ArrayList  values;

        for (int i = 0; i < node.Count; i++) {
            child = node[i];
            values = child.Values;
            if (values != null) {
                result.AddRange(values);
            }
        }
        return result;
    }
}