namespace Core.Library.RE;

internal class CombineElement : Element {
    private Element elem1;

    private Element elem2;

    public CombineElement(Element first, Element second) {
        elem1 = first;
        elem2 = second;
    }

    public override object Clone() {
        return new CombineElement(elem1, elem2);
    }

    public override int Match(Matcher m,
                              ReaderBuffer buffer,
                              int start,
                              int skip) {

        int  length1 = -1;
        int  length2 = 0;
        int  skip1 = 0;
        int  skip2 = 0;

        while (skip >= 0) {
            length1 = elem1.Match(m, buffer, start, skip1);
            if (length1 < 0) {
                return -1;
            }
            length2 = elem2.Match(m, buffer, start + length1, skip2);
            if (length2 < 0) {
                skip1++;
                skip2 = 0;
            } else {
                skip2++;
                skip--;
            }
        }

        return length1 + length2;
    }

    public override void PrintTo(TextWriter output, string indent) {
        elem1.PrintTo(output, indent);
        elem2.PrintTo(output, indent);
    }
}