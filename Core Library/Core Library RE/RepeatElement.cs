using System.Collections;

namespace Core.Library.RE;

internal class RepeatElement : Element {
    public enum RepeatType {
        GREEDY = 1,
        RELUCTANT = 2,
        POSSESSIVE = 3
    }

    private Element elem;

    private int min;

    private int max;

    private RepeatType type;

    private int matchStart;

    private BitArray matches;

    public RepeatElement(Element elem,
                         int min,
                         int max,
                         RepeatType type) {

        this.elem = elem;
        this.min = min;
        if (max <= 0) {
            this.max = Int32.MaxValue;
        } else {
            this.max = max;
        }
        this.type = type;
        this.matchStart = -1;
        this.matches = null;
    }

    public override object Clone() {
        return new RepeatElement((Element) elem.Clone(),
                                 min,
                                 max,
                                 type);
    }

    public override int Match(Matcher m,
                              ReaderBuffer buffer,
                              int start,
                              int skip) {

        if (skip == 0) {
            matchStart = -1;
            matches = null;
        }
        switch (type) {
        case RepeatType.GREEDY:
            return MatchGreedy(m, buffer, start, skip);
        case RepeatType.RELUCTANT:
            return MatchReluctant(m, buffer, start, skip);
        case RepeatType.POSSESSIVE:
            if (skip == 0) {
                return MatchPossessive(m, buffer, start, 0);
            }
            break;
        }
        return -1;
    }

    private int MatchGreedy(Matcher m,
                            ReaderBuffer buffer,
                            int start,
                            int skip) {

        if (skip == 0) {
            return MatchPossessive(m, buffer, start, 0);
        }

        if (matchStart != start) {
            matchStart = start;
            matches = new BitArray(10);
            FindMatches(m, buffer, start, 0, 0, 0);
        }

        for (int i = matches.Count - 1; i >= 0; i--) {
            if (matches[i]) {
                if (skip == 0) {
                    return i;
                }
                skip--;
            }
        }
        return -1;
    }

    private int MatchReluctant(Matcher m,
                               ReaderBuffer buffer,
                               int start,
                               int skip) {

        if (matchStart != start) {
            matchStart = start;
            matches = new BitArray(10);
            FindMatches(m, buffer, start, 0, 0, 0);
        }

        for (int i = 0; i < matches.Count; i++) {
            if (matches[i]) {
                if (skip == 0) {
                    return i;
                }
                skip--;
            }
        }
        return -1;
    }

    private int MatchPossessive(Matcher m,
                                ReaderBuffer buffer,
                                int start,
                                int count) {

        int  length = 0;
        int  subLength = 1;

        while (subLength > 0 && count < max) {
            subLength = elem.Match(m, buffer, start + length, 0);
            if (subLength >= 0) {
                count++;
                length += subLength;
            }
        }

        if (min <= count && count <= max) {
            return length;
        } else {
            return -1;
        }
    }

    private void FindMatches(Matcher m,
                             ReaderBuffer buffer,
                             int start,
                             int length,
                             int count,
                             int attempt) {

        int  subLength;

        if (count > max) {
            return;
        }
        if (min <= count && attempt == 0) {
            if (matches.Length <= length) {
                matches.Length = length + 10;
            }
            matches[length] = true;
        }

        subLength = elem.Match(m, buffer, start, attempt);
        if (subLength < 0) {
            return;
        } else if (subLength == 0) {
            if (min == count + 1) {
                if (matches.Length <= length) {
                    matches.Length = length + 10;
                }
                matches[length] = true;
            }
            return;
        }

        FindMatches(m, buffer, start, length, count, attempt + 1);
        FindMatches(m,
                    buffer,
                    start + subLength,
                    length + subLength,
                    count + 1,
                    0);
    }

    public override void PrintTo(TextWriter output, string indent) {
        output.Write(indent + "Repeat (" + min + "," + max + ")");
        if (type == RepeatType.RELUCTANT) {
            output.Write("?");
        } else if (type == RepeatType.POSSESSIVE) {
            output.Write("+");
        }
        output.WriteLine();
        elem.PrintTo(output, indent + "  ");
    }
}